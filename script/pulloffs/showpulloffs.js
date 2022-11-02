window.onload = init;

function init() {
	document.getElementById("pulloffmonth").onchange = sqlPulloffs;
	document.getElementById("showpulloffs").onclick = sqlPulloffs;
	fillMonths();
}

function sqlPulloffs() {
	var resultsElement = document.getElementById("results");
	resultsElement.innerHTML = "<p><b>LOADING DATA...</b></p>";
	pulloffViewRequest = createRequest();
	if (pulloffViewRequest == null) {
		alert("Unable to create request");
	}
	else {
		var pulloffmonthelement = document.getElementById("pulloffmonth");
		var monthVal = pulloffmonthelement.options[pulloffmonthelement.selectedIndex].value;

		var url = "ViewPulloffs.aspx/FillPulloffTable";
		var params = JSON.stringify({ "month": monthVal});

		pulloffViewRequest.onreadystatechange = function () {
			if (pulloffViewRequest.readyState == 4) {
				if (pulloffViewRequest.status == 200) {
					document.getElementById("results").innerHTML = pulloffViewRequest.response;
					if (document.getElementById("pulloffresults")) {
						document.getElementById("pulloffresults").
							querySelector("caption").innerHTML +=
							" (" + (document.getElementById("pulloffresults").rows.length - 1) + ")";
					}

					var delbuttons = document.getElementsByClassName("delpulloff");
					for (var i = 0; i < delbuttons.length; i++) {
						delbuttons[i].onclick = delPulloff;
					}
				}
				else {
					console.log(pulloffViewRequest);
				}
			}
		};
		pulloffViewRequest.open("POST", url, false);
		pulloffViewRequest.setRequestHeader('Content-type', 'application/json');
		pulloffViewRequest.send(params);
	}
}

function delPulloff(e) {
	delPulloffViewRequest = createRequest();
	if (delPulloffViewRequest == null) {
		alert("Unable to create request");
	}
	else {
		var confirmation = confirm("Are you sure you want to delete the highlited pulloff?");
		if (confirmation) {
			var monthElement = document.getElementById("pulloffmonth");
			var pulloffid = e.target.name;
			var monthVal = monthElement.options[monthElement.selectedIndex].value;

			var url = "ViewPulloffs.aspx/DelPulloffSQL";
			var params = JSON.stringify({ "pulloffid": pulloffid, "month": monthVal});

			delPulloffViewRequest.onreadystatechange = function () {
				if (delPulloffViewRequest.readyState == 4) {
					if (delPulloffViewRequest.status == 200) {
						document.getElementById("results").innerHTML = delPulloffViewRequest.responseText;
					}
					else {
						console.log(delPulloffViewRequest);
					}
				}
			};
			delPulloffViewRequest.open("POST", url, false);
			delPulloffViewRequest.setRequestHeader('Content-type', 'application/json');
			delPulloffViewRequest.send(params);

			sqlPulloffs();
		}
	}
}

function fillMonths() {
	document.getElementById("pulloffmonth").innerHTML += "<option value =\"\"></option>" +
		"<option value =\"01\">January</option>" +
		"<option value=\"02\" > February</option>" +
		"<option value=\"03\">March</option>" +
		"<option value=\"04\">April</option>" +
		"<option value=\"05\">May</option>" +
		"<option value=\"06\">June</option>" +
		"<option value=\"07\">July</option>" +
		"<option value=\"08\">August</option>" +
		"<option value=\"09\">September</option>" +
		"<option value=\"10\">October</option>" +
		"<option value=\"11\">November</option>" +
        "<option value =\"12\">December</option>";
}