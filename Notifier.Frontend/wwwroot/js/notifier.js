var notifier = {
    connection: null,
    init: function (host) {
        this.connection = new signalR.HubConnectionBuilder()
            .withUrl(host)
            .configureLogging(signalR.LogLevel.Information)
            .build();
        this.connection.onclose(async () => {
            await this.start();
        });
        this.start();

        this.connection.on("receiveMessage", (data) => {
            this.renderMessage(data);
        });

        this.connection.on("refreshUserList", (data) => {
            this.renderUserList(data);
        });
        let target = this;
        if (window.NTF.events && window.NTF.events.length > 0) {
            window.NTF.events.forEach(function (item, index) {
                console.log("监听事件[" + item.eventName + "]...");
                if (item.callback && typeof (item.callback) === "function") {
                    target.connection.on(item.eventName, (data) => {
                        item.callback(data);
                    });
                }
            });
        }
    },
    start: async function () {
        try {
            await notifier.connection.start();
            await sendSystemMessage("已成功连接到消息服务器");
            window.NTF.connectionId = notifier.connection.connectionId;
        } catch (err) {
            setTimeout(this.start, 5000);
        }
    },
    renderMessage: function (data) {
        if (window.NTF && window.NTF.onReceived) {
            window.NTF.onReceived(data);
        }
        else {
            this.appendMessage(data);
        }
    },
    appendMessage: function (data) {
        var html = `<li class="message-item ${data.target}">
                        <div class="message-user-avatar">
                            <img class="user-small-avatar"
                            src="https://s3-us-west-2.amazonaws.com/s.cdpn.io/1940306/chat_avatar_02.jpg" alt="">
                        </div>
                        <div class="message-item-box">
                            <div class="message-info">
                                <h3 class="message-time">${data.created_as_astring}</h3>                                
                            </div>
                            <div class="message-body">
                                ${data.data.message}
                            </div>
                        </div>
                    </li>`;
        $(".message-list").append(html);
        $(".message-list").animate({ scrollTop: $(".message-list")[0].scrollHeight }, 500);
        $(".message-count").text($(".message-list li").length);
    },
    renderUserList: function (data) {
        if (!data) {
            return;
        }
        $(".chat-user-list").html("");
        for (var i = 0; i < data.length; i++) {
            var user = data[i];
            var me = (user.connection_id == notifier.connection.connectionId ? "me" : "");
            var html = `<li class="user-item ${me}">
                            <img class="user-avatar"
                                 src="https://s3-us-west-2.amazonaws.com/s.cdpn.io/1940306/chat_avatar_02.jpg" alt="">
                            <div class="user-info">
                                <h2>${user.name}</h2>
                                <h3>
                                    <span class="status green"></span>
                                    在线
                                </h3>
                            </div>
                    </li>`;
            $(".chat-user-list").append(html);
        }
    }
};
notifier.init("http://localhost:5125/hub/notifier?uid=" + window.NTF.uid);

$(function () {
    $("#btnSend").click(async function () {
        var newMessage = $("#newMessage").val();
        if (!newMessage) {
            return;
        }
        try {
            await sendChatMessage("Rector", newMessage);
            $('#newMessage').val('').focus();

        } catch (err) {
            console.error(err);
        }
    });
    $(document).on('keypress', function (e) {
        if (e.which == 13) {
            $("#btnSend").trigger("click");
        }
    });
    $(".column-user-list").on("click", ".user-item", function () {
        console.log("switch user:", 123);
    });
});

function uuidv4() {
    return ([1e7] + -1e3 + -4e3 + -8e3 + -1e11).replace(/[018]/g, c =>
        (c ^ crypto.getRandomValues(new Uint8Array(1))[0] & 15 >> c / 4).toString(16)
    );
}
async function sendChatMessage(user, message) {
    await notifier.connection.invoke("SendMessage", { group: "chat", data: { "user": user, "message": message } });
}

async function sendSystemMessage(message, type) {
    type = type || "info";
    await notifier.connection.invoke("SendMessage", { group: "private", data: { "user": "系统", "message": message } });
}

function systemMessage(message) {
    var data = {
        created_as_astring: "现在",
        type: "info",
        "group": "chat",
        data: { user: "系统", message: message, created_as_astring_at: "现在" }
    };
    notifier.appendMessage(data);
}