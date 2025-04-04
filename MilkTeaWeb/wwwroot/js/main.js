// Lưu cookie
window.setCookie = (name, value, minutes) => {
    var expires = "";
    if (minutes) {
        var date = new Date();
        date.setTime(date.getTime() + minutes * 60 * 1000);
        expires = "; expires=" + date.toUTCString();
    }
    document.cookie = name + "=" + encodeURIComponent(value) + expires + "; path=/";
};

// Lấy cookie
window.getCookie = (name) => {
    var nameEQ = name + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') c = c.substring(1, c.length);
        if (c.indexOf(nameEQ) == 0) return decodeURIComponent(c.substring(nameEQ.length, c.length));
    }
    return null;
};

// Xóa cookie
window.deleteCookie = (name) => {
    document.cookie = name + "=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;";
};

// Kiểm tra và xóa cookie nếu hết hạn
window.checkCookieExpiration = () => {
    var userInfoJson = window.getCookie("UserInfo");
    if (userInfoJson) {
        var userInfo = JSON.parse(userInfoJson);
        var expireTime = new Date(userInfo.ExpireTime);

        // Kiểm tra xem cookie đã hết hạn chưa
        if (expireTime < new Date()) {
            console.log("Cookie expired, deleting...");
            window.deleteCookie("UserInfo"); // Xóa cookie
            window.location.href = "/dangnhap"; // Chuyển hướng đến trang đăng nhập
        }
    }
};

// Mở Giỏ Hàng
function toggleCart() {
    var cartModal = document.getElementById('cartModal');
    cartModal.classList.toggle('open');
}

// Gọi hàm kiểm tra cookie khi trang được tải
document.addEventListener("DOMContentLoaded", function () {
    window.checkCookieExpiration();
});
