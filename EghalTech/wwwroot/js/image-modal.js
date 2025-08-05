// Image Modal Functionality
document.addEventListener("DOMContentLoaded", () => {
    // Create modal HTML
    const modalHTML = `
        <div id="imageModal" class="image-modal">
            <span class="image-modal-close">&times;</span>
            <img class="image-modal-content" id="modalImage">
        </div>
    `

    // Add modal to body
    document.body.insertAdjacentHTML("beforeend", modalHTML)

    const modal = document.getElementById("imageModal")
    const modalImg = document.getElementById("modalImage")
    const closeBtn = document.querySelector(".image-modal-close")

    // Add click event to all product thumbnails
    function addImageClickEvents() {
        const thumbnails = document.querySelectorAll(".product-thumbnail")
        thumbnails.forEach((thumbnail) => {
            thumbnail.addEventListener("click", function () {
                modal.style.display = "block"
                modalImg.src = this.src
                modalImg.alt = this.alt
            })
        })
    }

    // Initial setup
    addImageClickEvents()

    // Re-add events after search results update
    const originalFetch = window.fetch
    window.fetch = function (...args) {
        return originalFetch.apply(this, args).then((response) => {
            // If this is a search request, re-add image click events after DOM update
            if (args[0] && args[0].includes("/Product/SearchProducts")) {
                setTimeout(addImageClickEvents, 100)
            }
            return response
        })
    }

    // Close modal events
    closeBtn.addEventListener("click", () => {
        modal.style.display = "none"
    })

    modal.addEventListener("click", (e) => {
        if (e.target === modal) {
            modal.style.display = "none"
        }
    })

    // Close modal with Escape key
    document.addEventListener("keydown", (e) => {
        if (e.key === "Escape" && modal.style.display === "block") {
            modal.style.display = "none"
        }
    })
})
