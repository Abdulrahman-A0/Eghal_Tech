// Store Product Page JavaScript

// Initialize when DOM is loaded
document.addEventListener("DOMContentLoaded", () => {
    initializeImageGallery()
    initializeProductTabs()
    initializeReviewForm()
    initializeStarRatings()
})

// Image Gallery Functionality - Simplified for single image
function initializeImageGallery() {
    const mainImage = document.getElementById("mainImage")

    // Add zoom functionality to main image
    if (mainImage) {
        mainImage.addEventListener("click", () => {
            openImageModal(mainImage.src)
        })
    }
}

// Image Modal for zoom
function openImageModal(imageSrc) {
    // Create modal if it doesn't exist
    let modal = document.getElementById("imageModal")
    if (!modal) {
        modal = document.createElement("div")
        modal.id = "imageModal"
        modal.className = "image-modal"
        modal.innerHTML = `
            <span class="image-modal-close">&times;</span>
            <img class="image-modal-content" id="modalImage">
        `
        document.body.appendChild(modal)

        // Add close functionality
        const closeBtn = modal.querySelector(".image-modal-close")
        closeBtn.addEventListener("click", () => {
            modal.style.display = "none"
            document.body.style.overflow = "auto"
        })

        modal.addEventListener("click", (e) => {
            if (e.target === modal) {
                modal.style.display = "none"
                document.body.style.overflow = "auto"
            }
        })
    }

    const modalImg = document.getElementById("modalImage")
    modal.style.display = "block"
    modalImg.src = imageSrc
    document.body.style.overflow = "hidden"
}

// Product Tabs Functionality
function initializeProductTabs() {
    const tabBtns = document.querySelectorAll(".tab-btn")
    const tabPanels = document.querySelectorAll(".tab-panel")

    tabBtns.forEach((btn) => {
        btn.addEventListener("click", () => {
            const targetTab = btn.dataset.tab

            // Remove active class from all tabs and panels
            tabBtns.forEach((b) => b.classList.remove("active"))
            tabPanels.forEach((p) => p.classList.remove("active"))

            // Add active class to clicked tab and corresponding panel
            btn.classList.add("active")
            const targetPanel = document.getElementById(targetTab)
            if (targetPanel) {
                targetPanel.classList.add("active")
            }
        })
    })
}

// Review Form Functionality
function initializeReviewForm() {
    const starRating = document.getElementById("starRating")
    const ratingValue = document.getElementById("ratingValue")

    if (starRating && ratingValue) {
        const starBtns = starRating.querySelectorAll(".star-btn")

        starBtns.forEach((btn, index) => {
            btn.addEventListener("click", () => {
                const rating = Number.parseInt(btn.dataset.rating)
                ratingValue.value = rating

                // Update star display
                starBtns.forEach((starBtn, starIndex) => {
                    if (starIndex < rating) {
                        starBtn.classList.add("active")
                        starBtn.innerHTML = '<i class="bi bi-star-fill"></i>'
                    } else {
                        starBtn.classList.remove("active")
                        starBtn.innerHTML = '<i class="bi bi-star"></i>'
                    }
                })
            })

            // Hover effect
            btn.addEventListener("mouseenter", () => {
                const rating = Number.parseInt(btn.dataset.rating)
                starBtns.forEach((starBtn, starIndex) => {
                    if (starIndex < rating) {
                        starBtn.innerHTML = '<i class="bi bi-star-fill"></i>'
                    } else {
                        starBtn.innerHTML = '<i class="bi bi-star"></i>'
                    }
                })
            })
        })

        // Reset to selected rating on mouse leave
        starRating.addEventListener("mouseleave", () => {
            const currentRating = Number.parseInt(ratingValue.value)
            starBtns.forEach((starBtn, starIndex) => {
                if (starIndex < currentRating) {
                    starBtn.innerHTML = '<i class="bi bi-star-fill"></i>'
                } else {
                    starBtn.innerHTML = '<i class="bi bi-star"></i>'
                }
            })
        })
    }
}

// Initialize Star Ratings Display
function initializeStarRatings() {
    const starContainers = document.querySelectorAll(".stars")

    starContainers.forEach((container) => {
        const rating = Number.parseFloat(container.dataset.rating) || 0
        const stars = container.querySelectorAll("i")

        stars.forEach((star, index) => {
            if (index < Math.floor(rating)) {
                star.className = "bi bi-star-fill"
            } else if (index < rating) {
                star.className = "bi bi-star-half"
            } else {
                star.className = "bi bi-star"
            }
        })
    })
}

// Show Review Form
function showReviewForm() {
    const reviewForm = document.getElementById("reviewForm")
    if (reviewForm) {
        reviewForm.style.display = "block"
        reviewForm.scrollIntoView({ behavior: "smooth" })
    }
}

// Hide Review Form
function hideReviewForm() {
    const reviewForm = document.getElementById("reviewForm")
    if (reviewForm) {
        reviewForm.style.display = "none"
        // Reset form
        reviewForm.querySelector("form").reset()
        document.getElementById("ratingValue").value = "5"
        // Reset stars
        const starBtns = document.querySelectorAll(".star-btn")
        starBtns.forEach((btn, index) => {
            if (index < 5) {
                btn.classList.add("active")
                btn.innerHTML = '<i class="bi bi-star-fill"></i>'
            } else {
                btn.classList.remove("active")
                btn.innerHTML = '<i class="bi bi-star"></i>'
            }
        })
    }
}

// Helpful Button Functionality
document.addEventListener("click", (e) => {
    if (e.target.closest(".helpful-btn")) {
        const btn = e.target.closest(".helpful-btn")
        // In a real app, this would make an AJAX call to update the helpful count
        showNotification("Thank you for your feedback!", "success")
    }
})

// Add CSS for notifications and modal
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

    .image-modal {
        display: none;
        position: fixed;
        z-index: 1000;
        left: 0;
        top: 0;
        width: 100%;
        height: 100%;
        background-color: rgba(0, 0, 0, 0.8);
        cursor: pointer;
    }

    .image-modal-content {
        position: absolute;
        top: 50%;
        left: 50%;
        transform: translate(-50%, -50%);
        max-width: 90%;
        max-height: 90%;
        border-radius: 0.5rem;
        box-shadow: 0 10px 25px rgba(0, 0, 0, 0.5);
    }

    .image-modal-close {
        position: absolute;
        top: 1rem;
        right: 1rem;
        color: white;
        font-size: 2rem;
        font-weight: bold;
        cursor: pointer;
        background: rgba(0, 0, 0, 0.5);
        border-radius: 50%;
        width: 40px;
        height: 40px;
        display: flex;
        align-items: center;
        justify-content: center;
        transition: background-color 0.3s ease;
    }

    .image-modal-close:hover {
        background: rgba(0, 0, 0, 0.7);
    }
`
document.head.appendChild(style)

// Close modal with Escape key
document.addEventListener("keydown", (e) => {
    if (e.key === "Escape") {
        const modal = document.getElementById("imageModal")
        if (modal && modal.style.display === "block") {
            modal.style.display = "none"
            document.body.style.overflow = "auto"
        }
    }
})
