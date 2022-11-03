window.onload = init;

function init() {
	fillMasterRouteList();

	document.getElementById("delroute").onclick = sqlDelMasterRoute;
}

function sqlDelMasterRoute() {
	delMRRequest = createRequest();
	if (delMRRequest == null) {
		alert("Unable to create request");
	}
	else {

		var masterRouteElement = document.getElementById("masterroutelist");
		var routeIdVal = masterRouteElement.options[masterRouteElement.selectedIndex].value;

		var url = "DelMRTemplate.aspx/DelMasterRouteSQL";
		var params = JSON.stringify({ "pk_route_id": routeIdVal });
			
		delMRRequest.onreadystatechange = function addMasterRoute() {
			if (delMRRequest.readyState == 4) {
				if (delMRRequest.status == 200) {
					document.getElementById("results").innerHTML = delMRRequest.responseText;
				}
			}
		};

		delMRRequest.open("POST", url, false);
		delMRRequest.setRequestHeader('Content-type', 'application/json');
		delMRRequest.send(params);
	}

	fillMasterRouteList();
}

function fillMasterRouteList() {
	masterRoutePreviewRequest = createRequest();
	if (masterRoutePreviewRequest == null) {
		alert("Unable to create request");
	}
	else {
		var url = "DelMRTemplate.aspx/FillMasterRouteList";

		masterRoutePreviewRequest.onreadystatechange = function () {
			if (masterRoutePreviewRequest.readyState == 4) {
				if (masterRoutePreviewRequest.status == 200) {
					document.getElementById("masterroutelist").innerHTML = masterRoutePreviewRequest.responseText;
				}
				else {
					console.log(masterRoutePreviewRequest);
				}
			}
		};
		masterRoutePreviewRequest.open("POST", url, false);
		masterRoutePreviewRequest.setRequestHeader('Content-type', 'application/json');
		masterRoutePreviewRequest.send(null);
	}
}