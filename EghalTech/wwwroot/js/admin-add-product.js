// Global variables
let selectedImage = null
let tags = []

// Initialize when DOM is loaded
document.addEventListener("DOMContentLoaded", () => {
    initializeImageUpload()
    initializeSpecifications()
    initializeTags()
    initializeFormValidation()
})

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
            selectedImage = {
                file: file,
                url: e.target.result,
                id: Date.now(),
            }
            displaySingleImagePreview()
        }
        reader.readAsDataURL(file)
    }
}

function displaySingleImagePreview() {
    const imagePreview = document.getElementById("imagePreview")

    imagePreview.innerHTML = `
    <div class="single-image-item">
      <img src="${selectedImage.url}" alt="Product image">
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
    selectedImage = null
    const imagePreview = document.getElementById("imagePreview")
    imagePreview.innerHTML = ""

    // Reset the file input
    const imageUpload = document.getElementById("imageUpload")
    imageUpload.value = ""
}

// Specifications Functionality
function initializeSpecifications() {
    const addSpecBtn = document.getElementById("addSpecBtn")
    addSpecBtn.addEventListener("click", addSpecification)
}

function addSpecification() {
    const container = document.getElementById("specificationsContainer")
    const specRow = document.createElement("div")
    specRow.className = "specification-row"

    specRow.innerHTML = `
        <input type="text" name="SpecificationKeys[]" placeholder="e.g., Display Size" class="spec-key">
        <input type="text" name="SpecificationValues[]" placeholder="e.g., 6.7 inches" class="spec-value">
        <button type="button" class="remove-spec-btn" onclick="removeSpecification(this)">
            <svg viewBox="0 0 24 24" fill="none" stroke="currentColor">
                <line x1="18" y1="6" x2="6" y2="18"/>
                <line x1="6" y1="6" x2="18" y2="18"/>
            </svg>
        </button>
    `

    container.appendChild(specRow)
}

function removeSpecification(button) {
    const specRow = button.closest(".specification-row")
    const container = document.getElementById("specificationsContainer")

    // Don't remove if it's the last specification row
    if (container.children.length > 1) {
        specRow.remove()
    }
}

// Tags Functionality
function initializeTags() {
    const tagInput = document.getElementById("tagInput")
    const addTagBtn = document.getElementById("addTagBtn")

    addTagBtn.addEventListener("click", addTag)
    tagInput.addEventListener("keypress", (e) => {
        if (e.key === "Enter") {
            e.preventDefault()
            addTag()
        }
    })
}

function addTag() {
    const tagInput = document.getElementById("tagInput")
    const tagValue = tagInput.value.trim()

    if (tagValue && !tags.includes(tagValue)) {
        tags.push(tagValue)
        displayTag(tagValue)
        updateTagsHidden()
        tagInput.value = ""
    }
}

function displayTag(tagValue) {
    const tagsContainer = document.getElementById("tagsContainer")

    const tagItem = document.createElement("div")
    tagItem.className = "tag-item"
    tagItem.dataset.tag = tagValue

    tagItem.innerHTML = `
        ${tagValue}
        <button type="button" class="tag-remove" onclick="removeTag('${tagValue}')">
            <svg viewBox="0 0 24 24" fill="none" stroke="currentColor">
                <line x1="18" y1="6" x2="6" y2="18"/>
                <line x1="6" y1="6" x2="18" y2="18"/>
            </svg>
        </button>
    `

    tagsContainer.appendChild(tagItem)
}

function removeTag(tagValue) {
    tags = tags.filter((tag) => tag !== tagValue)
    const tagItem = document.querySelector(`[data-tag="${tagValue}"]`)
    if (tagItem) {
        tagItem.remove()
    }
    updateTagsHidden()
}

function updateTagsHidden() {
    const tagsHidden = document.getElementById("tagsHidden")
    tagsHidden.value = tags.join(",")
}

// Form Validation
function initializeFormValidation() {
    const form = document.getElementById("addProductForm")

    form.addEventListener("submit", (e) => {
        if (!validateForm()) {
            e.preventDefault()
        }
    })

    // Real-time validation
    const requiredFields = form.querySelectorAll("input[required], textarea[required], select[required]")
    requiredFields.forEach((field) => {
        field.addEventListener("blur", () => {
            validateField(field)
        })
    })
}

function validateForm() {
    const form = document.getElementById("addProductForm")
    const requiredFields = form.querySelectorAll("input[required], textarea[required], select[required]")
    let isValid = true

    requiredFields.forEach((field) => {
        if (!validateField(field)) {
            isValid = false
        }
    })

    // Validate price
    const priceField = document.getElementById("price")
    if (priceField.value && Number.parseFloat(priceField.value) <= 0) {
        showFieldError(priceField, "Price must be greater than 0")
        isValid = false
    }

    // Validate stock
    const stockField = document.getElementById("stock")
    if (stockField.value && Number.parseInt(stockField.value) < 0) {
        showFieldError(stockField, "Stock cannot be negative")
        isValid = false
    }

    return isValid
}

function validateField(field) {
    clearFieldError(field)

    if (field.hasAttribute("required") && !field.value.trim()) {
        showFieldError(field, "This field is required")
        return false
    }

    if (field.type === "email" && field.value && !isValidEmail(field.value)) {
        showFieldError(field, "Please enter a valid email address")
        return false
    }

    return true
}

function showFieldError(field, message) {
    clearFieldError(field)

    field.style.borderColor = "#ef4444"

    const errorDiv = document.createElement("div")
    errorDiv.className = "field-error"
    errorDiv.style.color = "#ef4444"
    errorDiv.style.fontSize = "0.75rem"
    errorDiv.style.marginTop = "0.25rem"
    errorDiv.textContent = message

    field.parentNode.appendChild(errorDiv)
}

function clearFieldError(field) {
    field.style.borderColor = "#d1d5db"

    const existingError = field.parentNode.querySelector(".field-error")
    if (existingError) {
        existingError.remove()
    }
}

function isValidEmail(email) {
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/
    return emailRegex.test(email)
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
