document.addEventListener("DOMContentLoaded", function () {
    const primaryDealerDropdown = document.getElementById("primaryDealerDropdownCreate");
    const secondaryDealerDropdown = document.getElementById("secondaryDealerDropdownCreate");

    const primarySegment = document.getElementById("primarySegmentList");
    const secondarySegment = document.getElementById("secondarySegmentList");

    function fetchSegment(dealerId, targetInput, label) {
        if (!dealerId) {
            targetInput.textContent = "-";
            console.log(`[INFO] No ${label} dealer selected`);
            return;
        }

        console.log(`[DEBUG] Fetching segment for ${label} dealer ID: ${dealerId}`);

        fetch(`/Common/GetDealerSegment?dealer=${encodeURIComponent(dealerId)}`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            }
        })
            .then(res => res.json()) // ✅ Correctly parse JSON here
            .then(response => {
                console.log(`[DEBUG] Parsed response:`, response);

                // Clear any existing content
                targetInput.innerHTML = "";

                if (response.isSuccess && Array.isArray(response.data)) {
                    response.data.forEach(item => {
                        const li = document.createElement("li");
                        li.textContent = item.segment;
                        targetInput.appendChild(li);
                    });
                } else {
                    targetInput.textContent = "No segment assigned";
                }
            })
            .catch(err => {
                console.error(`[ERROR] Failed to fetch ${label} dealer segment:`, err);
                targetInput.textContent = "-";
            });
    }

    primaryDealerDropdown?.addEventListener("change", function () {
        const selectedOption = this.options[this.selectedIndex];
        const dealerId = selectedOption.getAttribute("data-dealerId");
        console.log("this is the primary dealer id: ", dealerId);
        fetchSegment(dealerId, primarySegment, "primary");
        secondarySegment.innerHTML = "-"
    });

    secondaryDealerDropdown?.addEventListener("change", function () {
        const selectedOption = this.options[this.selectedIndex];
        const dealerId = selectedOption.getAttribute("data-dealerId");
        fetchSegment(dealerId, secondarySegment, "secondary");
    });
});
