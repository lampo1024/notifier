const connection = new signalR.HubConnectionBuilder()
    .withUrl("http://localhost:5125/hub/notifier")
    .configureLogging(signalR.LogLevel.Information)
    .build();

async function start() {
    try {
        await connection.start();
        await sendSystemMessage("已成功连接到消息服务器");
        console.log("connection:", connection.connectionId);
    } catch (err) {
        console.error(err);
        setTimeout(start, 5000);
    }
}
connection.onclose(async () => {
    await start();
});
start();

connection.on("receiveMessage", (data) => {
    appendMessage(data);
});

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
    $(".column-user-list").on("click",".user-item",function(){
        console.log("switch user:",123);
    });
});


async function sendChatMessage(user, message) {
    await connection.invoke("SendMessage", { group: "chat", data: { "user": user, "message": message } });
}

async function sendSystemMessage(message, type) {
    type = type || "info";
    await connection.invoke("SendMessage", { group: "private", data: { "user": "系统", "message": message } });
}

function systemMessage(message) {
    var data = {
        created_as_astring: "现在",
        type: "info",
        "group": "chat",
        data: { user: "系统", message: message, created_as_astring_at: "现在" }
    };
    appendMessage(data);
}

function appendMessage(data) {
    console.log("data:", data);
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
}