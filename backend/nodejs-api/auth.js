const express = require('express');
const cors = require('cors');
const jwt = require('jsonwebtoken');
const Mobius = require("@mobius-network/mobius-client-js");

const authApp = express();
module.exports = authApp;

const corsOptions = (req, callback) => {
  callback(null, {
    origin: req.pubnet ? process.env.APP_STORE : true,
  });
};

authApp.use(cors(corsOptions));

authApp.get("/", (req, res) => {
  const APP_KEY = process.env.APP_KEY;
  console.log(APP_KEY)
  res.send(Mobius.Auth.Challenge.call(APP_KEY));
});

authApp.post("/", (req, res) => {
  const APP_KEY = process.env.APP_KEY;
  const APP_DOMAIN = process.env.APP_DOMAIN;

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

    res.send(jwt.sign(payload, APP_KEY));
  } catch (error) {
    res.status(401).json({ error: error.message });
  }
});