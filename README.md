# Flappy DApp Example Repo
This repository is dedicated to providing example API's using the different Mobius SDKs. The `backend` directory contains different API examples while the frontend of the Flappy Bird DApp is located in the `frontend` directory. Full documentation on this process can be found by choosing any example from the official <a href="https://docs.mobius.network/docs" target="_blank">Mobius Docs</a>.

## Getting Started
Clone this repository:
```console
$ git clone https://github.com/mobius-network/flappy-dapp.git
```

## Backend Configuration
Choose an API example you would like to run and navigate to that folder. Within each example, there are specific documentation instructions for setting the initial configuration.

### Serverless Quickstart

Run an API example using Node.js, the `mobius-client-js` SDK with the ability to easily deploy using Webtask.io in minutes. Navigate to the `serverless-quickstart` directory to run this API. For full documentation on this example, visit the official <a href="https://docs.mobius.network/docs/serverless-quickstart-api" target="_blank">Serverless Quickstart API docs</a>.

```console
$ cd backend/serverless-quickstart
```

### Node.js API

Run an API example using Node.js and the `mobius-client-js` SDK. Navigate to the `nodejs-api` directory to run this API. For full documentation on this example, visit the official <a href="https://docs.mobius.network/docs/nodejs-api" target="_blank">Node.js API docs</a>.

```console
$ cd backend/nodejs-api
```

### Python Flask API

Run an API example using Python and the `mobius-client-python` SDK. Navigate to the `python-flask-api` directory to run this API. For full documentation on this example, visit the official <a href="https://docs.mobius.network/docs/python-flask-api" target="_blank">Python Flask API docs</a>.

```console
$ cd backend/python-flask-api
```

### PHP API

Run an API example using PHP and the `mobius-client-php` SDK. Navigate to the `php-api` directory to run this API. For full documentation on this example, visit the official <a href="https://docs.mobius.network/docs/php-api" target="_blank">PHP API docs</a>.

```console
$ cd backend/php-api
```

### .NET Core API

Run an API example using .NET Core and the `mobius-client-dotnet` SDK. Navigate to the `dotnet-core-api` directory to run this API. For full documentation on this example, visit the official <a href="https://docs.mobius.network/docs/dotnet-core-api" target="_blank">.NET Core docs</a>.

```console
$ cd backend/php-api
```

## Frontend Configuration
The frontend of the application only needs minor setup and configuration to run. Full documentation on this process can be found by choosing any example from the official <a href="https://docs.mobius.network/docs" target="_blank">Mobius Docs</a>.

Navigate to `frontend`
```console
$ cd frontend
```

Install Dependencies
```console
$ yarn install
```

Set Endpoint in `main.js` 
```javascript
// The port may vary depending on what the local API is running on.
const DAPP_API = 'http://localhost:8080/api';
```

Run localhost Server
```console
$ yarn dev
```