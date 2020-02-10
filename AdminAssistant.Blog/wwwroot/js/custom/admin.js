Admin = {
    uploadImageStatus: '',
    subscribersTable: '',
    listOfSubscribers: []
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
    var intro = $("#introInput").val();

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
        Intro: intro,
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

Admin.copyLink = function(id) {

    var $temp = $("<input>");
    $("body").append($temp);

    if (id == null || id == "") {
        $temp.val($("#copyLinkValue").val()).select();
    }
    else {
        $temp.val($("#copyLinkValue_"+id).val()).select();
    }

    document.execCommand("copy");
    $temp.remove();
}

Admin.subscribe = function () {

    var email = $("#emailInput").val();

    if (email == null || email == "") {

        $("#emailvalidation").css("display", "block");
    }
    else {
        $("#emailvalidation").css("display", "none");

        $.ajax({
            url: '/Blog/Subscribe?email=' + email,
            type: "POST",
            success: function (result) {
                $("#emailInput").val("");
            },
            error: function (err) {
                alert(err.statusText);
            }
        });
    }
}

Admin.openDeleteSubscribeModal = function () {

    var rows_selected = Admin.subscribersTable.column(0).checkboxes.selected();

    $.each(rows_selected, function (index, rowId) {

        var row = Admin.subscribersTable.row(index).data();

        var email = row.email;

        $("#listOfDeleteSubscribers").text($("#listOfDeleteSubscribers").text() + " - " + email);
    });
};

Admin.deleteSubcribers = function () {

    $.ajax({
        url: '/Admin/DeleteSubscribers',
        type: "POST",
        data: Admin.listOfSubscribers,
        success: function (result) {
            alert(result);
        },
        error: function (err) {
            alert(err.statusText);
        }
    });
}

Admin.clearLabels = function () {

    $("#listOfDeleteSubscribers").text("");
}
