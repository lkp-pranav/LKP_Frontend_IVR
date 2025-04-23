window.addEventListener('DOMContentLoaded', () => {
    const toastData = document.getElementById("toastData");
    const successMessage = toastData.dataset.success;
    const errorMessage = toastData.dataset.error;
    const showToast = toastData.dataset.showtoast === "True";

    if (showToast) {
        if (errorMessage) {
            showToastMessage(errorMessage, 'danger');
        } else if (successMessage) {
            showToastMessage(successMessage, 'success');
        }
    }

    function showToastMessage(message, type) {
        const toastEl = document.getElementById("messageToast");
        const toastHeader = document.getElementById("toastHeader");
        const toastBody = document.getElementById("toastBody");

        toastBody.textContent = message;

        if (type === 'danger') {
            toastHeader.classList.remove("bg-success");
            toastHeader.classList.add("bg-danger");
            toastHeader.querySelector("strong").textContent = "Error";
        } else {
            toastHeader.classList.remove("bg-danger");
            toastHeader.classList.add("bg-success");
            toastHeader.querySelector("strong").textContent = "Success";
        }

        // Manually initialize the toast with autohide
        const toast = new bootstrap.Toast(toastEl, {
            autohide: true,
            delay: 1000
        });
        toast.show();
    }
});