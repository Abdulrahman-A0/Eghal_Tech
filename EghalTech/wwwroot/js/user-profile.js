function openDeleteModal() {
    const modal = document.getElementById('deleteAccountModal');
    modal.classList.add('show');
    document.body.style.overflow = 'hidden'; // Prevent background scrolling
}

function closeDeleteModal() {
    const modal = document.getElementById('deleteAccountModal');
    modal.classList.remove('show');
    document.body.style.overflow = 'auto'; // Restore scrolling
}

// Close modal when clicking outside of it
document.getElementById('deleteAccountModal').addEventListener('click', function (e) {
    if (e.target === this) {
        closeDeleteModal();
    }
});

// Close modal with Escape key
document.addEventListener('keydown', function (e) {
    if (e.key === 'Escape') {
        closeDeleteModal();
    }
});