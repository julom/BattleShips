// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// function to select ship
$(document).ready(function () {
    $(".lbl-check").click(function () {

        $(this).find(".btn-check").toggleClass("check");

        $(this).find(".cbx-check").prop('checked', function (i, value) {
            return !value;
        });

    });
});

// function to reset ships positions
$(document).ready(function () {
    $("#btn-reset").click(function () {

        var userTable = $("#table-user-input");

        var buttons = userTable.find(".btn-check");
        buttons.removeClass("check");

        var checkboxes = userTable.find(".cbx-check");
        checkboxes.prop("checked", false);

    });
});

// function to mark fields after turn
$(document).ready(function () {
    $(".table-board .lbl-check").each(function () {

        console.log("cbx value is " + $(this).find(".cbx-check").val());
        var isFieldToMark = $(this).find(".cbx-check").val() === "value";

        if (isFieldToMark) {
            console.log("field is to mark");
            $(this).find(".btn-check").toggleClass("check");

            $(this).prop('checked', function (i, value) {
                return !value;
            });
        };
    });
});