// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(document).ready(function (e) {
    $(".lbl-check").click(function () {
        console.log("before classes: " + this.classList);

        $(this).find(".btn-check").toggleClass("check");

        $(this).find(".cbx-check").prop('checked', function (i, value) {
            return !value;
        });

        console.log("after classes: " + this.classList);
    });
});