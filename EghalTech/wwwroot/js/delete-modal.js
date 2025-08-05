// Delete Modal Functionality
let productToDelete = null

// Show delete confirmation modal
function showDeleteModal(productId, productName) {
    productToDelete = {
        id: productId,
        name: productName,
    }

    const modal = document.getElementById("deleteModal")
    const productNameElement = document.getElementById("productNameToDelete")

    productNameElement.textContent = productName
    modal.style.display = "block"

    // Prevent body scrolling when modal is open
    document.body.style.overflow = "hidden"

    // Focus on the cancel button for accessibility
    setTimeout(() => {
        const cancelBtn = modal.querySelector(".btn-secondary")
        if (cancelBtn) cancelBtn.focus()
    }, 100)
}

// Close delete confirmation modal
function closeDeleteModal() {
    const modal = document.getElementById("deleteModal")
    modal.style.display = "none"
    productToDelete = null

    // Restore body scrolling
    document.body.style.overflow = "auto"
}

// Confirm delete action
function confirmDelete() {
    if (!productToDelete) return

    const confirmBtn = document.getElementById("confirmDeleteBtn")
    const originalText = confirmBtn.textContent

    // Show loading state
    confirmBtn.classList.add("loading")
    confirmBtn.disabled = true

    // Create a form and submit it to delete the product
    const form = document.createElement("form")
    form.method = "POST"
    form.action = `/Product/Delete/${productToDelete.id}`

    // Add anti-forgery token
    const token = document.querySelector('input[name="__RequestVerificationToken"]')
    if (token) {
        const tokenInput = document.createElement("input")
        tokenInput.type = "hidden"
        tokenInput.name = "__RequestVerificationToken"
        tokenInput.value = token.value
        form.appendChild(tokenInput)
    }

    // Add the form to the page and submit it
    document.body.appendChild(form)
    form.submit()
}

// Initialize modal functionality when DOM is loaded
document.addEventListener("DOMContentLoaded", () => {
    const modal = document.getElementById("deleteModal")
    const confirmBtn = document.getElementById("confirmDeleteBtn")

    // Add click event to confirm button
    if (confirmBtn) {
        confirmBtn.addEventListener("click", confirmDelete)
    }

    // Close modal when clicking outside of it
    if (modal) {
        modal.addEventListener("click", (e) => {
            if (e.target === modal) {
                closeDeleteModal()
            }
        })
    }

    // Close modal with Escape key
    document.addEventListener("keydown", (e) => {
        if (e.key === "Escape" && modal && modal.style.display === "block") {
            closeDeleteModal()
        }
    })
})

// Show notification function (if not already defined)
function showNotification(message, type = "success") {
    const notification = document.createElement("div")
    notification.className = `notification notification-${type}`
    notification.style.cssText = `
        position: fixed;
        top: 20px;
        right: 20px;
        padding: 1rem 1.5rem;
        border-radius: 0.375rem;
        color: white;
        font-weight: 500;
        z-index: 1001;
        animation: slideInNotification 0.3s ease;
    `

    if (type === "success") {
        notification.style.backgroundColor = "#10b981"
    } else if (type === "error") {
        notification.style.backgroundColor = "#ef4444"
    } else if (type === "info") {
        notification.style.backgroundColor = "#3b82f6"
    }

    notification.textContent = message
    document.body.appendChild(notification)

    setTimeout(() => {
        notification.remove()
    }, 3000)
}

// Add notification animation
const notificationStyle = document.createElement("style")
notificationStyle.textContent = `
    @keyframes slideInNotification {
        from {
            transform: translateX(100%);
            opacity: 0;
        }
        to {
            transform: translateX(0);
            opacity: 1;
        }
    }
`
document.head.appendChild(notificationStyle)
