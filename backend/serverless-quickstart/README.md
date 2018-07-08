# Mobius DApp Server

This repo provides a default implementation of Mobius DApp backend running on [Webtask.io](https://webtask.io).
It exposes some generic API calls, like `/balance` and `/charge`. If your DApp frontend requires something
different, you can fork this repo and extend the API up to your needs.

## Install

```console
$ git clone https://github.com/mobius-network/dapp-server-js.git
$ cd dapp-server-js
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

# Open development page
$ open dev.html

# Deploy to Webtask.io
$ yarn deploy

# Deploy to Webtask.io and re-deploy on changes
$ yarn watch
```
