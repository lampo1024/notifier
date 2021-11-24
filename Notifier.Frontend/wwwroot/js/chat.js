function importScript(url, callback) {
    var script = document.createElement("script");
    script.type = "text/javascript";
    script.src = url;
    if (callback && typeof callback === 'function') {
        script.onload = function () {
            callback();
        };
    }
    document.head.appendChild(script);
};
//importScript("http://localhost:5126/lib/microsoft-signalr/signalr.min.js", function () {
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
let scripts = [
    "http://localhost:5126/lib/microsoft-signalr/signalr.min.js"
];
let promises = [];
scripts.forEach(function (url) {
    promises.push(loadScript(url));
});
Promise.all(promises).then(() => {
    importScript("http://localhost:5126/js/notifier.js");
});