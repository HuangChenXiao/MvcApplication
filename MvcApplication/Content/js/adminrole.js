var vm = new Vue({
    el: "#AdminRole",
    data: {
        RoleList: '',
        total: ''
    },
    methods: {
        getList: function () {
            var _this = this;
            $.post('/Role/AuthorityList', function (data) {
                _this.RoleList = data.data;
                _this.total = data.total;
            })
        },
        /*管理员-角色-添加*/
        admin_role_add: function (title, url, w, h) {
            layer_show(title, url, w, h);
        },
        /*管理员-角色-编辑*/
        admin_role_edit: function (title, url, id, w, h) {
            layer_show(title, url + "?AuthorityId="+id, w, h);
        },
        /*管理员-角色-删除*/
        admin_role_del: function (e, id) {
            var obj = e.currentTarget;
            layer.confirm('角色删除须谨慎，确认要删除吗？', function (index) {
                //此处请求后台程序，下方是成功后的前台处理……
                $.post('/Role/DelRole', { "AuthorityId": id }, function (data) {
                    if (data.data == 'success') {
                        $(obj).parents("tr").remove();
                        layer.msg('已删除!', { icon: 1, time: 1000 });
                    }
                });
            });
        },
        /*管理员-角色-批量删除*/
        admin_role_alldel: function () {
            var strAuthorityId = '';
            for (var i = 0; i < $(".table-bordered input[type=checkbox]").length ; i++) {
                if ($($(".table-bordered input[type=checkbox]")[i]).is(":checked")) {
                    strAuthorityId += $(".table-bordered input[type=checkbox]")[i].value + ',';
                }
            }
            if (strAuthorityId) {
                layer.confirm('角色删除须谨慎，确认要批量删除吗？', function () {
                    //此处请求后台程序，下方是成功后的前台处理……

                    $.post('/Role/DelRole', { "AuthorityId": $.splitSymbol(strAuthorityId) }, function (data) {
                        if (data.data == 'success') {
                            layer.msg('已删除!', { icon: 1, time: 1000 }, function () {
                                $.reload();
                            });
                        }
                    });
                });
            }
            else {
                layer.msg('请选择要删除的行!', { icon: 0, time: 1000 });
                return;
            }
        }
    }
})
vm.getList();
