(function (host) {
    //function importScript(url, callback) {
    //    var script = document.createElement("script");
    //    script.type = "text/javascript";
    //    script.src = url;
    //    if (callback && typeof callback === 'function') {
    //        script.onload = function () {
    //            callback();
    //        };
    //    }
    //    document.head.appendChild(script);
    //};
    //importScript(host + "/lib/microsoft-signalr/signalr.min.js", function () {
    //    importScript(host + "/js/site.js");
    //});

    var loadScript = function (uri) {
        return new Promise((resolve, reject) => {
            var tag = document.createElement('script');
            tag.src = uri;
            tag.async = true;
            tag.onload = () => {
                resolve();
            };
            document.head.appendChild(tag);
        });
    }
    var s1 = loadScript(host + "/lib/microsoft-signalr/signalr.min.js");
    //var s2 = loadScript(host + "/js/site.js");
    Promise.all([s1]).then(() => {
        loadScript(host + "/js/site.js")
    });

})("http://localhost:5126");