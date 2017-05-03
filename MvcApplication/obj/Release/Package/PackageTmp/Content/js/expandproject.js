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
        "bautoWidth": true,//禁止自动计算列宽
        "bSort": false,
        "ajax":
            {
                "url": "/ExpandProject/GetExpandProjectList",
                "type": "post",
                "dataType": "Json",
                "data": {
                    "IsWonderfulCase":"false",
                    "Code": $("#Code").val(),
                }
            },
        "columns": [
            {
                "data": null,
                "mRender": function (data, type, full) {
                    return '<input type="checkbox" value="' + data.ProjectId + '" name="">'
                },
                "sTitle": "<input type='checkbox' onclick='chkcheckbox(this)' name=''></input>",
                "width": "25px"
            },
            { "data": "ProjectTypeName", "sTitle": "项目类型" },
            { "data": "CityName", "sTitle": "城市" },
            { "data": "ProjectName", "sTitle": "项目名称" },
            { "data": "ProjectTime", "sTitle": "项目时间" },
            { "data": "ProjectPrice", "sTitle": "项目报价" },
            { "data": "ProjectTripDays", "sTitle": "行程天数" },
            { "data": "ProjectTitle", "sTitle": "活动主题" },
            { "data": "ProjectSpecial", "sTitle": "活动特色" },
            {
                "data": null,
                "mRender": function (data, type, full) {
                    return '<img src="' + data.ProjectMinPic + '" style="width:40px;height:40px"/>';
                },
                "sTitle": "缩略图"
            },
            {
                "data": null,
                "mRender": function (data, type, full) {
                    return splitSymbolPic(data.ProjectPic);
                },
                "sTitle": "项目图片"
            },
            {
                "data": null,
                "mRender": function (data, type, full) {
                    return data.Status == 1 ? "<span class='text-ssuccess'>上架</span>" : "<span class='text-swarning'>下架</span>";
                },
                "width":"50px",
                "sTitle": "项目状态"
            },
            {
                "data": null,
                "mRender": function (data, type, full) {
                    return data.IsHot == 1 ? "<span style='color:red;font-weight:800'>是</span>" : "否";
                },
                "sTitle": "是否热门"
            },
            { "data": "ProjectDescription", "sTitle": "活动简介", "width": "100px" },
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
                    return '<a title="编辑" href="javascript:;" onclick="layer_open(\'编辑\',\'ExpandProjectEdit?ProjectId=' + data.ProjectId + '\')" class="ml-5" style="text-decoration:none"><i class="Hui-iconfont">&#xe6df;</i></a>'
                         + '<a title="详细项目" href="/ExpandProjectDetail/ExpandProjectDetailIndex?ProjectId=' + data.ProjectId + '" target="_blank"  class="ml-5" style="text-decoration:none"><i class="Hui-iconfont">&#xe667;</i></a>'
                         + '<a title="删除" href="javascript:;" data-id="' + data.ProjectId + '" onclick="single_delete(this)" class="ml-5" style="text-decoration:none"><i class="Hui-iconfont">&#xe6e2;</i></a>'
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
            strImg += '<img src="' + List[i] + '" style="width:40px;height:40px;margin:5px"/>';
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
/*项目-批量删除*/
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

            $.post('/ExpandProject/DelExpandProject', { "strId": $.splitSymbol(chkId) }, function (data) {
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
        $.post('/ExpandProject/DelExpandProject', { "strId": Id }, function (data) {
            if (data.data == 'success') {
                $(_this).parents("tr").remove();
                layer.msg('已删除!', { icon: 1, time: 1000 });
            }
        });
    });
}
