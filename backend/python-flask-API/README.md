# Mobius Python Flask API

This repo provides a default implementation of a Mobius DApp using a Python Flask API. It exposes some generaic API calls, like `/balance` and `/charge`. Note this is only intended to be a working example and is not for production use.

## Install

```console
# From the root of the folder, run the following commands:

# Clone `mobius-client-python`
$ https://github.com/mobius-network/mobius-client-python.git

# Run
$ python setup.py install

# Create virtualenv
$ virtualenv -p python3 env

# For first time use, activate env
$ source env/bin/activate

# Install requirements
$ pip install -r requirements.txt
```

## Set ENV Variables

A dev-wallet is needed to set `APP_KEY` environment variable. If you do not have a dev-wallet see <a href="https://docs.mobius.network/docs/installation#section-generating-key-pairs" target="_blank">Generating Key Pairs</a> in Mobius Docs.

```console
# Set development env
$ export FLASK_ENV=development

# Set the developer secret seed using dev-wallet.html
$ export APP_KEY=YOUR-SECRET-SEED

# Set the path to api.py
$ export FLASK_APP=PATH-TO-API.PY
```

## Run

```console
# Run locally
$ python -m flask run
```