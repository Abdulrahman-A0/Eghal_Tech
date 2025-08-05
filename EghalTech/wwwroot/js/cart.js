// Smooth scroll to top when navigating
function scrollToTop() {
    window.scrollTo({
        top: 0,
        behavior: "smooth",
    })
}

function AddToCart(prodId, btnElement) {
    const originalContent = btnElement.innerHTML;
    btnElement.innerHTML = '<i class="bi bi-arrow-clockwise"></i> Adding...';
    btnElement.disabled = true;

    $.ajax({
        url: `/Cart/AddToCart`,
        type: 'POST',
        data: { prodId: prodId },
        headers: {
            'X-Requested-With': 'XMLHttpRequest'
        },
        success: function (result) {
            $(".cart-count").html(result.count);
            setTimeout(() => {
                btnElement.innerHTML = originalContent;
                btnElement.disabled = false;

                if (result.message) {
                    showNotification(result.message, "success");
                }
            }, 500);
        },
        error: function (xhr) {
            console.error("AddToCart failed", xhr.status, xhr.responseText);
            setTimeout(() => {
                btnElement.innerHTML = originalContent;
                btnElement.disabled = false;

                if (xhr.status === 401 || xhr.responseText.includes("/Account/Login")) {
                    window.location.href = "/Account/Login?ReturnUrl=" + encodeURIComponent(window.location.pathname);
                } else {
                    showNotification("An error occurred while adding to cart.", "error");
                }
            }, 500);
        }
    });
}

