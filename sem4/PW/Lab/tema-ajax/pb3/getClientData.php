<?php
$conn = new mysqli("localhost", "root", "", "MyDatabase");

if ($conn->connect_error) {
    die("Connection failed: " . $conn->connect_error);
}

$id = $_GET["clientId"];

$stmt = $conn->prepare("SELECT firstname, lastname, phoneNumber, email FROM clients WHERE id = ?;");
$stmt->bind_param("i", $id);
$stmt->execute();
$stmt->bind_result($firstname, $lastname, $phoneNumber, $email);

if($stmt->fetch()) {
    $user = array("firstname" => $firstname, "lastname" => $lastname, "phoneNumber" => $phoneNumber, "email" => $email);
    echo json_encode($user);
}
else
    echo "error";

$stmt->close();
$conn->close();
