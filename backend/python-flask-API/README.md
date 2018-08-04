These are instructions to run the floppy-bird Flask API
Remember this is a example app and is not intended for production use, only to test the mobius python sdk and showcase It's Classes/Functions,
So there's some steps to take before you can run the app.

1. Create virtualenv with: $ virtualenv -p python3 env
2. Activate env with : $ source env/bin/activate
3. Install requirements from python sdk with : $ pip install -r requirements.txt
4. Export needed vars for the api to work : $ export FLASK_ENV=development
                                           $ export APP_KEY={you're developer keypair}
                                           $ export FLASK_APP={path to api.py}
5. Run the API with (Do this from directory outside the mobius sdk folder): $ python -m flask run
6. Get the challenge and token from API recommended to use some API development tool like Postman or another rest client
  - Get challenge xdr from : GET http://{API_DOMAIN}/api/
  - Post challenge xdr to get the token: : POST http://{API_DOMAIN}/api/ with {"xdr":"(you're challenge xdr from last step)"}
(I'll leave get_token.py script that does above requests and writes token to token.txt)
- Post token to : POST http://{API_DOMAIN}/api/test?token={you're token} to get public key
7. Go to https://mobius.network/friendbot and add some MOBI coins to you're account
8. Change DAPP_API in flappy-dapp/frontend/public/js/main.js into url of the api (localhost:5000/api in my case)  
9. go to http://{APP_DOMAIN}/?token={you're token} and enjoy the game.