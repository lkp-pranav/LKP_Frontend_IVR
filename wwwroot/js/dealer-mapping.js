document.addEventListener("DOMContentLoaded", function () {
    const zoneDropdown = document.getElementById("zoneDropdown");
    const modal = document.getElementById("addDealerModal");

    const primaryDealerDropdown = document.getElementById("primaryDealerDropdown");
    const secondaryDealerDropdown = document.getElementById("secondaryDealerDropdown");

    const primaryCTCL = document.getElementById("primaryCTCL");
    const secondaryCTCL = document.getElementById("secondaryCTCL");

    const primarySegment = document.getElementById("primarySegment");
    const secondarySegment = document.getElementById("secondarySegment");

    modal.addEventListener("shown.bs.modal", function () {
        fetch('/Common/GetZones')
            .then(res => res.json())
            .then(response => {
                if (response.isSuccess && response.data) {
                    zoneDropdown.innerHTML = '<option value="">Select Zone</option>';
                    response.data.forEach(zone => {
                        const opt = document.createElement("option");
                        opt.value = zone;
                        opt.textContent = zone;
                        zoneDropdown.appendChild(opt);
                    });
                }
            })
            .catch(error => console.error("Error fetching zones:", error));
    });

    modal.addEventListener("hidden.bs.modal", function () {
        // Reset all input/select fields inside the modal
        const inputs = modal.querySelectorAll("input, select");

        inputs.forEach(input => {
            if (input.tagName === "SELECT") {
                input.selectedIndex = 0;
            } else {
                input.value = "-";
            }
        });

        // Disable secondary dealer dropdown again
        const secondaryDealerDropdown = document.getElementById("secondaryDealerDropdown");
        if (secondaryDealerDropdown) {
            secondaryDealerDropdown.disabled = true;
        }
    });

    zoneDropdown.addEventListener("change", function () {
        const selectedZone = this.value;

        // Clear dealer dropdowns and CTCL fields
        primaryDealerDropdown.innerHTML = '<option value="">Select Dealer</option>';
        secondaryDealerDropdown.innerHTML = '<option value="">Select Dealer</option>';
        primaryCTCL.value = "-";
        secondaryCTCL.value = "-";
        secondaryDealerDropdown.disabled = true;

        if (!selectedZone) {
            return;
        }

        fetch(`/Common/GetDealerByZone?Zone=${encodeURIComponent(selectedZone)}`)
            .then(res => res.json())
            .then(response => {
                if (response.isSuccess && response.data) {
                    const dealerOptions = response.data.map(dealerObj => {
                        const opt = document.createElement("option");
                        opt.value = dealerObj.dealer;
                        opt.textContent = dealerObj.dealer;
                        opt.setAttribute("data-ctcl", dealerObj.ctclLoginid);
                        return opt;
                    });

                    dealerOptions.forEach(opt => {
                        primaryDealerDropdown.appendChild(opt.cloneNode(true));
                        secondaryDealerDropdown.appendChild(opt.cloneNode(true));
                    });
                }
            })
            .catch(error => console.error("Error fetching dealers:", error));
    });

    primaryDealerDropdown.addEventListener("change", function () {
        const ctcl = this.options[this.selectedIndex].getAttribute("data-ctcl");
        primaryCTCL.value = ctcl || "-";
    });

    secondaryDealerDropdown.addEventListener("change", function () {
        const ctcl = this.options[this.selectedIndex].getAttribute("data-ctcl");
        secondaryCTCL.value = ctcl || "-";
    });

    primaryDealerDropdown.addEventListener("change", function () {
        const selectedPrimaryValue = this.value;
        const selectedPrimaryOption = this.options[this.selectedIndex];
        const ctcl = selectedPrimaryOption.getAttribute("data-ctcl");
        primaryCTCL.value = ctcl || "-";

        // Enable or disable secondary dropdown
        if (selectedPrimaryValue) {
            secondaryDealerDropdown.disabled = false;

            // Re-populate secondary dropdown excluding the selected primary dealer
            const allPrimaryOptions = Array.from(primaryDealerDropdown.options).filter(opt => opt.value && opt.value !== selectedPrimaryValue);

            secondaryDealerDropdown.innerHTML = '<option value="">Select Dealer</option>';
            allPrimaryOptions.forEach(opt => {
                const newOpt = document.createElement("option");
                newOpt.value = opt.value;
                newOpt.textContent = opt.textContent;
                newOpt.setAttribute("data-ctcl", opt.getAttribute("data-ctcl"));
                secondaryDealerDropdown.appendChild(newOpt);
            });

            // Clear previously selected secondary
            secondaryCTCL.value = "-";
        } else {
            secondaryDealerDropdown.disabled = true;
            secondaryDealerDropdown.innerHTML = '<option value="">Select Dealer</option>';
            secondaryCTCL.value = "-";
        }
    });
});



