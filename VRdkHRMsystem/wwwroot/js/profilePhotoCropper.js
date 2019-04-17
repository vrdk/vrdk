$(document).ready(function () {
    $('#croppImage').on('click', function () {
        setImage();
    });
    $('#cancel').on('click', function () {
        $('#image').attr('src', '');
    });
});
var cropper;
var out;
var outimage = $("#user_photo");
function setupCropper(input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();
        if (cropper) {
            cropper.destroy();
        }
        reader.onload = function (e) {
            $('#image').attr('src', e.target.result);
            var image = document.getElementById('image');
            image.crossOrigin = "anonymous";
            cropper = new Cropper(image, {
                aspectRatio: 1 / 1,
                crop() {
                    let canvas = this.cropper.getCroppedCanvas({
                        width: 300,
                        height: 300,
                        maxWidth: 800,
                        maxHeight: 800,
                        fillColor: '#fff'
                    });
                    out = canvas.toDataURL();
                }
            });
        };
        reader.readAsDataURL(input.files[0]);
        $('#user_photo_modal').modal();
    }
    else {
        input.value = null;
    }
}

const setImage = () => {
    if (out) {
        var id = $('#EmployeeId').val();
        if (id) {
            var toast = toastr.info('Изменение фото...','', { timeOut: 0, extendedTimeOut: 0 });
            var formData = new FormData();
            formData.set("photo", dataURItoBlob(out));
            formData.set("id", id);
            $.ajax({
                url: "/File/UploadUserPhoto",
                method: "post",
                data: formData,
                processData: false,
                contentType: false,
                success() {
                    outimage.attr("src", out);
                    toastr.clear(toast);
                    toastr.success('Фото изменено');
                },
                error() {
                    toastr.clear(toast);
                    toastr.error('Не удалось изменить фото');
                }
            });
        }
        else {
            toastr.clear(toast);
            toastr.error('Не удалось определить пользователя');
        }
    }
};
function dataURItoBlob(dataURI) {
    var byteString = atob(dataURI.split(',')[1]);

    var mimeString = dataURI.split(',')[0].split(':')[1].split(';')[0];

    var ab = new ArrayBuffer(byteString.length);
    var ia = new Uint8Array(ab);
    for (var i = 0; i < byteString.length; i++) {
        ia[i] = byteString.charCodeAt(i);
    }
    return new Blob([ab], { type: mimeString });
}
