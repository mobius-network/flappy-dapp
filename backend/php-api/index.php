<?php
include_once 'config.php';
require __DIR__ . '/vendor/autoload.php';

$url = (isset($_SERVER['HTTPS']) && $_SERVER['HTTPS'] === 'on' ? "https" : "http") . "://$_SERVER[HTTP_HOST]$_SERVER[REQUEST_URI]";
$endpoint = parse_url($url, PHP_URL_PATH);

header("Access-Control-Allow-Origin: *");
header('Content-type:application/json;charset=utf-8'); 

if (!function_exists('getPublicKey')) {
    function getPublicKey($token) {
        $jwt = new Mobius\Client\Auth\JWT(APP_KEY);
        $jwt_token = $jwt->decode($token);
        return $jwt_token->public_key;
    }
}

if (!function_exists('getRestJSON')) {
    function getRestJSON() {
        $rest_json = file_get_contents("php://input");
        return json_decode($rest_json, true);
    }
}

if($endpoint == '/auth') {
    if($_SERVER['REQUEST_METHOD'] == 'GET'){
        try{
            $expire_in = 86400;
            $challenge = Mobius\Client\Auth\Challenge::generate_challenge(APP_KEY, $expire_in);
            echo $challenge;
        }catch(\Exception $e){
            echo $e->getMessage();
        }
        die;
    }
    else if($_SERVER['REQUEST_METHOD'] == 'POST') {
        try{
            $xdr = base64_decode($_REQUEST['xdr']);
            $public_key = $_REQUEST['public_key'];   
            
            $token = new Mobius\Client\Auth\Token(APP_KEY, $xdr, $public_key);
            $token->validate();
            $jwt_token = new Mobius\Client\Auth\JWT(APP_KEY);
            echo $jwt_token->encode($token);
        }catch(\Exception $e){
            echo $e->getMessage();
        }
        die;
    }
}
else if($endpoint == '/api/balance' && $_SERVER['REQUEST_METHOD'] == 'GET'){
	try{
        $public_key = getPublicKey($_GET['token']);
        $dapp = new Mobius\Client\App(APP_KEY, $public_key);

        echo json_encode( array( 'balance' => $dapp->user_balance()));
    }catch(\Exception $e){
        echo $e->getMessage();
    }
    die;
}
else if($endpoint == '/api/charge' && $_SERVER['REQUEST_METHOD'] == 'POST'){
	try{
        $public_key = getPublicKey($_GET['token']);

        if(isset($_REQUEST['amount'])){
            $amount = $_REQUEST['amount'];
        }
        else{
            $amount = 1;
        }

        $dapp = new Mobius\Client\App(APP_KEY, $public_key);
        $response = $dapp->charge($amount);
        echo json_encode( array(
                'status'    => 'ok',
                'tx_hash'   => $response->getRawData()['hash'],
                'balance' => $dapp->user_balance()
            )
        );
    }catch(\Exception $e){
        echo $e->getMessage();
    }
    die;
}
else if($endpoint == '/api/payout' && $_SERVER['REQUEST_METHOD'] == 'POST'){
	try{
        $request = getRestJSON();
        $amount = $request['amount'];
        $target_address = $request['target_address'];
        $public_key = getPublicKey($_GET['token']);

        if(!isset($amount)){
            echo 'Invalid Amount!';
            die;
        }
        if(!isset($target_address)){
            echo 'Invalid Target Address!';
            die;
        }
        
        $dapp = new Mobius\Client\App(APP_KEY, $public_key);
        $response = $dapp->payout($amount, $target_address);
        echo json_encode( array(
                'status'  => 'ok',
                'tx_hash' => $response->getRawData()['hash'],
                'balance' => $dapp->user_balance()
            )
        );
    }catch(\Exception $e){
        echo $e->getMessage();
    }
    die;
}
else if($endpoint == '/api/transfer' && $_SERVER['REQUEST_METHOD'] == 'POST'){
	try{
        $request = getRestJSON();
        $amount = $request['amount'];
        $target_address = $request['target_address'];
        $public_key = getPublicKey($_GET['token']);

        if(!isset($amount)){
            echo 'Invalid Amount!';
            die;
        }
        if(!isset($target_address)){
            echo 'Invalid Target Address!';
            die;
        }
        $dapp = new Mobius\Client\App(APP_KEY, $public_key);
        $response = $dapp->transfer($amount, $target_address);

        echo json_encode( array(
                'status'    => 'ok',
                'tx_hash'   => $response->getRawData()['hash'],
                'balance' => $dapp->user_balance()
            )
        );
    }catch(\Exception $e){
        echo $e->getMessage();
    }
    die;
}
else {
    echo $endpoint . ' Not Found';
    die;
}
