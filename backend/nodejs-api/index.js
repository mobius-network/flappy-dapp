const express = require('express');
const StellarSdk = require("stellar-sdk");
const Mobius = require("@mobius-network/mobius-client-js");
const dotenv = require('dotenv').config();

const mobius = new Mobius.Client();
const app = express();

app.enable('strict routing');

app.use(express.json());
app.use(express.urlencoded({ extended: true }));

app.use((req, res, next) => {
	const network = process.env.NETWORK;
  req.pubnet = network === 'public';
  mobius.network = req.pubnet ? StellarSdk.Networks.PUBLIC : StellarSdk.Networks.TESTNET;
  next();
});

app.use("/api", require("./api"));
app.use("/auth", require("./auth"));

const PORT = process.env.PORT || 3000;
app.listen(PORT, () => console.log(`Listening on port ${PORT}...`));