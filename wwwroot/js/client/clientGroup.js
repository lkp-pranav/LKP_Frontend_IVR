//document.addEventListener("DOMContentLoaded", () => {
//    const clientInfoModal = document.getElementById("clientInfoModal");

//    //clientInfoModal.addEventListener("hidden.bs.modal", function () { }

//    document.querySelectorAll(".info-btn").forEach(button => {
//        button.addEventListener("click", function () {
//            const clientCode = button.getAttribute("data-id");

//            fetch(`/ClientDealer/GetClientGroups?clientCode=${encodeURIComponent(clientCode)}`, {
//                method: 'POST',
//                headers: {
//                    'Content-Type': 'application/json'
//                }
//            })
//                .then(res => res.json())
//                .then(response => {
//                    if (response.isSuccess && response.data) {
//                        const groups = {
//                            group1: response.data.group1,
//                            group2: response.data.group2,
//                            group3: response.data.group3,
//                            group4: response.data.group4
//                        };

//                        Object.entries(groups).forEach(([groupKey, groupData]) => {
//                            const container = document.getElementById(`${groupKey}Body`);
//                            container.innerHTML = ""; // Clear previous content

//                            if (!groupData || groupData.length === 0) {
//                                container.innerHTML = `<p class="text-muted">Group is empty</p>`;
//                            } else {
//                                groupData.forEach(dealer => {
//                                    const dealerInfo = `
//                                        <div class="mb-2 p-2 border rounded bg-light">
//                                            <p class="mb-1"><strong>Dealer ID:</strong> ${dealer.dealerID}</p>
//                                            <p class="mb-1"><strong>Name:</strong> ${dealer.dealerName}</p>
//                                            <p class="mb-1"><strong>CTCL Login:</strong> ${dealer.ctclLoginid}</p>
//                                            <p class="mb-0"><strong>Status:</strong> ${dealer.workstatus}</p>
//                                        </div>
//                                    `;
//                                    container.insertAdjacentHTML("beforeend", dealerInfo);
//                                });
//                            }
//                        });

//                        const modal = new bootstrap.Modal(clientInfoModal);
//                        modal.show();
//                    }
//                })
//                .catch(err => {
//                    console.error("[ERROR] Failed to fetch client groups:", err);
//                });
//        });
//    });
//});


document.addEventListener("DOMContentLoaded", () => {
    const clientInfoModal = document.getElementById("clientInfoModal");

    document.querySelectorAll(".info-btn").forEach(button => {
        button.addEventListener("click", function () {
            const clientCode = button.getAttribute("data-id");

            fetch(`/ClientDealer/GetClientGroups?clientCode=${encodeURIComponent(clientCode)}`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                }
            })
                .then(res => res.json())
                .then(response => {
                    if (response.isSuccess && response.data) {
                        const groups = {
                            group1: response.data.group1,
                            group2: response.data.group2,
                            group3: response.data.group3,
                            group4: response.data.group4
                        };

                        Object.entries(groups).forEach(([groupKey, groupData]) => {
                            const container = document.getElementById(`${groupKey}Body`);
                            container.innerHTML = ""; // Clear previous content

                            if (!groupData || groupData.length === 0) {
                                container.innerHTML = `<p class="text-muted">Group is empty</p>`;
                            } else {
                                const table = document.createElement("table");
                                table.className = "table table-sm table-bordered table-striped";

                                // Table for Group
                                table.innerHTML = `
                                    <thead class="table-light">
                                        <tr>
                                            <th>Dealer ID</th>
                                            <th>Name</th>
                                            <th>CTCL Login</th>
                                            <th>Status</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        ${groupData.map(dealer => `
                                            <tr>
                                                <td>${dealer.dealerID}</td>
                                                <td>${dealer.dealerName}</td>
                                                <td>${dealer.ctclLoginid}</td>
                                                <td>${dealer.workstatus}</td>
                                            </tr>
                                        `).join("")}
                                    </tbody>
                                `;

                                container.appendChild(table);
                            }
                        });
                    }
                });
        });
    });
});
