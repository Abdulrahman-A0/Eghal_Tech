// Simplified edit product JavaScript - only includes necessary functions

// Initialize when DOM is loaded
document.addEventListener("DOMContentLoaded", () => {
    initializeImageUpload()
})

// Remove current image functionality
function removeCurrentImage() {
    const currentImageContainer = document.getElementById("currentImageContainer")
    const removeImageInput = document.getElementById("removeImage")

    if (currentImageContainer && removeImageInput) {
        currentImageContainer.classList.add("removed")
        removeImageInput.value = "true"

        // Show notification
        showNotification("Current image will be removed when you save", "info")
    }
}

// Single Image Upload Functionality
function initializeImageUpload() {
    const imageUpload = document.getElementById("imageUpload")
    const imageUploadArea = document.getElementById("imageUploadArea")

    // Handle file selection
    imageUpload.addEventListener("change", (e) => {
        if (e.target.files.length > 0) {
            handleSingleImage(e.target.files[0])
        }
    })

    // Handle drag and drop
    imageUploadArea.addEventListener("dragover", (e) => {
        e.preventDefault()
        imageUploadArea.style.borderColor = "#ef4444"
    })

    imageUploadArea.addEventListener("dragleave", (e) => {
        e.preventDefault()
        imageUploadArea.style.borderColor = "#d1d5db"
    })

    imageUploadArea.addEventListener("drop", (e) => {
        e.preventDefault()
        imageUploadArea.style.borderColor = "#d1d5db"
        if (e.dataTransfer.files.length > 0) {
            handleSingleImage(e.dataTransfer.files[0])
        }
    })
}

function handleSingleImage(file) {
    if (file.type.startsWith("image/")) {
        const reader = new FileReader()
        reader.onload = (e) => {
            displaySingleImagePreview(e.target.result)

            // If there's a current image, hide the container since we're replacing it
            const currentImageContainer = document.getElementById("currentImageContainer")
            if (currentImageContainer) {
                currentImageContainer.style.display = "none"
            }
        }
        reader.readAsDataURL(file)
    }
}

function displaySingleImagePreview(imageUrl) {
    const imagePreview = document.getElementById("imagePreview")

    imagePreview.innerHTML = `
    <div class="single-image-item">
      <img src="${imageUrl}" alt="New product image">
      <button type="button" class="remove-image-btn" onclick="removeSingleImage()">
        <svg viewBox="0 0 24 24" fill="none" stroke="currentColor">
          <line x1="18" y1="6" x2="6" y2="18"/>
          <line x1="6" y1="6" x2="18" y2="18"/>
        </svg>
      </button>
    </div>
  `
}

function removeSingleImage() {
    const imagePreview = document.getElementById("imagePreview")
    imagePreview.innerHTML = ""

    // Reset the file input
    const imageUpload = document.getElementById("imageUpload")
    imageUpload.value = ""

    // Show current image again if it exists
    const currentImageContainer = document.getElementById("currentImageContainer")
    const removeImageInput = document.getElementById("removeImage")

    if (currentImageContainer && removeImageInput.value !== "true") {
        currentImageContainer.style.display = "block"
    }
}

// Utility Functions
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
    z-index: 1000;
    animation: slideIn 0.3s ease;
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

// Add CSS animation for notifications
const style = document.createElement("style")
style.textContent = `
  @keyframes slideIn {
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
document.head.appendChild(style)
