document.addEventListener("DOMContentLoaded", () => {
    const clientInfoModal = document.getElementById("clientInfoModal");
    //const group1 = document.getElementById("group1Body");
    //const group2 = document.getElementById("group2Body");
    //const group3 = document.getElementById("group3Body");
    //const group4 = document.getElementById("group4Body");

    document.querySelectorAll(".info-btn").forEach(button => {
        button.addEventListener("click", function () {
            const clientCode = button.getAttribute("data-id");

            fetch(`${window.appBasePath}/ClientDealer/GetClientGroups?clientCode=${encodeURIComponent(clientCode)}`, {
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
                            const header = document.getElementById(`${groupKey}`)
                            if (groupData.length) {
                                header.innerHTML = `Group ${groupKey.substring(5)}: ${groupData[0].groupCode}`;
                            }
                            container.innerHTML = ""; // Clear previous content
                            console.log(groupData);

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

    clientInfoModal.addEventListener("hidden.bs.modal", function () {
        for(let i = 1; i <= 4; i++) {
            const container = document.getElementById(`group${i}Body`);
            const header = document.getElementById(`group${i}`);
            container.innerHTML = "";
            header.innerHTML = `Group ${i}`
        }
    });
});
