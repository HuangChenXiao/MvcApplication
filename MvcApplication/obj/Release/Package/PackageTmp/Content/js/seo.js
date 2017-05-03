var vm = new Vue({
    el: "#myApp",
    data: {
        List: '',
        total: ''
    },
    methods: {
        getList: function () {
            var _this = this;
            $.post('/Seo/GetSeoIndex', function (data) {
                _this.List = data.data;
                _this.total = data.total;
            })
        },
        /*管理员-角色-添加*/
        seo_add: function (title, url, w, h) {
            layer_show(title, url, w, h);
        },
        /*管理员-角色-编辑*/
        seo_edit: function (title, url, id, w, h) {
            layer_show(title, url + "?Id="+id, w, h);
        },
        /*管理员-角色-删除*/
        seo_del: function (e, id) {
            var obj = e.currentTarget;
            layer.confirm('角色删除须谨慎，确认要删除吗？', function (index) {
                //此处请求后台程序，下方是成功后的前台处理……
                $.post('/Seo/DelSeo', { "strId": id }, function (data) {
                    if (data.data == 'success') {
                        $(obj).parents("tr").remove();
                        layer.msg('已删除!', { icon: 1, time: 1000 });
                    }
                });
            });
        },
        /*管理员-角色-批量删除*/
        seo_alldel: function () {
            var strId = '';
            for (var i = 0; i < $(".table-bordered input[type=checkbox]").length ; i++) {
                if ($($(".table-bordered input[type=checkbox]")[i]).is(":checked")) {
                    strId += $(".table-bordered input[type=checkbox]")[i].value + ',';
                }
            }
            if (strId) {
                layer.confirm('角色删除须谨慎，确认要批量删除吗？', function () {
                    //此处请求后台程序，下方是成功后的前台处理……

                    $.post('/Seo/DelSeo', { "strId": $.splitSymbol(strId) }, function (data) {
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
