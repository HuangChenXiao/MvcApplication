//转换时间格式/Date(1408943415760+0800)/
Vue.filter('time', function (value) {
    if (value) {
        var NewDtime = new Date(parseInt(value.slice(6, 19)));
        return formatDate(NewDtime);
    }
})
//截取图片
Vue.filter('goodspic', function (value) {
    if (value) {
        return value.substring(value.lastIndexOf('/') + 1);
    }
})

function formatDate(dt) {
    var year = dt.getFullYear();
    var month = dt.getMonth() + 1;
    var date = dt.getDate();
    var hour = dt.getHours();
    var minute = dt.getMinutes();
    var second = dt.getSeconds();

    month = month < 10 ? "0" + month : month;
    date = date < 10 ? "0" + date : date;
    hour = hour < 10 ? "0" + hour : hour;
    minute = minute < 10 ? "0" + minute : minute;
    second = second < 10 ? "0" + second : second;

    return year + "-" + month + "-" + date + " " + hour + ":" + minute + ":" + second;
}
