document.addEventListener("DOMContentLoaded", () => {
    const modal = document.getElementById("addCustomGroup");
    const groupCodeInput = document.getElementById("groupCode");
    const zoneDropdown = document.getElementById("zoneDropdown");
    const branchDropdown = document.getElementById("branchDropdown");
    const dealerContainer = document.getElementById("dealerContainer");
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
                //console.log(response.data)
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

    modal.addEventListener("hidden.bs.modal", function () {
        form.reset();
        dealerContainer.innerHTML = `
        <label class="form-label">Dealer List</label>
        <div id="dealerList" class="form-check"></div>
    `;
    });


    zoneDropdown.addEventListener("change", (event) => {
        const selectedZone = event.target.value;
        branchDropdown.innerHTML = '<option value="">Select Branch</option>';
        dealerContainer.innerHTML = `
        <label class="form-label">Dealer List</label>
        <div id="dealerList" class="form-check"></div>
    `;

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
                    //console.log(response.data)
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

        dealerContainer.innerHTML = `
        <label class="form-label">Dealer List</label>
        <div id="dealerList" class="form-check"></div>
    `

        const dealerList = document.getElementById("dealerList");

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
                    response.data.forEach((item, index) => {
                        // Creating container div for each checkbox
                        const checkboxContainer = document.createElement('div');
                        checkboxContainer.classList.add('form-check');

                        const checkbox = document.createElement('input');
                        checkbox.type = 'checkbox';
                        checkbox.className = 'form-check-input';
                        checkbox.id = `dealer_${index}`;
                        checkbox.dataset.dealerId = item.dealerID;
                        checkbox.dataset.dealerName = item.dealerName;
                        checkbox.dataset.ctclLoginid = item.ctclLoginid;

                        const label = document.createElement('label');
                        label.className = 'form-check-label';
                        label.htmlFor = `dealer_${index}`;
                        label.textContent = `${item.dealerName} (${item.ctclLoginid})`;

                        // Append checkbox and label to the container div
                        checkboxContainer.appendChild(checkbox);
                        checkboxContainer.appendChild(label);

                        // Append the container div to the dealer list
                        dealerList.appendChild(checkboxContainer);
                    });
                }
            })
            .catch(error => console.error("Error fetching dealer list:", error));
    });

    // Function to make comma separated list;
    function getSelectedDealers() {
        const dealerIds = [];
        const dealerNames = [];
        const ctclLoginids = [];

        const checkboxes = dealerList.querySelectorAll('input[type="checkbox"]:checked');

        checkboxes.forEach(checkbox => {
            dealerIds.push(checkbox.dataset.dealerId);
            dealerNames.push(checkbox.dataset.dealerName);
            ctclLoginids.push(checkbox.dataset.ctclLoginid);
        });

        return {
            dealerIds: dealerIds.join(','),         
            dealerNames: dealerNames.join(','),     
            ctclLoginids: ctclLoginids.join(',')    
        };
    }

    form.addEventListener("submit", function (event) {
        event.preventDefault();

        const selectedDealers = getSelectedDealers();
        const activeCheckbox = document.getElementById("activeCheckbox");

        const payload = {
            GroupCode: groupCodeInput.value,
            Zone: zoneDropdown.value,
            Branch: branchDropdown.value,
            DealerID: selectedDealers.dealerIds,        
            DealerName: selectedDealers.dealerNames,    
            ctclLoginId: selectedDealers.ctclLoginids,
            Active: activeCheckbox.checked ? "Y" : "N"
        };

        console.log("Payload to send:", payload);

        fetch('/CustomGroup/CreateCustomGroup', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(payload)
        })
            .then(response => response.json())
            .then(data => {
                if (data.success) {
                    console.log("Success:", data.message);
                    bootstrap.Modal.getInstance(modal)?.hide();
                    window.location.href = "/CustomGroup/Index"; // manually reload/redirect
                } else {
                    alert(data.message)
                    console.error("Server Error:", data.message);
                }
            })
            .catch(error => {
                console.error("Fetch Error:", error);
            });

    });


});