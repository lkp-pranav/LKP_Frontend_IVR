//document.addEventListener("DOMContentLoaded", () => {

//    // Delete Logic
//    document.querySelectorAll(".delete-btn").forEach(button => {
//        button.addEventListener("click", () => {
//            const rowId = button.getAttribute("data-id");

//            fetch(`/DealerMapping/DeleteMapping?rowId=${rowId}`, {
//                method: "DELETE"
//            })
//                .then(response => {
//                    if (response.redirected) {
//                        window.location.href = response.url;
//                    } else {
//                        return response.json();
//                    }
//                })
//                .catch(err => console.error("Error during deletion:", err));
//        });
//    });

//    // Update Logic
//    const updateModal = new bootstrap.Modal(document.getElementById("updateDealerModal"));
//    const addDealerModal = document.getElementById("addDealerModal");
//    const zoneDropdownUpdate = document.getElementById("zoneDropdown");
//    const primaryDealerDropdownUpdate = document.getElementById("primaryDealerDropdownUpdate");
//    const secondaryDealerDropdownUpdate = document.getElementById("secondaryDealerDropdownUpdate");

//    updateDealerModal.addEventListener('hidden.bs.modal', () => {
//        clearFormFields();
//    });

//    addDealerModal.addEventListener('hidden.bs.modal', () => {
//        clearFormFields();
//    });

//    function clearFormFields() {
//        // Clear the form fields inside the update modal
//        document.getElementById("rowIdInputUpdate").value = "";

//        // Zone
//        document.getElementById("zoneDropdown").innerHTML = '<option value="">Select Zone</option>';

//        // Dealer dropdowns
//        const primaryDealerDropdownUpdate = document.getElementById("primaryDealerDropdown");
//        const secondaryDealerDropdownUpdate = document.getElementById("secondaryDealerDropdown");

//        primaryDealerDropdownUpdate.innerHTML = '<option value="">Select Dealer</option>';
//        secondaryDealerDropdownUpdate.innerHTML = '<option value="">Select Dealer</option>';

//        // Clear all input fields
//        document.getElementById("primaryDealerId").value = "-";
//        document.getElementById("secondaryDealerId").value = "-";
//        document.getElementById("primaryCTCL").value = "-";
//        document.getElementById("secondaryCTCL").value = "-";
//        document.getElementById("primarySegment").value = "-";
//        document.getElementById("secondarySegment").value = "-";
//    }

//    document.querySelectorAll(".update-btn").forEach(button => {
//        button.addEventListener("click", function () {
//            const row = document.querySelector(`tr[data-rowid="${this.dataset.id}"]`);

//            const rowId = row.getAttribute("data-rowid");
//            const zone = row.children[0].innerText.trim();  // Extract zone from the row
//            const primaryDealer = row.children[1].innerText.trim();
//            const primaryDealerName = row.children[2].innerText.trim();
//            const primaryCTCLLogin = row.children[3].innerText.trim();
//            const secondaryDealer = row.children[4].innerText.trim();
//            const secondaryDealerName = row.children[5].innerText.trim();
//            const secondaryCTCLLogin = row.children[6].innerText.trim();
//            console.log(zone)
//            console.log(rowId, primaryCTCLLogin, primaryDealer, primaryDealerName)
//            console.log(rowId, secondaryCTCLLogin, secondaryDealer, secondaryDealerName)

//            // Set the hidden input field for RowId
//            document.getElementById("rowIdInputUpdate").value = rowId;

//            // Pre-populate Zone dropdown (it should be read-only, so no need to populate with other zones)
//            zoneDropdownUpdate.innerHTML = `<option value="${zone}" selected>${zone}</option>`; // Only the selected zone is shown

//            // Fetch dealers for the selected zone (primary dealer dropdown)
//            fetch(`/Common/GetDealerByZone?Zone=${encodeURIComponent(zone)}`)
//                .then(res => res.json())
//                .then(response => {
//                    if (response.isSuccess && response.data) {
//                        console.log(response)
//                        // Clear previous options
//                        primaryDealerDropdownUpdate.innerHTML = '<option value="">Select Dealer</option>';
//                        secondaryDealerDropdownUpdate.innerHTML = '<option value="">Select Dealer</option>';

//                        // Flag to check if the primary and secondary dealer options are found
//                        let primaryDealerFound = false;
//                        let secondaryDealerFound = false;

//                        response.data.forEach(d => {
//                            const primaryOption = document.createElement("option");
//                            primaryOption.value = d.dealerName;
//                            primaryOption.textContent = d.dealerName;
//                            primaryOption.setAttribute("data-dealerId", d.dealerID);
//                            primaryOption.setAttribute("data-ctcl", d.ctclLoginid);

//                             //Check if the primary dealer matches and select it
//                            if (d.dealerName === primaryDealerName) {
//                                primaryOption.selected = true;
//                                primaryDealerFound = true;
//                            }

//                            primaryDealerDropdownUpdate.appendChild(primaryOption);

//                            const secondaryOption = document.createElement("option");
//                            secondaryOption.value = d.dealerName;
//                            secondaryOption.textContent = d.dealerName;
//                            secondaryOption.setAttribute("data-dealerId", d.dealerID);
//                            secondaryOption.setAttribute("data-ctcl", d.ctclLoginid);

//                            // Check if the secondary dealer matches and select it
//                            if (d.dealerName === secondaryDealerName) {
//                                secondaryOption.selected = true;
//                                secondaryDealerFound = true;
//                            }

//                            secondaryDealerDropdownUpdate.appendChild(secondaryOption);
//                        });

//                        // If a dealer was not found in the dropdown, fall back to pre-selected value
//                        if (!primaryDealerFound) {
//                            document.querySelector("#updateDealerForm input[name='PrimaryDealer']").value = primaryDealer;
//                        } else {
//                            // Set the Primary Dealer ID and CTCL Login values
//                            const selectedPrimary = primaryDealerDropdownUpdate.querySelector(`option[selected]`);
//                            document.querySelector("#updateDealerForm input[name='PrimaryDealer']").value = selectedPrimary.getAttribute("data-dealerId") || "-";
//                            document.querySelector("#updateDealerForm input[name='PrimaryDealerCTCLLogin']").value = selectedPrimary.getAttribute("data-ctcl") || "-";
//                        }

//                        if (!secondaryDealerFound) {
//                            document.querySelector("#updateDealerForm input[name='SecondaryDealer']").value = secondaryDealer;
//                        } else {
//                            // Set the Secondary Dealer ID and CTCL Login values
//                            const selectedSecondary = secondaryDealerDropdownUpdate.querySelector(`option[selected]`);
//                            document.querySelector("#updateDealerForm input[name='SecondaryDealer']").value = selectedSecondary.getAttribute("data-dealerId") || "-";
//                            document.querySelector("#updateDealerForm input[name='SecondaryDealerCTCLLogin']").value = selectedSecondary.getAttribute("data-ctcl") || "-";
//                        }

//                         //Set other fields
//                        document.querySelector("#updateDealerForm input[name='PrimaryDealerCTCLLogin']").value = primaryCTCLLogin || "-";
//                        document.querySelector("#updateDealerForm input[name='SecondaryDealerCTCLLogin']").value = secondaryCTCLLogin || "-";

//                        // Show the modal
//                        updateModal.show();
//                    }
//                })
//                .catch(err => console.error("Error fetching dealers:", err));
//        });
//    });

//    // Update the Dealer ID and CTCL Login when the primary dealer dropdown changes
//    primaryDealerDropdownUpdate.addEventListener("change", function () {
//        const selectedOption = this.options[this.selectedIndex];
//        document.querySelector("#updateDealerForm input[name='PrimaryDealer']").value = selectedOption.getAttribute("data-dealerId") || "-";
//        document.querySelector("#updateDealerForm input[name='PrimaryDealerCTCLLogin']").value = selectedOption.getAttribute("data-ctcl") || "-";
//    });

//    // Update the Dealer ID and CTCL Login when the secondary dealer dropdown changes
//    secondaryDealerDropdownUpdate.addEventListener("change", function () {
//        const selectedOption = this.options[this.selectedIndex];
//        document.querySelector("#updateDealerForm input[name='SecondaryDealer']").value = selectedOption.getAttribute("data-dealerId") || "-";
//        document.querySelector("#updateDealerForm input[name='SecondaryDealerCTCLLogin']").value = selectedOption.getAttribute("data-ctcl") || "-";
//    });
//});

//document.addEventListener("DOMContentLoaded", () => {

//    // Delete Logic
//    document.querySelectorAll(".delete-btn").forEach(button => {
//        button.addEventListener("click", () => {
//            const rowId = button.getAttribute("data-id");

//            fetch(`/DealerMapping/DeleteMapping?rowId=${rowId}`, {
//                method: "DELETE"
//            })
//                .then(response => {
//                    if (response.redirected) {
//                        window.location.href = response.url;
//                    } else {
//                        return response.json();
//                    }
//                })
//                .catch(err => console.error("Error during deletion:", err));
//        });
//    });

//    // Update Logic
//    const updateModal = new bootstrap.Modal(document.getElementById("updateDealerModal"));
//    const addDealerModal = document.getElementById("addDealerModal");

//    const zoneDropdownUpdate = document.getElementById("zoneDropdownUpdate");
//    const primaryDealerDropdownUpdate = document.getElementById("primaryDealerDropdownUpdate");
//    const secondaryDealerDropdownUpdate = document.getElementById("secondaryDealerDropdownUpdate");

//    document.querySelectorAll(".update-btn").forEach(button => {
//        button.addEventListener("click", function () {
//            const row = document.querySelector(`tr[data-rowid="${this.dataset.id}"]`);
//            console.log(row)

//            const rowId = row.getAttribute("data-rowid");
//            const zone = row.children[0].innerText.trim();
//            const primaryDealer = row.children[1].innerText.trim();
//            const primaryDealerName = row.children[2].innerText.trim();
//            const primaryCTCLLogin = row.children[3].innerText.trim();
//            const secondaryDealer = row.children[4].innerText.trim();
//            const secondaryDealerName = row.children[5].innerText.trim();
//            const secondaryCTCLLogin = row.children[6].innerText.trim();

//            document.getElementById("rowIdInputUpdate").value = rowId;

//            // Only selected zone shown (read-only)
//            zoneDropdownUpdate.innerHTML = `<option value="${zone}" selected>${zone}</option>`;

//            fetch(`/Common/GetDealerByZone?Zone=${encodeURIComponent(zone)}`)
//                .then(res => res.json())
//                .then(response => {
//                    if (response.isSuccess && response.data) {
//                        primaryDealerDropdownUpdate.innerHTML = '<option value="">Select Dealer</option>';
//                        secondaryDealerDropdownUpdate.innerHTML = '<option value="">Select Dealer</option>';

//                        let primaryDealerFound = false;
//                        let secondaryDealerFound = false;

//                        response.data.forEach(d => {
//                            const primaryOption = document.createElement("option");
//                            primaryOption.value = d.dealerName;
//                            primaryOption.textContent = d.dealerName;
//                            primaryOption.setAttribute("data-dealerId", d.dealerID);
//                            primaryOption.setAttribute("data-ctcl", d.ctclLoginid);

//                            if (d.dealerName === primaryDealerName) {
//                                primaryOption.selected = true;
//                                primaryDealerFound = true;
//                            }

//                            primaryDealerDropdownUpdate.appendChild(primaryOption);

//                            const secondaryOption = document.createElement("option");
//                            secondaryOption.value = d.dealerName;
//                            secondaryOption.textContent = d.dealerName;
//                            secondaryOption.setAttribute("data-dealerId", d.dealerID);
//                            secondaryOption.setAttribute("data-ctcl", d.ctclLoginid);

//                            if (d.dealerName === secondaryDealerName) {
//                                secondaryOption.selected = true;
//                                secondaryDealerFound = true;
//                            }

//                            secondaryDealerDropdownUpdate.appendChild(secondaryOption);
//                        });

//                        if (!primaryDealerFound) {
//                            document.querySelector("#updateDealerForm input[name='PrimaryDealer']").value = primaryDealer;
//                        } else {
//                            const selectedPrimary = primaryDealerDropdownUpdate.querySelector(`option[selected]`);
//                            document.querySelector("#updateDealerForm input[name='PrimaryDealer']").value = selectedPrimary.getAttribute("data-dealerId") || "-";
//                            document.querySelector("#updateDealerForm input[name='PrimaryDealerCTCLLogin']").value = selectedPrimary.getAttribute("data-ctcl") || "-";
//                        }

//                        if (!secondaryDealerFound) {
//                            document.querySelector("#updateDealerForm input[name='SecondaryDealer']").value = secondaryDealer;
//                        } else {
//                            const selectedSecondary = secondaryDealerDropdownUpdate.querySelector(`option[selected]`);
//                            document.querySelector("#updateDealerForm input[name='SecondaryDealer']").value = selectedSecondary.getAttribute("data-dealerId") || "-";
//                            document.querySelector("#updateDealerForm input[name='SecondaryDealerCTCLLogin']").value = selectedSecondary.getAttribute("data-ctcl") || "-";
//                        }

//                        document.querySelector("#updateDealerForm input[name='PrimaryDealerCTCLLogin']").value = primaryCTCLLogin || "-";
//                        document.querySelector("#updateDealerForm input[name='SecondaryDealerCTCLLogin']").value = secondaryCTCLLogin || "-";

//                        updateModal.show();
//                    }
//                })
//                .catch(err => console.error("Error fetching dealers:", err));
//        });
//    });

//    primaryDealerDropdownUpdate.addEventListener("change", function () {
//        const selectedOption = this.options[this.selectedIndex];
//        document.querySelector("#updateDealerForm input[name='PrimaryDealer']").value = selectedOption.getAttribute("data-dealerId") || "-";
//        document.querySelector("#updateDealerForm input[name='PrimaryDealerCTCLLogin']").value = selectedOption.getAttribute("data-ctcl") || "-";
//    });

//    secondaryDealerDropdownUpdate.addEventListener("change", function () {
//        const selectedOption = this.options[this.selectedIndex];
//        document.querySelector("#updateDealerForm input[name='SecondaryDealer']").value = selectedOption.getAttribute("data-dealerId") || "-";
//        document.querySelector("#updateDealerForm input[name='SecondaryDealerCTCLLogin']").value = selectedOption.getAttribute("data-ctcl") || "-";
//    });
//});


document.addEventListener("DOMContentLoaded", () => {

    // Delete Logic
    document.querySelectorAll(".delete-btn").forEach(button => {
        button.addEventListener("click", () => {
            const rowId = button.getAttribute("data-id");

            fetch(`/DealerMapping/DeleteMapping?rowId=${rowId}`, {
                method: "DELETE"
            })
                .then(response => {
                    if (response.redirected) {
                        window.location.href = response.url;
                    } else {
                        return response.json();
                    }
                })
                .catch(err => console.error("Error during deletion:", err));
        });
    });

    // Update Logic
    const updateModal = new bootstrap.Modal(document.getElementById("updateDealerModal"));
    const addDealerModal = document.getElementById("addDealerModal");

    const zoneDropdownUpdate = document.getElementById("zoneDropdownUpdate");
    const primaryDealerDropdownUpdate = document.getElementById("primaryDealerDropdownUpdate");
    const secondaryDealerDropdownUpdate = document.getElementById("secondaryDealerDropdownUpdate");

    const primarySegmentField = document.getElementById("primarySegmentUpdate");
    const secondarySegmentField = document.getElementById("secondarySegmentUpdate");

    // Helper to fetch and set segment
    function fetchSegment(dealerId, segmentField) {
        if (!dealerId || dealerId === "-") {
            segmentField.value = "-";
            return;
        }

        fetch(`/Common/GetDealerSegment?dealer=${encodeURIComponent(dealerId)}`)
            .then(res => res.json())
            .then(response => {
                if (response.isSuccess && response.data) {
                    segmentField.value = response.data;
                } else {
                    segmentField.value = "-";
                }
            })
            .catch(err => {
                console.error("Error fetching segment:", err);
                segmentField.value = "-";
            });
    }

    document.querySelectorAll(".update-btn").forEach(button => {
        button.addEventListener("click", function () {
            const row = document.querySelector(`tr[data-rowid="${this.dataset.id}"]`);

            const rowId = row.getAttribute("data-rowid");
            const zone = row.children[0].innerText.trim();
            const primaryDealer = row.children[1].innerText.trim();
            const primaryDealerName = row.children[2].innerText.trim();
            const primaryCTCLLogin = row.children[3].innerText.trim();
            const secondaryDealer = row.children[4].innerText.trim();
            const secondaryDealerName = row.children[5].innerText.trim();
            const secondaryCTCLLogin = row.children[6].innerText.trim();

            document.getElementById("rowIdInputUpdate").value = rowId;
            zoneDropdownUpdate.innerHTML = `<option value="${zone}" selected>${zone}</option>`;

            fetch(`/Common/GetDealerByZone?Zone=${encodeURIComponent(zone)}`)
                .then(res => res.json())
                .then(response => {
                    if (response.isSuccess && response.data) {
                        primaryDealerDropdownUpdate.innerHTML = '<option value="">Select Dealer</option>';
                        secondaryDealerDropdownUpdate.innerHTML = '<option value="">Select Dealer</option>';

                        let primaryDealerFound = false;
                        let secondaryDealerFound = false;

                        response.data.forEach(d => {
                            const primaryOption = document.createElement("option");
                            primaryOption.value = d.dealerName;
                            primaryOption.textContent = d.dealerName;
                            primaryOption.setAttribute("data-dealerId", d.dealerID);
                            primaryOption.setAttribute("data-ctcl", d.ctclLoginid);

                            if (d.dealerName === primaryDealerName) {
                                primaryOption.selected = true;
                                primaryDealerFound = true;
                            }

                            primaryDealerDropdownUpdate.appendChild(primaryOption);

                            const secondaryOption = document.createElement("option");
                            secondaryOption.value = d.dealerName;
                            secondaryOption.textContent = d.dealerName;
                            secondaryOption.setAttribute("data-dealerId", d.dealerID);
                            secondaryOption.setAttribute("data-ctcl", d.ctclLoginid);

                            if (d.dealerName === secondaryDealerName) {
                                secondaryOption.selected = true;
                                secondaryDealerFound = true;
                            }

                            secondaryDealerDropdownUpdate.appendChild(secondaryOption);
                        });

                        const selectedPrimary = primaryDealerDropdownUpdate.querySelector(`option[value="${primaryDealerDropdownUpdate.value}"]`);


                        const selectedSecondary = secondaryDealerDropdownUpdate.querySelector(`option[value="${secondaryDealerDropdownUpdate.value}"]`);


                        if (!primaryDealerFound) {
                            document.querySelector("#updateDealerForm input[name='PrimaryDealer']").value = primaryDealer;
                        } else {
                            document.querySelector("#updateDealerForm input[name='PrimaryDealer']").value = selectedPrimary.getAttribute("data-dealerId") || "-";
                            document.querySelector("#updateDealerForm input[name='PrimaryDealerCTCLLogin']").value = selectedPrimary.getAttribute("data-ctcl") || "-";
                            fetchSegment(selectedPrimary.getAttribute("data-dealerId"), primarySegmentField);
                        }

                        if (!secondaryDealerFound) {
                            document.querySelector("#updateDealerForm input[name='SecondaryDealer']").value = secondaryDealer;
                        } else {
                            document.querySelector("#updateDealerForm input[name='SecondaryDealer']").value = selectedSecondary.getAttribute("data-dealerId") || "-";
                            document.querySelector("#updateDealerForm input[name='SecondaryDealerCTCLLogin']").value = selectedSecondary.getAttribute("data-ctcl") || "-";
                            fetchSegment(selectedSecondary.getAttribute("data-dealerId"), secondarySegmentField);
                        }

                        document.querySelector("#updateDealerForm input[name='PrimaryDealerCTCLLogin']").value = primaryCTCLLogin || "-";
                        document.querySelector("#updateDealerForm input[name='SecondaryDealerCTCLLogin']").value = secondaryCTCLLogin || "-";

                        updateModal.show();
                    }
                })
                .catch(err => console.error("Error fetching dealers:", err));
        });
    });

    // Handle dropdown changes
    primaryDealerDropdownUpdate.addEventListener("change", function () {
        const selectedOption = this.options[this.selectedIndex];
        const dealerId = selectedOption.getAttribute("data-dealerId");
        console.log("primary dealer id: ", dealerId)
        const ctcl = selectedOption.getAttribute("data-ctcl");

        document.querySelector("#updateDealerForm input[name='PrimaryDealer']").value = dealerId || "-";
        document.querySelector("#updateDealerForm input[name='PrimaryDealerCTCLLogin']").value = ctcl || "-";

        fetchSegment(dealerId, primarySegmentField);
    });

    secondaryDealerDropdownUpdate.addEventListener("change", function () {
        const selectedOption = this.options[this.selectedIndex];
        const dealerId = selectedOption.getAttribute("data-dealerId");
        console.log("secondary dealer id: ", dealerId)
        const ctcl = selectedOption.getAttribute("data-ctcl");

        document.querySelector("#updateDealerForm input[name='SecondaryDealer']").value = dealerId || "-";
        document.querySelector("#updateDealerForm input[name='SecondaryDealerCTCLLogin']").value = ctcl || "-";

        fetchSegment(dealerId, secondarySegmentField);
    });
});
