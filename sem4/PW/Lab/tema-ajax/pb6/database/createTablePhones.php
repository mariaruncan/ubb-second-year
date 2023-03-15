<?php
// Create connection
$conn = new mysqli("localhost", "root", "", "MyDatabase");
// Check connection
if ($conn->connect_error) {
    die("Connection failed: " . $conn->connect_error);
}

// sql to create table
$sql = "CREATE TABLE phones
        (
        id INT(6) UNSIGNED AUTO_INCREMENT PRIMARY KEY,
        producer VARCHAR(20),
        model VARCHAR(20),
        RAM_memory VARCHAR(10),
        intern_memory VARCHAR(10),
        os VARCHAR(10)
        )";

if ($conn->query($sql) === TRUE) {
    echo "Table Phones created successfully";
} else {
    echo "Error creating table: " . $conn->error;
}

$conn->close();
