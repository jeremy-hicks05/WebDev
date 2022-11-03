<%@ Page Language="C#" CodeFile="~/apps/masterroute/AddMRTemplate.aspx.cs" Inherits="MTAIntranet.AddMRTemplate" AutoEventWireup="True" %>

<Doctype html>
<html>
<head>
    <title>MTA Flint Intranet</title>
    <link rel="stylesheet" href="../../style/style.css" />
    <script src="../../script/utils.js"></script>
    <script src="../../script/masterroute/addmr.js"></script>
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
        <h1>Add MasterRoute Template</h1>
        <form id="formaddmrtemplate" runat="server">
        <table id="addmrtemplatetable">
            <tr><td>Route Name</td><td colspan="8"><input id="mrroutename" type="text"/></td></tr>
            <tr><td>Mode</td><td colspan="8"><input id="mrmode" type="text"/></td></tr>
            <tr><td>Route Num</td><td colspan="8"><input id="mrroutenum" type="text"/></td></tr>
            <tr><td>Run</td><td colspan="8"><input id="mrrun" type="text"/></tr>
            <tr><td>Suffix</td><td colspan="8"><input id="mrsuffix" type="text"/></td></tr>
            <tr><td>Pull Out Time</td><td colspan="2">Hour <select id="mrpothour"></select></td><td colspan="5">Min <select id="mrpotmin"></select></td></tr>
            <tr><td>Pull In Time</td><td colspan="2">Hour <select id="mrpithour"></select></td><td colspan="2">Min <select id="mrpitmin"></select></td></tr>
            <tr>
                <td>Days</td>
                <td>M</td>
                <td>T</td>
                <td>W</td>
                <td>H</td>
                <td>F</td>
                <td>S</td>
                <td>Y</td>
                <td>N</td>
            </tr>
            <tr>
                <td></td>
                <td><input id="monday" type="checkbox" /></td>
                <td><input id="tuesday" type="checkbox" /></td>
                <td><input id="wednesday" type="checkbox" /></td>
                <td><input id="thursday" type="checkbox" /></td>
                <td><input id="friday" type="checkbox" /></td>
                <td><input id="saturday" type="checkbox" /></td>
                <td><input id="sunday" type="checkbox" /></td>
                <td><input id="none" checked="checked" type="checkbox" /></td>
            </tr>
            <tr><td>Beg DH Miles</td><td colspan="7"><input id="mrbegdh" type="number"/></td></tr>
            <tr><td>End DH Miles</td><td colspan="7"><input id="mrenddh" type="number"/></td></tr>
        </table>
        <button type="button" id="addroute">Add Route</button>
            <div id="err">
                <p id="error" style="color:red; font-style:italic; font-weight:bold"></p>
            </div>
        <div id="runpreview"></div>
        <div id="results"></div>
    </form>
    </div>
</body>
</html>