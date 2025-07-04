﻿document.addEventListener("DOMContentLoaded", () => {
    let rowIdToDelete = null;

    document.querySelectorAll(".delete-btn").forEach(button => {
        button.addEventListener("click", () => {
            rowIdToDelete = button.getAttribute("data-id");
            const deleteInput = document.getElementById("deleteInput");
            deleteInput.value = rowIdToDelete;
            const deleteModal = new bootstrap.Modal(document.getElementById("deleteConfirmModal"));
            deleteModal.show();
        });
    });

    const updateModal = new bootstrap.Modal(document.getElementById("updateBranchModal"));
    const zoneDropdownUpdate = document.getElementById("zoneDropdownUpdate");
    const dealerDropdownUpdate = document.getElementById("dealerDropdownUpdate");

    document.querySelectorAll(".update-btn").forEach(button => {
        button.addEventListener("click", function () {
            const row = document.querySelector(`tr[data-rowid="${this.dataset.id}"]`);

            const rowId = row.getAttribute("data-rowid");
            const zone = row.children[0].innerText.trim();  // Extract zone from the row
            const dealerId = row.children[1].innerText.trim();
            const dealerName = row.children[2].innerText.trim();
            const ctclLogin = row.children[3].innerText.trim();
            
            // Set the hidden input field for RowId
            document.getElementById("rowIdInputUpdate").value = rowId;

            // Pre-populate Zone dropdown (it should be read-only, so no need to populate with other zones)
            zoneDropdownUpdate.innerHTML = `<option value="${zone}" selected>${zone}</option>`; // Only the selected zone is shown
            if (zone == "PPAL") {
                dealerDropdownUpdate.innerHTML = "-";
                const opt = document.createElement("option");
                opt.value = "-";
                opt.textContent = "-";
                dealerDropdownUpdate.appendChild(opt);
                document.querySelector("#updateBranchForm input[name='DealerID']").value = "-";
                document.querySelector("#updateBranchForm input[name='CtclLoginId']").value = "PPAL01";
                return;
            }
            // Fetch dealers for the selected zone
            fetch(`${window.appBasePath}/Common/GetDealerByZone?Zone=${encodeURIComponent(zone)}`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                }
            })
            .then(res => res.json())
            .then(response => {
                if (response.isSuccess && response.data) {
                    dealerDropdownUpdate.innerHTML = '<option value="">Select Dealer</option>';
                    response.data.forEach(d => {
                        const opt = document.createElement("option");
                        opt.value = d.dealerName;
                        opt.textContent = d.dealerName;
                        opt.setAttribute("data-dealerId", d.dealerID);
                        opt.setAttribute("data-ctcl", d.ctclLoginid);
                        if (d.dealerName === dealerName) opt.selected = true;  // Pre-select the dealer
                        dealerDropdownUpdate.appendChild(opt);
                    });

                    // Set the Dealer ID and CTCL Login values
                    document.querySelector("#updateBranchForm input[name='DealerID']").value = dealerId;
                    document.querySelector("#updateBranchForm input[name='CtclLoginId']").value = ctclLogin;

                    // Show the modal
                    updateModal.show();
                }
            })
            .catch(err => console.error("Error fetching dealers:", err));
        });
    });

    // Update the Dealer ID and CTCL Login when the dealer dropdown changes
    dealerDropdownUpdate.addEventListener("change", function () {
        const selectedOption = this.options[this.selectedIndex];
        document.querySelector("#updateBranchForm input[name='DealerID']").value = selectedOption.getAttribute("data-dealerId") || "-";
        document.querySelector("#updateBranchForm input[name='CtclLoginId']").value = selectedOption.getAttribute("data-ctcl") || "-";
    });
});
