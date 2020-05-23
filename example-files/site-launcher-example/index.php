<?php 

	$autoUSER = "test" ; // Username to replace info with
	$autoPASS = "test" ; // Password to replace info with, leave blank if you don't want to send password data over base64 encoding
	$serverHost = "10.0.1.55" ; // Optional server name, the launcher uses this name if no custom "configName" is defined in the .aelcf file data
	// Load the base config file into memory
	$launchFileString = file_get_contents('startgame.aelcf');
	// Update the username and password fields (or other things you want here)
	$launchFileString = str_replace("{{USER}}",$autoUSER,$launchFileString);
	$launchFileString = str_replace("{{PASS}}",$autoPASS,$launchFileString);
	$launchFileString = str_replace("{{HOST}}",$serverHost,$launchFileString);
	$launchFileQueryString = "?v=c"	;
	
?><!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <title>AAEmu Test</title>
</head>
<body>
<h1 align="center">Logged in as <?php echo $autoUSER ?><br></h1>
<hr>
<h1 align="center"><a href="aelcf://<?php echo  $serverHost ; ?>/<?php echo base64_encode($launchFileString); echo $launchFileQueryString ; ?>">Start Game</a><br></h1>
<h1 align="center"><a href="register/">Register New User</a><br></h1>
</body>
</html>