document.addEventListener("DOMContentLoaded", () => {
    const deleteModal = new bootstrap.Modal(document.getElementById("deleteDealerModal"));
    const confirmDeleteBtn = document.getElementById("confirmDeleteBtn");
    let deleteRowId = null;

    document.querySelectorAll(".delete-btn").forEach(button => {
        button.addEventListener("click", () => {
            deleteRowId = button.dataset.id;
            deleteModal.show();
        });
    });

    confirmDeleteBtn.addEventListener("click", () => {
        if (!deleteRowId) return;

        fetch(`${window.appBasePath}/DealerMapping/DeleteMapping?rowId=${deleteRowId}`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' }
        })
            .then(response => response.redirected ? window.location.href = response.url : response.json())
            .catch(err => console.error("Error during deletion:", err))
            .finally(() => {
                deleteModal.hide();
                deleteRowId = null;
            });
    });
});
