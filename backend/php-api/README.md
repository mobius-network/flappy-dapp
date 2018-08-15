 # Mobius PHP API

This repo provides a default implementation of a Mobius DApp using a Python Flask API. It exposes some generic API endpoints like `/api/balance` and `/api/charge`. For more detailed documentation on this example, see the official <a href="https://docs.mobius.network/docs/php-api" target="_blank">Mobius Docs</a>.

## Install

```console
$ cd backend/php-api
$ composer install
```

## Set ENV Variables

A dev-wallet is needed to set `APP_KEY` environment variable. If you do not have a dev-wallet see <a href="https://docs.mobius.network/docs/installation#section-generating-key-pairs" target="_blank">Generating Key Pairs</a> in Mobius Docs.

```php
// Open config.php and set the APP_KEY
define('APP_KEY', 'YOUR-SECRET-KEY');
```

## Run

```console
# Run locally
$ php -S localhost:8080
```