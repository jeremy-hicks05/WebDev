<%@ Page Language="C#" CodeFile="~/apps/masterroute/DelMRTemplate.aspx.cs" Inherits="MTAIntranet.DelMRTemplate" AutoEventWireup="True" %>

<Doctype html>
<html>
<head>
    <title>MTA Flint Intranet</title>
    <link rel="stylesheet" href="../../style/style.css" />
    <script src="../../script/utils.js"></script>
    <script src="../../script/masterroute/delmr.js"></script>
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
        <h1>Delete MasterRoute Template</h1>
        <form id="formdelmrtemplate" runat="server">
        <table id="delmrtemplatetable">
            <tr><td><select id="masterroutelist"></select></td></tr>
        </table>
        <button type="button" id="delroute">Delete Route</button>
        <div id="runpreview"></div>
        <div id="results"></div>
    </form>
    </div>
</body>
</html>