var cropper;
var out;

var outimage = $("#user_photo");
var id = $("#EmployeeId").val();
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
            },
            error() {
                alert('Upload error');
            }
        });

    }
};
function dataURItoBlob(dataURI) {
    // convert base64 to raw binary data held in a string
    var byteString = atob(dataURI.split(',')[1]);

    var mimeString = dataURI.split(',')[0].split(':')[1].split(';')[0];

    var ab = new ArrayBuffer(byteString.length);
    var ia = new Uint8Array(ab);
    for (var i = 0; i < byteString.length; i++) {
        ia[i] = byteString.charCodeAt(i);
    }
    return new Blob([ab], { type: mimeString });
}
