function deletePost(postId) {
    swal({
        title: 'Warning',
        text: "Are you sure you want to delete this post?",
        type: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.value) {
            $.ajax({
                url: "/User/Posts/Delete/" + postId,
                type: "POST",
                data: {
                    PostId: postId
                }
            })
                .done(function (data) {
                    swal({
                        title: "Deleted",
                        text: "Post has been successfully deleted",
                        type: "success"
                    }).then(() => { location.reload(); });
                })
                .error(function (data) {
                    swal("Oops", "Something went wrong!", "error");
                });
        }
    })
}

function unfriendUser(userToUnfriend) {
    swal({
        title: 'Warning',
        text: "Are you sure you want to unfriend this person?",
        type: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes!'
    }).then((result) => {
        if (result.value) {
            $.ajax({
                url: "/User/Profile/CancelFriendRequest?usernameToUnfriend=" + userToUnfriend + "&returnUrl=" + window.location,
                type: "GET"
            })
                .done(function () {
                    swal({
                        title: "Success",
                        text: "User removed from friends.",
                        type: "success"
                    }).then(() => { location.reload(); });
                })
                .error(function (data) {
                    swal("Oops", "Something went wrong!", "error");
                });
        }
    })
}

function deleteProfile(usernameToDelete) {
    swal({
        title: 'Warning',
        text: "Are you sure you want to delete user with username '" + usernameToDelete + "'",
        type: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes!'
    }).then((result) => {
        if (result.value) {
            $.ajax({
                url: "/Admin/Profile/DeleteProfile?usernameToDelete=" + usernameToDelete,
                type: "GET"
            })
                .done(function () {
                    swal({
                        title: "Success",
                        text: "Account with username '" + usernameToDelete + "' successfuly deleted.",
                        type: "success"
                    }).then(() => { window.location.href = "/" });
                })
                .error(function (data) {
                    swal("Oops", "Something went wrong!", "error");
                });
        }
    })
}

function createNewAlbum() {
    swal.setDefaults({
        input: 'text',
        confirmButtonText: 'Next &rarr;',
        showCancelButton: true,
        progressSteps: ['1', '2']
    })

    var steps = [
        {
            title: 'Name',
            text: 'Give a name to your new album'
        },
        {
            title: 'Description',
            text: 'Give a brief description of your new album'
        }
    ]

    swal.queue(steps).then((result) => {
        swal.resetDefaults()

        if (result.value) {
            var albumTokens = result.value;

            swal({
                title: 'Warning',
                text: "Are you sure you want to create the album",
                type: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Yes!'
            }).then((result) => {
                if (result.value) {
                    $.ajax({
                        url: "/User/Photos/CreateAlbum",
                        type: "POST",
                        data: {
                            title: albumTokens[0],
                            description: albumTokens[1]
                        }
                    })
                        .done(function () {
                            swal({
                                title: "Success",
                                text: "Album created",
                                type: "success"
                            }).then(() => { location.reload(); });
                        })
                        .error(function (data) {
                            swal("Oops", "Something went wrong!", "error");
                        });
                }
            })
        }
    })
}

function deleteAlbum(albumId) {
    swal({
        title: 'Warning',
        text: "Are you sure you want to delete this album?",
        type: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.value) {
            $.ajax({
                url: "/User/Photos/DeleteAlbum/" + albumId,
                type: "POST"
            })
                .done(function (data) {
                    swal({
                        title: "Deleted",
                        text: "Album has been successfully deleted",
                        type: "success"
                    }).then(() => { window.location.href = "/"; });
                })
                .error(function (data) {
                    swal("Oops", "Something went wrong!", "error");
                });
        }
    })
}

function deleteAlbumPicture(albumId, pictureId) {
    swal({
        title: 'Warning',
        text: "Are you sure you want to delete this picture?",
        type: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.value) {
            $.ajax({
                url: "/User/Photos/DeleteAlbumPicture/?albumId=" + albumId + "&pictureId=" + pictureId,
                type: "POST"
            })
                .done(function (data) {
                    swal({
                        title: "Deleted",
                        text: "Picture has been successfully deleted",
                        type: "success"
                    }).then(() => { location.reload(); });
                })
                .error(function (er) {
                    swal("Oops", "Something went wrong.", "error");
                });
        }
    })
}