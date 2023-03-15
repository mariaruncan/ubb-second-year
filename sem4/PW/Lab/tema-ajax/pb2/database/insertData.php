<?php
$conn = new mysqli("localhost", "root", "", "MyDatabase");

// Check connection
if ($conn->connect_error) {
die("Connection failed: " . $conn->connect_error);
}

// prepare and bind
$stmt = $conn->prepare("INSERT INTO users (firstname, lastname, phoneNumber, email) VALUES (?, ?, ?, ?)");
$stmt->bind_param("ssss", $firstname, $lastname, $phoneNumber, $email);

// set parameters and execute
$firstname = "Maria";
$lastname = "Runcan";
$phoneNumber = "1234567890";
$email = "maria@example.com";
$stmt->execute();

$firstname = "Ion";
$lastname = "Pop";
$phoneNumber = "0987654321";
$email = "ion@example.com";
$stmt->execute();

$firstname = "Test";
$lastname = "Test";
$phoneNumber = "1234567890";
$email = "test@example.com";
$stmt->execute();

$firstname = "Ana";
$lastname = "Xulescu";
$phoneNumber = "0987654321";
$email = "ana@example.com";
$stmt->execute();

$firstname = "Andrei";
$lastname = "Popescu";
$phoneNumber = "1234567890";
$email = "andrei@example.com";
$stmt->execute();

$firstname = "George";
$lastname = "George";
$phoneNumber = "0987654321";
$email = "george@example.com";
$stmt->execute();

$firstname = "Alina";
$lastname = "Alina";
$phoneNumber = "1234567890";
$email = "alina@example.com";
$stmt->execute();


echo "New records created successfully";

$stmt->close();
$conn->close();
