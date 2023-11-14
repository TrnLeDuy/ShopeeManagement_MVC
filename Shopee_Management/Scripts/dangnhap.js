const container = document.getElementById('container');
const registerBtn = document.getElementById('register');
const loginBtn = document.getElementById('login');

registerBtn.addEventListener('click', () => {
    container.classList.add("active");
});

loginBtn.addEventListener('click', () => {
    container.classList.remove("active");
});
function togglePassword(inputId, iconId) {
    var x = document.getElementById(inputId);
    var y = document.getElementById(iconId);
    if (x.type === "password") {
        x.type = "text";
        y.classList.remove("bxs-lock-alt");
        y.classList.add("bxs-unlock");
    } else {
        x.type = "password";
        y.classList.remove("bxs-unlock");
        y.classList.add("bxs-lock-alt");
    }
}


registerlink.addEventListener('click', () => {
    $.ajax({
        url: '/KhachHang/DangKy',
        type: 'GET',
        success: function (data) {
            formContainer.innerHTML = data;
            handleRegistrationFormSubmission();
        }
    });
});

loginlink.addEventListener('click', () => {
    $.ajax({
        url: '/KhachHang/DangNhap',
        type: 'GET',
        success: function (data) {
            formContainer.innerHTML = data;
            handleLoginFormSubmission();
        }
    });
});

function handleLoginFormSubmission() {
    $('form').submit(function (e) {
        e.preventDefault();
        // Handle the login form submission
        $.ajax({
            type: 'POST',
            url: '/KhachHang/DangNhap',
            data: $(this).serialize(),
            success: function (response) {
                // Handle the response after form submission
            },
            error: function (error) {
                // Handle any errors during form submission
            }
        });
    });
}

function handleRegistrationFormSubmission() {
    $('form').submit(function (e) {
        e.preventDefault();
        // Handle the registration form submission
        $.ajax({
            type: 'POST',
            url: '/KhachHang/DangKy',
            data: $(this).serialize(),
            success: function (response) {
                // Handle the response after form submission
            },
            error: function (error) {
                // Handle any errors during form submission
            }
        });
    });
}