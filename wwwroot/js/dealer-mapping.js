document.addEventListener("DOMContentLoaded", function () {
    const zoneDropdown = document.getElementById("zoneDropdown");
    const modal = document.getElementById("addDealerModal");

    const primaryDealerDropdown = document.getElementById("primaryDealerDropdown");
    const secondaryDealerDropdown = document.getElementById("secondaryDealerDropdown");

    const primaryDealerId = document.getElementById("primaryDealerId");
    const secondaryDealerId = document.getElementById("secondaryDealerId");

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
        const inputs = modal.querySelectorAll("input, select");
        inputs.forEach(input => {
            if (input.tagName === "SELECT") {
                input.selectedIndex = 0;
            } else {
                input.value = "-";
            }
        });

        if (secondaryDealerDropdown) {
            secondaryDealerDropdown.disabled = true;
        }
    });

    zoneDropdown.addEventListener("change", function () {
        const selectedZone = this.value;

        primaryDealerDropdown.innerHTML = '<option value="">Select Dealer</option>';
        secondaryDealerDropdown.innerHTML = '<option value="">Select Dealer</option>';
        primaryCTCL.value = "-";
        secondaryCTCL.value = "-";
        primaryDealerId.value = "-";
        secondaryDealerId.value = "-";
        secondaryDealerDropdown.disabled = true;

        if (!selectedZone) return;

        fetch(`/Common/GetDealerByZone?Zone=${encodeURIComponent(selectedZone)}`)
            .then(res => res.json())
            .then(response => {
                if (response.isSuccess && response.data) {
                    const dealerOptions = response.data.map(dealerObj => {
                        const opt = document.createElement("option");
                        opt.value = dealerObj.dealerName;
                        opt.textContent = dealerObj.dealerName;
                        opt.setAttribute("data-ctcl", dealerObj.ctclLoginid);
                        opt.setAttribute("data-dealerid", dealerObj.dealerID);
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
        const selectedPrimaryOption = this.options[this.selectedIndex];
        const ctcl = selectedPrimaryOption.getAttribute("data-ctcl");
        const dealerId = selectedPrimaryOption.getAttribute("data-dealerid");

        primaryCTCL.value = ctcl || "-";
        primaryDealerId.value = dealerId || "-";

        if (this.value) {
            secondaryDealerDropdown.disabled = false;

            // Re-populate secondary dealer dropdown excluding selected primary
            const allPrimaryOptions = Array.from(primaryDealerDropdown.options).filter(opt => opt.value && opt.value !== this.value);

            secondaryDealerDropdown.innerHTML = '<option value="">Select Dealer</option>';
            allPrimaryOptions.forEach(opt => {
                const newOpt = document.createElement("option");
                newOpt.value = opt.value;
                newOpt.textContent = opt.textContent;
                newOpt.setAttribute("data-ctcl", opt.getAttribute("data-ctcl"));
                newOpt.setAttribute("data-dealerId", opt.getAttribute("data-dealerid"));
                secondaryDealerDropdown.appendChild(newOpt);
            });

            secondaryCTCL.value = "-";
            secondaryDealerId.value = "-";
        } else {
            secondaryDealerDropdown.disabled = true;
            secondaryDealerDropdown.innerHTML = '<option value="">Select Dealer</option>';
            secondaryCTCL.value = "-";
            secondaryDealerId.value = "-";
        }
    });

    secondaryDealerDropdown.addEventListener("change", function () {
        const selectedSecondaryOption = this.options[this.selectedIndex];
        const ctcl = selectedSecondaryOption.getAttribute("data-ctcl");
        const dealerId = selectedSecondaryOption.getAttribute("data-dealerid");

        secondaryCTCL.value = ctcl || "-";
        secondaryDealerId.value = dealerId || "-";
    });
});
