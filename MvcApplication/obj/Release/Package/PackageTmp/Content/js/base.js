$.extend({
    reload: function () {
        location.replace(location.href)
    },
    parentReload: function () {
        parent.location.replace(parent.location.href);
    },
    GetRequest: function () {
        var url = location.search;
        var theRequest = new Object();
        if (url.indexOf("?") != -1) {
            var str = url.substr(1);

            strs = str.split("&");

            for (var i = 0; i < strs.length; i++) {
                theRequest[strs[i].split("=")[0]] = (strs[i].split("=")[1]);
            }
        }
        return theRequest;
    },
    splitSymbol: function (strAuthorityId) {
        if (strAuthorityId) {
            return strAuthorityId.substring(0, strAuthorityId.length - 1)
        }
    },
    getCurrentMonthFirst: function () {
        var date_ = new Date();
        var year = date_.getFullYear();
        var month = date_.getMonth() + 1;
        var firstdate = year + '-' + month + '-01'
        return firstdate;
    },
    getCurrentMonthLast: function () {
        var date_ = new Date();
        var year = date_.getFullYear();
        var month = date_.getMonth() + 1;
        var day = new Date(year, month, 0);
        var lastdate = year + '-' + month + '-' + day.getDate();
        return lastdate;
    }, formatTime: function (val) {
        if (val) {
            var re = /-?\d+/;
            var m = re.exec(val);
            var d = new Date(parseInt(m[0]));
            // 按【2012-02-13 09:09:09】的格式返回日期
            return d.format("yyyy-MM-dd hh:mm:ss");
        }
        return '';
    },
    formatDate: function (val) {
        if (val) {
            var re = /-?\d+/;
            var m = re.exec(val);
            var d = new Date(parseInt(m[0]));
            // 按【2012-02-13 09:09:09】的格式返回日期
            return d.format("yyyy-MM-dd");
        }
        return '';
    }
})

/**
*JSON返回DateTime/Date('123123123')/解决办法
**/
Date.prototype.format = function (format) //author: meizz
{
    var o = {
        "M+": this.getMonth() + 1, //month
        "d+": this.getDate(),    //day
        "h+": this.getHours(),   //hour
        "m+": this.getMinutes(), //minute
        "s+": this.getSeconds(), //second
        "q+": Math.floor((this.getMonth() + 3) / 3),  //quarter
        "S": this.getMilliseconds() //millisecond
    }
    if (/(y+)/.test(format)) format = format.replace(RegExp.$1,
    (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var k in o) if (new RegExp("(" + k + ")").test(format))
        format = format.replace(RegExp.$1,
      RegExp.$1.length == 1 ? o[k] :
        ("00" + o[k]).substr(("" + o[k]).length));
    return format;
}
