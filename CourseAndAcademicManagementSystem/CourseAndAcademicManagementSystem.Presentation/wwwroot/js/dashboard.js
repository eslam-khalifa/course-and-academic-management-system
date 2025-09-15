// dashboard.js
document.addEventListener('DOMContentLoaded', function () {
    const sidebarToggle = document.getElementById('sidebarToggle');
    const sidebar = document.querySelector('.sidebar');
    const body = document.body;

    // لو الزر مش موجود، نوقف الكود
    if (!sidebarToggle || !sidebar) return;

    // Toggle sidebar
    sidebarToggle.addEventListener('click', function (e) {
        e.stopPropagation(); // عشان ميقفلش على طول لما تضغط على الزر
        sidebar.classList.toggle('show');
        body.classList.toggle('sidebar-open');
    });

    // Close sidebar لما تضغط براها
    document.addEventListener('click', function (e) {
        if (body.classList.contains('sidebar-open') &&
            !sidebar.contains(e.target) &&
            e.target !== sidebarToggle) {
            sidebar.classList.remove('show');
            body.classList.remove('sidebar-open');
        }
    });

    // Close sidebar لما الشاشة ترجع للـ desktop
    window.addEventListener('resize', function () {
        if (window.innerWidth > 768) {
            sidebar.classList.remove('show');
            body.classList.remove('sidebar-open');
        }
    });
});

