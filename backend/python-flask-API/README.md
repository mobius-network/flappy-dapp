# Mobius Python Flask API

This repo provides a default implementation of a Mobius DApp using a Python Flask API. It exposes some generaic API calls, like `/api/balance` and `/api/charge`. This is only intended to be a working example and is not for production use. For more detailed documentation on this example, see the official <a href="https://docs.mobius.network/docs/python-flask-api" target="_blank">Mobius Docs</a>.

## Install

```console
# From the root of the folder, run the following commands:

# Create virtualenv
$ virtualenv -p python3 env

# For first time use, activate env
$ source env/bin/activate

# Install requirements
$ pip install -r requirements.txt

# Clone mobius-client-python
$ git clone https://github.com/mobius-network/mobius-client-python.git

# Navigate to mobius-client-python
$ cd mobius-client-python

# Run
$ python setup.py install
```

## Set ENV Variables

A dev-wallet is needed to set `APP_KEY` environment variable. If you do not have a dev-wallet see <a href="https://docs.mobius.network/docs/installation#section-generating-key-pairs" target="_blank">Generating Key Pairs</a> in Mobius Docs.

```console
# Navigate back to the root of the python-flask-API folder

# Set development env
$ export FLASK_ENV=development

# Set the developer secret seed from dev-wallet.html
$ export APP_KEY=YOUR-SECRET-SEED

# Set the path to api.py
$ export FLASK_APP=PATH-TO-API.PY
```

## Run

```console
# Run locally
$ python -m flask run
```