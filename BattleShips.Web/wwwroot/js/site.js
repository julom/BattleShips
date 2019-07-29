// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// function to select ships on input board
$(document).ready(function () {
    $("#table-user-input .lbl-check").click(function () {

        $(this).find(".btn-check").toggleClass("ship");

        $(this).find(".cbx-check").attr("checked", function (i, value) {
            return !value;
        });

    });

    $("#table-user-input .lbl-check").each(function () {

        var isShipFieldToMark = $(this).find(".cbx-check").attr("checked");

        if (isShipFieldToMark) {
            $(this).find(".btn-check").toggleClass("ship");
        };
    });
});

// function to shoot fields
$(document).ready(function () {
    $(".table-board .lbl-check").click(function () {

        $(this).find(".cell").toggleClass("is-hit");

    });
});

// function to reset ships positions
$(document).ready(function () {
    $("#btn-reset").click(function () {

        var userTable = $("#table-user-input");

        var buttons = userTable.find(".btn-check");
        buttons.removeClass("ship");

        var checkboxes = userTable.find(".cbx-check");
        checkboxes.removeAttr("checked");

    });
});

// function to mark ship fields after turn
$(document).ready(function () {
    $("#table-board-player .lbl-check").each(function () {

        var isShipFieldToMark = $(this).find(".cbx-check").val() === "value";

        if (isShipFieldToMark) {
            $(this).find(".btn-check").toggleClass("ship");
        };

    });
});

// function to mark shot fields after turn
$(document).ready(function () {
    $(".table-board .lbl-check").each(function () {

        var isShootFieldToMark = $(this).find(".cell-param-is-hit").val();

        if (isShootFieldToMark) {
            $(this).find(".cell").toggleClass("is-hit");

            $(this).find(".btn.btn-check").prop("disabled", true);


            var isShipFieldToMark = $(this).find(".cbx-check").val() === "value";

            if (isShipFieldToMark) {
                $(this).find(".btn-check").addClass("ship");
            };
        };

    });
});