<%@ Page Language="C#" CodeFile="ViewFMTR.aspx.cs" Inherits="MTAIntranet.ViewFMTR" AutoEventWireup="True"%>

<Doctype html>
<html>
<head>
    <title>MTA Flint Intranet</title>
    <link rel="stylesheet" href="../../style/style.css" />
    <script src="../../script/utils.js"></script>
    <script src="../../script/fuelmaster/showfuelmastertr.js"></script>
</head>

<body>
    
    <h1>View Fuelmaster Transactions</h1>
    <nav>
        <a href="../../index.html">Home</a>
    </nav>
    <form id="formviewfmtr" runat="server">
    <span id="label1">--Veh ID</span><span id="label2">----Odometer</span>
        <div id="dropdowns">
    <span>
        <select id="fuelmastervehid">
        </select>
        <input type="text" id="fuelmastervehod"/>
    </span>
            
    <button type="button" id="showfuelmastertr">View Fuelmaster Transactions</button>
    </div>
    <div id="fmresults"></div>
    </form>
</body>
    </html>