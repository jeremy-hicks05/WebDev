window.onload = init;

function init() {
	fillHoursAndMinutes();

	document.getElementById("addroute").onclick = sqlAddMasterRoute;

	document.getElementById("mrroutename").onblur = verifyRouteName;
	document.getElementById("mrmode").onblur = verifyMode;
	document.getElementById("mrroutenum").onblur = verifyRouteNum;
	document.getElementById("mrrun").onblur = verifyRun;
	document.getElementById("mrsuffix").onblur = verifySuffix;
	document.getElementById("mrbegdh").onblur = verifyBegDH;
	document.getElementById("mrenddh").onblur = verifyEndDH;

	document.getElementById("monday").onchange = clearNoneDaysOfWeek;
	document.getElementById("tuesday").onchange = clearNoneDaysOfWeek;
	document.getElementById("wednesday").onchange = clearNoneDaysOfWeek;
	document.getElementById("thursday").onchange = clearNoneDaysOfWeek;
	document.getElementById("friday").onchange = clearNoneDaysOfWeek;
	document.getElementById("saturday").onchange = clearNoneDaysOfWeek;
	document.getElementById("sunday").onchange = clearNoneDaysOfWeek;

	document.getElementById("none").onchange = clearDaysOfWeek;
}

function fillHoursAndMinutes() {
	var pothours = document.getElementById("mrpothour");
	var potmins = document.getElementById("mrpotmin");

	var pithours = document.getElementById("mrpithour");
	var pitmins = document.getElementById("mrpitmin");

	for (var i = 0; i < 24; i++) {
		pothours.innerHTML += "<option value = " + "\"" + i + "\">" + i + "</option>";
		pithours.innerHTML += "<option value = " + "\"" + i + "\">" + i + "</option>";
	}

	for (var i = 0; i < 60; i += 5) {
		potmins.innerHTML += "<option value = " + "\"" + i + "\">" + i + "</option>";
		pitmins.innerHTML += "<option value = " + "\"" + i + "\">" + i + "</option>";
    }
}

function verifyRouteName() {
	document.getElementById("error").innerHTML = "";
	if (document.getElementById("mrroutename").value == "") {
		document.getElementById("error").innerHTML = "Check Route Name";
    }
}

function verifyMode() {
	document.getElementById("error").innerHTML = "";
	if (document.getElementById("mrmode").value == "") {
		document.getElementById("error").innerHTML = "Check Mode";
	}
}

function verifyRouteNum() {
	document.getElementById("error").innerHTML = "";
	if (document.getElementById("mrroutenum").value == "") {
		document.getElementById("error").innerHTML = "Check Route Num";
	}
}

function verifyRun() {
	document.getElementById("error").innerHTML = "";
	if (document.getElementById("mrrun").value == "") {
		document.getElementById("error").innerHTML = "Check Run";
	}
}

function verifySuffix() {
	document.getElementById("error").innerHTML = "";
	if (document.getElementById("mrsuffix").value == "") {
		document.getElementById("error").innerHTML = "Check Suffix";
	}
}

function verifyBegDH() {
	document.getElementById("error").innerHTML = "";
	if (document.getElementById("mrbegdh").value == "") {
		document.getElementById("error").innerHTML = "Check Beginning Deadhead";
	}
}

function verifyEndDH() {
	document.getElementById("error").innerHTML = "";
	if (document.getElementById("mrenddh").value == "") {
		document.getElementById("error").innerHTML = "Check Ending Deadhead";
	}
}

function sqlAddMasterRoute() {
	addMRRequest = createRequest();
	if (addMRRequest == null) {
		alert("Unable to create request");
	}
	else {

		var allrequiredboxes = false;
		var routeElement = document.getElementById("mrroutename");
		var routeVal = routeElement.value;

		var modeElement = document.getElementById("mrmode");
		var modeVal = modeElement.value;

		var routeNumElement = document.getElementById("mrroutenum");
		var routeNumVal = routeNumElement.value;

		var runElement = document.getElementById("mrrun");
		var runVal = runElement.value;

		var suffixElement = document.getElementById("mrsuffix");
		var suffixVal = suffixElement.value;

		var pothourElement = document.getElementById("mrpothour");
		var pothourVal = pothourElement.value;

		var potminElement = document.getElementById("mrpotmin");
		var potminVal = potminElement.value;

		var pithourElement = document.getElementById("mrpithour");
		var pithourVal = pithourElement.value;

		var pitminElement = document.getElementById("mrpitmin");
		var pitminVal = pitminElement.value;

		var mr_start_date_time = "2021-01-01 " + pothourVal + ":" + potminVal;
		var mr_return_date_time = "2021-01-01 " + pithourVal + ":" + pitminVal;

		var daysOfWeek = "";

		var mCheckBox = document.getElementById("monday");
		var tCheckBox = document.getElementById("tuesday");
		var wCheckBox = document.getElementById("wednesday");
		var hCheckBox = document.getElementById("thursday");
		var fCheckBox = document.getElementById("friday");
		var sCheckBox = document.getElementById("saturday");
		var yCheckBox = document.getElementById("sunday");
		var nCheckBox = document.getElementById("none");

		if (mCheckBox.checked) {
			daysOfWeek += "M";
		}
		if (tCheckBox.checked) {
			daysOfWeek += "T";
		}
		if (wCheckBox.checked) {
			daysOfWeek += "W";
		}
		if (hCheckBox.checked) {
			daysOfWeek += "H";
		}
		if (fCheckBox.checked) {
			daysOfWeek += "F";
		}
		if (sCheckBox.checked) {
			daysOfWeek += "S";
		}
		if (yCheckBox.checked) {
			daysOfWeek += "Y";
		}
		if (nCheckBox.checked) {
			daysOfWeek += "N";
		}

		var begdhElement = document.getElementById("mrbegdh");
		var begdhVal = begdhElement.value;

		var enddhElement = document.getElementById("mrenddh");
		var enddhVal = enddhElement.value;

		if (routeVal != "" &&
			modeVal != "" && routeNumVal != "" && runVal != "" &&
			suffixVal != "" && pothourVal != "" && potminVal != "" &&
			pithourVal != "" && pitminVal != "" && begdhVal != "" &&
			enddhVal != "" &&
			// at least one checkbox checked
			!(mCheckBox.checked == false &&
				tCheckBox.checked == false &&
				wCheckBox.checked == false &&
				hCheckBox.checked == false &&
				fCheckBox.checked == false &&
				sCheckBox.checked == false &&
				yCheckBox.checked == false &&
				nCheckBox.checked == false))
		{
			allrequiredboxes = true;
        }

		// if all required boxes are filled in
		if (allrequiredboxes) {
			var params = JSON.stringify({
				"route_name": routeVal,
				"mr_start_date_time": mr_start_date_time,
				"mr_return_date_time": mr_return_date_time,
				"mr_mode": modeVal,
				"mr_suffix": suffixVal,
				"mr_route_num": routeNumVal,
				"mr_run_num": runVal,
				"mr_day_of_week": daysOfWeek, //MTWHFSYN
				"beg_dh_miles": begdhVal,
				"end_dh_miles": enddhVal
			});
			
			var url = "AddMRTemplate.aspx/AddMasterRouteSQL";
			
			addMRRequest.onreadystatechange = function addMasterRoute() {
				if (addMRRequest.readyState == 4) {
					if (addMRRequest.status == 200) {
						document.getElementById("results").innerHTML = addMRRequest.responseText;
					}
				}
			};

			addMRRequest.open("POST", url, false);
			addMRRequest.setRequestHeader('Content-type', 'application/json');
			addMRRequest.send(params);
		}
		else {
			alert("Please enter a value for all boxes and select at least one option for day of the week");
        }
	}
}

function resetElements() {
	resetElement(document.getElementById("returnminute"));
	resetElement(document.getElementById("returnhour"));
	resetElement(document.getElementById("pulloffminute"));
	resetElement(document.getElementById("pulloffhour"));
	resetElement(document.getElementById("routeinfo"));
	resetElement(document.getElementById("pulloffday"));
}

function resetElement(element) {
	element.selectedIndex= 0;
	element.disabled = true;
}

function masterRoutePreview(routeInfo) {
	masterRoutePreviewRequest = createRequest();
	if (masterRoutePreviewRequest == null) {
		alert("Unable to create request");
	}
	else {
		var pulloffmonthelement = document.getElementById("pulloffmonth");
		var pulloffdayelement = document.getElementById("pulloffday");
		var pulloffinfoelement = document.getElementById("routeinfo");

		var monthVal = pulloffmonthelement.options[pulloffmonthelement.selectedIndex].value;
		var dayVal = pulloffdayelement.options[pulloffdayelement.selectedIndex].value;
		var infoVal = pulloffinfoelement.options[pulloffinfoelement.selectedIndex].value;

		var url = "AddPulloff.aspx/FillRoutePreview";
		var params = JSON.stringify({ "routeInfo": infoVal, "month": monthVal, "day": dayVal });

		masterRoutePreviewRequest.onreadystatechange = function () {
			if (masterRoutePreviewRequest.readyState == 4) {
				if (masterRoutePreviewRequest.status == 200) {
					document.getElementById("pulloffspreview").innerHTML = masterRoutePreviewRequest.responseText;
				}
				else {
					console.log(masterRoutePreviewRequest);
				}
			}
		};
		masterRoutePreviewRequest.open("POST", url, false);
		masterRoutePreviewRequest.setRequestHeader('Content-type', 'application/json');
		masterRoutePreviewRequest.send(params);
	}
}

function clearDaysOfWeek() {
	var mon = document.getElementById("monday");
	var tue = document.getElementById("tuesday");
	var wed = document.getElementById("wednesday");
	var thu = document.getElementById("thursday");
	var fri = document.getElementById("friday");
	var sat = document.getElementById("saturday");
	var sun = document.getElementById("sunday");

	mon.checked = false;
	tue.checked = false;
	wed.checked = false;
	thu.checked = false;
	fri.checked = false;
	sat.checked = false;
	sun.checked = false;
}

function clearNoneDaysOfWeek() {
	var non = document.getElementById("none");
	non.checked = false;
}