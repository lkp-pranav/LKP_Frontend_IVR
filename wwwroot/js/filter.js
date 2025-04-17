document.getElementById("filterForm").addEventListener("submit", function (e) {
    const selectedField = document.getElementById("filter").value;
    const inputValue = document.getElementById("filterValue").value;
    const dynamicInput = document.getElementById("dynamicInput");
    console.log(selectedField)
    console.log(inputValue)

    // Set correct name and value dynamically
    dynamicInput.name = selectedField;
    dynamicInput.value = inputValue;
});


