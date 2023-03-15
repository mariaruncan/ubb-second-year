<?php
$filename = $_GET["file"];

if(is_dir($filename)) { // is directory
    $files = scandir($filename, SCANDIR_SORT_NONE);
    $result = array("type" => "directory", "content" => array_slice($files, 2, count($files) - 2));
    echo json_encode($result);
}

else if(is_file($filename)){ // is file
    echo json_encode(array("type" => "file", "content" => file_get_contents($filename)));
}

else {
    echo json_encode(array("type" => "error", "content" => "Request can not be completed!"));
}


