"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();
document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (user, message) {   
    var msg = `@${user} says: ${message}`;
    var html = createOthersHtmlMessage(msg);
    var divNode = document.createElement('div');
    divNode.innerHTML = html;
    document.getElementById("chat-content").appendChild(divNode);
});

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var user = document.getElementById("user_nickname").value;
    var message = document.getElementById("messageInput").value;

    connection.invoke("SendMessage", user, message).catch(function (err) {
        return console.error(err.toString());
    });

    var html = createMeHtmlMessage(message);
    var divNode = document.createElement('div');
    divNode.innerHTML = html;
    document.getElementById("chat-content").appendChild(divNode);

    event.preventDefault();
});

function createMeHtmlMessage(message)
{
    return `<div class="media media-chat">
                <img class="avatar" src="https://img.icons8.com/color/36/000000/administrator-male.png" alt="...">
                <div class="media-body">
                    <p>You: ${message}</p>
                    <p class="meta">${new Date().toLocaleString()}</p>
                </div>
           </div>`;
}

function createOthersHtmlMessage(message) {
    return `<div class="media media-chat media-chat-reverse">
                <div class="media-body">
                    <p>${message}</p>
                    <p class="meta">${new Date().toLocaleString()}</p>
                </div>
            </div>`;
}