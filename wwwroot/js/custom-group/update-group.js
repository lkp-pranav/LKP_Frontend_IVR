document.addEventListener("DOMContentLoaded", () => {
    const modal = document.getElementById("updateCustomGroup");
    const form = document.getElementById("customGroupFormUpdate");
    const groupCodeInput = document.getElementById("groupCodeUpdate");
    const zoneDropdown = document.getElementById("zoneDropdownUpdate");
    const branchDropdown = document.getElementById("branchDropdownUpdate");
    const dealerListContainer = document.getElementById("dealerListUpdate");
    const activeCheckbox = document.getElementById("activeCheckboxUpdate");

    let currentGroupCode = '';
    let currentZone = '';
    let currentBranch = '';
    let selectedDealerIds = [];

    // Utility functions
    async function getGroupDealers(groupCode) {
        try {
            const res = await fetch(`${window.appBasePath}/Common/FetchDealerByGroup?groupCode=${encodeURIComponent(groupCode)}`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' }
            });
            const response = await res.json();
            if (response.isSuccess && response.data) {
                return response.data.map(dealer => dealer.dealerID);
            }
        } catch (error) {
            console.error("Error fetching dealers:", error);
        }
        return [];
    }

    async function populateZones(selectedZone = '') {
        try {
            const res = await fetch(`${window.appBasePath}/Common/GetZones`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' }
            });
            const response = await res.json();
            if (response.isSuccess && response.data) {
                zoneDropdown.innerHTML = '<option value="">Select Zone</option>';
                response.data.forEach(zone => {
                    if (zone != "PPAL") {
                        const option = document.createElement('option');
                        option.value = zone;
                        option.textContent = zone;
                        if (zone === selectedZone) option.selected = true;
                        zoneDropdown.appendChild(option);
                    }
                });
            }
        } catch (error) {
            console.error("Error fetching zones:", error);
        }
    }

    async function populateBranches(zone, selectedBranch = '') {
        try {
            const res = await fetch(`${window.appBasePath}/Common/FetchBranch?Zone=${encodeURIComponent(zone)}`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' }
            });
            const response = await res.json();
            if (response.isSuccess && response.data) {
                branchDropdown.innerHTML = '<option value="">Select Branch</option>';
                response.data.forEach(branch => {
                    const option = document.createElement('option');
                    option.value = branch;
                    option.textContent = branch;
                    if (branch === selectedBranch) option.selected = true;
                    branchDropdown.appendChild(option);
                });
            }
        } catch (error) {
            console.error("Error fetching branches:", error);
        }
    }

    async function populateDealers(branch) {
        try {
            const res = await fetch(`${window.appBasePath}/Common/FetchDealerByBranch?branchCode=${encodeURIComponent(branch)}`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' }
            });
            const response = await res.json();
            if (response.isSuccess && response.data) {
                dealerListContainer.innerHTML = '';
                response.data.forEach((dealer, index) => {
                    const container = document.createElement('div');
                    container.classList.add('form-check');

                    const checkbox = document.createElement('input');
                    checkbox.type = 'checkbox';
                    checkbox.className = 'form-check-input';
                    checkbox.id = `dealer_${index}`;
                    checkbox.dataset.dealerId = dealer.dealerID;
                    checkbox.dataset.dealerName = dealer.dealerName;
                    checkbox.dataset.ctclLoginid = dealer.ctclLoginid;

                    if (selectedDealerIds.includes(dealer.dealerID)) {
                        checkbox.checked = true;
                    }

                    const label = document.createElement('label');
                    label.className = 'form-check-label';
                    label.htmlFor = `dealer_${index}`;
                    label.textContent = dealer.dealerName;

                    container.appendChild(checkbox);
                    container.appendChild(label);
                    dealerListContainer.appendChild(container);
                });
            } else {
                dealerListContainer.innerHTML = '<p class="text-muted">No dealers found.</p>';
            }
        } catch (error) {
            console.error("Error fetching dealers:", error);
        }
    }

    function resetForm() {
        form.reset();
        groupCodeInput.value = '';
        zoneDropdown.innerHTML = '<option value="">Select Zone</option>';
        branchDropdown.innerHTML = '<option value="">Select Branch</option>';
        dealerListContainer.innerHTML = '';
        currentGroupCode = '';
        currentZone = '';
        currentBranch = '';
        selectedDealerIds = [];
    }

    // Modal open and close handlers
    modal.addEventListener('hidden.bs.modal', resetForm);

    document.querySelectorAll(".update-btn").forEach(button => {
        button.addEventListener("click", async function () {
            resetForm(); // Reset before populating new data
            const row = document.querySelector(`tr[data-rowid="${this.dataset.id}"]`);

            currentGroupCode = row.children[0].innerText.trim();
            currentZone = row.children[1].innerText.trim();
            currentBranch = row.children[2].innerText.trim();
            currentActive = row.children[3].innerText.trim();
            if (currentActive === "Y") {
                activeCheckbox.checked = true;
            } else {
                activeCheckbox.checked = false;
            }
            
            
            groupCodeInput.value = currentGroupCode;

            selectedDealerIds = await getGroupDealers(currentGroupCode);
            await populateZones(currentZone);
            await populateBranches(currentZone, currentBranch);
            await populateDealers(currentBranch);
        });
    });

    // Zone change listener
    zoneDropdown.addEventListener('change', async () => {
        const selectedZone = zoneDropdown.value;
        if (selectedZone) {
            await populateBranches(selectedZone);
            dealerListContainer.innerHTML = '<p class="text-muted">Select branch to load dealers.</p>';
        } else {
            branchDropdown.innerHTML = '<option value="">Select Branch</option>';
            dealerListContainer.innerHTML = '<p class="text-muted">Select zone first.</p>';
        }
    });

    // Branch change listener
    branchDropdown.addEventListener('change', async () => {
        const selectedBranch = branchDropdown.value;
        if (selectedBranch) {
            await populateDealers(selectedBranch);
        } else {
            dealerListContainer.innerHTML = '<p class="text-muted">Select branch to load dealers.</p>';
        }
    });

    function getSelectedDealers() {
        const dealerIds = [];
        const dealerNames = [];
        const ctclLoginids = [];

        const checkboxes = dealerListContainer.querySelectorAll('input[type="checkbox"]:checked');

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

        if (selectedDealers.dealerIds === "") {
            alert("Please select atleast one dealer");
            return;
        }

        const payload = {
            GroupCode: groupCodeInput.value,
            Zone: zoneDropdown.value,
            Branch: branchDropdown.value,
            DealerID: selectedDealers.dealerIds,
            DealerName: selectedDealers.dealerNames,
            ctclLoginId: selectedDealers.ctclLoginids,
            Active: activeCheckbox.checked ? "Y" : "N"
        };

        const submitButton = form.querySelector('button[type="submit"]');
        submitButton.disabled = true;
        submitButton.innerText = "Updating...";

        fetch(`${window.appBasePath}/CustomGroup/UpdateCustomGroup`, { 
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(payload)
        })
            .then(response => response.json())
            .then(data => {
                if (data.success) { 
                    bootstrap.Modal.getInstance(modal)?.hide();
                    window.location.href = `${window.appBasePath}/CustomGroup/Index`;
                } else {
                    alert(data.message);
                    submitButton.disabled = false;
                    submitButton.innerText = "Update";
                }
            })
            .catch(error => {
                alert("Something went wrong. Please try again.");
                submitButton.disabled = false;
                submitButton.innerText = "Update";
            });
    });

});
