// Store Product Page JavaScript

// Initialize when DOM is loaded
document.addEventListener("DOMContentLoaded", () => {
    initializeImageGallery()
    initializeProductTabs()
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

// Review Form
$(document).ready(function () {

    // Star rating selection
    $(".star-btn").click(function () {
        const rating = $(this).data("rating");
        $("#ratingValue").val(rating);
        $(".star-btn").removeClass("active");
        $(".star-btn i").removeClass("bi-star-fill").addClass("bi-star");
        $(".star-btn").each(function () {
            if ($(this).data("rating") <= rating) {
                $(this).addClass("active");
                $(this).find("i").removeClass("bi-star").addClass("bi-star-fill");
            }
        });
    });

    $("#submitReviewBtn").click(function () {

        $("#ratingValidation").text("");
        $("#commentValidation").text("");

        const model = {
            ProductId: $("#productId").val(),
            Rating: parseInt($("#ratingValue").val()),
            Comment: $("#reviewComment").val().trim()
        };

        let hasError = false;
        if (model.Rating <= 0) {
            $("#ratingValidation").text("Please select a rating.");
            hasError = true;
        }

        if (hasError) return;

        $.ajax({
            url: "/Review/Add",
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify(model),
            success: function (response) {
                if (response.success) {
                    const review = response.review;

                    let stars = "";
                    for (let i = 1; i <= 5; i++) {
                        stars += `<i class="bi ${(i <= review.rating ? "bi-star-fill" : "bi-star")}"></i>`;
                    }

                    const html = `
                    <div class="review-item">
                        <div class="review-header">
                            <div class="reviewer-info">
                                <span class="reviewer-name">${review.userName}</span>
                            </div>
                            <div class="review-meta">
                                <div class="stars" data-rating="${review.rating}">
                                    ${stars}
                                </div>
                                <span class="review-date">${review.date}</span>
                            </div>
                        </div>
                        <p class="review-comment">${review.comment}</p>
                        <div class="review-actions"></div>
                    </div>
                `;
                    $(".reviews-list").prepend(html);
                    $("#reviewFormContainer").hide();
                    $("#ratingValue").val(0);
                    $("#reviewComment").val("");
                    $(".star-btn i").removeClass("bi-star-fill").addClass("bi-star");
                    showNotification("Thank you for your feedback!", "success");
                }
            },
            error: function (xhr) {
                if (xhr.status === 401 || xhr.responseText.includes("/Account/Login")) {
                    window.location.href = "/Account/Login?ReturnUrl=" + encodeURIComponent(window.location.pathname);
                }
                else if (xhr.responseJSON?.errors) {
                    const errors = xhr.responseJSON.errors;
                    if (errors.Rating?.length) {
                        $("#ratingValidation").text(errors.Rating[0]);
                    }

                } else {
                    alert("Something went wrong. Please try again.");
                }
            }
        });
    });
});