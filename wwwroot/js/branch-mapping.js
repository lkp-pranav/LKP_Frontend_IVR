document.addEventListener("DOMContentLoaded", function () {
    const zoneDropdown = document.getElementById("zoneDropdown");
    const modal = document.getElementById("addBranchModal");
    const dealerDropdown = document.getElementById("dealerDropdown"); // Make sure your select has this ID
    const ctclInput = document.querySelector('input[name="CtclLoginId"]'); // Input for CTCL

    if (!zoneDropdown || !modal || !dealerDropdown || !ctclInput) return;

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

    zoneDropdown.addEventListener("change", function () {
        const selectedZone = this.value;

        // Clear dealer dropdown and CTCL input
        dealerDropdown.innerHTML = '<option value="">Select Dealer</option>';
        ctclInput.value = "";

        if (!selectedZone) return;

        fetch(`/Common/GetDealerByZone?Zone=${encodeURIComponent(selectedZone)}`)
            .then(res => res.json())
            .then(response => {
                if (response.isSuccess && response.data) {
                    response.data.forEach(dealerObj => {
                        const opt = document.createElement("option");
                        opt.value = dealerObj.dealer;
                        opt.textContent = dealerObj.dealer;
                        opt.setAttribute("data-ctcl", dealerObj.ctclLoginid);
                        dealerDropdown.appendChild(opt);
                    });
                }
            })
            .catch(error => console.error("Error fetching dealers:", error));
    });

    dealerDropdown.addEventListener("change", function () {
        const selectedOption = dealerDropdown.options[dealerDropdown.selectedIndex];
        const ctcl = selectedOption.getAttribute("data-ctcl");
        ctclInput.value = ctcl || "";
    });
});