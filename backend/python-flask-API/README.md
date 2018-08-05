# Mobius Python Flask API

This repo provides a default implementation of a Mobius DApp using a Python Flask API. It exposes some generic
API endpoints like `/api/balance` and `/api/charge`. For more detailed documentation on this example, see the official <a href="https://docs.mobius.network/docs/python-flask-api" target="_blank">Mobius Docs</a>.

## Install

```console
$ cd backend/python-flask-api

# Create virtualenv
$ virtualenv -p python3 env

# Activate env
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