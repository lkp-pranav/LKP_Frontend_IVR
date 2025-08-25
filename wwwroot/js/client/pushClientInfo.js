document.addEventListener("DOMContentLoaded", () => {
    const modal = document.getElementById("pushClientInfoModal");
    const zoneDropdown = document.getElementById("zoneDropdown");
    const branchDropdown = document.getElementById("branchDropdown");
    const dealerCode = document.getElementById("dealerCode");
    const clientCode = document.getElementById("clientCode");
    const excelButton = document.getElementById("pushClientInfoExcel")
    const form = document.getElementById("pushClientInfoForm");
    

    // Populate zones when modal is shown
    modal.addEventListener("shown.bs.modal", () => {
        fetch(`${window.appBasePath}/Common/GetZones`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            }
        })
            .then(res => res.json())
            .then(response => {
                if (response.isSuccess && response.data) {
                    dealerCode.value = "ALL";
                    clientCode.value = "ALL";
                    zoneDropdown.innerHTML = '<option value="ALL">ALL</option>';
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
            .catch(error => console.error("Error fetching zones:", error));
    });

    modal.addEventListener("hidden.bs.modal", function () {
        form.reset();
    });


    zoneDropdown.addEventListener("change", (event) => {
        const selectedZone = event.target.value;
        branchDropdown.innerHTML = '<option value="ALL">ALL</option>';
        
        if (!selectedZone) return;

        fetch(`${window.appBasePath}/Common/FetchBranch?Zone=${encodeURIComponent(selectedZone)}`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            }
        })
            .then(res => res.json())
            .then(response => {
                if (response.isSuccess && response.data) {
                    response.data.forEach(branch => {
                        const opt = document.createElement("option");
                        opt.value = branch;
                        opt.textContent = branch;
                        branchDropdown.appendChild(opt);
                    })
                }
            })
            .catch(error => console.error("Error fetching branch list:", error));
    });

    excelButton.addEventListener("click", () => {
        console.log(zoneDropdown.value);
        console.log(branchDropdown.value);
        console.log(dealerCode.value);
        console.log(clientCode.value);

        const payload = {
            Zone: zoneDropdown.value,
            Branch: branchDropdown.value,
            DealerCode: dealerCode.value,
            ClientCode: clientCode.value
        };

        fetch(`${window.appBasePath}/ClientDealer/PushClientInfoExcel`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(payload)
        })
            .then(res => {
                if (!res.ok) throw new Error("Failed to download file.");
                return res.blob(); // Convert response to Blob for file download
            })
            .then(blob => {
                const url = window.URL.createObjectURL(blob);
                const a = document.createElement("a");
                a.href = url;
                a.download = `PushClientInfo_${new Date().toISOString().slice(0, 10)}.xlsx`;
                document.body.appendChild(a);
                a.click();
                a.remove();
                window.URL.revokeObjectURL(url);
            })
            .catch(error => {
                console.error("Error downloading Excel file:", error);
                alert("Failed to download Excel file.");
            });
    });


   
    //form.addEventListener("submit", function (event) {
    //    event.preventDefault();

    //    const payload = {
            
    //        Zone: zoneDropdown.value,
    //        Branch: branchDropdown.value,
    //        DealerCode: dealerCode.value,
    //        ClientCode: clientCode.value
    //    };

    //    fetch(`${window.appBasePath}/ClientDealer/PushClientInfo`, {
    //        method: 'POST',
    //        headers: {
    //            'Content-Type': 'application/json'
    //        },
    //        body: JSON.stringify(payload)
    //    })
    //        .then(response => response.json())
    //        .then(data => {
    //            if (data.success) {
    //                bootstrap.Modal.getInstance(modal)?.hide();
                    
    //            } else {
    //            }
    //        })
    //        .catch(error => {
    //        });

    //});


});