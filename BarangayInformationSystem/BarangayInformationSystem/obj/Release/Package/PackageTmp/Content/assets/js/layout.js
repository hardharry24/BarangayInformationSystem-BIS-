function logout() {
    
    swal({
        title: "Are you sure?",
        text: "You want to logout?",
        icon: "warning",
        buttons: true,
        dangerMode: true,
    })
    .then((willDelete) => {
        if (willDelete) {
            swal("Poof! Your imaginary file has been deleted!", {
                icon: "success",
            });

            window.location = "/Home/Logout";
        } else {
            swal("Cancel");
        }
    });
}