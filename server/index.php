<?php

use MongoDB\Driver\Query;

include("database.php");
$db = new dbObj();
$connection =  $db->getConnection();
$request_method = $_SERVER["REQUEST_METHOD"];
switch ($request_method) {
  case 'GET':
    if (!empty($_GET["userid"])) {
      getMyAllLego($_GET["userid"]);
    } else if (!empty($_GET["category"])) {
      if ($_GET["category"] == -1) {
        getAllCategory();
      } else {
        $category = intval($_GET["category"]);
        get_categoryWithId($category);
      }
    } else {
      if (!empty($_GET["code"])) {
        $code = intval($_GET["code"]);
        get_legoWithCode($code);
      } else {
        get_allLego();
      }
    }
    break;
  case 'POST':
    if (isset($_GET["tok"]) && isset($_GET["userid"]) && isset($_GET["legoid"])) {
      insertToMyLego($_GET["userid"], $_GET["legoid"]);
    } else if (isset($_GET["tok"])) {
      $token = $_GET["tok"];
      if (gotAdminAccess($token)) {
        insert_lego();
      } else if (gotUserAccess($token)) {
        echo "csak user vagy";
      } else {
        echo "Nincs jogosultság";
      }
    } else {
      echo "bejelentkezés szükséges";
    }
    break;

  case 'PUT':
    $token = $_GET["tok"];
    if (gotAdminAccess($token)) {
      $post_vars = json_decode(file_get_contents("php://input"), true);
      $id = $post_vars["Id"];
      update_lego($id);
    } else {
      echo "Admin jogosultság szükséges";
    }
    break;

  case 'DELETE':
    $token = $_GET["tok"];
    if (gotAdminAccess($token) && isset($_GET["code"])) {
      $code = intval($_GET["code"]);
      delete_lego($code);
    } else if (isset($_GET["tok"]) && isset($_GET["userid"]) && isset($_GET["legoid"])) {
      deleteFromMyLego($_GET["userid"], $_GET["legoid"]);
    }
    break;
  default:
    header("HTTP/1.0 405 Method Not Allowed");
    break;
}

function getAllCategory()
{
  global $connection;
  $query = "SELECT * FROM category";
  $response = array();
  $result = mysqli_query($connection, $query);
  while ($row = mysqli_fetch_array($result)) {
    $response[] = $row;
  }
  header('Content-Type: application/json');
  echo json_encode($response);
}

function get_categoryWithId($k)
{
  global $connection;
  $query = "SELECT * FROM lego where category = $k";
  $response = array();
  $result = mysqli_query($connection, $query);
  while ($row = mysqli_fetch_array($result)) {
    $response[] = $row;
  }
  header('Content-Type: application/json');
  echo json_encode($response);
}
function get_allLego()
{
  global $connection;
  $query = "SELECT * FROM lego";
  $response = array();
  $result = mysqli_query($connection, $query);
  while ($row = mysqli_fetch_array($result)) {
    $response[] = $row;
  }
  header('Content-Type: application/json');
  echo json_encode($response);
}

function get_legoWithCode($code = 0)
{
  global $connection;
  $query = "SELECT * FROM lego";
  if ($code != 0) {
    $query .= " WHERE code like \"" . $code . "\" LIMIT 1";
  }
  $response = array();
  $result = mysqli_query($connection, $query);
  $response[] = mysqli_fetch_array($result); //csak egy result lesz vagy annyit szabad figyelembe vennünk
  header('Content-Type: application/json');
  echo json_encode($response);
}

function insert_lego()
{
  global $connection;

  $data = json_decode(file_get_contents('php://input'), true); //kliens által body-ba fűzött adatok megszerzése
  $code = $data["Code"];
  $name = $data["Name"];
  $category = $data["Category"];
  $price = $data["HUFprice"];

  echo $query = "INSERT INTO lego SET code='" . $code . "', name='" . $name . "', category='" . $category . "', HUFprice='" . $price . "'";
  if (mysqli_query($connection, $query)) {
    $response = array(
      'status' => 1,
      'status_message' => 'Lego Added Successfully.'
    );
  } else {
    $response = array(
      'status' => 0,
      'status_message' => 'Lego Addition Failed.'
    );
  }
  header('Content-Type: application/json');
  echo json_encode($response);
}
function delete_lego($code)
{
  global $connection;

  $query = "DELETE FROM lego WHERE code like\"" . $code . "\"";
  if (mysqli_query($connection, $query)) {
    $response = array(
      'status' => 1,
      'status_message' => 'Lego Deleted Successfully. ' . $code
    );
  } else {
    $response = array(
      'status' => 0,
      'status_message' => 'Lego Deletion Failed.'
    );
  }
  header('Content-Type: application/json');
  echo json_encode($response);
}

function update_lego($id)
{
  global $connection;
  $post_vars = json_decode(file_get_contents("php://input"), true);
  $code = $post_vars["Code"];
  $name = $post_vars["Name"];
  $price = $post_vars["HUFprice"];
  $category = $post_vars["Category"];

  $query = "uPDATE lego SET name='" . $name . "', HUFprice='" . $price . "', code='" . $code . "', category='" . $category . "' WHERE id=" . $id;
  echo $query;
  if (mysqli_query($connection, $query)) {
    $response = array(
      'status' => 1,
      'status_message' => 'Lego Updated Successfully.'
    );
  } else {
    $response = array(
      'status' => 0,
      'status_message' => 'Lego Updation Failed.'
    );
  }
  header('Content-Type: application/json');
  echo json_encode($response);
}

function gotUserAccess($token)
{
  global $connection;
  $query = "select * from tok where value like \"$token\"";
  if (mysqli_query($connection, $query)) {
    $row = mysqli_fetch_array(mysqli_query($connection, $query));
    if ($row == null) {
      return false;
    } else {
      return true;
    }
  }
}
function gotAdminAccess($token)
{
  if (explode("|", $token)[1] == 1) {
    global $connection;
    $query = "select * from tok where value like \"$token\"";
    if (mysqli_query($connection, $query)) {
      $row = mysqli_fetch_array(mysqli_query($connection, $query));
      if ($row == null) {
        return false;
      } else {
        return true;
      }
    }
  }
}
function getMyAllLego($uid)
{
  $query = "select lego.id, code, name, category, HUFprice from lego, user_lego where uid = $uid and lid = lego.id group by lid";
  global $connection;
  $response = array();
  $result = mysqli_query($connection, $query);
  while ($row = mysqli_fetch_array($result)) {
    $response[] = $row;
  }
  header('Content-Type: application/json');
  echo json_encode($response);
}
function insertToMyLego($uid, $legoid)
{
  global $connection;
  $query = "iNSERT INTO user_lego( uid, lid) VALUES ($uid,$legoid)";
  $result = mysqli_query($connection, $query);
  echo "Successfully added";
}
function deleteFromMyLego($uid, $legoid)
{
  global $connection;
  $query = "delete from user_lego where  uid=$uid and lid=$legoid";
  $result = mysqli_query($connection, $query);
  echo "Successfully deleted";
}
