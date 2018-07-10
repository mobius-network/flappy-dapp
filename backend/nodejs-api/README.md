# Mobius Node.js API 

This repo provides a default implementation of a Mobius DApp using a Node.js API. It exposes some generic API calls, like `/balance` and `/charge` and is used to play Flappy Bird.

## Install

```console
$ yarn install
```

## Set ENV Variables

create `.env` file and populate with your secret seed from your generated `dev-wallet.html`. If you do not have a dev-wallet see <a href="https://docs.mobius.network/docs/installation#section-generating-key-pairs" target="_blank">Generating Key Pairs</a> in Mobius Docs.

```console
PORT=8080
NETWORK=testnet
APP_NAME=Flappy Bird
APP_DOMAIN=flappy.mobius.network
APP_STORE=store.beta.mobius.network
APP_KEY=YOUR-SECRET-SEED
```

## Run

```console
# Run locally
$ yarn dev
```
