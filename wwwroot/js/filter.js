document.getElementById("filterForm").addEventListener("submit", function (e) {
    const selectedField = document.getElementById("filterField").value;  // Corrected ID
    const inputValue = document.getElementById("filterValue").value;
    const dynamicInput = document.getElementById("dynamicInput");

    // Dynamically set the name and value for dynamicInput
    dynamicInput.name = selectedField;
    dynamicInput.value = inputValue;
});
