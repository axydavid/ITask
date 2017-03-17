
$(document).ready(function () {
    document.getElementById("ID").addEventListener("keydown", function (e) {
    var keyCode = e.keyCode || e.which;
    if (keyCode === 13) {
        getEvent(this.value);
    }
    }, false);

    var url = window.location.href; // or window.location.href for current url
    try{
        var captured = /p=([^&]+)/.exec(url)[1]; // Value is in [1] ('384' in our case)
        $("#ID").val(captured);
        getEvent(captured);
    } catch (x) { }

    $("#tbDetails").delegate('tr', 'click', function () {

        var ID, usrID, checkin,maxCheckin;
        $(usrID = this.cells['0'].innerHTML);
        $(maxCheckin = this.cells['5'].innerHTML);
        checkin = $(this).find(".chkin").val();
        ID = $("#ID").val();

        if (ID != "") {
            if (checkin == "0" && $(this).hasClass('greenBg')) { $(this).removeClass('greenBg'); }
            else if (checkin == "0" && maxCheckin > "1") { $(this).addClass('yellowBg'); $(this).find(".chkin").val("1"); checkin = 1; }
            else if (checkin == "0") { $(this).addClass('greenBg'); $(this).find(".chkin").val("1"); checkin = 1; }
            else if (checkin == "1" && maxCheckin > "1") { $(this).removeClass('yellowBg'); $(this).addClass('greenBg'); $(this).find(".chkin").val(maxCheckin); checkin = maxCheckin; }
            else if (checkin == "1") { $(this).removeClass('greenBg'); $(this).find(".chkin").val("0"); checkin = 0; }
            else if (checkin == maxCheckin && $(this).hasClass('greenBg')) { $(this).removeClass('greenBg'); $(this).find(".chkin").val("0"); checkin = 0; }
            else if (checkin > "1") { $(this).addClass('greenBg'); }
    checkIn(ID, usrID, checkin);
        }
    });

});

function getEvent(values) {
    $("#tbDetails tbody").empty();
    var row = "";
    var data = { "ID": values }
    $.ajax({
        type: "POST",
        url: "http://localhost:49943/WebService.asmx/atendantsReader",
        contentType: "application/json",
        data: JSON.stringify(data),
        dataType: "json",
        success: function (msg) {

            for (i = 0; i < msg.index; i++) {
                row += "<tr ><td>" + msg[i].ID + "</td><td>" + msg[i].name + "</td><td>" + msg[i].surname + "</td></tr>" + "</td><td>" + msg[i].email + "</td></tr>"
                 + "</td><td>" + msg[i].phone + "</td></tr>" + "</td><td>" + msg[i].amount + "</td><td>" + msg[i].time + "</td><td>" + msg[i].checkin + "</td></tr>";
            }
            $("#tbDetails tbody").append(row);

        },
        error: function (data) {
            if (data.status = 200) {

                var response = data.responseText.substring(0, data.responseText.length - 10);
                response = jQuery.parseJSON(response);
                for (i = 0; i < response.length; i++) {
                    row += '<tr name="rows"><td>' + response[i].ID + "</td><td>" + response[i].name + "</td><td>" + response[i].surname + "</td><td>" + response[i].email + "</td><td>"
                     + response[i].phone + "</td><td>" + response[i].amount + "</td><td>" + response[i].time + "</td><td><input class='chkin' type='text' value='" + response[i].checkin + "'></td></tr>";
                }

                $("#tbDetails tbody").append(row);
            } else {

                alert(data.status);
            }
        }
    });
}
function checkIn(ID,usrID,checkin) {
    var data = { "ID": ID, "usrID": usrID, "checkin": checkin }
    $.ajax({
        type: "POST",
        url: "http://localhost:49943/WebService.asmx/CheckIn",
        contentType: "application/json",
        data: JSON.stringify(data),
        dataType: "json",
        success: function (msg) {
            msg = msg;
        },
        error: function (msg) {
            msg = msg;

        }

    });
}


