# Mobius DApp Serverless Quickstart

This repo provides a default implementation of Mobius DApp backend running on [Webtask.io](https://webtask.io).
It exposes some generic API endpoints like `/api/balance` and `/api/charge`. For more detailed documentation on this example, see the official <a href="https://docs.mobius.network/docs/serverless-quickstart-api" target="_blank">Mobius Docs</a>.

## Install

```console
$ cd backend/serverless-quickstart
$ yarn install
```

## Configure

```console
# Register an account on webtask.io:
$ yarn wt profile init

# Setup a new DApp for development:
$ yarn setup
``` 

## Run

```console
# Run locally
$ yarn dev

# Deploy to Webtask.io
$ yarn deploy

# Deploy to Webtask.io and re-deploy on changes
$ yarn watch
```
