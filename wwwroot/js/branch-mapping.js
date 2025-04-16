document.addEventListener("DOMContentLoaded", function () {
    const zoneDropdown = document.getElementById("zoneDropdown");
    const modal = document.getElementById("addBranchModal");
    const dealerDropdown = document.getElementById("dealerDropdown");
    const ctclInput = document.querySelector('input[name="CtclLoginId"]');
    const dealerIdInput = document.querySelector('input[name="DealerID"]');
    const rowIdInput = document.getElementById("rowIdInput");
    const form = document.getElementById("branchForm");

    if (!zoneDropdown || !modal || !dealerDropdown || !ctclInput || !form) return;

    // Populate zones when modal is shown
    modal.addEventListener("shown.bs.modal", function () {
        fetch('/Common/GetZones', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            }
        })
        .then(res => res.json())
        .then(response => {
            console.log(response.data)
            if (response.isSuccess && response.data) {
                zoneDropdown.innerHTML = '<option value="">Select Zone</option>';
                response.data.forEach(zone => {
                    const opt = document.createElement("option");
                    opt.value = zone;
                    opt.textContent = zone;
                    zoneDropdown.appendChild(opt);
                });
            }
            console.log(zoneDropdown)
        })
        .catch(error => console.error("Error fetching zones:", error));
    });

    // Clear form when modal is hidden
    modal.addEventListener("hidden.bs.modal", function () {
        form.reset();
        zoneDropdown.innerHTML = '<option value="">Select Zone</option>';
        dealerDropdown.innerHTML = '<option value="">Select Dealer</option>';
        ctclInput.value = "-";
        rowIdInput.value = "";
        form.action = "/BranchMapping/CreateMapping";
        document.getElementById("addBranchModalLabel").textContent = "Add Branch Mapping";
    });

    // Fetch dealers based on zone
    zoneDropdown.addEventListener("change", function () {
        const selectedZone = this.value;
        dealerDropdown.innerHTML = '<option value="">Select Dealer</option>';
        ctclInput.value = "";

        if (!selectedZone) return;

        fetch(`/Common/GetDealerByZone?Zone=${encodeURIComponent(selectedZone)}`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            }
        })
        .then(res => res.json())
        .then(response => {
            console.log(response);
            if (response.isSuccess && response.data) {
                response.data.forEach(dealerObj => {
                    const opt = document.createElement("option");
                    opt.value = dealerObj.dealerName;
                    opt.textContent = dealerObj.dealerName;
                    opt.setAttribute("data-ctcl", dealerObj.ctclLoginid);
                    opt.setAttribute("data-dealerId", dealerObj.dealerID);
                    dealerDropdown.appendChild(opt);
                });
            }
        })
        .catch(error => console.error("Error fetching dealers:", error));
    });

    // Set CTCL on dealer change
    dealerDropdown.addEventListener("change", function () {
        const selectedOption = dealerDropdown.options[dealerDropdown.selectedIndex];
        const ctcl = selectedOption.getAttribute("data-ctcl");
        const dealerId = selectedOption.getAttribute("data-dealerId");
        ctclInput.value = ctcl || "-";
        dealerIdInput.value = dealerId || "-";
    });
});
