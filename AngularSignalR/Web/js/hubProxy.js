﻿//SignalR HubProxy

var hubProxy = $.connection.storeHub;

// Create a function that the hub can call to broadcast messages.
hubProxy.client.broadcastMessage = function (name, message) {
    // Html encode display name and message. 
    var encodedName = $('<div />').text(name).html();
    var encodedMsg = $('<div />').text(message).html();
    // Add the message to the page. 
    $('#discussion').append('<li><strong>' + encodedName
        + '</strong>:&nbsp;&nbsp;' + encodedMsg + '</li>');
};