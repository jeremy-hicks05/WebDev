window.onload = init;

function init() {
	// populate vehicle IDs dropdown
	sqlFillVehIds();

	// attach onclick listener to Show Fuel Master Transactions link
	document.getElementById("showfuelmastertr").onclick = sqlFMTR;

	// attack onkeyup and onblur listener to prevent input errors
	document.getElementById("fuelmastervehod").onkeyup = checkOD;
	document.getElementById("fuelmastervehod").onblur = checkOD;

}

function sqlFMTR() {
	fuelMasterTransactionsRequest = createRequest();
	if (fuelMasterTransactionsRequest == null) {
		alert("Unable to create request");
	}
	else {
		var fmVehID = document.getElementById("fuelmastervehid");
		var fmVehOD = document.getElementById("fuelmastervehod");

		var fmVehIDValue = fmVehID.options[fmVehID.selectedIndex].value;

		var fmVehODValue = fmVehOD.value;

		if (fmVehID == "" || fmVehODValue == "") {
			alert("Please enter a value for Vehicle ID and Odometer");
		}
		else {
			var url = "ViewFMTR.aspx/GetFMTransactions";
			var params = JSON.stringify({ "vehID": fmVehIDValue, "vehOdometer": fmVehODValue });

			fuelMasterTransactionsRequest.onreadystatechange = function () {
				if (fuelMasterTransactionsRequest.readyState == 4) {
					if (fuelMasterTransactionsRequest.status == 200) {
						console.log(fuelMasterTransactionsRequest);
						document.getElementById("fmresults").innerHTML = fuelMasterTransactionsRequest.responseText;
					}
				}
            };
			fuelMasterTransactionsRequest.open("POST", url, false);
			fuelMasterTransactionsRequest.setRequestHeader('Content-type', 'application/json');
			fuelMasterTransactionsRequest.send(params);
        }
	}
}

function sqlFillVehIds(){
	vehIDRequest = createRequest();
	if (vehIDRequest == null) {
		alert("Unable to create request");
	}
	else {
		var url = "ViewFMTR.aspx/FillVehIDDropBox";

		vehIDRequest.onreadystatechange = function () {
			if (vehIDRequest.readyState == 4) {
				if (vehIDRequest.status == 200) {
					console.log(vehIDRequest);
					document.getElementById("fuelmastervehid").innerHTML = vehIDRequest.responseText;
				}
			}
        };
		vehIDRequest.open("POST", url, false);
		vehIDRequest.setRequestHeader('Content-type', 'application/json');
		vehIDRequest.send(null);
	}
}

function checkOD() {
	var inputValue = document.getElementById("fuelmastervehod").value;
	if (isNaN(inputValue) || inputValue.includes(".")) {
		alert("Only whole numbers allowed in odometer input");
		document.getElementById("fuelmastervehod").value = clearNonDigits(inputValue);
	}
}