const express = require('express');
const expressJwt = require('express-jwt');
const cors = require('cors');

const Mobius = require("@mobius-network/mobius-client-js");

const apiApp = express();
module.exports = apiApp;

const corsOptions = (req, callback) => {
  callback(null, {
    origin: req.pubnet ? req.webtaskContext.meta.APP_DOMAIN : true,
  });
};

apiApp.use(cors(corsOptions));

apiApp.use((req, res, next) => {
  const { APP_KEY } = req.webtaskContext.secrets;
  const { APP_DOMAIN } = req.webtaskContext.meta;

  expressJwt({
    secret: APP_KEY,
    issuer: `https://${APP_DOMAIN}/`,
    algorithms: ['HS256'],
    getToken
  })(req, res, next);
});

apiApp.get("/test", (req, res) => {
  console.log("User: ", req.user.sub);
  res.json({ user: req.user });
});

apiApp.get("/balance", async (req, res, next) => {
  try {
    const { APP_KEY } = req.webtaskContext.secrets;
    const dapp = await Mobius.AppBuilder.build(APP_KEY, req.user.sub);

    res.json({balance: dapp.userBalance});
  } catch (e) {
    next(e);
  }
});

apiApp.post("/charge", async (req, res, next) => {
  try {
    const { APP_KEY } = req.webtaskContext.secrets;
    const dapp = await Mobius.AppBuilder.build(APP_KEY, req.user.sub);

    const { amount, target_address } = req.body;

    if (amount === null || isNaN(Number(amount))) {
      return res.status(400).json({ error: "Invalid amount" })
    }

    const response = await dapp.charge(amount, target_address);
    res.json({
      status: "ok",
      tx_hash: response.hash,
      balance: dapp.userBalance
    });
  } catch (e) {
    next(e);
  }
});

apiApp.post("/payout", async (req, res, next) => {
  try {
    const { APP_KEY } = req.webtaskContext.secrets;
    const dapp = await Mobius.AppBuilder.build(APP_KEY, req.user.sub);

    const { amount, target_address } = req.body;

    if (!amount || isNaN(Number(amount))) {
      return res.status(400).json({ error: "Invalid amount" })
    }

    const response = await dapp.payout(amount, target_address);
    res.json({
      status: "ok",
      tx_hash: response.hash,
      balance: dapp.userBalance
    });
  } catch (e) {
    next(e);
  }
});

apiApp.post("/transfer", async (req, res, next) => {
  try {
    const { APP_KEY } = req.webtaskContext.secrets;
    const dapp = await Mobius.AppBuilder.build(APP_KEY, req.user.sub);

    const { amount, target_address } = req.body;

    if (!amount || isNaN(Number(amount))) {
      return res.status(400).json({ error: "Invalid amount" })
    }

    if (!target_address) {
      return res.status(400).json({ error: "Missing target_address parameter" })
    }

    const response = await dapp.transfer(amount, target_address);
    res.json({
      status: "ok",
      tx_hash: response.hash,
      balance: dapp.userBalance
    });
  } catch (e) {
    next(e);
  }
});

apiApp.use(logErrors);
apiApp.use(errorHandler);

function getToken(req) {
  if (req.headers.authorization && req.headers.authorization.split(' ')[0] === 'Bearer') {
    return req.headers.authorization.split(' ')[1];
  } else if (req.query && req.query.token) {
    return req.query.token;
  }
  return null;
}

function logErrors(err, req, res, next) {
  console.error(err);
  next(err);
}

function errorHandler(err, req, res, next) {
  res.status(500).json({ error: "Internal server error" });
}
