const express = require('express');
const cors = require('cors');
const cookieParser = require('cookie-parser');
const jwt = require('jsonwebtoken');
const stellarSdk = require('stellar-sdk');
const Mobius = require("@mobius-network/mobius-client-js");

const authApp = express();
module.exports = authApp;

const corsOptions = (req, callback) => {
  callback(null, {
    origin: req.pubnet ? req.webtaskContext.meta.APP_STORE : true,
  });
};

authApp.use(cors(corsOptions));
authApp.use(cookieParser());

authApp.get("/", (req, res) => {
  const { APP_KEY } = req.webtaskContext.secrets;
  res.send(Mobius.Auth.Challenge.call(APP_KEY));
});

authApp.post("/", (req, res) => {
  const { APP_KEY } = req.webtaskContext.secrets;
  const { APP_DOMAIN } = req.webtaskContext.meta;

  try {
    const token = new Mobius.Auth.Token(
      APP_KEY,
      req.body.xdr || req.query.xdr,
      req.body.public_key || req.query.public_key
    );
    token.validate();

    const payload = {
      sub: token._address,
      jti: token.hash("hex").toString(),
      iss: 'https://' + APP_DOMAIN + '/',
      iat: parseInt(token.timeBounds.minTime, 10),
      exp: parseInt(token.timeBounds.maxTime, 10),
    };

    const signed_payload = jwt.sign(payload, APP_KEY);
    const app_public_key = stellarSdk.Keypair.fromSecret(APP_KEY).publicKey();

    // You might use cookie to authorise
    res.cookie("MOBIUS_DAPP_" + app_public_key, signed_payload);
    res.send(signed_payload);
  } catch (error) {
    res.status(401).json({ error: error.message });
  }
});
