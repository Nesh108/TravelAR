<?php

$config = require('./config.php');

// Create connection
$conn = new mysqli($config['servername'], $config['username'], $config['password'], $config['dbname']);

// Check connection
if ($conn->connect_error) {
    die("Connection failed: " . $conn->connect_error);
}

$sql = "SELECT id, parent, marker, type, username, content, position, date_dropped FROM drops";
$result = $conn->query($sql);
$drops = [];
if ($result->num_rows > 0) {
    // output data of each row
    while($row = $result->fetch_assoc()) {
        $drops[] = [
            "id" => $row["id"],
            "parent" => $row["parent"],
            "marker" => $row["marker"],
            "type" => $row["type"],
            "username" => $row["username"],
            "content" => $row["content"],
            "position" => $row["position"],
            "date_dropped" => $row["date_dropped"],
        ];
    }
}

echo(json_encode($drops));
$conn->close();

?>