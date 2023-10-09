// Load the JWT token from localStorage
var token = localStorage.getItem("jwtToken");

// Set the token in Swagger UI, if available
if (token) {
    var swaggerUI = document.querySelector("#swagger-ui");
    swaggerUI.addEventListener("DOMContentLoaded", function () {
        setTimeout(function () {
            var authorizeButton = document.querySelector(".authorize-wrapper .btn");
            if (authorizeButton) {
                authorizeButton.click(); // Open the "Authorize" dialog
            }

            var tokenInput = document.querySelector(".authorize__input");
            if (tokenInput) {
                tokenInput.value = "Bearer " + token; // Set the token value
            }

            var authorizeAuthorizeButton = document.querySelector(".modal-ux .btn.authorize__btn");
            if (authorizeAuthorizeButton) {
                authorizeAuthorizeButton.click(); // Authorize with the token
            }
        }, 1000); // Delay execution to ensure Swagger UI is fully loaded
    });
}