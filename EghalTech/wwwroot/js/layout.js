// Layout JavaScript

// Initialize when DOM is loaded
document.addEventListener("DOMContentLoaded", () => {
    initializeMobileMenu()
    initializeSearch()
    initializeNewsletterForm()
    initializeScrollEffects()
})

// Mobile Menu Functionality
function initializeMobileMenu() {
    const mobileMenuBtn = document.querySelector(".mobile-menu-btn")
    const mobileMenu = document.getElementById("mobileMenu")
    const mobileMenuClose = document.querySelector(".mobile-menu-close")
    const mobileOverlay = document.querySelector(".mobile-overlay")
    const mobileDropdownTrigger = document.querySelector(".mobile-dropdown-trigger")
    const mobileDropdown = document.querySelector(".mobile-dropdown")

    // Create overlay if it doesn't exist
    if (!mobileOverlay) {
        const overlay = document.createElement("div")
        overlay.className = "mobile-overlay"
        document.body.appendChild(overlay)
    }

    // Toggle mobile menu
    function toggleMobileMenu() {
        const overlay = document.querySelector(".mobile-overlay")
        mobileMenu.classList.toggle("active")
        overlay.classList.toggle("active")
        document.body.style.overflow = mobileMenu.classList.contains("active") ? "hidden" : ""
    }

    // Close mobile menu
    function closeMobileMenu() {
        const overlay = document.querySelector(".mobile-overlay")
        mobileMenu.classList.remove("active")
        overlay.classList.remove("active")
        document.body.style.overflow = ""
    }

    // Event listeners
    if (mobileMenuBtn) {
        mobileMenuBtn.addEventListener("click", toggleMobileMenu)
    }

    if (mobileMenuClose) {
        mobileMenuClose.addEventListener("click", closeMobileMenu)
    }

    // Close menu when clicking overlay
    document.addEventListener("click", (e) => {
        if (e.target.classList.contains("mobile-overlay")) {
            closeMobileMenu()
        }
    })

    // Mobile dropdown functionality
    if (mobileDropdownTrigger && mobileDropdown) {
        mobileDropdownTrigger.addEventListener("click", () => {
            mobileDropdown.classList.toggle("active")
        })
    }
}

// Search Functionality
function initializeSearch() {
    const searchInput = document.querySelector(".search-input")
    const searchBtn = document.querySelector(".search-btn")

    if (searchBtn) {
        searchBtn.addEventListener("click", (e) => {
            e.preventDefault()
            const query = searchInput.value.trim()
            window.location.href = `/Store/Search?prodName=${encodeURIComponent(query)}`;
        })
    }

    if (searchInput) {
        searchInput.addEventListener("keypress", (e) => {
            if (e.key === "Enter") {
                e.preventDefault()
                searchBtn.click()
            }
        })
    }
}

// Newsletter Form
function initializeNewsletterForm() {
    const newsletterForm = document.querySelector(".newsletter-form")

    if (newsletterForm) {
        newsletterForm.addEventListener("submit", (e) => {
            e.preventDefault()
            const emailInput = newsletterForm.querySelector(".newsletter-input")
            const email = emailInput.value.trim()

            if (email && isValidEmail(email)) {
                // Implement newsletter subscription here
                console.log("Newsletter subscription for:", email)
                showNotification("Thank you for subscribing to our newsletter!", "success")
                emailInput.value = ""
            } else {
                showNotification("Please enter a valid email address.", "error")
            }
        })
    }
}

// Scroll Effects
function initializeScrollEffects() {
    const header = document.querySelector(".main-header")
    let lastScrollY = window.scrollY

    window.addEventListener("scroll", () => {
        const currentScrollY = window.scrollY

        // Add shadow to header when scrolling
        if (currentScrollY > 10) {
            header.style.boxShadow = "0 2px 10px rgba(0, 0, 0, 0.1)"
        } else {
            header.style.boxShadow = "0 1px 3px rgba(0, 0, 0, 0.1)"
        }

        lastScrollY = currentScrollY
    })
}

// Notification System
function showNotification(message, type = "success") {
    // Remove existing notifications
    const existingNotifications = document.querySelectorAll(".notification")
    existingNotifications.forEach((n) => n.remove())

    const notification = document.createElement("div")
    notification.className = `notification notification-${type}`
    notification.style.cssText = `
        position: fixed;
        top: 50px;
        right: 20px;
        padding: 1rem 1.5rem;
        border-radius: 0.375rem;
        color: white;
        font-weight: 500;
        z-index: 1000;
        animation: slideIn 0.3s ease;
        max-width: 300px;
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
    }, 2000)
}

// Utility Functions
function isValidEmail(email) {
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/
    return emailRegex.test(email)
}

function debounce(func, wait) {
    let timeout
    return function executedFunction(...args) {
        const later = () => {
            clearTimeout(timeout)
            func(...args)
        }
        clearTimeout(timeout)
        timeout = setTimeout(later, wait)
    }
}



// Export functions for use in other scripts
window.EghalTech = {
    showNotification,
    updateCartCount,
    isValidEmail,
    debounce,
}
