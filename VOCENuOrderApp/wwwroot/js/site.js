(function () {
    'use strict';

    var STORAGE_KEY = 'sidebar-collapsed';
    var sidebar = document.getElementById('sidebar');
    var mainContent = document.getElementById('mainContent');
    var toggleBtn = document.getElementById('sidebarToggle');
    var toggleMobileBtn = document.getElementById('sidebarToggleMobile');
    var overlay = document.getElementById('sidebarOverlay');
    var collapseIcon = document.getElementById('collapseIcon');

    if (!sidebar) return;

    // Restore desktop collapsed state
    if (localStorage.getItem(STORAGE_KEY) === '1') {
        sidebar.classList.add('collapsed');
    }

    // Desktop toggle
    if (toggleBtn) {
        toggleBtn.addEventListener('click', function () {
            sidebar.classList.toggle('collapsed');
            localStorage.setItem(STORAGE_KEY, sidebar.classList.contains('collapsed') ? '1' : '0');
        });
    }

    // Mobile toggle open
    if (toggleMobileBtn) {
        toggleMobileBtn.addEventListener('click', function () {
            sidebar.classList.toggle('mobile-open');
            overlay.classList.toggle('show');
        });
    }

    // Mobile overlay close
    if (overlay) {
        overlay.addEventListener('click', function () {
            sidebar.classList.remove('mobile-open');
            overlay.classList.remove('show');
        });
    }

    // Close mobile sidebar on navigation (for same-page anchors)
    sidebar.addEventListener('click', function (e) {
        var link = e.target.closest('a[href]:not([data-bs-toggle])');
        if (link && window.innerWidth < 992) {
            sidebar.classList.remove('mobile-open');
            overlay.classList.remove('show');
        }
    });

    // On resize, clear mobile state if crossing breakpoint
    var mql = window.matchMedia('(min-width: 992px)');
    mql.addEventListener('change', function () {
        sidebar.classList.remove('mobile-open');
        if (overlay) overlay.classList.remove('show');
    });
})();
