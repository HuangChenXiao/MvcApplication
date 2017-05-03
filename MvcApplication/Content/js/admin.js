
var vm = new Vue({
    el: "#myAdmin",
    data: {
        AdminList: '',
        total: ''
    },
    methods: {
        getAdminList: function () {
            var _this = this;
            $.post('/Account/GetAllUserInfo', function (data) {
                _this.AdminList = data.data;
                _this.total = data.total;
            })
        },
        admin_stop: function (e, id) {
            /*管理员-停用*/
            var obj = e.currentTarget;
            layer.confirm('确认要停用吗？', function (index) {
                //此处请求后台程序，下方是成功后的前台处理……
                $.post('/Account/EditStatus', { "Status": 0, "UserId": id }, function (data) {
                    if (data.data == "success") {
                        layer.msg('已停用!', { icon: 5, time: 1000 }, function () {
                            location.replace(location.href);
                        });
                    }
                })
            });
        },

        admin_start: function (e, id) {
            /*管理员-启用*/
            var obj = e.currentTarget;
            layer.confirm('确认要启用吗？', function (index) {
                //此处请求后台程序，下方是成功后的前台处理……
                $.post('/Account/EditStatus', { "Status": 1, "UserId": id }, function (data) {
                    layer.msg('已启用!', { icon: 6, time: 1000 }, function () {
                        location.replace(location.href);
                    });
                });
            });
        },
        /*管理员-删除*/
        admin_del: function (e, id) {
            var obj = e.currentTarget;
            layer.confirm('确认要删除吗？', function (index) {
                //此处请求后台程序，下方是成功后的前台处理……
                $.post('/Account/DelAdmin', { "UserId": id }, function (data) {
                    if (data.data == 'success') {
                        $(obj).parents("tr").remove();
                        layer.msg('已删除!', { icon: 1, time: 1000 });
                    }
                });

            });
        },
        /*
	        参数解释：
	        title	标题
	        url		请求的url
	        id		需要操作的数据id
	        w		弹出层宽度（缺省调默认值）
	        h		弹出层高度（缺省调默认值）
        */
        /*管理员-增加*/
        admin_add: function (title, url, w, h) {
            layer_show(title, url, w, h);
        },
        /*管理员-编辑*/
        admin_edit: function (title, url, id, w, h) {
            layer_show(title, url+"?UserId="+id, w, h);
        }
    }
})
vm.getAdminList();






