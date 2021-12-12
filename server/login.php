<?php

use MongoDB\Driver\Query;

include("database.php");
$db = new dbObj();
$connection =  $db->getConnection();
$request_method = $_SERVER["REQUEST_METHOD"];

switch ($request_method) {
    case 'POST':
        if (isset($_GET["login"]))
            login();
        else if (isset($_GET["logout"]))
            logout($_GET["logout"]);

        break;
}
function login()
{
    $username = $_GET["u"];
    $password = $_GET["p"];
    $query = "select * from users where username like \"$username\" and password like \"$password\"";
    global $connection;
    if (mysqli_query($connection, $query)) {
        $row = mysqli_fetch_array(mysqli_query($connection, $query));
        if ($row != null) {

            $token = date("h/d/m") . $username . "" . date("Y/m/d") . "|" . $row["isAdmin"];
            mysqli_query($connection, "iNSERT INTO tok(value) VALUES (\"$token\")");
            $response = array(
                'status' => 1,
                'status_message' => "Logged in as $username",
                'Token' => $token,
                'Id' => $row["id"]
            );
        } else {
            $response = array(
                'status' => 0,
                'status_message' => "Login failed"
            );
        }
    } else {
        $response = array(
            'status' => 0,
            'status_message' => 'Login failed.'
        );
    }
    header('Content-Type: application/json');
    echo json_encode($response);
}
function logout($token)
{
    global $connection;
    mysqli_query($connection, "delete from tok where value like \"$token\"");
    echo "logout from $token";
}
