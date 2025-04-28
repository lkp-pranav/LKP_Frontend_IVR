document.addEventListener("DOMContentLoaded", () => {
    const modal = document.getElementById("groupInfoModal");
    const modalBody = modal.querySelector(".modal-body");
    const groupCodeSpan = document.getElementById("groupCodeSpan");

    document.querySelectorAll(".info-btn").forEach(button => {
        button.addEventListener("click", function () {
            const groupCode = button.getAttribute("data-id");
            const zone = button.getAttribute("data-zone");
            const branch = button.getAttribute("data-branch");
            const active = button.getAttribute("data-active");

            // Set the group code in the modal title
            groupCodeSpan.textContent = groupCode;

            // Clear the modal body before filling it
            modalBody.innerHTML = "";

            // Create a section to display Group Info
            const groupInfoSection = document.createElement("div");
            groupInfoSection.className = "mb-3";
            groupInfoSection.innerHTML = `
                <h6>Group Details</h6>
                <ul class="list-group list-group-flush">
                    <li class="list-group-item"><strong>Group Code:</strong> ${groupCode}</li>
                    <li class="list-group-item"><strong>Zone:</strong> ${zone}</li>
                    <li class="list-group-item"><strong>Branch:</strong> ${branch}</li>
                    <li class="list-group-item"><strong>Active:</strong> ${active}</li>
                </ul>
            `;
            modalBody.appendChild(groupInfoSection);

            // Fetch dealers
            fetch(`/Common/FetchDealerByGroup?groupCode=${encodeURIComponent(groupCode)}`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                }
            })
                .then(res => res.json())
                .then(response => {
                    console.log(response);
                    if (response.isSuccess && response.data && response.data.length > 0) {

                        const table = document.createElement("table");
                        table.className = "table table-sm table-bordered table-striped";

                        table.innerHTML = `
                        <thead class="table-light">
                            <tr>
                                <th>Dealer ID</th>
                                <th>Dealer Name</th>
                                <th>CTCL Login ID</th>
                                <th>Work Status</th>
                            </tr>
                        </thead>
                        <tbody>
                            ${response.data.map(dealer => `
                                <tr>
                                    <td>${dealer.dealerID}</td>
                                    <td>${dealer.dealerName}</td>
                                    <td>${dealer.ctclLoginid}</td>
                                    <td>${dealer.workstatus}</td>
                                </tr>
                            `).join("")}
                        </tbody>
                    `;

                        modalBody.appendChild(table);

                    } else {
                        const noDataMessage = document.createElement("p");
                        noDataMessage.className = "text-muted";
                        noDataMessage.textContent = "No dealers found for this group.";
                        modalBody.appendChild(noDataMessage);
                    }
                });
        });
    });
});
