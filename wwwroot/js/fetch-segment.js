document.addEventListener("DOMContentLoaded", function () {
    const primaryDealerDropdown = document.getElementById("primaryDealerDropdown");
    const secondaryDealerDropdown = document.getElementById("secondaryDealerDropdown");

    const primarySegment = document.getElementById("primarySegment");
    const secondarySegment = document.getElementById("secondarySegment");

    function fetchSegment(dealerValue, targetInput, label) {
        if (!dealerValue) {
            targetInput.value = "-";
            console.log(`[INFO] No ${label} dealer selected`);
            return;
        }

        console.log(`[DEBUG] Fetching segment for ${label} dealer: ${dealerValue}`);

        fetch(`/Common/GetDealerSegment?dealer=${encodeURIComponent(dealerValue)}`)
            .then(res => res.json())
            .then(response => {
                console.log(`[DEBUG] Response for ${label} dealer:`, response);
                if (response.isSuccess && response.data) {
                    targetInput.value = response.data;
                } else {
                    targetInput.value = "-";
                }
            })
            .catch(err => {
                console.error(`[ERROR] Failed to fetch ${label} dealer segment:`, err);
                targetInput.value = "-";
            });
    }

    primaryDealerDropdown?.addEventListener("change", function () {
        fetchSegment(this.value, primarySegment, "primary");
    });

    secondaryDealerDropdown?.addEventListener("change", function () {
        fetchSegment(this.value, secondarySegment, "secondary");
    });
});