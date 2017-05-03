var Login_Fun = function () {
    $("#loginform").submit(function () {
        $(this).ajaxSubmit({
            url: "/Login/CheckUserLogin",
            type: "POST",
            dataType: "json",
            data: { "UserCode": $("#UserCode").val(), "UserPassword": $("#UserPassword").val() },
            success: function (data) {
                if (data.result == "success") {
                    window.location.href = "/Home/Index";
                }
                else {
                    layer.msg(data.content, { icon: 0 }, function () {
                        window.location.href = "/Login/Login";
                    });
                }
            },
            error: function (xhr, error, ex) {
                layer.msg("出现错误，请稍后再试，带来不便，敬请谅解", { icon: 0 });
                window.location.href = "/Account/Login";
            }
        });
        return false; //不刷新页面  
    });
}