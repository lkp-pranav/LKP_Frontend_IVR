//document.addEventListener("DOMContentLoaded", function () {
//    const primaryDealerDropdown = document.getElementById("primaryDealerDropdown");
//    const secondaryDealerDropdown = document.getElementById("secondaryDealerDropdown");

//    const primarySegment = document.getElementById("primarySegment");
//    const secondarySegment = document.getElementById("secondarySegment");

//    function fetchSegment(dealerId, targetInput, label) {
//        if (!dealerId) {
//            targetInput.value = "-";
//            console.log(`[INFO] No ${label} dealer selected`);
//            return;
//        }

//        console.log(`[DEBUG] Fetching segment for ${label} dealer ID: ${dealerId}`);

//        fetch(`/Common/GetDealerSegment?dealer=${encodeURIComponent(dealerId)}`)
//            .then(res => res.json())
//            .then(response => {
//                console.log(`[DEBUG] Response for ${label} dealer:`, response);
//                if (response.isSuccess && response.data) {
//                    targetInput.value = response.data;
//                } else {
//                    targetInput.value = "-";
//                }
//            })
//            .catch(err => {
//                console.error(`[ERROR] Failed to fetch ${label} dealer segment:`, err);
//                targetInput.value = "-";
//            });
//    }

//    primaryDealerDropdown?.addEventListener("change", function () {
//        const selectedOption = this.options[this.selectedIndex];
//        const dealerId = selectedOption.getAttribute("data-dealerId");
//        console.log("this is the primary dealer id: ", dealerId);
//        fetchSegment(dealerId, primarySegment, "primary");
//    });

//    secondaryDealerDropdown?.addEventListener("change", function () {
//        const selectedOption = this.options[this.selectedIndex];
//        const dealerId = selectedOption.getAttribute("data-dealerId");
//        fetchSegment(dealerId, secondarySegment, "secondary");
//    });
//});

document.addEventListener("DOMContentLoaded", function () {
    const primaryDealerDropdown = document.getElementById("primaryDealerDropdownCreate");
    const secondaryDealerDropdown = document.getElementById("secondaryDealerDropdownCreate");

    const primarySegment = document.getElementById("primarySegmentCreate");
    const secondarySegment = document.getElementById("secondarySegmentCreate");

    function fetchSegment(dealerId, targetInput, label) {
        if (!dealerId) {
            targetInput.value = "-";
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
        const selectedOption = this.options[this.selectedIndex];
        const dealerId = selectedOption.getAttribute("data-dealerId");
        console.log("this is the primary dealer id: ", dealerId);
        fetchSegment(dealerId, primarySegment, "primary");
    });

    secondaryDealerDropdown?.addEventListener("change", function () {
        const selectedOption = this.options[this.selectedIndex];
        const dealerId = selectedOption.getAttribute("data-dealerId");
        fetchSegment(dealerId, secondarySegment, "secondary");
    });
});
