// Admin Navbar JavaScript
document.addEventListener("DOMContentLoaded", () => {
    const mobileMenuToggle = document.getElementById("mobileMenuToggle")
    const mobileNavOverlay = document.getElementById("mobileNavOverlay")
    const mobileNavClose = document.getElementById("mobileNavClose")

    // Open mobile menu
    if (mobileMenuToggle) {
        mobileMenuToggle.addEventListener("click", () => {
            mobileNavOverlay.classList.add("active")
            document.body.style.overflow = "hidden"
        })
    }

    // Close mobile menu
    function closeMobileMenu() {
        mobileNavOverlay.classList.remove("active")
        document.body.style.overflow = ""
    }

    if (mobileNavClose) {
        mobileNavClose.addEventListener("click", closeMobileMenu)
    }

    // Close mobile menu when clicking overlay
    if (mobileNavOverlay) {
        mobileNavOverlay.addEventListener("click", (e) => {
            if (e.target === mobileNavOverlay) {
                closeMobileMenu()
            }
        })
    }

    // Close mobile menu on escape key
    document.addEventListener("keydown", (e) => {
        if (e.key === "Escape" && mobileNavOverlay.classList.contains("active")) {
            closeMobileMenu()
        }
    })

    // Close mobile menu when window is resized to desktop
    window.addEventListener("resize", () => {
        if (window.innerWidth > 768 && mobileNavOverlay.classList.contains("active")) {
            closeMobileMenu()
        }
    })
})
