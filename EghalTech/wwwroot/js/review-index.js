// Search functionality
const searchBox = document.getElementById("searchBox");
const tableBody = document.querySelector("tbody");

searchBox.addEventListener("input", function () {
    const searchText = searchBox.value.toLowerCase();
    const rows = tableBody.querySelectorAll("tr");

    rows.forEach(row => {
        const productName = row.querySelector(".product-name-cell")?.textContent.toLowerCase() || "";
        const reviewText = row.querySelector(".review-text")?.textContent.toLowerCase() || "";

        if (productName.includes(searchText) || reviewText.includes(searchText)) {
            row.style.display = "";
        } else {
            row.style.display = "none";
        }
    });

    // Show no results message if no rows are visible
    const visibleRows = Array.from(rows).filter(row => row.style.display !== "none");
    if (visibleRows.length === 0 && searchText !== "") {
        if (!document.querySelector(".no-results-row")) {
            const noResultsRow = document.createElement("tr");
            noResultsRow.className = "no-results-row";
            noResultsRow.innerHTML = `
                    <td colspan="6" class="no-results">
                        <i class="bi bi-search"></i>
                        <p>No reviews found matching "${searchText}"</p>
                    </td>
                `;
            tableBody.appendChild(noResultsRow);
        }
    } else {
        const noResultsRow = document.querySelector(".no-results-row");
        if (noResultsRow) {
            noResultsRow.remove();
        }
    }
});

// Clear search when input is empty
searchBox.addEventListener("input", function () {
    if (this.value === "") {
        const noResultsRow = document.querySelector(".no-results-row");
        if (noResultsRow) {
            noResultsRow.remove();
        }
    }
});

// Toggle review text expansion
function toggleReviewText(reviewId) {
    const reviewElement = document.getElementById(`review-${reviewId}`);
    const button = reviewElement.querySelector('.expand-btn');

    if (reviewElement.classList.contains('review-text-full')) {
        reviewElement.classList.remove('review-text-full');
        button.textContent = 'Show more';
    } else {
        reviewElement.classList.add('review-text-full');
        button.textContent = 'Show less';
    }
}

// Delete modal functions
function showDeleteModal(reviewId, productName) {
    document.getElementById('productNameToDelete').innerText = productName;
    document.getElementById('deleteModal').style.display = 'flex';

    // Remove any existing event listeners to prevent multiple calls
    const confirmDeleteBtn = document.getElementById('confirmDeleteBtn');

    // Define the event handler
    const confirmDeleteHandler = function () {
        $.ajax({
            url: `/Review/Delete`,
            type: `POST`,
            data: { reviewId: reviewId },
            success: function (result) {
                closeDeleteModal();
                $(`[data-item-id='${result.reviewId}']`).remove();
                showNotification("Review has been deleted", "success");
            }
        })
    };

    confirmDeleteBtn.removeEventListener('click', confirmDeleteHandler);
    // Add the event listener
    confirmDeleteBtn.addEventListener('click', confirmDeleteHandler);
}

function closeDeleteModal() {
    document.getElementById('deleteModal').style.display = 'none';
}

// Close modal when clicking outside
window.addEventListener('click', function (event) {
    const modal = document.getElementById('deleteModal');
    if (event.target === modal) {
        closeDeleteModal();
    }
});