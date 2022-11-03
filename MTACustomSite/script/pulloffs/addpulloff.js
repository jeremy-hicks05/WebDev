window.onload = init;

function init() {
	fillYears();
	fillMonths();
	

	document.getElementById("addpulloff").disabled = true;

	// chain fill listeners
	document.getElementById("pulloffmonth").onchange = fillDays;
	document.getElementById("pulloffday").onchange = fillRouteInfo;
	document.getElementById("routeinfo").onchange = fillPulloffHours;
	document.getElementById("pulloffhour").onchange = fillPulloffMinutes;
	document.getElementById("pulloffminute").onchange = fillReturnHours;
	document.getElementById("returnhour").onchange = fillReturnMinute;
	document.getElementById("returnminute").onchange = function () {
		document.getElementById("addpulloff").disabled = false;
	};

	//document.getElementById("addpulloff").onclick = sqlAddPulloffObject;
	document.getElementById("addpulloff").onclick = sqlAddPulloff;

	document.getElementById("pulloffday").disabled = true;
	document.getElementById("routeinfo").disabled = true;
	document.getElementById("pulloffhour").disabled = true;
	document.getElementById("pulloffminute").disabled = true;
	document.getElementById("returnhour").disabled = true;
	document.getElementById("returnminute").disabled = true;
}

//function testFunction() {
//	testRequest = createRequest();

//	var testObject = {
//		pulloffid : "9999"
//    }

//	if (testRequest == null) {
//		alert("Unable to create request");
//	}
//	else {
//		var url = "AddPulloff.aspx/TestFunction";
//		testRequest.onreadystatechange = function () {
//			if (testRequest.readyState == 4) {
//				if (testRequest.status == 200) {
//					//var testObject = new Object();
//					console.log(testRequest.responseText);
//				}
//			}
//		};
//		testRequest.open("POST", url, false);
//		testRequest.setRequestHeader('Content-type', 'application/json');
//		testRequest.send(JSON.stringify({ pulloff: testObject }));
//	}
//}

function fillYears() {
	yearsRequest = createRequest();

	if (yearsRequest == null) {
		alert("Unable to create request");
	}
	else {
		var url = "AddPulloff.aspx/FillYears";
		yearsRequest.onreadystatechange = function () {
			if (yearsRequest.readyState == 4) {
				if (yearsRequest.status == 200) {
					var pulloffYear = document.getElementById("pulloffyear");
					pulloffYear.innerHTML = yearsRequest.responseText;
				}
			}
		};
		yearsRequest.open("POST", url, false);
		yearsRequest.setRequestHeader('Content-type', 'application/json');
		yearsRequest.send(null);
	}
}

function fillMonths() {
	monthsRequest = createRequest();

	if (monthsRequest == null) {
		alert("Unable to create request");
	}
	else {
		if (document.getElementById("results") != null) {
			document.getElementById("results").innerHTML = "";
		}

		var url = "AddPulloff.aspx/FillMonths";
		monthsRequest.onreadystatechange = function () {
			if (monthsRequest.readyState == 4) {
				if (monthsRequest.status == 200) {
					var pulloffMonth = document.getElementById("pulloffmonth");
					pulloffMonth.innerHTML = monthsRequest.responseText;
				}
			}
		};
		monthsRequest.open("POST", url, false);
		monthsRequest.setRequestHeader('Content-type', 'application/json');
		monthsRequest.send(null);
	}
}

function fillDays() {
	daysRequest = createRequest();

	if (daysRequest == null) {
		alert("Unable to create request");
	}
	else {

		document.getElementById("pulloffday").disabled = false;
		document.getElementById("addpulloff").disabled = true;

		if (document.getElementById("pulloffspreview") != null) {
			document.getElementById("pulloffspreview").innerHTML = "";
		}
		if (document.getElementById("results") != null) {
			document.getElementById("results").innerHTML = "";
		}

		resetElement(document.getElementById("routeinfo"));
		resetElement(document.getElementById("pulloffhour"));
		resetElement(document.getElementById("pulloffminute"));
		resetElement(document.getElementById("returnhour"));
		resetElement(document.getElementById("returnminute"));

		var url = "AddPulloff.aspx/FillDays";
		var monthVal = document.getElementById("pulloffmonth").options[document.getElementById("pulloffmonth").selectedIndex].value;
		var params = JSON.stringify({ "month": monthVal });

		daysRequest.onreadystatechange = function () {
			if (daysRequest.readyState == 4) {
				if (daysRequest.status == 200) {
					var pulloffDay = document.getElementById("pulloffday");
					pulloffDay.innerHTML = daysRequest.responseText;
				}
			}
		};

		daysRequest.open("POST", url, false);
		daysRequest.setRequestHeader('Content-type', 'application/json');
		daysRequest.send(params);
	}
}

function fillRouteInfo() {
	document.getElementById("routeinfo").disabled = false;
	routeRequest = createRequest();

	if (routeRequest == null) {
		alert("Unable to create request");
	}
	else {
		document.getElementById("addpulloff").disabled = true;
		if (document.getElementById("results") != null) {
			document.getElementById("results").innerHTML = "";
		}

		var url = "AddPulloff.aspx/FillRouteInfo";
		var monthVal = document.getElementById("pulloffmonth").options[document.getElementById("pulloffmonth").selectedIndex].value;
		var dayVal = document.getElementById("pulloffday").options[document.getElementById("pulloffday").selectedIndex].value;

		resetElement(document.getElementById("pulloffhour"));
		resetElement(document.getElementById("pulloffminute"));
		resetElement(document.getElementById("returnhour"));
		resetElement(document.getElementById("returnminute"));

		var params = JSON.stringify({
			"month": monthVal,
			"day": dayVal
		});

		routeRequest.onreadystatechange = function () {
			if (routeRequest.readyState == 4) {
				if (routeRequest.status == 200) {
					var routeInfo = document.getElementById("routeinfo");
					if (routeInfo) {
						routeInfo.innerHTML = routeRequest.responseText;
					}
				}
			}
		};
		routeRequest.open("POST", url, false);
		routeRequest.setRequestHeader('Content-type', 'application/json');
		routeRequest.send(params);
	}
}

function fillPulloffHours() {
	hoursRequest = createRequest();
	
	if (hoursRequest == null) {
		alert("Unable to create request");
	}
	else {

		document.getElementById("pulloffhour").disabled = false;
		document.getElementById("addpulloff").disabled = true;

		if (document.getElementById("results") != null) {
			document.getElementById("results").innerHTML = "";
		}

		resetElement(document.getElementById("pulloffminute"));
		resetElement(document.getElementById("returnhour"));
		resetElement(document.getElementById("returnminute"));

		var url = "AddPulloff.aspx/FillPulloffHours";

		var pulloffMonthVal = document.getElementById("pulloffmonth").options[document.getElementById("pulloffmonth").selectedIndex].value;
		var pulloffDayVal = document.getElementById("pulloffday").options[document.getElementById("pulloffday").selectedIndex].value;
		var routeInfoVal = document.getElementById("routeinfo").options[document.getElementById("routeinfo").selectedIndex].value;

		var params = JSON.stringify({
			"routeInfo": routeInfoVal,
			"pulloffMonth": pulloffMonthVal,
			"pulloffDay": pulloffDayVal
		});
		hoursRequest.onreadystatechange = function () {
			if (hoursRequest.readyState == 4) {
				if (hoursRequest.status == 200) {
					var pulloffHour = document.getElementById("pulloffhour");
					pulloffHour.innerHTML = hoursRequest.responseText;
					pulloffsPreview();
                }
            }
		};
		hoursRequest.open("POST", url, false);
		hoursRequest.setRequestHeader('Content-type', 'application/json');
		hoursRequest.send(params);
	}
}

function fillPulloffMinutes() {
	minutesRequest = createRequest();

	if (minutesRequest == null) {
		alert("Unable to create request");
	}
	else {

		document.getElementById("pulloffminute").disabled = false;
		document.getElementById("addpulloff").disabled = true;

		if (document.getElementById("results") != null) {
			document.getElementById("results").innerHTML = "";
		}

		resetElement(document.getElementById("returnhour"));
		resetElement(document.getElementById("returnminute"));

		var url = "AddPulloff.aspx/FillPulloffMinutes";
		var routeInfoVal = document.getElementById("routeinfo").options[document.getElementById("routeinfo").selectedIndex].value;
		var pulloffMonthVal = document.getElementById("pulloffmonth").options[document.getElementById("pulloffmonth").selectedIndex].value;
		var pulloffDayVal = document.getElementById("pulloffday").options[document.getElementById("pulloffday").selectedIndex].value;
		var pulloffHourVal = document.getElementById("pulloffhour").options[document.getElementById("pulloffhour").selectedIndex].value;
		
		var params = JSON.stringify({
			"routeInfo": routeInfoVal,
			"pulloffMonth" : pulloffMonthVal,
			"pulloffDay": pulloffDayVal,
			"pulloffHour" : pulloffHourVal
		});
		minutesRequest.onreadystatechange = function () {
			if (minutesRequest.readyState == 4) {
				if (minutesRequest.status == 200) {
					var pulloffMinute = document.getElementById("pulloffminute");
					pulloffMinute.innerHTML = minutesRequest.responseText;
                }
            }
		};
		minutesRequest.open("POST", url, false);
		minutesRequest.setRequestHeader('Content-type', 'application/json');
		minutesRequest.send(params);
	}
}

function fillReturnHours() {
	returnHoursRequest = createRequest();

	if (returnHoursRequest == null) {
		alert("Unable to create request");
	}
	else {

		document.getElementById("returnhour").disabled = false;
		document.getElementById("addpulloff").disabled = true;

		if (document.getElementById("results") != null) {
			document.getElementById("results").innerHTML = "";
		}

		resetElement(document.getElementById("returnminute"));
		
		var url = "AddPulloff.aspx/FillReturnHours";

		//string routeInfo, string month, string day, string pHour
		var pHourVal = document.getElementById("pulloffhour").options[document.getElementById("pulloffhour").selectedIndex].value;

		var routeInfoVal = document.getElementById("routeinfo").options[document.getElementById("routeinfo").selectedIndex].value;
		var pulloffMonthVal = document.getElementById("pulloffmonth").options[document.getElementById("pulloffmonth").selectedIndex].value;
		var pulloffDayVal = document.getElementById("pulloffday").options[document.getElementById("pulloffday").selectedIndex].value;

		var params = JSON.stringify({"routeInfo":routeInfoVal, "month": pulloffMonthVal, "day": pulloffDayVal, "pHour": pHourVal});
		returnHoursRequest.onreadystatechange = function () {
			if (returnHoursRequest.readyState == 4) {
				if (returnHoursRequest.status == 200) {
					var returnHour = document.getElementById("returnhour");
					returnHour.innerHTML = returnHoursRequest.responseText;
                }
            }
		};

		returnHoursRequest.open("POST", url, false);
		returnHoursRequest.setRequestHeader('Content-type', 'application/json');
		returnHoursRequest.send(params);
	}
}

function fillReturnMinute(startHour) {
	returnMinutesRequest = createRequest();

	if (returnMinutesRequest == null) {
		alert("Unable to create request");
	}
	else {
		document.getElementById("returnminute").disabled = false;
		document.getElementById("addpulloff").disabled = true;

		if (document.getElementById("results") != null) {
			document.getElementById("results").innerHTML = "";
		}

		//string routeInfo, string month, string day, string pHour, string rHour, string pMin
		var url = "AddPulloff.aspx/FillReturnMinutes";

		var routeInfoVal = document.getElementById("routeinfo").options[document.getElementById("routeinfo").selectedIndex].value;
		var pulloffMonthVal = document.getElementById("pulloffmonth").options[document.getElementById("pulloffmonth").selectedIndex].value;
		var pulloffDayVal = document.getElementById("pulloffday").options[document.getElementById("pulloffday").selectedIndex].value;

		var pHourVal = document.getElementById("pulloffhour").options[document.getElementById("pulloffhour").selectedIndex].value;
		var pMinVal = document.getElementById("pulloffminute").options[document.getElementById("pulloffminute").selectedIndex].value;
		var rHourVal = document.getElementById("returnhour").options[document.getElementById("returnhour").selectedIndex].value;
		var params = JSON.stringify({"routeInfo": routeInfoVal, "month": pulloffMonthVal, "day": pulloffDayVal, "pHour": pHourVal, "pMin": pMinVal, "rHour": rHourVal});
		returnMinutesRequest.onreadystatechange = function () {
			if (returnMinutesRequest.readyState == 4) {
				if (returnMinutesRequest.status == 200) {
					var returnMinute = document.getElementById("returnminute");
					returnMinute.innerHTML = returnMinutesRequest.responseText;
                }
            }
		};
		returnMinutesRequest.open("POST", url, false);
		returnMinutesRequest.setRequestHeader('Content-type', 'application/json');
		returnMinutesRequest.send(params);
	}
}

function sqlAddPulloff() {
	addPulloffRequest = createRequest();
	if (addPulloffRequest == null) {
		alert("Unable to create request");
	}
	else {
		var route = document.getElementById("routeinfo");
		var routeVal = route.options[route.selectedIndex].value;

		var pulloffmonth = document.getElementById("pulloffmonth");
		var pulloffmonthVal = pulloffmonth.options[pulloffmonth.selectedIndex].value;

		var pulloffday = document.getElementById("pulloffday");
		var pulloffdayVal = pulloffday.options[pulloffday.selectedIndex].value;

		var pulloffyear = document.getElementById("pulloffyear");
		var pulloffyearVal = pulloffyear.options[pulloffyear.selectedIndex].value;

		var pulloffhour = document.getElementById("pulloffhour");
		var pulloffhourVal = pulloffhour.options[pulloffhour.selectedIndex].value;

		var pulloffminute = document.getElementById("pulloffminute");
		var pulloffminuteVal = pulloffminute.options[pulloffminute.selectedIndex].value;

		var returnminute = document.getElementById("returnminute");
		var returnminuteVal = returnminute.options[returnminute.selectedIndex].value;

		var returnhour = document.getElementById("returnhour");
		var returnhourVal = returnhour.options[returnhour.selectedIndex].value;

		if (pulloffyearVal != "" &&
			pulloffmonthVal != ""  &&
			pulloffdayVal != ""  &&
			pulloffhourVal != ""  &&
			pulloffminuteVal != ""  &&
			returnhourVal != ""  &&
			returnminuteVal != ""  &&
			routeVal != "" ) {

			var url = "AddPulloff.aspx/AddPulloffSQL";
			var params = JSON.stringify({
				"route": routeVal,
				"pulloffmonth": pulloffmonthVal,
				"pulloffday": pulloffdayVal,
				"pulloffyear": pulloffyearVal,
				"pulloffhour" : pulloffhourVal,
				"pulloffminute": pulloffminuteVal,
				"returnhour": returnhourVal,
				"returnminute": returnminuteVal
			});

			addPulloffRequest.onreadystatechange = function addPulloffs() {
				if (addPulloffRequest.readyState == 4) {
					if (addPulloffRequest.status == 200) {
						document.getElementById("results").innerHTML = addPulloffRequest.responseText;
					}
				}
			};

			addPulloffRequest.open("POST", url, false);
			addPulloffRequest.setRequestHeader('Content-type', 'application/json');
			addPulloffRequest.send(params);

			// preview pulloffs
			pulloffsPreview();

			// reset elements
			resetElement(document.getElementById("returnminute"));
			resetElement(document.getElementById("returnhour"));
			resetElement(document.getElementById("pulloffminute"));
			resetElement(document.getElementById("pulloffhour"));
			resetElement(document.getElementById("routeinfo"));

			// fill route info
			fillRouteInfo();
		}
		else {
			alert("Please enter a value for all boxes");
        }
	}
}

//function sqlAddPulloffObject() {
//	addPulloffObjectRequest = createRequest();
//	if (addPulloffObjectRequest == null) {
//		alert("Unable to create request");
//	}
//	else {
//		//alert("Got the request object");

//		var route = document.getElementById("routeinfo");
//		var routeVal = route.options[route.selectedIndex].value;
//		//alert("Route:" + routeVal);

//		var pulloffmonth = document.getElementById("pulloffmonth");
//		var pulloffmonthVal = pulloffmonth.options[pulloffmonth.selectedIndex].value;
//		//alert("Month: " + monthVal);

//		var pulloffday = document.getElementById("pulloffday");
//		var pulloffdayVal = pulloffday.options[pulloffday.selectedIndex].value;
//		//alert("Day: " + dayVal);

//		var pulloffyear = document.getElementById("pulloffyear");
//		var pulloffyearVal = pulloffyear.options[pulloffyear.selectedIndex].value;
//		//alert("Year: " + yearVal);

//		var pulloffhour = document.getElementById("pulloffhour");
//		var pulloffhourVal = pulloffhour.options[pulloffhour.selectedIndex].value;
//		//alert("Day: " + dayVal);

//		var pulloffminute = document.getElementById("pulloffminute");
//		var pulloffminuteVal = pulloffminute.options[pulloffminute.selectedIndex].value;
//		//alert("Minute: " + minuteVal);

//		var returnminute = document.getElementById("returnminute");
//		var returnminuteVal = returnminute.options[returnminute.selectedIndex].value;

//		var returnhour = document.getElementById("returnhour");
//		var returnhourVal = returnhour.options[returnhour.selectedIndex].value;

//		if (pulloffyearVal != "" &&
//			pulloffmonthVal != "" &&
//			pulloffdayVal != "" &&
//			pulloffhourVal != "" &&
//			pulloffminuteVal != "" &&
//			returnhourVal != "" &&
//			returnminuteVal != "" &&
//			routeVal != "") {

//			var url = "AddPulloff.aspx/AddPulloffSQL";
//			var pulloff = {
//				"route": routeVal,
//				"pulloffmonth": pulloffmonthVal,
//				"pulloffday": pulloffdayVal,
//				"pulloffyear": pulloffyearVal,
//				"pulloffhour": pulloffhourVal,
//				"pulloffminute": pulloffminuteVal,
//				"returnhour": returnhourVal,
//				"returnminute": returnminuteVal
//			};

//		var url = "AddPulloff.aspx/AddPulloffObjectSQL";

//		addPulloffObjectRequest.onreadystatechange = function addPulloffs() {
//			if (addPulloffObjectRequest.readyState == 4) {
//				if (addPulloffObjectRequest.status == 200) {
//					document.getElementById("results").innerHTML = addPulloffObjectRequest.responseText;
//				}
//			}
//		};

//		addPulloffObjectRequest.open("POST", url, false);
//		addPulloffObjectRequest.setRequestHeader('Content-type', 'application/json');
//		addPulloffObjectRequest.send(JSON.stringify({ pulloff: pulloff }));
//	}
//}

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

function pulloffsPreview() {
	pulloffPreviewRequest = createRequest();
	if (pulloffPreviewRequest == null) {
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

		pulloffPreviewRequest.onreadystatechange = function () {
			if (pulloffPreviewRequest.readyState == 4) {
				if (pulloffPreviewRequest.status == 200) {
					document.getElementById("pulloffspreview").innerHTML = pulloffPreviewRequest.responseText;
				}
				else {
					console.log(pulloffPreviewRequest);
				}
			}
		};
		pulloffPreviewRequest.open("POST", url, false);
		pulloffPreviewRequest.setRequestHeader('Content-type', 'application/json');
		pulloffPreviewRequest.send(params);
	}
}