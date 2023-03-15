<?php
$conn = new mysqli("localhost", "root", "", "MyDatabase");

if ($conn->connect_error) {
    die("Connection failed: " . $conn->connect_error);
}

$stmt = $conn->prepare("SELECT id FROM clients;");
$stmt->execute();
$stmt->bind_result($result);

$ids = array();

while ($stmt->fetch()) {
    $ids[] = $result;
}

echo json_encode($ids);

$stmt->close();
$conn->close();
