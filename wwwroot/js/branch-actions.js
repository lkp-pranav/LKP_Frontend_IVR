document.addEventListener("DOMContentLoaded", () => {
    document.querySelectorAll(".delete-btn").forEach(button => {
        button.addEventListener("click", () => {
            const rowId = button.getAttribute("data-id");

            fetch(`/BranchMapping/DeleteMapping?rowId=${rowId}`, {
                method: "DELETE"
            })
                .then(response => {
                    if (response.redirected) {
                        window.location.href = response.url;
                    } else {
                        return response.json();
                    }
                })
                .catch(err => console.error("Error during deletion:", err));
        });
    });
});
