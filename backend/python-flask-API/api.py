import os
import json
import binascii
import datetime
from functools import wraps

from flask import Flask
from flask_cors import CORS
from flask import jsonify, abort, request

from mobius_client_python.app.app_builder import AppBuilder
from mobius_client_python.auth.challenge import Challenge
from mobius_client_python.auth.token import Token
from mobius_client_python.auth.jwt import Jwt
from mobius_client_python.auth.sign import Sign
from mobius_client_python.blockchain.friend_bot import FriendBot 
from mobius_client_python.blockchain.add_cosigner import AddCosigner 
from mobius_client_python.blockchain.create_trustline import CreateTrustline 
from mobius_client_python.client import Client 

from stellar_base.keypair import Keypair

from flask_jwt import JWT, jwt_required, current_identity
from werkzeug.security import safe_str_cmp

# Flask app
app = Flask(__name__)

# Enable cors
CORS(app)

app.config['DEBUG'] = True
app.config['APP_KEY'] = os.environ['APP_KEY']
app_key = app.config['APP_KEY']

# Developer
f = FriendBot()

dev_keypair = Keypair.from_seed(app_key)
dev_created = f.call(dev_keypair)
trust_line_dev = CreateTrustline().call(keypair=dev_keypair)

class User(object):
    def __init__(self, id, username, password):
        self.id = id
        self.username = username
        self.password = password
        self.keypair = Keypair.random()
        self.address = self.keypair.address().decode()

        # Mobi balance and dev user cosign config
        user_created = f.call(self.keypair)
        cosig = AddCosigner().call(keypair=self.keypair, cosigner_keypair=dev_keypair)
        trust_line_user = CreateTrustline().call(keypair=self.keypair)

    def __str__(self):
        return "User(id='%s')" % self.id


users = [
    User(1, 'test', 'test'),
    User(2, 'mobius', 'mobius'),
    User(3, 'crowdbotics', 'crowdbotics'),
]

username_table = {u.username: u for u in users}
userid_table = {u.id: u for u in users}

# quick auth handle
def authenticate(username, password):
    user = username_table.get(username, None)
    if user and safe_str_cmp(user.password.encode('utf-8'), password.encode('utf-8')):
        return user

def identity(payload):
    user_id = payload['identity']
    return userid_table.get(user_id, None)

def app_instance(user):
    app = AppBuilder().build(dev_keypair.seed(),user.address)

    return app

jwt = JWT(app, authenticate, identity)

def login_required(f):
    @wraps(f)
    def decorated_function(*args, **kwargs):
        if request.args.get('token') is None:
            return abort(403, description='Token missing')
        else:
            token = request.args.get('token')
            jwt = Jwt(secret=app_key)
            jwt_token = jwt.decode(value=token)
            current_user = None

            for user in users:
                if jwt_token['public_key'] == user.address:
                    current_user = user

            if current_user == None:
                return abort(403, description='Token not matched')

        return f(user=current_user, *args, **kwargs)
    return decorated_function



@app.route('/api/', methods=['GET','POST'])
def index():
    try:
        user = users[0] # test user

        if request.method == 'GET':

            time = datetime.datetime.now() + datetime.timedelta(days=60)

            challenge_te_xdr = Challenge(developer_secret=dev_keypair.seed(),
                             expires_in=time)\
                             .call()


            te_xdr = Sign(user_secret=user.keypair.seed(),te_xdr=challenge_te_xdr.decode(),address=dev_keypair.address().decode()).call()


            return jsonify(te_xdr)

        elif request.method == 'POST':

            data = json.loads(request.data.decode('utf-8'))
            dapp = app_instance(user)


            token = Token(
                        developer_secret=app_key,
                        te_xdr=data['xdr'],
                        address=user.keypair.address().decode(),
            )

            token.validate()
            jwt = Jwt(secret=app_key)

            jwt_token = jwt.encode(token=token)

            return jsonify(jwt_token.decode())

    except Exception as error:
        return error

@app.route('/api/test', methods=['POST'])
@login_required
def test(user=None):
    try:
        print(user)
        return jsonify(user.address)
    except Exception as error:
        return error

@app.route('/api/balance',methods=['GET'])
@login_required
def balance(user=None):
    try:
        dapp = app_instance(user)
        return jsonify({'balance': dapp.user_balance()})
    except Exception as error:
        return error

@app.route('/api/charge',methods=['POST'])
@login_required
def charge(user=None):
    try:
        dapp = app_instance(user)

        try:
            data = json.loads(request.data.decode('utf-8'))
            amount = data['amount']
        except:
            amount = 1

        if not amount or not float(amount):
            return abort(400, description='Invalid amount')

        response = dapp.charge(amount)

        hash_meta = binascii.hexlify(response.hash_meta()).decode()

        return jsonify({
            'status': 'ok',
            'tx_hash': hash_meta,
            'balance': dapp.user_balance(),
        })

    except Exception as error:
        return error

@app.route('/api/payout')
@login_required
def payout(user=None):
    try:
        dapp = app_instance(current_identity)
        data = json.loads(request.data.decode('utf-8'))
        amount = data['amount']
        target = data['target_address']

        if not amount or not float(amount):
            return abort(400, description='Invalid amount')

        response = dapp.payout(amount,target)

        return jsonify({
            'status': 'ok',
            'tx_hash': response.hash_meta(),
            'balance': dapp.user_balance(),
        })

    except Exception as error:
        return error

@app.route('/api/transfer')
@login_required
def transfer(user=None):
    try:
        dapp = app_instance(user)
        data = json.loads(request.data.decode('utf-8'))

        amount = data['amount']
        target = data['target_address']

        if not amount or not float(amount):
            return abort(400, description='Invalid amount')

        response = dapp.transfer(amount,target)

        return jsonify({
            'status': 'ok',
            'tx_hash': response.hash_meta(),
            'balance': response.user_balance(),
        })

    except Exception as error:
        return error
