<script>
    window.onscroll = function() {
        const backToTopButton = document.querySelector('.back-to-top');
        if (document.body.scrollTop > 100 || document.documentElement.scrollTop > 100) {
        backToTopButton.style.display = "block";
        } else {
        backToTopButton.style.display = "none";
        }
    };

    document.querySelector('.back-to-top').addEventListener('click', function() {
        window.scrollTo({ top: 0, behavior: 'smooth' });
    });

    $(document).ready(function(){
        $(".product-carousel").owlCarousel({
            items: 1, // Số lượng sản phẩm hiển thị
            loop: true, // Cho phép lặp lại carousel
            margin: 10,
            nav: true, // Hiển thị mũi tên điều hướng
            autoplay: true, // Tự động phát
            autoplayTimeout: 3000, // Thời gian giữa mỗi lần chuyển slide
            autoplayHoverPause: true, // Dừng khi hover chuột
        });
    });

    // Back to Top Button Show/Hide
    $(window).scroll(function () {
            if ($(this).scrollTop() > 200) {
        $('.back-to-top').fadeIn();
            } else {
        $('.back-to-top').fadeOut();
            }
        });

    // Smooth Scroll for Back to Top Button
    $('.back-to-top').click(function () {
        $('html, body').animate({ scrollTop: 0 }, 'slow');
    return false;
        });
</script>
