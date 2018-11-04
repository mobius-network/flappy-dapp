# Mobius C# .NET Core API 

This repo provides a default implementation of a Mobius DApp using a C# .NET Core API. It exposes some generic
API endpoints like `/api/balance` and `/api/charge`. For more detailed
documentation on this example, see the official <a href="https://docs.mobius.network/docs/dotnet-core-api" target="_blank">Mobius Docs</a>.

## Install

```console
$ cd backend/dotnet-core-api
```

## Set ENV Variables

Using the `appsettings.Development.json` file, populate `APP_KEY` with your secret seed from your generated `dev-wallet.html`. If you do not have a 
dev-wallet see <a href="https://docs.mobius.network/docs/installation#section-generating-key-pairs" target="_blank">Generating Key Pairs</a>
in Mobius Docs.

```json
{
  "APP_KEY": "YOUR-SECRET-SEED"
}
```

## Run

```console
$ dotnet run
```