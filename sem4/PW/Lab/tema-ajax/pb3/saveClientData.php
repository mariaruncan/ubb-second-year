<?php
$conn = new mysqli("localhost", "root", "", "MyDatabase");

if ($conn->connect_error) {
    die("Connection failed: " . $conn->connect_error);
}

$stmt = $conn->prepare("UPDATE clients SET 
            firstname = ?, lastname = ?, 
            phoneNumber = ?, email = ?
            WHERE id = ?;");
$stmt -> bind_param("ssssi", $_GET["firstname"], $_GET["lastname"], $_GET["phoneNumber"],
    $_GET["email"], $_GET["clientId"]);
$stmt -> execute();

echo "ok";

$stmt->close();
$conn->close();
