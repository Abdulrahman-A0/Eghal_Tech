// Product Details JavaScript

// Image Modal Functionality
function openImageModal(imageSrc) {
    const modal = document.getElementById("imageModal")
    const modalImg = document.getElementById("modalImage")

    modal.style.display = "block"
    modalImg.src = imageSrc

    // Prevent body scrolling when modal is open
    document.body.style.overflow = "hidden"
}

function closeImageModal() {
    const modal = document.getElementById("imageModal")
    modal.style.display = "none"

    // Restore body scrolling
    document.body.style.overflow = "auto"
}

// Delete Modal Functionality
function showDeleteModal(productId, productName) {
    const modal = document.getElementById("deleteModal")
    const productNameElement = document.getElementById("productNameToDelete")

    productNameElement.textContent = productName
    modal.style.display = "block"

    // Prevent body scrolling when modal is open
    document.body.style.overflow = "hidden"

    // Set up delete confirmation
    const confirmBtn = document.getElementById("confirmDeleteBtn")
    confirmBtn.onclick = () => {
        // Create form and submit for deletion
        const form = document.createElement("form")
        form.method = "POST"
        form.action = `/Product/Delete/${productId}`

        // Add anti-forgery token if available
        const token = document.querySelector('input[name="__RequestVerificationToken"]')
        if (token) {
            const tokenInput = document.createElement("input")
            tokenInput.type = "hidden"
            tokenInput.name = "__RequestVerificationToken"
            tokenInput.value = token.value
            form.appendChild(tokenInput)
        }

        document.body.appendChild(form)
        form.submit()
    }
}

function closeDeleteModal() {
    const modal = document.getElementById("deleteModal")
    modal.style.display = "none"

    // Restore body scrolling
    document.body.style.overflow = "auto"
}

// Initialize when DOM is loaded
document.addEventListener("DOMContentLoaded", () => {
    const imageModal = document.getElementById("imageModal")
    const deleteModal = document.getElementById("deleteModal")

    // Close modals when clicking outside
    if (imageModal) {
        imageModal.addEventListener("click", (e) => {
            if (e.target === imageModal) {
                closeImageModal()
            }
        })
    }

    if (deleteModal) {
        deleteModal.addEventListener("click", (e) => {
            if (e.target === deleteModal) {
                closeDeleteModal()
            }
        })
    }

    // Close modals with Escape key
    document.addEventListener("keydown", (e) => {
        if (e.key === "Escape") {
            if (imageModal && imageModal.style.display === "block") {
                closeImageModal()
            }
            if (deleteModal && deleteModal.style.display === "block") {
                closeDeleteModal()
            }
        }
    })

    // Add smooth scrolling for any anchor links
    document.querySelectorAll('a[href^="#"]').forEach((anchor) => {
        anchor.addEventListener("click", function (e) {
            e.preventDefault()
            const target = document.querySelector(this.getAttribute("href"))
            if (target) {
                target.scrollIntoView({
                    behavior: "smooth",
                })
            }
        })
    })
})

// Utility function for notifications
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

// Add notification animation styles
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
