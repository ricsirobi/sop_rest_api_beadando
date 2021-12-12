<?php
include("./database.php");

$request=$_SERVER["REQUEST_METHOD"];

switch($request){
    case "GET":
        if(!empty($_GET["list_all"]))
        {
            ListAll();
        }
        elseif(!empty($_GET["list_own"]))
        {
            ListOwn($_GET["list_own"]);
        }
        break;
    case "POST":
            if(!empty($_GET["add"]) && isUserLoggedIn($_GET["add"]))
            {
                $delete_vars=json_decode(file_get_contents('php://input'),true);
                $name = $delete_vars["Name"];
                $count = $delete_vars["Count"];
                $price = $delete_vars["Price"];
                Add($name,$count,$price,$_GET["add"]);
            }
            break;
    case "PUT":
            if(!empty($_GET["modify"]) && isUserLoggedIn($_GET["modify"]))
            {
                $modify_vars=json_decode(file_get_contents('php://input'),true);
                $id = $modify_vars['Id'];
                $name = $modify_vars['Name'];
                $count = $modify_vars['Count'];
                $price = $modify_vars['Price'];
                if(isUserLoggedIn($id))
                {
                    Modify($id,$name,$count,$price);
                }
            }
            break;
    case "DELETE":
            if(!empty($_GET["delete"]))
            {
                $delete_vars=json_decode(file_get_contents('php://input'),true);
                $id = $delete_vars['Id'];
                if(isUserLoggedIn($id))
                {
                    Delete($_GET["delete"]);
                }
            }
            break;
        default:
		header('HTTP/1.1 405 Method Not Allowed');
		header('Allow: GET, POST, PUT, DELETE');
		break;
}

function ListAll()
{
    $query="SELECT * FROM goods";

    $result=classList($query);
    echo json_encode($result);
}

function Delete($id)
{
    $query = "DELETE FROM goods WHERE id=".$id;
    executeQuery($query);
    $response = array(
        'status' => 1,
        'status_message' => 'Delete successful!',
    );
    echo json_encode($response);
}

function Add($name,$count,$price,$id)
{
    $query="INSERT INTO goods (name,count,price) VALUES ('".$name."',".$count.",".$price.");";
    executeQuery($query);
    $query="INSERT INTO users_goods (uid,gid) VALUES (".$id.",(SELECT id FROM goods WHERE name='".$name."' AND count=".$count." AND price=".$price." ORDER BY id DESC LIMIT 1))";
    executeQuery($query);
    $response = array(
        'status' => 1,
        'status_message' => 'POST successful!',
    );
    echo json_encode($response);
}

function Modify($id,$name,$count,$price)
{
    $query="UPDATE goods SET name='".$name."', count=".$count.", price=".$price." WHERE id=".$id.";";
    executeQuery($query);
    $response = array(
        'status' => 1,
        'status_message' => 'PUT successful!',
    );
    echo json_encode($response);
}

function ListOwn($id)
{
    $query="SELECT id,name,count,price FROM goods, (SELECT gid FROM users_goods WHERE uid=".$id.") switch WHERE id=switch.gid;";

    $result=classList($query);
    echo json_encode($result);
}
?>