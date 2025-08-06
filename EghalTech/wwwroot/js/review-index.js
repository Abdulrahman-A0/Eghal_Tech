function deleteReview(id) {

    $.ajax({
        url: `/Review/Delete`,
        type: `POST`,
        data: { reviewId: id },
        success: function (result) {
            $(`[data-item-id='${result.reviewId}']`).remove();
            showNotification("Review has been deleted", "success");
        }
    })
}