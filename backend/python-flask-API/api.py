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

from stellar_base.keypair import Keypair

from flask_jwt import JWT
from werkzeug.security import safe_str_cmp

# Flask app
app = Flask(__name__)

# Enable cors
CORS(app)

app.config['DEBUG'] = True
app.config['APP_KEY'] = os.environ['APP_KEY']
app_key = app.config['APP_KEY']

dev_keypair = Keypair.from_seed(app_key)

def app_instance(public_key):
	app = AppBuilder().build(dev_keypair.seed(), public_key)

	return app


def login_required(f):
	@wraps(f)
	def decorated_function(*args, **kwargs):
		if request.args.get('token') is None:
			return abort(403, description='Token missing.')
		else:
			token = request.args.get('token')
			jwt = Jwt(secret=app_key)
			jwt_token = jwt.decode(value=token)

		return f(public_key=jwt_token['public_key'], *args, **kwargs)
	return decorated_function


@app.route('/auth', methods=['GET','POST'])
def index():
	try:
		if request.method == 'GET':
			time = datetime.datetime.now() + datetime.timedelta(days=60)

			challenge_te_xdr = Challenge(developer_secret=dev_keypair.seed(),
								expires_in=time)\
								.call()

			return challenge_te_xdr

		elif request.method == 'POST':
			token = Token(
				developer_secret=app_key,
				te_xdr=request.args['xdr'],
				address=request.args['public_key']
			)

			token.validate()

			jwt = Jwt(secret=app_key)

			jwt_token = jwt.encode(token=token)

			return jsonify(jwt_token.decode())
	except Exception as error:
		return error


@app.route('/api/test', methods=['POST'])
@login_required
def test(public_key):
	try:
		print(user)
		return jsonify(user.address)
	except Exception as error:
		return error


@app.route('/api/balance',methods=['GET'])
@login_required
def balance(public_key):
	try:
		dapp = app_instance(public_key)
		return jsonify({'balance': dapp.user_balance()})
	except Exception as error:
		return error


@app.route('/api/charge', methods=['POST'])
@login_required
def charge(public_key):
	try:
		dapp = app_instance(public_key)

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


@app.route('/api/payout', methods=['POST'])
@login_required
def payout(public_key):
	try:
		dapp = app_instance(public_key)
		data = json.loads(request.data.decode('utf-8'))
		amount = data['amount']
		target = data['target_address']

		if not amount or not float(amount):
			return abort(400, description='Invalid amount')

		if not target:
			return abort(400, description='Invalid target address')

		response = dapp.payout(amount, target)
		hash_meta = binascii.hexlify(response.hash_meta()).decode()

		return jsonify({
			'status': 'ok',
			'tx_hash': hash_meta,
			'balance': dapp.user_balance(),
		})

	except Exception as error:
		return error


@app.route('/api/transfer', methods=['POST'])
@login_required
def transfer(public_key):
	try:
		dapp = app_instance(public_key)
		data = json.loads(request.data.decode('utf-8'))

		amount = data['amount']
		target = data['target_address']

		if not amount or not float(amount):
			return abort(400, description='Invalid amount')
        
		if not target:
			return abort(400, description='Invalid target address')

		response = dapp.transfer(amount, target)
		hash_meta = binascii.hexlify(response.hash_meta()).decode()

		return jsonify({
			'status': 'ok',
			'tx_hash': hash_meta,
			'balance': dapp.user_balance(),
		})
	except Exception as error:
		return error