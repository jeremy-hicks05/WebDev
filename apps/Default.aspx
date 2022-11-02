<%@ Page Language="C#" CodeFile="~/apps/Default.aspx.cs" Inherits="MTAIntranet.Default" AutoEventWireup="True" %>

<Doctype html>
<html>
<head>
    <title>MTA Flint Intranet</title>
	<link rel="stylesheet" href="../style/style.css" />
</head>
<body>
	<div id="frame">
        <nav>
            <h1>MTA Intranet</h1>
            <menu class="frame">
                <li><a href="/index.html">Home</a></li>
                <!--<li><a href="pcissues.html">Link to Common PC Issues...</a></li>-->
                <li>
                    <a href="/apps/Default.aspx">Apps</a>
                    <ul class="frame">
                        <li><a href="/apps/pulloffs/AddPulloff.aspx">Add Pulloff</a></li>
                        <li><a href="/apps/pulloffs/ViewPulloffs.aspx">View Pulloffs</a></li>
                        <li><a href="/apps/masterroute/AddMRTemplate.aspx">Add MasterRoute</a></li>
                        <li><a href="/apps/masterroute/DelMRTemplate.aspx">Delete MasterRoute</a></li>
                    </ul>
                </li>
            </menu>
        </nav>
    </div>
	<div id="main">
		<h1>MTA Apps</h1>
		<a href="pulloffs/ViewPulloffs.aspx">View Pulloffs</a> <br />
		<a href="pulloffs/AddPulloff.aspx">Add Pulloff</a> <br />
        <a href="masterroute/AddMRTemplate.aspx">Add MasterRoute</a> <br />
		<a href="masterroute/DelMRTemplate.aspx">Delete MasterRoute</a> <br />
		<a href="fuelmaster/fuelmaster.html">Fuelmaster apps</a>
	</div>
</body>
</html>