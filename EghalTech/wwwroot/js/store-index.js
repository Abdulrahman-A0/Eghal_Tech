// Smooth scroll for pagination
const paginationLinks = document.querySelectorAll(".page-link")
paginationLinks.forEach((link) => {
    link.addEventListener("click", () => {
        window.scrollTo({ top: 0, behavior: "smooth" })
    })
})