// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();
if (document.getElementById("sendButton") != null) {
    document.getElementById("sendButton").disabled = true;
}


connection.on("ReceivedMess", function (user, message) {
    var li = document.createElement("li");
    document.getElementById("messageList").appendChild(li);
    li.textContent = user+" says: "+message;
});

connection.start().then(function () {
    //if (document.getElementById("sendButton") != null) {
    //    document.getElementById("sendButton").disable = false;
    //}
}
).catch(function (err) {
    return console.error(err.toString());
});

if (document.getElementById("startBtn") != null) {
    document.getElementById("startBtn").addEventListener("click", function (event) {
        var user = document.getElementById("myUserName").value;
        var chatId = document.getElementById("chatId").value;
        connection.invoke("SetUserName", user, chatId).catch(function (err) {
            return console.error(err.toString());
        });
        setTimeout(function () {
            alert("You may send and receive message now")
            document.getElementById("sendButton").disabled = false;
        }, 5000);
        event.preventDefault();
    });
}

if (document.getElementById("sendButton") != null) {
    document.getElementById("sendButton").addEventListener("click", function (event) {
        var user = document.getElementById("myUserName").value;
        var chatId = document.getElementById("chatId").value;
        var message = document.getElementById("messageInput").value;
        connection.invoke("SendMess", user, chatId, message).catch(function (err) {
            return console.error(err.toString());
        });
        event.preventDefault();
    });
}


//document.getElementById("loginBtn").addEventListener("click", function (event) {
//    var user = document.getElementById("username").value;
//    connection.invoke("SetUserName", user).catch(function (err) {
//        return console.error(err.toString());
//    });
//    alert("set Name Done")
//    event.preventDefault();
//});