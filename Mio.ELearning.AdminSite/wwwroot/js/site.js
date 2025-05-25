// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function () {
    // Handle active state for sidebar items
    const sidebarLinks = $('.list-group-item');
    function setActiveLink() {
        const currentPath = window.location.pathname;
        sidebarLinks.each(function () {
            $(this).removeClass('active');
            const href = $(this).attr('href') || $(this).find('a').attr('href');
            if (href && (currentPath === href || currentPath.startsWith(href + '/'))) {
                $(this).addClass('active');
            }
        });
    }

    // Set active link on page load
    setActiveLink();

    // Handle click events
    sidebarLinks.on('click', function () {
        sidebarLinks.removeClass('active');
        $(this).addClass('active');
        // Đóng sidebar trên mobile sau khi bấm
        if ($(window).width() <= 768) {
            $('#sidebar-wrapper').collapse('hide');
        }
    });
});