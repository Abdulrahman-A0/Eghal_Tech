// Home Page JavaScript

// Initialize when DOM is loaded
document.addEventListener("DOMContentLoaded", () => {
    initializeProductCards()
    initializeCategoryCards()
    initializeNewsletterForm()
    initializeHeroAnimations()
})

// Product Cards Functionality
function initializeProductCards() {
    const productCards = document.querySelectorAll(".product-card")

    productCards.forEach((card) => {
        const wishlistBtn = card.querySelector(".wishlist-btn")
        const addToCartBtn = card.querySelector(".add-to-cart-btn")

        // Wishlist functionality
        if (wishlistBtn) {
            wishlistBtn.addEventListener("click", (e) => {
                e.preventDefault()
                toggleWishlist(wishlistBtn)
            })
        }

        // Add to cart functionality
        if (addToCartBtn) {
            addToCartBtn.addEventListener("click", (e) => {
                e.preventDefault()
                addToCart(card)
            })
        }
    })
}

function toggleWishlistStyle(btn, isInWishlist) {
    const icon = btn.querySelector("i");

    if (isInWishlist) {
        icon.className = "bi bi-heart-fill";
        btn.classList.add("active");
        showNotification("Added to wishlist!", "success");
    } else {
        icon.className = "bi bi-heart";
        btn.classList.remove("active");
        showNotification("Removed from wishlist", "info");
    }
}


// Category Cards Functionality
function initializeCategoryCards() {
    const categoryCards = document.querySelectorAll(".category-card")

    categoryCards.forEach((card) => {
        card.addEventListener("click", () => {
            const categoryTitle = card.querySelector(".category-title").textContent
            // In a real app, this would navigate to the category page
            window.location.href = `/Store/Index`
        })
    })
}

// Newsletter Form
function initializeNewsletterForm() {
    const newsletterForm = document.querySelector(".newsletter-form-large")

    if (newsletterForm) {
        newsletterForm.addEventListener("submit", (e) => {
            e.preventDefault()

            const emailInput = newsletterForm.querySelector(".newsletter-input-large")
            const email = emailInput.value.trim()

            if (email && window.EghalTech.isValidEmail(email)) {
                // Show loading state
                const submitBtn = newsletterForm.querySelector(".newsletter-btn-large")
                const originalContent = submitBtn.innerHTML
                submitBtn.innerHTML = '<i class="bi bi-arrow-clockwise"></i> Subscribing...'
                submitBtn.disabled = true

                // Simulate API call
                setTimeout(() => {
                    // Reset form
                    emailInput.value = ""
                    submitBtn.innerHTML = originalContent
                    submitBtn.disabled = false

                    // Show success message
                    window.EghalTech.showNotification("Successfully subscribed to newsletter!", "success")
                }, 1500)
            } else {
                window.EghalTech.showNotification("Please enter a valid email address.", "error")
            }
        })
    }
}

// Hero Animations
function initializeHeroAnimations() {
    // Animate hero elements on scroll
    const heroSection = document.querySelector(".hero-section")
    const heroTitle = document.querySelector(".hero-title")
    const heroDescription = document.querySelector(".hero-description")
    const heroActions = document.querySelector(".hero-actions")
    const heroStats = document.querySelector(".hero-stats")

    if (heroSection) {
        // Add entrance animations
        setTimeout(() => {
            if (heroTitle) heroTitle.style.animation = "fadeInUp 0.8s ease forwards"
        }, 200)

        setTimeout(() => {
            if (heroDescription) heroDescription.style.animation = "fadeInUp 0.8s ease forwards"
        }, 400)

        setTimeout(() => {
            if (heroActions) heroActions.style.animation = "fadeInUp 0.8s ease forwards"
        }, 600)

        setTimeout(() => {
            if (heroStats) heroStats.style.animation = "fadeInUp 0.8s ease forwards"
        }, 800)
    }

    // Animate sections on scroll
    const observerOptions = {
        threshold: 0.1,
        rootMargin: "0px 0px -50px 0px",
    }

    const observer = new IntersectionObserver((entries) => {
        entries.forEach((entry) => {
            if (entry.isIntersecting) {
                entry.target.style.animation = "fadeInUp 0.8s ease forwards"
                observer.unobserve(entry.target)
            }
        })
    }, observerOptions)

    // Observe sections
    const sections = document.querySelectorAll(".categories-section, .featured-section, .features-section")
    sections.forEach((section) => {
        section.style.opacity = "0"
        section.style.transform = "translateY(30px)"
        observer.observe(section)
    })
}

// Smooth scrolling for anchor links
document.addEventListener("click", (e) => {
    const target = e.target.closest('a[href^="#"]')
    if (target) {
        e.preventDefault()
        const targetId = target.getAttribute("href").substring(1)
        const targetElement = document.getElementById(targetId)

        if (targetElement) {
            targetElement.scrollIntoView({
                behavior: "smooth",
                block: "start",
            })
        }
    }
})

// Add CSS animations
const homeAnimationStyle = document.createElement("style")
homeAnimationStyle.textContent = `
  @keyframes fadeInUp {
    from {
      opacity: 0;
      transform: translateY(30px);
    }
    to {
      opacity: 1;
      transform: translateY(0);
    }
  }

  .hero-title,
  .hero-description,
  .hero-actions,
  .hero-stats {
    opacity: 0;
    transform: translateY(30px);
  }

  .product-card {
    transition: all 0.3s ease;
  }

  .category-card {
    transition: all 0.3s ease;
  }

  .feature-card {
    transition: all 0.3s ease;
  }

  /* Loading animation for buttons */
  @keyframes spin {
    0% { transform: rotate(0deg); }
    100% { transform: rotate(360deg); }
  }

  .bi-arrow-clockwise {
    animation: spin 1s linear infinite;
  }
`
document.head.appendChild(homeAnimationStyle)
