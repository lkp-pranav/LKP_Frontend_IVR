document.addEventListener("DOMContentLoaded", () => {
    const modal = document.getElementById("addCustomGroup");
    const groupCode = document.getElementById("groupCode");
    const zoneDropdown = document.getElementById("zoneDropdown");
    const branchDropdown = document.getElementById("branchDropdown");
    const dealerDropdown = document.getElementById("dealerDropdown");
    const form = document.getElementById("customGroupForm");

    // Populate zones when modal is shown
    modal.addEventListener("shown.bs.modal", () => {
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

    //zoneDropdown.addEventListener("change", () => {
    //    const selectedZone = this.value;
    //    branchDropdown.innerHTML = '<option value="">Select Branch</option>';

    //    if (!selectedZone) return;

    //    fetch(`/Common/FetchBranch?Zone=${encodeURIComponent(selectedZone)}`, {
    //        method: 'POST',
    //        headers: {
    //            'Content-Type': 'application/json'
    //        }
    //    })
    //        .then(res => res.json())
    //        .then(response => {
    //            console.log(response);
    //        })
    //        .catch(error => console.error("Error fetching branch list:", error));
    //})
    modal.addEventListener("hidden.bs.modal", function () {
        form.reset();
        dealerDropdown.innerHTML = '<option value="">Select Dealer</option>';

    });

    zoneDropdown.addEventListener("change", (event) => {
        const selectedZone = event.target.value;
        branchDropdown.innerHTML = '<option value="">Select Branch</option>';

        if (!selectedZone) return;

        fetch(`/Common/FetchBranch?Zone=${encodeURIComponent(selectedZone)}`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            }
        })
            .then(res => res.json())
            .then(response => {
                if (response.isSuccess && response.data) {
                    console.log(response.data)
                    response.data.forEach(branch => {
                        const opt = document.createElement("option");
                        opt.value = branch;
                        opt.textContent = branch;
                        branchDropdown.appendChild(opt);
                    } )
                }
            })
            .catch(error => console.error("Error fetching branch list:", error));
    });

    branchDropdown.addEventListener("change", (event) => {
        const selectedBranch = event.target.value;
        dealerDropdown.innerHTML = '<option value="">Select Dealers</option>';

        if (!selectedBranch) return;

        fetch(`/Common/FetchDealerByBranch?branchCode=${encodeURIComponent(selectedBranch)}`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            }
        })
            .then(res => res.json())
            .then(response => {
                if (response.isSuccess && response.data) {
                    console.log(response);
                    response.data.forEach(item => {
                        const opt = document.createElement("option");
                        opt.value = item.dealerName;
                        opt.textContent = item.dealerName;
                        dealerDropdown.appendChild(opt);
                    })
                }
            })
            .catch(error => console.error("Error fetching dealer list:", error));
    })
});