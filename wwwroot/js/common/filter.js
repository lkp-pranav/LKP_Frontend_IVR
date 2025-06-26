document.getElementById("filterForm").addEventListener("submit", function (e) {
    const selectedField = document.getElementById("filterField").value; 
    const inputValue = document.getElementById("filterValue").value;
    const dynamicInput = document.getElementById("dynamicInput");

    // Dynamically set the name and value for dynamicInput
    dynamicInput.name = selectedField;
    dynamicInput.value = inputValue.trim();

    const clientType = document.getElementById("clientType");
    const hiddenClientType = document.getElementById("hiddenClientType");
    if (clientType && hiddenClientType) {
        hiddenClientType.value = clientType.value;
    }
});

document.getElementById("filterField").addEventListener("change", () => {
    const inputValue = document.getElementById("filterValue").value = null;
})