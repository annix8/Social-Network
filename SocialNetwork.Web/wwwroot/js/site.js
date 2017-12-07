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