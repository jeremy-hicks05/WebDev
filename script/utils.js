var createRequest = function(){
	try {
		// statements
		request = new XMLHttpRequest();
	} catch(tryMS) {
		// statements
		try {
			// statements
			request = new ActiveXObject("Msxml2.XMLHTTP");
		} catch(otherMS) {
			// statements
			try {
				// statements
				request = new ActiveXObject("Microsoft.XMLHTTP");
			} catch(failed) {
				// statements
				request = null;
			}
		}
	}
	return request;
}

function clearNonDigits(element) {
	element = element.replace(/\D/g, ''); // replaces all non digits
	return element;
}