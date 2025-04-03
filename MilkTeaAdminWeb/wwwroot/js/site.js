// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

const connection = new signalR.HubConnectionBuilder()
    .withUrl("/signalhub")
    .build();

connection.on("LoadPage", function (pageName) {
    location.href = `/${pageName}/Index`;
});

connection.start().catch(err => console.error(err));