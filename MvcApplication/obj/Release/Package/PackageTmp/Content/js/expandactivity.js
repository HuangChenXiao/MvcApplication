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
    $.post("/ProjectType/GetProjectTypeIndex", function (data) {
        $("#ProjectTypeId").append('<option value="0">--请选择--</option>');
        for (var i = 0; i < data.data.length; i++) {
            $("#ProjectTypeId").append('<option value="' + data.data[i].ProjectTypeId + '">' + data.data[i].ProjectTypeName + '</option>');
        }
        ProjectType_Change($("#ProjectTypeId").val())
    })

}
function ProjectType_Change(ProjectTypeId) {
    $.post("GetExpandActivityByType", { "ProjectTypeId": ProjectTypeId }, function (data) {
        $("#ExpandTypeId").empty()
        $("#ExpandTypeId").append('<option value="0">--请选择--</option>');
        for (var i = 0; i < data.data.length; i++) {
            $("#ExpandTypeId").append('<option value="' + data.data[i].ExpandTypeId + '">' + data.data[i].ExpandTypeName + '</option>');
        }
    })
    initDataTable();
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
                "url": "/ExpandActivity/GetExpandActivityList",
                "type": "post",
                "dataType": "Json",
                "data": {
                    "Code": $("#Code").val(),
                    "ProjectTypeId": $("#ProjectTypeId").val(),
                    "ExpandTypeId": $("#ExpandTypeId").val(),
                }
            },
        "columns": [
            {
                "data": null,
                "mRender": function (data, type, full) {
                    return '<input type="checkbox" value="' + data.ActivityId + '" name="">'
                },
                "sTitle": "<input type='checkbox' onclick='chkcheckbox(this)' name=''></input>",
                "width": "25px"
            },
            { "data": "ProjectTypeName", "sTitle": "项目类型名称" },
            { "data": "ExpandTypeName", "sTitle": "活动类型名称" },
            { "data": "ActivityName", "sTitle": "活动名称" },
            { "data": "AddUser", "sTitle": "新增人" },
            {
                "data": null,
                "mRender": function (data, type, full) {
                    return $.formatTime(data.AddTime);
                },
                "sTitle": "增加时间"
            },
            {
                "data": null,
                "mRender": function (data, type, full) {
                    return '<a title="编辑" href="javascript:;" onclick="layer_open(\'编辑\',\'ExpandActivityEdit?ActivityId=' + data.ActivityId + '\')" class="ml-5" style="text-decoration:none"><i class="Hui-iconfont">&#xe6df;</i></a>'
                         + '<a title="删除" href="javascript:;" data-id="' + data.ActivityId + '" onclick="single_delete(this)" class="ml-5" style="text-decoration:none"><i class="Hui-iconfont">&#xe6e2;</i></a>'
                },
                "sTitle": "操作"
            }
        ]
    });
}

//图片
function splitSymbolPic(strpic) {
    if (strpic) {
        var List = $.splitSymbol(strpic).split(',');
        var strImg = '';
        for (var i = 0; i < List.length; i++) {
            strImg += '<img src="' + List[i] + '" style="width:40px;height:40px"/>';
        }
        return strImg;
    }
    return "";
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

            $.post('/ExpandActivity/DelExpandActivity', { "strId": $.splitSymbol(chkId) }, function (data) {
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

function single_delete(_this) {
    var Id = $(_this).attr("data-id");
    layer.confirm('删除须谨慎，确认要删除吗？', function (index) {
        //此处请求后台程序，下方是成功后的前台处理……
        $.post('/ExpandActivity/DelExpandActivity', { "strId": Id }, function (data) {
            if (data.data == 'success') {
                $(_this).parents("tr").remove();
                layer.msg('已删除!', { icon: 1, time: 1000 });
            }
        });
    });
}
