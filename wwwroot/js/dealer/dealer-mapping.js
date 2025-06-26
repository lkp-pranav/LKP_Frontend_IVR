document.addEventListener("DOMContentLoaded", function () {
    const modal = document.getElementById("addDealerModal");

    const zoneDropdown = document.getElementById("zoneDropdownCreate");
    const primaryDealerDropdown = document.getElementById("primaryDealerDropdownCreate");
    const secondaryDealerDropdown = document.getElementById("secondaryDealerDropdownCreate");

    const primaryDealerId = document.getElementById("primaryDealerIdCreate");
    const secondaryDealerId = document.getElementById("secondaryDealerIdCreate");

    const primaryCTCL = document.getElementById("primaryCTCLCreate");
    const secondaryCTCL = document.getElementById("secondaryCTCLCreate");

    const primarySegment = document.getElementById("primarySegmentList");
    const secondarySegment = document.getElementById("secondarySegmentList");

    const resetDropdown = (dropdown, placeholder = "Select") => {
        dropdown.innerHTML = `<option value="">${placeholder}</option>`;
    };

    const resetDealerInfo = () => {
        [primaryCTCL, secondaryCTCL, primaryDealerId, secondaryDealerId].forEach(el => el.value = "-");
        [primarySegment, secondarySegment].forEach(el => el.innerHTML = "-");
    };

    const populateDealers = (dealers, ...dropdowns) => {
        const options = dealers.map(dealer => {
            const opt = document.createElement("option");
            opt.value = dealer.dealerName;
            opt.textContent = dealer.dealerName;
            opt.dataset.ctcl = dealer.ctclLoginid;
            opt.dataset.dealerid = dealer.dealerID;
            return opt;
        });

        dropdowns.forEach(dropdown => {
            resetDropdown(dropdown, "Select Dealer");
            options.forEach(opt => dropdown.appendChild(opt.cloneNode(true)));
        });
    };

    modal.addEventListener("shown.bs.modal", function () {
        fetch(`${window.appBasePath}/Common/GetZones`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' }
        })
            .then(res => res.json())
            .then(response => {
                if (response.isSuccess && response.data) {
                    resetDropdown(zoneDropdown, "Select Zone");
                    response.data.forEach(zone => {
                        if (zone != "PPAL") {
                            const opt = document.createElement("option");
                            opt.value = zone;
                            opt.textContent = zone;
                            zoneDropdown.appendChild(opt);
                        }
                    });
                }
            })
            .catch(err => console.error("Error fetching zones:", err));
    });

    modal.addEventListener("hidden.bs.modal", function () {
        modal.querySelectorAll("input, select").forEach(el => {
            if (el.tagName === "SELECT") el.selectedIndex = 0;
            else el.value = "-";
        });

        resetDealerInfo();
        if (secondaryDealerDropdown) secondaryDealerDropdown.disabled = true;
    });

    zoneDropdown.addEventListener("change", function () {
        const selectedZone = this.value;

        resetDropdown(primaryDealerDropdown, "Select Dealer");
        resetDropdown(secondaryDealerDropdown, "Select Dealer");
        secondaryDealerDropdown.disabled = true;
        resetDealerInfo();

        if (!selectedZone) return;

        fetch(`${window.appBasePath}/Common/GetDealerByZone?Zone=${encodeURIComponent(selectedZone)}`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' }
        })
            .then(res => res.json())
            .then(response => {
                if (response.isSuccess && response.data) {
                    populateDealers(response.data, primaryDealerDropdown, secondaryDealerDropdown);
                }
            })
            .catch(err => console.error("Error fetching dealers:", err));
    });

    primaryDealerDropdown.addEventListener("change", function () {
        const selected = this.selectedOptions[0];
        const selectedValue = this.value;

        primaryCTCL.value = selected?.dataset.ctcl || "-";
        primaryDealerId.value = selected?.dataset.dealerid || "-";

        if (selectedValue) {
            secondaryDealerDropdown.disabled = false;

            const validOptions = Array.from(primaryDealerDropdown.options)
                .filter(opt => opt.value && opt.value !== selectedValue);

            resetDropdown(secondaryDealerDropdown, "Select Dealer");
            validOptions.forEach(opt => {
                const clone = opt.cloneNode(true);
                secondaryDealerDropdown.appendChild(clone);
            });

            secondaryCTCL.value = "-";
            secondaryDealerId.value = "-";
            secondarySegment.innerHTML = "-";
        } else {
            secondaryDealerDropdown.disabled = true;
            resetDropdown(secondaryDealerDropdown, "Select Dealer");
            secondaryCTCL.value = "-";
            secondaryDealerId.value = "-";
            secondarySegment.innerHTML = "-";
        }
    });

    secondaryDealerDropdown.addEventListener("change", function () {
        const selected = this.selectedOptions[0];
        secondaryCTCL.value = selected?.dataset.ctcl || "-";
        secondaryDealerId.value = selected?.dataset.dealerid || "-";
    });
});
