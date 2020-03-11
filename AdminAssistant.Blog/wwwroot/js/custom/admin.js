Admin = {
    uploadImageStatus: '',
    subscribersTable: '',
    listOfSubscribers: [],
    currentPageName: ''
}

Admin.initSubscribersTable = function () {
    $.ajax({
        url: "/Admin/GetSubscribers",
        dataType: 'json',
        success: function (data) {
            Admin.subscribersTable = $('#example').DataTable({
                select: true,
                data: data,
                columns: [
                    { "data": "checked" },
                    { "data": "id" },
                    { "data": "email" },
                    { "data": "subscribeDateString" },
                    { "data": "category" },
                    { "data": "isActive" }
                ],
                'columnDefs': [
                    {
                        'targets': 0,
                        'checkboxes': {
                            'selectRow': true
                        }
                    }
                ],
                'select': {
                    'style': 'multi'
                },
                'order': [[1, 'asc']]
            });
        },
        error: function (error) {
            alert(error);
        }
    });
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
                if (result == true || result == "True") {
                    $.notify("You successfully uploaded photo!", "success");
                    $("#submitUploadingPhoto").attr("disabled", "disabled");
                }
                else {
                    $.notify("Something went wrong! Please try later.");
                }
            },
            error: function (err) {
                $.notify("Something went wrong! Please try later.");
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
            if (result == true || result == "True") {
                $.notify("You successfully deleted post! Please refresh the page.", "success");
            }
            else {
                $.notify("Something went wrong! Please try later.");
            }
        },
        error: function (err) {
            $.notify("Something went wrong! Please try later.");
        }
    });
}

Admin.createNewPost = function () {

    var title = $("#titleInput").val();
    var body = $("#bodyInputValue").val();
    var intro = $("#introInput").val();

    if (title == "") {
        $("#titleValidation").css("display", "block");

        return;
    }
    else {
        $("#titleValidation").css("display", "none");
    }

    if (intro == "") {
        $("#introValidation").css("display", "block");

        return;
    }
    else {
        $("#introValidation").css("display", "none");
    }

    if (body == "") {
        $("#bodyValidation").css("display", "block");

        return;
    }
    else {
        $("#bodyValidation").css("display", "none");
    }

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
            if (result == true || result == "True") {
                $.notify("You successfully added new post! Please refresh the page.", "success");
            }
            else {
                $.notify("Something went wrong! Please try later.");
            }

            $("#addNewPost").modal('toggle');
        },
        error: function (err) {
            $.notify("Something went wrong! Please try later.");
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

                if (result == true || result == "True") {
                    $("#successfullySubscribedLabel").css('display', 'block');
                    $("#unsuccessfullySubscribedLabel").css('display', 'none');
                    $("#subscribedModal").modal();
                }
                else {
                    $("#successfullySubscribedLabel").css('display', 'none');
                    $("#unsuccessfullySubscribedLabel").css('display', 'block');
                    $("#subscribedModal").modal();
                }
            },
            error: function (err) {
                $("#successfullySubscribedLabel").css('display', 'none');
                $("#unsuccessfullySubscribedLabel").css('display', 'block');
                $("#subscribedModal").modal();
            }
        });
    }
}

Admin.openDeleteSubscribeModal = function () {

    for (var i = 0; i < Admin.listOfSubscribers.length; i++) {

        var email = Admin.listOfSubscribers[i];

        $("#listOfDeleteSubscribers").text($("#listOfDeleteSubscribers").text() + " - " + email);
    }
};

Admin.deleteSubcribers = function () {

    $.ajax({
        url: '/Admin/DeleteSubscribers',
        type: "POST",
        data: { users: Admin.listOfSubscribers },
        dataType: 'json',
        success: function (result) {
            alert(result);
        },
        error: function (err) {
            alert(err.statusText);
        }
    });
}

Admin.sendMail = function () {

    var mail = {
        Subject: $("#subjectInput").val(),
        Body: $("#bodyInputValue").val(),
        Users: Admin.listOfSubscribers
    };

    $.ajax({
        url: '/Admin/SendEmail',
        type: "POST",
        data: mail,
        dataType: 'json',
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

Admin.editPageContent = function (e) {

    var id = e.srcElement.id;

    var name = id.substring(5, id.length);

    Admin.currentPageName = name;

    $("#editContentTitle").text("Edit Page content");
    var currentContent = $("#currentContent_" + name).html();
    $(".note-editable").html(currentContent);

    $("#editPageContent").modal();
}

Admin.updatePageContent = function () {

    var body = $("#bodyInputValue").val();
    var id = Admin.currentPageName;

    var page = {
        Id: id,
        Text: body
    }

    $.ajax({
        url: '/Admin/UpdateContent',
        type: "POST",
        data: page,
        dataType: 'json',
        success: function (result) {

            $("#editPageContent").modal('toggle');
            if (result == true || result == "True") {
                $.notify("You successfully updated the page! Please refresh the page.", "success");
            }
            else {
                $.notify("Something went wrong! Please try later.");
            }
        },
        error: function (err) {
            $.notify("Something went wrong! Please try later.");
        }
    });
}

Admin.addNewTag = function() {

    var tagName = $("#newTagInput").val();

    var newTag = {
        Name: tagName
    }

    $.ajax({
        url: '/Admin/AddNewTag',
        type: "POST",
        data: newTag,
        dataType: 'json',
        success: function(result) {

            var input = document.createElement("input");
            input.type = "checkbox";
            input.name = "tags";
            input.value = result.id;
            input.title = result.name;

            var label = document.createElement("label");
            label.htmlFor = "tags";
            label.innerHTML = result.name;
            label.style.marginRight = "10px";

            var tagsDiv = document.getElementById("tagsDiv");

            tagsDiv.append(input);
            tagsDiv.append(label);
        },
        error: function(err) {
            alert(err.statusText);
        }
    });
}

Admin.backToPreviousPage = function() {

    window.location = localStorage.getItem("backPage");
}

$("#addNewPostButton").on('click', function () {

    $("#titleInput").val('');
    $("#bodyInputValue").val('');
    $("#introInput").val('');
    $("#file").val(null);
    $("#fileUpload").val(null);
    $("#newTagInput").val('');

    if ($(".note-editable")[0].innerHTML != "<p><br></p>") {
        $(".note-icon-code").click();
        $(".note-editable")[0].innerHTML = ''
    }

    var checkboxes = document.querySelectorAll('input[name="categories"]:checked');
    Array.prototype.forEach.call(checkboxes, function (el) {
        el.checked = false;
    });

    var checkboxesTags = document.querySelectorAll('input[name="tags"]:checked');
    Array.prototype.forEach.call(checkboxesTags, function (el) {
        el.checked = false;
    });
});