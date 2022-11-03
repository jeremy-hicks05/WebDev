<%@ Page Language="C#" CodeFile="~/apps/pulloffs/ViewPulloffs.aspx.cs" Inherits="MTAIntranet.ViewPulloffs" AutoEventWireup="True" %>

<Doctype html>
<html>
<head>
    <title>MTA Flint Intranet</title>
    <link rel="stylesheet" href="../../style/style.css" />
    <script src="../../script/utils.js"></script>
    <script src="../../script/pulloffs/showpulloffs.js"></script>
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
            <h1>View Pulloffs</h1>
            <form id="formviewpulloffs" runat="server">
                <span>
                    <select id="pulloffmonth"></select>
                </span>

                <button type="button" id="showpulloffs">Show Pulloffs</button>
            </form>
        <div id="results"></div>
    </div>
</body>
</html>