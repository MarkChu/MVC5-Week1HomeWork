var ft_option = {
    "sorting": { "enabled": true },
    "filtering": { "enabled": false },
    "paging": { "enabled": false, "size": 10 },
    "state": { "enabled": false },
    "empty": "",
    "on": {
        "preinit.ft.table": function (e, ft) {

        },
        "init.ft.table": function (e, ft) {
            // bind to the plugin initialize event to do something
        },
        "predraw.ft.table": function (e, ft) {
            //this.fooTable.loader(true);
        },
        "draw.ft.table": function (e, ft) {
            //this.fooTable.loader(false);
        },
        "postdraw.ft.table": function (e, ft) {

        },
    },
}