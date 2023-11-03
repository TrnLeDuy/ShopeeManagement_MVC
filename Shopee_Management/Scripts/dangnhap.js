const loginsec = document.querySelector('.login-section')
const loginlink = document.querySelector('.login-link')
const registerlink = document.querySelector('.register-link')
registerlink.addEventListener('click', () => {
    loginsec.classList.add('active')
})
loginlink.addEventListener('click', () => {
    loginsec.classList.remove('active')
})

function togglePassword() {
    var passwordInput = document.getElementById('passwordInput');
    var passwordIcon = document.getElementById('passwordIcon');
    if (passwordInput.type === 'password') {
        passwordInput.type = 'text';
        passwordIcon.classList.remove('bx', 'bxs-lock-alt');
        passwordIcon.classList.add('bx', 'bxs-lock-open-alt');
    } else {
        passwordInput.type = 'password';
        passwordIcon.classList.remove('bx', 'bxs-lock-open-alt');
        passwordIcon.classList.add('bx', 'bxs-lock-alt');
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