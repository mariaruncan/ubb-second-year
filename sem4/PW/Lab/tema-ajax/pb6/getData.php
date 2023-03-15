<?php
$conn = new mysqli("localhost", "root", "", "MyDatabase");

if ($conn->connect_error) {
    die("Connection failed: " . $conn->connect_error);
}

$stmt = $conn->prepare("SELECT id, producer, model, RAM_memory, internal_memory, os FROM phones;");
$stmt->execute();
$stmt->bind_result($id, $producer, $model, $ram, $memory, $os);

$phones = array();

while ($stmt->fetch()) {
    if(($producer == $_GET["producer"] || $_GET["producer"] == "null") &&
        ($model == $_GET["model"] || $_GET["model"] == "null") &&
        ($ram == $_GET["ram"] || $_GET["ram"] == "null") &&
        ($memory == $_GET["memory"] || $_GET["memory"] == "null") &&
        ($os == $_GET["os"] || $_GET["os"] == "null"))
        $phones[] = array("id" => $id, "producer" => $producer, "model" => $model, "ram" => $ram, "memory" => $memory,
            "os" => $os);
}

echo json_encode($phones);

$stmt->close();
$conn->close();
