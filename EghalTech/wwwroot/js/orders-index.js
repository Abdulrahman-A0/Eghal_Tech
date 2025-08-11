

// Search functionality
const searchBox = document.getElementById("searchBox");
const tableBody = document.querySelector("tbody");

searchBox.addEventListener("input", function () {
    const searchText = searchBox.value.toLowerCase();
    const rows = tableBody.querySelectorAll("tr");

    rows.forEach(row => {
        const orderId = row.querySelector(".order-id")?.textContent.toLowerCase() || "";
        const customerName = row.querySelector(".customer-name")?.textContent.toLowerCase() || "";

        if (orderId.includes(searchText) || customerName.includes(searchText)) {
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
                    <td colspan="8" class="no-results">
                        <i class="bi bi-search"></i>
                        <p>No orders found matching "${searchText}"</p>
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

// Update order status
function updateOrderStatus(selectElement, orderId) {
    const newStatus = selectElement.value;
    $.ajax({
        url: `/Order/UpdateStatus`,
        type: `POST`,
        data: { ordId: orderId, status: newStatus },
        success: function (result) {
            console.log("ok");
            selectElement.className = `status-select status-${newStatus}`;
            window.showNotification(result.message, "success");
        }
    })
}

// Delete modal functions
//function showDeleteModal(orderId, orderNumber) {
//    document.getElementById('orderIdToDelete').innerText = orderNumber;
//    document.getElementById('deleteModal').style.display = 'flex';

//    // Remove any existing event listeners to prevent multiple calls
//    const confirmDeleteBtn = document.getElementById('confirmDeleteBtn');

//    // Define the event handler
//    const confirmDeleteHandler = function () {
//        $.ajax({
//            url: `/Order/Delete`,
//            type: `POST`,
//            data: { ordId: orderId },
//            success: function (result) {
//                showNotification(result.message, "success");
//                closeDeleteModal();
//                $(`[data-item-id=${orderId}]`).remove();
//            }
//        })
//    };

//    confirmDeleteBtn.removeEventListener('click', confirmDeleteHandler);
//    // Add the event listener
//    confirmDeleteBtn.addEventListener('click', confirmDeleteHandler);
//}

//function closeDeleteModal() {
//    document.getElementById('deleteModal').style.display = 'none';
//}

//// Close modal when clicking outside
//window.addEventListener('click', function (event) {
//    const modal = document.getElementById('deleteModal');
//    if (event.target === modal) {
//        closeDeleteModal();
//    }
//});



function showActionModal(options) {
    const { orderId, title, message, warning, buttonText, ajaxUrl, removeSelector } = options;

    document.getElementById('actionModalTitle').innerText = title;
    document.getElementById('actionModalMessage').innerHTML = message.replace('{orderId}', `<strong>${orderId}</strong>`);
    document.getElementById('actionWarningText').innerText = warning;
    const confirmBtn = document.getElementById('confirmActionBtn');
    confirmBtn.innerText = buttonText;

    document.getElementById('actionModal').style.display = 'flex';

    // Remove old listeners
    const newConfirmBtn = confirmBtn.cloneNode(true);
    confirmBtn.parentNode.replaceChild(newConfirmBtn, confirmBtn);

    // Add new listener for this action
    newConfirmBtn.addEventListener('click', function () {
        $.ajax({
            url: ajaxUrl,
            type: 'POST',
            data: { ordId: orderId },
            success: function (result) {
                showNotification(result.message, "success");
                closeActionModal();
                if (removeSelector) {
                    $(`[data-item-id=${orderId}]`).remove();
                }
                else {
                    const statusEl = document.getElementById(`status-${orderId}`);
                    statusEl.textContent = "Cancelled";
                    statusEl.className = "status-select status-cancelled";
                }
            }
        });
    });
}

function closeActionModal() {
    document.getElementById('actionModal').style.display = 'none';
}

// Close when clicking outside
window.addEventListener('click', function (event) {
    const modal = document.getElementById('actionModal');
    if (event.target === modal) {
        closeActionModal();
    }
});
