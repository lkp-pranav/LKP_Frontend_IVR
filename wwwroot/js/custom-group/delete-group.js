document.addEventListener("DOMContentLoaded", () => {
    let groupToDelete = null;

    document.querySelectorAll(".delete-btn").forEach(button => {
        button.addEventListener("click", () => {
            groupToDelete = button.getAttribute("data-id");
            const deleteModal = new bootstrap.Modal(document.getElementById("deleteConfirmModal"));
            deleteModal.show();
        });
    });

    document.getElementById("confirmDeleteBtn").addEventListener("click", () => {
        if (!groupToDelete) return;

        fetch(`/CustomGroup/DeleteCustomGroup?groupCode=${groupToDelete}`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            }
        })
            .then(response => {
                if (response.redirected) {
                    window.location.href = response.url;
                } else {
                    return response.json();
                }
            })
            .catch(err => console.error("Error during deletion:", err))
            .finally(() => {
                const deleteModal = bootstrap.Modal.getInstance(document.getElementById("deleteConfirmModal"));
                deleteModal.hide();
            });
    });
});