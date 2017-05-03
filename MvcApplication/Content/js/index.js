
var vm = new Vue({
    el: "#myApp",
    data: {
        MenuData: '',
        User: JSON.parse($.cookie("UserInfoCookie"))
    },
    methods: {
        getMenu: function () {
            var _this = this;
            $.post('/Home/GetMenu', function (data) {
                _this.MenuData = data.data;
            })
        }
    }, updated: function () {
        $.Huifold(".menu_dropdown dl dt", ".menu_dropdown dl dd", "fast", 1, "click");
    }
})
vm.getMenu();
