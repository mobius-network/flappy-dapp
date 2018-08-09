# Mobius PHP API 

This repo provides a default implementation of a Mobius DApp using PHP API. It exposes some generic
API endpoints like `/balance` and `/charge`.

## Install

Run this composer command
```console
$ composer require mobius-network/flappy-dapp-php
```

## Set JWT and Seed

There is `config.php` file in root directory of this PHP repo. You can put your own values for `SECRET_KEY` and `JWT_SECRET` constants. Currently there is a constant `STELLAR_PUBLICNET` that is set to `false` (for testnet) by default. You can set it to `true` for public net.
