document.addEventListener("DOMContentLoaded", () => {
    initializeCategoryPage()
})

function initializeCategoryPage() {
    initializeSearch()
    initializeDeleteModal()
    initializeNotifications()
}

function initializeSearch() {
    const searchBox = document.getElementById("searchBox")
    const tableBody = document.querySelector("tbody")

    if (searchBox && tableBody) {
        searchBox.addEventListener("input", () => {
            const searchText = searchBox.value

            // If search is empty, reload the page to show all categories
            if (searchText === "") {
                location.reload()
                return
            }

            fetch(`/Category/SearchCategories?categoryName=${encodeURIComponent(searchText)}`)
                .then((response) => response.json())
                .then((data) => {
                    tableBody.innerHTML = "" // Clear current rows

                    if (data.data && data.data.length > 0) {
                        data.data.forEach((category) => {
                            const row = document.createElement("tr")
                            row.innerHTML = `
                                <td class="product-name">${category.name}</td>
                                <td class="actions">
                                    <a href="/Category/Edit/${category.id}" class="btn btn-edit">
                                        Edit
                                    </a>
                                </td>
                                <td class="actions">
                                    <button type="button" class="btn btn-delete" onclick="showDeleteModal(${category.id}, '${category.name}')">
                                        Delete
                                    </button>
                                </td>
                            `
                            tableBody.appendChild(row)
                        })
                    } else {
                        const noResultsRow = document.createElement("tr")
                        noResultsRow.innerHTML = `
                            <td colspan="3" class="no-results">
                                <i class="bi bi-search"></i>
                                <p>No categories found matching "${searchText}"</p>
                            </td>
                        `
                        tableBody.appendChild(noResultsRow)
                    }
                })
                .catch((error) => {
                    console.error("Search error:", error)
                    showNotification("Search failed. Please try again.", "error")
                })
        })
    }
}

function initializeDeleteModal() {
    const deleteModal = document.getElementById("deleteModal")

    if (deleteModal) {
        // Close modal when clicking outside
        deleteModal.addEventListener("click", (e) => {
            if (e.target === deleteModal) {
                closeDeleteModal()
            }
        })

        // Close modal with Escape key
        document.addEventListener("keydown", (e) => {
            if (e.key === "Escape" && deleteModal.style.display === "flex") {
                closeDeleteModal()
            }
        })
    }
}

function showDeleteModal(categoryId, categoryName) {
    const deleteModal = document.getElementById("deleteModal")
    const categoryNameElement = document.getElementById("categoryNameToDelete")
    const confirmDeleteBtn = document.getElementById("confirmDeleteBtn")

    if (deleteModal && categoryNameElement && confirmDeleteBtn) {
        categoryNameElement.textContent = categoryName
        deleteModal.style.display = "flex"

        // Remove any existing event listeners to prevent multiple calls
        const newConfirmBtn = confirmDeleteBtn.cloneNode(true)
        confirmDeleteBtn.parentNode.replaceChild(newConfirmBtn, confirmDeleteBtn)

        // Define the event handler
        const confirmDeleteHandler = () => {
            window.location.href = `/Category/Delete/${categoryId}`
        }

        // Add the event listener
        newConfirmBtn.addEventListener("click", confirmDeleteHandler)
    }
}

function closeDeleteModal() {
    const deleteModal = document.getElementById("deleteModal")
    if (deleteModal) {
        deleteModal.style.display = "none"
    }
}

function initializeNotifications() {
    // This function handles TempData notifications
    // The actual notification display is handled by the showNotification function
}

function showNotification(message, type = "info") {
    // Create notification container if it doesn't exist
    let notificationContainer = document.getElementById("notificationContainer")
    if (!notificationContainer) {
        notificationContainer = document.createElement("div")
        notificationContainer.id = "notificationContainer"
        notificationContainer.style.cssText = `
            position: fixed;
            top: 20px;
            right: 20px;
            z-index: 1000;
            display: flex;
            flex-direction: column;
            gap: 0.5rem;
        `
        document.body.appendChild(notificationContainer)
    }

    // Create notification element
    const notification = document.createElement("div")
    notification.className = `notification notification-${type}`

    // Set notification styles based on type
    let backgroundColor
    switch (type) {
        case "success":
            backgroundColor = "#10b981"
            break
        case "error":
            backgroundColor = "#ef4444"
            break
        case "warning":
            backgroundColor = "#f59e0b"
            break
        default:
            backgroundColor = "#3b82f6"
    }

    notification.style.cssText = `
        background-color: ${backgroundColor};
        color: white;
        padding: 1rem 1.5rem;
        border-radius: 0.375rem;
        font-weight: 500;
        box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
        transform: translateX(100%);
        opacity: 0;
        transition: all 0.3s ease;
        display: flex;
        align-items: center;
        justify-content: space-between;
        gap: 1rem;
        min-width: 300px;
    `

    notification.innerHTML = `
        <span>${message}</span>
        <button onclick="this.parentElement.remove()" style="
            background: none;
            border: none;
            color: white;
            font-size: 1.25rem;
            cursor: pointer;
            padding: 0;
            line-height: 1;
        ">&times;</button>
    `

    notificationContainer.appendChild(notification)

    // Animate in
    setTimeout(() => {
        notification.style.transform = "translateX(0)"
        notification.style.opacity = "1"
    }, 10)

    // Auto remove after 5 seconds
    setTimeout(() => {
        if (notification.parentNode) {
            notification.style.transform = "translateX(100%)"
            notification.style.opacity = "0"
            setTimeout(() => {
                if (notification.parentNode) {
                    notification.remove()
                }
            }, 300)
        }
    }, 5000)
}
