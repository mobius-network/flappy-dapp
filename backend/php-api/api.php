<?php
include_once 'config.php';

$url = (isset($_SERVER['HTTPS']) && $_SERVER['HTTPS'] === 'on' ? "https" : "http") . "://$_SERVER[HTTP_HOST]$_SERVER[REQUEST_URI]";
$endpoint = parse_url($url, PHP_URL_PATH);
if($endpoint == '/auth') {
    if($_SERVER['REQUEST_METHOD'] == 'GET'){
        try{
            $expire_in = 86400;
            $ch = Mobius\Client\Auth\Challenge::generate_challenge(SECRET_KEY, $expire_in);
            echo $ch;
        }catch(\Exception $e){
            echo $e->getMessage();
        }
        die;
    }
    else if($_SERVER['REQUEST_METHOD'] == 'POST') {
        try{
            $xdr = base64_decode($_REQUEST['xdr']);
            $public_key = $_REQEST['public_key'];   
            
            $token = new Mobius\Client\Auth\Token(SECRET_KEY, $xdr, $public_key);
            $token->validate();

            $jwt_token = new Mobius\Client\Auth\JWT(JWT_SECRET);
            echo $jwt_token->encode($token);
        }catch(\Exception $e){
            echo $e->getMessage();
        }
        die;
    }
}
else if($endpoint == '/balance' && $_SERVER['REQUEST_METHOD'] == 'GET'){
	try{
        $public_key = $_REQUEST['public_key'];
        $app = new Mobius\Client\App(SECRET_KEY, $public_key);
        echo json_encode( array( 'balance' => $app->balance()));
    }catch(\Exception $e){
        echo $e->getMessage();
    }
    die;
}
else if($endpoint == '/charge' && $_SERVER['REQUEST_METHOD'] == 'POST'){
	try{
        if(isset($_REQUEST['amount'])){
            $amount = $_REQUEST['amount'];
        }
        else{
            $amount = 1;
        }
        $public_key = $_REQUEST['public_key'];
        $app = new Mobius\Client\App(SECRET_KEY, $public_key);
        $response = $app->charge($amount);
        echo json_encode( array(
                'status'    => 'ok',
                'tx_hash'   => $response->getRawData()['hash'],
                'balance' => $app->balance()
            )
        );
    }catch(\Exception $e){
        echo $e->getMessage();
    }
    die;
}
else if($endpoint == '/payout' && $_SERVER['REQUEST_METHOD'] == 'POST'){
	try{
        if(!isset($_REQUEST['amount'])){
            echo 'Invalid Amount!';
            die;
        }
        if(!isset($_REQUEST['target_address'])){
            echo 'Invalid Target Address!';
            die;
        }
        
        $amount = $_REQUEST['amount'];
        $target_address = $_REQUEST['target_address'];
        $public_key = $_REQUEST['public_key'];
        
        $app = new Mobius\Client\App(SECRET_KEY, $public_key);
        $response = $app->payout($amount, $target_address);
        echo json_encode( array(
                'status'    => 'ok',
                'tx_hash'   => $response->getRawData()['hash'],
                'balance' => $app->balance()
            )
        );
    }catch(\Exception $e){
        echo $e->getMessage();
    }
    die;
}
else if($endpoint == '/transfer' && $_SERVER['REQUEST_METHOD'] == 'POST'){
	try{
        if(!isset($_REQUEST['amount'])){
            echo 'Invalid Amount!';
            die;
        }
        if(!isset($_REQUEST['target_address'])){
            echo 'Invalid Target Address!';
            die;
        }
        
        $amount = $_REQUEST['amount'];
        $target_address = $_REQUEST['target_address'];
        $public_key = $_REQUEST['public_key'];
        
        $app = new Mobius\Client\App(SECRET_KEY, $public_key);
        $response = $app->transfer($amount, $target_address);
        echo json_encode( array(
                'status'    => 'ok',
                'tx_hash'   => $response->getRawData()['hash'],
                'balance' => $app->balance()
            )
        );
    }catch(\Exception $e){
        echo $e->getMessage();
    }
    die;
}