function addToWishList(prodId, btnelement) {

    $.ajax({
        url: `/WishList/ToggleWishList`,
        type: `POST`,
        data: { prodId: prodId },
        headers: {
            'X-Requested-With': 'XMLHttpRequest'
        },
        success: function (result) {
            toggleWishlistStyle(btnelement, result.isInWishList)
            refreshWishlistIcon()
        },
        error: function (xhr) {
            if (xhr.status === 401 || xhr.responseText.includes("/Account/Login")) {
                window.location.href = "/Account/Login?ReturnUrl=" + encodeURIComponent(window.location.pathname);
            }
            else {
                showNotification("An error occurred while adding to wishList.", "error");
            }
        }
    });
}

function refreshWishlistIcon() {
    $.ajax({
        url: "/WishList/HasItems",
        type: "GET",
        headers: {
            'X-Requested-With': 'XMLHttpRequest'
        },
        success: function (result) {
            const headerIcon = document.querySelector("#wish-list-i");
            if (result.hasItems) {
                headerIcon.className = "bi bi-heart-fill red-heart";
            } else {
                headerIcon.className = "bi bi-heart";
            }
        },
        error: function () {
            console.error("Could not refresh wishlist icon");
        }
    });
}

function DeleteFromWishList(itemId) {
    $.ajax({
        url: `WishList/Delete`,
        type: `POST`,
        data: { id: itemId },
        success: function (result) {
            $(`[data-item-id=${result.itemId}]`).remove();
            $(".items-count").html(result.itemsCount);
            $(".items-price").html(result.itemsPrice);
            $(".items-stock").html(result.itemsStock);

            if (result.itemsCount === 0) {
                $(".wishlist-summary").hide();
                $(".wishlist-item").hide();
                $(".empty-state").fadeIn();
            }

            refreshWishlistIcon()
        }
    });
};