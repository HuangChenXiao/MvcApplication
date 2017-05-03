function IsAvailable(Status) {
    if (Status) {
        return "<b style='color:green'>启用</b>";
    }
    else {
        return "<b style='color:#808080'>禁用</b>";
    }
}