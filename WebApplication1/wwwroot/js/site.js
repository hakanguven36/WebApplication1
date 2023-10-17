
const GET = "GET";
const POST = "POST";

function OzAjax(url, method, sendData, CallBackSuccess, CallBackError, cllBckFnProcess) {
    this.url = url;
    this.method = method || GET;
    this.sendData = sendData;
    this.CallBackSuccess = CallBackSuccess;
    this.CallBackError = CallBackError;
    this.cllBckFnProcess = cllBckFnProcess;
}

OzAjax.prototype.Send = function () {
    var that = this;
    $.ajax({
        url: this.url,
        method: this.method,
        data: this.sendData,
        dataType: "text",
        success: function (data) {
            //if (isHtmlSayfasi(data)) {
            //    window.location = "/Home/Index?scrlp=login";
            //    return;
            //}

            if (that.CallBackSuccess != null)
                that.CallBackSuccess(data);
            else
                that.Success(data);

            function isHtmlSayfasi(p) {
                return JSON.stringify(p.slice(0, 17)).search("<!DOCTYPE html>") != -1;
            }
        },
        error: this.CallBackError || this.Error
    });
};

OzAjax.prototype.MultiSend = function () {
    var that = this;
    $.ajax({
        url: this.url,
        method: "POST",
        data: this.sendData,
        dataType: "text",
        enctype: "multipart/form-data",
        timeout: 180000,
        contentType: false,
        processData: false,
        cache: false,
        async: true,
        success: this.CallBackSuccess,
        error: this.Error,
        xhr: function () {
            var myxhr = $.ajaxSettings.xhr();
            myxhr.upload.addEventListener("progress", function (evt) {
                if (evt.lengthComputable) {
                    let percent = Math.ceil(evt.loaded / evt.total * 100);
                    that.cllBckFnProcess(percent);
                }
            }, false);
            return myxhr;
        }
    });
}

OzAjax.prototype.Success = function (data) {
    new JsonAnswerHandler(data);
};
OzAjax.prototype.Error = function (message) {
    OzModal.Info("Connection error!", JSON.stringify(message), 10000);
};
//OzAjax.prototype.Process = function (event) {
//    var percent = 0;
//    var position = event.loaded || event.position;
//    var total = event.total;
//    if (event.lengthComputable) {
//        percent = Math.ceil(position / total * 100);
//    }
//    this.cllBckFnProcess(percent);
//};


function FirstLettersCapital(htmlobj) {
    let str = $(htmlobj).val();
    let yenistr = str.toLocaleLowerCase('TR').replace(/(?:^|\s|,|;|!|:|-|\.|\?)[a-z0-9ğçşüöı]/g, function (match) {
        return match.toLocaleUpperCase('TR');
    });
    $(htmlobj).val(yenistr);
}
function AllLettersCapital(htmlobj) {
    let str = $(htmlobj).val();
    let yenistr = str.toLocaleUpperCase('TR')
    $(htmlobj).val(yenistr);
}

function OzSetCookie(cname, cvalue, exdays) {
    const d = new Date();
    d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
    let expires = "expires=" + d.toUTCString();
    document.cookie = cname + "=" + cvalue + ";" + expires + ";path=/";
}

function OzGetCookie(cname) {
    let name = cname + "=";
    let decodedCookie = decodeURIComponent(document.cookie);
    let ca = decodedCookie.split(';');
    for (let i = 0; i < ca.length; i++) {
        let c = ca[i];
        while (c.charAt(0) == ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(name) == 0) {
            return c.substring(name.length, c.length);
        }
    }
    return "";
}

Number.prototype.clamp = function (min, max) {
    return Math.min(Math.max(this, min), max);
};

function isDarkBackground(hex) {
    let color = hexToRgb(hex);
    let esik = 120;
    if (color.r > esik && color.g > esik && color.b > esik)
        return false;
    return true;
}

function hexToRgb(hex) {
    // Expand shorthand form (e.g. "03F") to full form (e.g. "0033FF")
    var shorthandRegex = /^#?([a-f\d])([a-f\d])([a-f\d])$/i;
    hex = hex.replace(shorthandRegex, function (m, r, g, b) {
        return r + r + g + g + b + b;
    });

    var result = /^#?([a-f\d]{2})([a-f\d]{2})([a-f\d]{2})$/i.exec(hex);
    return result ? {
        r: parseInt(result[1], 16),
        g: parseInt(result[2], 16),
        b: parseInt(result[3], 16)
    } : null;
}


// #region OzModal
function OZMODAL() {

    let modal = $("<div>")
        .addClass("modal fade");
    let modaldialog = $("<div>")
        .addClass("modal-dialog");
    let modalcontent = $("<div>")
        .addClass("modal-content");
    let modalheader = $("<div>")
        .addClass("modal-header");
    let modaltitle = $("<h5>")
        .addClass("modal-title");
    let modalclosebutton = $("<button>")
        .addClass("btn-close")
        .attr("aria-label", "Close")
        .attr("data-bs-dismiss", "modal");
    let modalbody = $("<div>")
        .addClass("modal-body");
    let modalfooter = $("<div>")
        .addClass("modal-footer");

    modal.append(modaldialog);
    modaldialog.append(modalcontent);
    modalcontent.append(modalheader);
    modalcontent.append(modalbody);
    modalcontent.append(modalfooter);
    modalheader.append(modaltitle);
    modalheader.append(modalclosebutton);
    this.modaltitle = modaltitle;
    this.modalbody = modalbody;
    this.modalfooter = modalfooter;
    $(document).ready(function () {
        $("body").append(modal);
        new bootstrap.Modal($(".modal"));
    });

    this.Show = function (title, body) {
        $(".modal-title").html(title);
        $(".modal-body").html(body);
        $(".modal-footer").remove();
        $(".modal").modal("show");
    }
    this.Info = function (title, message, miliseconds) {
        this.Show(title, message);
        setTimeout(function () { $(".modal").modal("hide") }, miliseconds || 2000);
    }
    this.Close = function () {
        $(".modal").modal("hide");
    }
}

OZMODAL.prototype.Confirmer = function (question, answer1, answer2, ansclass1, ansclass2, confirmCcallback) {
    var questionDiv = $("<h3>").html(question);
    var answer1Btn = $("<button>").addClass("btn " + ansclass1).html(answer1).click(confirmCcallback);
    var answer2Btn = $("<button>").addClass("btn " + ansclass2).html(answer2).click(this.Close);
    var buttonsPanel = $("<div>");
    var confirmerBody = $("<div>");

    confirmerBody.append(questionDiv);
    buttonsPanel.append(answer1Btn);
    buttonsPanel.append(answer2Btn);
    confirmerBody.append(buttonsPanel);
    this.Show("Sorulur", confirmerBody);
}

var OzModal = new OZMODAL();


/*
 function tryMe(param1, param2) {
  alert(param1 + " and " + param2);
}

function callbackTester(callback) {
  callback(arguments[1], arguments[2]);
}

callbackTester(tryMe, "hello", "goodbye");
 */