
$(document).ready(function () {
    var row = "";
    $.ajax({

        type: "POST",
        url: "http://localhost:49943/WebService.asmx/EventReader",
        contentType: "application/json",
        dataType: "json",
        success: function (msg) {

            for (i = 0; i < msg.index; i++) {
                row += "<tr><td>" + msg[i].img + "</td><td>" + msg[i].ID + "</td><td>" + msg[i].name + "</td><td>" + msg[i].amount + "</td></tr>" + "</td><td>" + msg[i].timedate
                    + "</td><td>" + msg[i].eventscol + "</td><td>" + msg[i].admin + "</td></tr>";
            }
            $("#tbDetails ").append(row);

        },
        error: function (data) {
            if (data.status = 200) {

                var response = data.responseText.substring(0, data.responseText.length - 10);
                response = jQuery.parseJSON(response);
                for (i = 0; i < response.length; i++) {
                    row += "<tr style='border:1px solid #F2F2F2;'><td style=background:url('" + response[i].img + "');height:100px;width:100px;></td><td>" + response[i].ID + "</td><td>" + response[i].name + "</td><td>" + response[i].amount + "</td><td>" + response[i].date +
                        "</td><td>" + response[i].time + "</td><td>" + response[i].admins + "</td><td>" + response[i].text + "</td></tr>";
                }

                $("#tbDetails").append(row);
            } else {

                alert(data.status);
            }
        }
    });
});