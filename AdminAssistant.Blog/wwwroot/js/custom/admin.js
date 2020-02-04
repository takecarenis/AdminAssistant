Admin = {
    uploadImageStatus: ''
}

Admin.uploadImage = function () {

    if (Admin.uploadImageStatus == '') {
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

                Admin.uploadImageStatus = result;
            },
            error: function (err) {
                alert(err.statusText);
            }
        });
    }
}

Admin.deletePost = function (e) {

    var id = parseInt(e.srcElement.id);

    var post = {
        Id: id
    }

    $.ajax({
        url: '/Admin/DeletePost',
        type: "POST",
        data: post,
        success: function (result) {
            alert(result);
        },
        error: function (err) {
            alert(err.statusText);
        }
    });
}

Admin.createNewPost = function () {

    var title = $("#titleInput").val();
    var body = $("#bodyInputValue").val();

    var checkboxes = document.querySelectorAll('input[name="categories"]:checked'), categories = [];
    Array.prototype.forEach.call(checkboxes, function (el) {
        categories.push({
            Id: el.value,
            Name: el.title
        });
    });

    var checkboxesTags = document.querySelectorAll('input[name="tags"]:checked'), tags = [];
    Array.prototype.forEach.call(checkboxesTags, function (el) {
        tags.push({
            Id: el.value,
            Name: el.title
        });
    });

    var post = {
        Title: title,
        Body: body,
        Categories: categories,
        Tags: tags
    };

    $.ajax({
        url: '/Admin/CreatePost',
        type: "POST",
        data: post,
        success: function (result) {
            alert(result);
        },
        error: function (err) {
            alert(err.statusText);
        }
    });
}