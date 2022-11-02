<%@ Page Language="C#" CodeFile="~/apps/pulloffs/AddPulloff.aspx.cs" Inherits="MTAIntranet.AddPulloff" AutoEventWireup="True" %>

<Doctype html>
<html>
<head>
    <title>MTA Flint Intranet</title>
    <link rel="stylesheet" href="../../style/style.css" />
    <script src="../../script/utils.js"></script>
    <script src="../../script/pulloffs/addpulloff.js"></script>
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
        <h1>Add Pulloffs</h1>
        <form id="formaddpulloffs" runat="server">
        <table id="addpullofftable">
            <tr><td>Year</td><td><select id="pulloffyear" disabled="disabled"></select></td></tr>
            <tr><td>Month</td><td><select id="pulloffmonth"></select></td></tr>
            <tr><td>Day</td><td><select id="pulloffday"></select></td></tr>
            <tr><td>Route</td><td><select id="routeinfo"></select></td></tr>
            <tr><td>Pulloff Time</td><td><select id="pulloffhour"></select><select id="pulloffminute"></select></td></tr>
            <tr><td>Return to Service</td><td><select id="returnhour"></select><select id="returnminute"></select></td></tr>
        </table>
        <button type="button" id="addpulloff">Add Pulloff</button>
        <div id="pulloffspreview"></div>
        <div id="results"></div>
    </form>
    </div>
</body>
</html>