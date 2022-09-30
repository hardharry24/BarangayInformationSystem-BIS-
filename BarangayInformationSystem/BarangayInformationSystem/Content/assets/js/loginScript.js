$("input[type=checkbox]").change(function () {
    if ($(this).prop("checked")) {
        $(this).val(true);
    } else {
        $(this).val(false);
    }
});

window.fbAsyncInit = function () {
    FB.init({
        appId: '802512921165202',
        cookie: true,
        xfbml: true,
        version: 'v2.8'
    });

    FB.AppEvents.logPageView();

};

(function (d, s, id) {
    var js, fjs = d.getElementsByTagName(s)[0];
    if (d.getElementById(id)) { return; }
    js = d.createElement(s); js.id = id;
    js.src = "https://connect.facebook.net/en_US/sdk.js";
    fjs.parentNode.insertBefore(js, fjs);
}(document, 'script', 'facebook-jssdk'));


function checkFBLoginState() {
    FB.login(function (response) {
        //console.log(response);
        if (response.authResponse) {
            console.log('Welcome!  Fetching your information.... ');
            FB.api('/me', function (response) {
                console.log('Good to see you, ' + response.name + '.');
                var fb = 0;
 
                var param = "?type=" + fb + "&userId=" + response.id + "&name=" + response.name;

                var xhttp = new XMLHttpRequest();
                xhttp.onreadystatechange = function () {
                    if (this.readyState == 4 && this.status == 200) {
                        var resp = JSON.parse(this.responseText);
                        console.log(resp.code);
                        if (resp.code == "1")
                        {
                            window.location = resp.message;
                        }
                    }
                };
                xhttp.open("GET", "/Base/LoginOption" + param, true);
                xhttp.send();

            });
        } else {
            console.log('User cancelled login or did not fully authorize.');
            Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: 'User cancelled login or did not fully authorize.!',
                footer: ''
            })
           
        }
    });

}