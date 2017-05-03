function projecttype(type) {
    switch (type) {
        case 1:
            return "活动特色";
        case 2:
            return "行程安排";
        case 3:
            return "住宿参考";
        case 4:
            return "费用说明";
        default:
            return "暂无";
    }
}