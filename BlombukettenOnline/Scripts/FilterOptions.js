$(document).ready(function () {
    $('.filter-options').on('click', 'a', function (event) {
        event.preventDefault();
        $(this).toggleClass('active');
    });
});