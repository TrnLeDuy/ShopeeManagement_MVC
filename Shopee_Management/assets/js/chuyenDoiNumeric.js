document.addEventListener("DOMContentLoaded", function () {
    var priceElements = document.querySelectorAll('.numeric_value');

    priceElements.forEach(function (element) {
        var price = parseFloat(element.innerText.replace('đ', '').replace(',', ''));
        var formattedPrice = formatPriceWithoutDecimal(price);
        element.innerText = formattedPrice + ' đ';
    });

    function formatPriceWithoutDecimal(price) {
        return price.toFixed(0).replace(/\d(?=(\d{3})+$)/g, '$&,');
    }
});