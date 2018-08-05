# Mobius Node.js API 

This repo provides a default implementation of a Mobius DApp using a Node.js API. It exposes some generic
API endpoints like `/api/balance` and `/api/charge`. For more detailed
documentation on this example, see the official <a href="https://docs.mobius.network/docs/nodejs-api" target="_blank">Mobius Docs</a>.

## Install

```console
$ cd backend/nodejs-api
$ yarn install
```

## Set ENV Variables

Create `.env` file and populate with your secret seed from your generated `dev-wallet.html`. If you do not have a 
dev-wallet see <a href="https://docs.mobius.network/docs/installation#section-generating-key-pairs" target="_blank">Generating Key Pairs</a>
in Mobius Docs.

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
$ yarn dev
```
