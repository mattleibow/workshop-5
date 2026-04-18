// Toggle menu visibility when hamburger is clicked
const menuToggle = document.getElementById('menu-toggle');
const menu = document.getElementById('menu');

if (menuToggle && menu) {
    menuToggle.addEventListener('click', function (e) {
        e.stopPropagation();
        menu.classList.toggle('hidden');
        const isExpanded = !menu.classList.contains('hidden');
        menuToggle.setAttribute('aria-expanded', String(isExpanded));
    });

    // Close menu when clicking outside
    document.addEventListener('click', function (event) {
        const isClickInside = menuToggle.contains(event.target) || menu.contains(event.target);
        if (!isClickInside && !menu.classList.contains('hidden')) {
            menu.classList.add('hidden');
            menuToggle.setAttribute('aria-expanded', 'false');
        }
    });

    // Close menu on navigation (Blazor enhanced navigation)
    document.addEventListener('click', function (event) {
        const link = event.target.closest('#menu a');
        if (link) {
            menu.classList.add('hidden');
            menuToggle.setAttribute('aria-expanded', 'false');
        }
    });
}
