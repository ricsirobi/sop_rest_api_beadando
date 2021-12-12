<?php
class dbObj{
function getConnection()
{
    $server = 'localhost';
    $username = 'root';
    $password = '';
    $database = 'sop';
    $connection = mysqli_connect($server, $username, $password, $database);
    return $connection;
}

function executeQuery($query)
{
    $connection = getConnection();
    $statement = mysqli_prepare($connection, $query);
    $success = mysqli_stmt_execute($statement);
    if ($success) {
        echo "siker";
    } else {

        echo "fail: " . $query;
    }
    mysqli_stmt_close($statement);
    mysqli_close($connection);
}

function classList($query)
{
    try {
        $connection = getConnection();
        $statement = mysqli_prepare($connection, $query);
        $success = mysqli_stmt_execute($statement);
        $result = [];
        echo "<div hidden>" . $query . "</div>";
        if ($success === TRUE) {
            $idk = $statement->get_result();
            while ($temp = $idk->fetch_assoc()) {
                $result[] = mb_convert_encoding($temp, "utf8");
            }
        } else {
            echo $query;
            die('Sikertelen végrehajtás');
        }
        mysqli_stmt_close($statement);
        mysqli_close($connection);
        $result = mb_convert_encoding($result, "utf8");
        return $result;
    } catch (Exception $e) {
        $connection = getConnection();

        $statement = mysqli_prepare($connection, $query);
        $statement = mysqli_query($connection, $query);
        $result = [];
        echo "<div hidden>" . $query . "</div>";
        while ($temp = $statement->fetch_assoc()) {
            $result[] =  mb_convert_encoding($temp, "utf8");
        }
        mysqli_close($connection);
        return $result;
    }
}
function isUserLoggedIn()
{
    return $_SESSION  != null && array_key_exists('uid', $_SESSION) && is_numeric($_SESSION['uid']);
}
function userLogout()
{
    session_unset();
    session_destroy();
    header('Location: index.php?P=home');
}
}
