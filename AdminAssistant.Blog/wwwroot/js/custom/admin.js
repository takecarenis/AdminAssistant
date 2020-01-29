Admin = {

}

Admin.uploadImage = function () {

    var fileUpload = $("#fileUpload").get(0);
    var files = fileUpload.files;

    var fileData = new FormData();

    // Looping over all files and add it to FormData object  
    for (var i = 0; i < files.length; i++) {
        fileData.append(files[i].name, files[i]);
    }

    $.ajax({
        url: '/Admin/FileUpload',
        type: "POST",
        contentType: false, // Not to set any content header  
        processData: false, // Not to process data  
        data: fileData,
        success: function (result) {
            alert(result);
        },
        error: function (err) {
            alert(err.statusText);
        }
    });

    return false;
}

Admin.createNewPost = function () {

    var formArray = $('#createPostForm').serializeArray();

    var title = formArray[0].value;
    var body = formArray[1].value;

    var post = {
        Title: title,
        Body: body
    }

    $.ajax({
        url: '/Admin/CreatePost',
        type: "POST",
        contentType: false, // Not to set any content header  
        processData: false, // Not to process data  
        data: post,
        success: function (result) {
            alert(result);
        },
        error: function (err) {
            alert(err.statusText);
        }
    });

}