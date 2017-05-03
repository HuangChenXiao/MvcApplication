var Request = $.GetRequest();
var index_fun = function () {
   
    initDataTable();

    $('.table-sort tbody').on('click', 'tr', function () {
        if ($(this).hasClass('selected')) {
            $(this).removeClass('selected');
        }
        else {
            $(this).removeClass('selected');
            $(this).addClass('selected');
        }
    });
    $(".btn-success").click(function () {
        initDataTable();
    })
}

function initDataTable() {
    $('.table-sort').DataTable({
        "processing": true,
        "serverSide": true,
        "deferRender": false,//延迟加载
        "bFilter": false, //是否启动过滤、搜索功能
        "aLengthMenu": [50, 100, 200], //更改显示记录数选项
        "iDisplayLength": 50, //默认显示的记录数
        "bInfo": true, //是否显示页脚信息，DataTables插件左下角显示记录数
        "bDestroy": true,//销毁datatables
        "bautoWidth": false,//禁止自动计算列宽
        "bSort": false,
        "ajax":
            {
                "url": "/Customized/GetCustomizedList",
                "type": "post",
                "dataType": "Json",
                "data": {
                    "Type":"0",
                    "Code": $("#Code").val(),
                }
            },
        "columns": [
            {
                "data": null,
                "mRender": function (data, type, full) {
                    return '<input type="checkbox" value="' + data.Id + '" name="">'
                },
                "sTitle": "<input type='checkbox' onclick='chkcheckbox(this)' name=''></input>",
                "width": "25px"
            },
            { "data": "Name", "sTitle": "名称" },
            { "data": "Phone", "sTitle": "手机" },
            { "data": "Sex", "sTitle": "性别" },
            { "data": "MailBox", "sTitle": "邮箱" },
            { "data": "Remarks", "sTitle": "备注" },
            {
                "data": null,
                "mRender": function (data, type, full) {
                    return $.formatTime(data.AddTime);
                },
                "sTitle": "留言时间"
            },
        ]
    });
}


//全选全不选
function chkcheckbox(_this) {
    var chk = $(_this).is(":checked");
    if (chk) {
        $("tbody input[type=checkbox]").prop("checked", true);
    }
    else {
        $("tbody input[type=checkbox]").prop("checked", false);
    }
}

function layer_open(title, url) {
    var index = layer.open({
        type: 2,
        title: title,
        content: url
    });
    layer.full(index);
}

/*商品-批量删除*/
function dataAllDel() {
    var chkId = '';
    var chkbox = $(".table-bordered td input[type=checkbox]");
    for (var i = 0; i < chkbox.length ; i++) {
        if ($(chkbox[i]).is(":checked")) {
            chkId += chkbox[i].value + ',';
        }
    }
    if (chkId) {
        layer.confirm('删除须谨慎，确认要批量删除吗？', function () {
            //此处请求后台程序，下方是成功后的前台处理……

            $.post('/ExpandProjectDetail/', { "strId": $.splitSymbol(chkId) }, function (data) {
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

function single_dingzhi(_this,status) {
    var Id = $(_this).attr("data-id");
    var content = '确认该用户定制？';
    if (status==0) {
        content = '取消该用户定制？';
    }
    layer.confirm(content, function (index) {
        //此处请求后台程序，下方是成功后的前台处理……
        $.post('/Customized/EditCustomized', { "strId": Id, "Status": status }, function (data) {
            if (data.data == 'success') {
                layer.msg('操作成功!', { icon: 1, time: 1000 }, function () {
                    $.reload();
                });
            }
        });
    });
}
