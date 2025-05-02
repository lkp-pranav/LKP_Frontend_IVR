document.addEventListener("DOMContentLoaded", () => {
    // Update Logic
    const updateModal = new bootstrap.Modal(document.getElementById("updateDealerModal"));
    const addDealerModal = document.getElementById("addDealerModal");

    const zoneDropdownUpdate = document.getElementById("zoneDropdownUpdate");
    const primaryDealerDropdownUpdate = document.getElementById("primaryDealerDropdownUpdate");
    const secondaryDealerDropdownUpdate = document.getElementById("secondaryDealerDropdownUpdate");

    const primarySegmentField = document.getElementById("primarySegmentUpdate");
    const secondarySegmentField = document.getElementById("secondarySegmentUpdate");

    // Helper to fetch and set segment
    function fetchSegment(dealerId, targetInput) {
        if (!dealerId || dealerId === "-") {
            targetInput.value = "-";
            return;
        }

        fetch(`/Common/GetDealerSegment?dealer=${encodeURIComponent(dealerId)}`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            }
        })
            .then(res => res.json()) // ✅ Correctly parse JSON here
            .then(response => {
                console.log(`[DEBUG] Parsed response:`, response);

                // Clear any existing content
                targetInput.innerHTML = "";

                if (response.isSuccess && Array.isArray(response.data)) {
                    response.data.forEach(item => {
                        const li = document.createElement("li");
                        li.textContent = item.segment;
                        targetInput.appendChild(li);
                    });
                } else {
                    targetInput.textContent = "No segment assigned";
                }
            })
            .catch(err => {
                //console.error(`[ERROR] Failed to fetch ${label} dealer segment:`, err);
                targetInput.textContent = "-";
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

            fetch(`/Common/GetDealerByZone?Zone=${encodeURIComponent(zone)}`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                }
            })
                .then(res => res.json())
                .then(response => {
                    if (response.isSuccess && response.data) {
                        // Disable or show read-only Primary Dealer dropdown
                        primaryDealerDropdownUpdate.innerHTML = '';
                        const readonlyOption = document.createElement("option");
                        readonlyOption.textContent = primaryDealerName;
                        readonlyOption.value = primaryDealerName;
                        //readonlyOption.disabled = true;
                        readonlyOption.selected = true;
                        primaryDealerDropdownUpdate.appendChild(readonlyOption);
                        //primaryDealerDropdownUpdate.disabled = true;

                        // Fill Secondary Dealer dropdown and exclude primary dealer
                        secondaryDealerDropdownUpdate.innerHTML = '<option value="">Select Dealer</option>';
                        let secondaryDealerFound = false;

                        response.data.forEach(d => {
                            // Skip primary dealer from the secondary dropdown
                            if (d.dealerName === primaryDealerName) return;

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

                        // Set hidden values for primary dealer
                        document.querySelector("#updateDealerForm input[name='PrimaryDealer']").value = primaryDealer;
                        document.querySelector("#updateDealerForm input[name='PrimaryDealerCTCLLogin']").value = primaryCTCLLogin;

                        // Fetch primary segment (read-only)
                        fetchSegment(primaryDealer, primarySegmentField);

                        // Set hidden or editable values for secondary dealer
                        if (!secondaryDealerFound) {
                            document.querySelector("#updateDealerForm input[name='SecondaryDealer']").value = secondaryDealer;
                        } else {
                            const selectedSecondary = secondaryDealerDropdownUpdate.querySelector(`option[value="${secondaryDealerDropdownUpdate.value}"]`);
                            document.querySelector("#updateDealerForm input[name='SecondaryDealer']").value = selectedSecondary?.getAttribute("data-dealerId") || "-";
                            document.querySelector("#updateDealerForm input[name='SecondaryDealerCTCLLogin']").value = selectedSecondary?.getAttribute("data-ctcl") || "-";
                            fetchSegment(selectedSecondary?.getAttribute("data-dealerId"), secondarySegmentField);
                        }

                        // Default fallback for CTCL if not found
                        document.querySelector("#updateDealerForm input[name='SecondaryDealerCTCLLogin']").value = secondaryCTCLLogin || "-";

                        updateModal.show();
                    }
                })
                .catch(err => console.error("Error fetching dealers:", err));
        });
    });

    // Handle secondary dealer dropdown changes
    secondaryDealerDropdownUpdate.addEventListener("change", function () {
        const selectedOption = this.options[this.selectedIndex];
        const dealerId = selectedOption.getAttribute("data-dealerId");
        const ctcl = selectedOption.getAttribute("data-ctcl");

        document.querySelector("#updateDealerForm input[name='SecondaryDealer']").value = dealerId || "-";
        document.querySelector("#updateDealerForm input[name='SecondaryDealerCTCLLogin']").value = ctcl || "-";

        fetchSegment(dealerId, secondarySegmentField);
    });


});
