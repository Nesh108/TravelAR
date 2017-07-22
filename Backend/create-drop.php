<?php

$config = require('./config.php');

$marker = $_GET['marker'];
$type = $_GET['type'];
$username = $_GET['username'];
$content = $_GET['content'];
$position = $_GET['position'];

$errors = [];

if(!isset($marker)) {
	$errors[] = 'Marker is required. Marker should be an integer.';
}

if(!isset($type)) {
	$errors[] = 'Type is required. Type should be a string.';
}

if(!isset($username)) {
	$errors[] = 'Username is required. Username should be a string.';
}

if(!isset($content)) {
	$errors[] = 'Content is required.';
}

if(!isset($position)) {
	$errors[] = 'Position is required. Position should be a string.';
}


if(sizeof($errors) > 0) {
	echo json_encode($errors);

} else {

	// Create connection
	$conn = new mysqli($config['servername'], $config['username'], $config['password'], $config['dbname']);

	// Check connection
	if ($conn->connect_error) {
	    die("Connection failed: " . $conn->connect_error);
	}

	$sql = "INSERT INTO `drops` (marker, type, username, content, position) VALUES ('" 
	. $marker . "', '"
	. $type . "', '"
	. $username . "', '"
	. $content . "', '"
	. $position . "');";

	if ($conn->query($sql) === TRUE) {
	    echo "A drop was dropped!";

	} else {
	    echo "Error during drop: " . $conn->error;
	}
	$conn->close();
}

?>