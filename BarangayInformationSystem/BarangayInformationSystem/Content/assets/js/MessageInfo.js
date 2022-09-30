function MsgBox(title,message,type)
{
    swal({
        title: title,
        text: message,
        icon: type,
    });
}