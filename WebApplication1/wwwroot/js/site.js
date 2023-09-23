
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
            if (isHtmlSayfasi(data)) {
                window.location = "/Home/Index?scrlp=login";
                return;
            }

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



// #region JSONAnswer
function JsonAnswerHandler(data) {
    let dataparsed = JSON.parse(data);
    if (dataparsed.success == true) {
        if (dataparsed.message)
            OzModal.Info("Bilgi", dataparsed.message, 2000);
        else
            OzModal.Info("Bilgi", "İşlem başarılı.", 2000);
        return true;
    }
    else
        OzModal.Info("Error!", dataparsed.message, 8000);
    return false;
}
// #endregion

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
    this.PleaseWait = function () {
        let pleasewait = $("<div>").addClass("d-flex justify-content-center");
        let spinner = $("<div>").addClass("spinner-border").attr("role", "status");

        pleasewait.append(spinner);


        this.Show("Yükleniyor", pleasewait);
    }
    this.Confirm = function (question, btncls, yesCallBack) {
        let yesBtn = $("<button>").addClass("btn ms-auto me-0 " + btncls).html("Evet").click(yesCallBack);
        let noBtn = $("<button>").addClass("btn btn-secondary ms-4 me-0").html("Hayır").click(this.Close);
        let buttonsDiv = $("<div>").addClass("d-flex").append(yesBtn).append(noBtn);
        let confirmBody = $("<div>");
        confirmBody.append("<center><h3>" + question + "</h3></center>");
        confirmBody.append(buttonsDiv);
        this.Show("Sorulur", confirmBody);
    }
    this.Close = function () {
        $(".modal").modal("hide");
    }
}


var OzModal = new OZMODAL();

var PleaseWait = function (div) {
    let pleasewait = $("<div>").addClass("d-flex justify-content-center").html("Lütfen bekleyiniz ");
    let spinner = $("<div>").addClass("spinner-border").attr("role", "status");
    pleasewait.append("&nbsp;");
    pleasewait.append(spinner);
    $(div).html(pleasewait);
}


//OzModal.prototype.Ask = function ({title, question, yesTxt,yescls,noTxt,nocls,callbackyes, callbackno }) {
//    this.modaltitle.html(title);
//    this.modalbody.html(question);
//    let btnyes = $("<button>")
//        .addClass("btn")
//        .addClass(yescls || "btn-danger")
//        .attr("data-bs-dismiss", "modal")
//        .click(callbackyes)
//        .html(yesTxt || "Yes");
//    let btnno = $("<button>")
//        .addClass("btn")
//        .addClass(nocls || "btn-secondary")
//        .attr("data-bs-dismiss", "modal")
//        .click(callbackno)
//        .html(noTxt || "No");
//    this.modalfooter.append(btnyes);
//    this.modalfooter.append(btnno);

//    this.modal
//        .attr("data-bs-backdrop", "static")
//        .attr("data-bs-keyboard", "false");

//    OzModal.Show();
//}
// #endregion

// #region HalfCollapse
function HalfCollapse(dom_id, maxheight_px, closeit_dom_id) {
    function getBackgroundColor(jqueryElement) {
        var color = jqueryElement.css("background-color");

        if (color !== 'rgba(0, 0, 0, 0)') {
            return color;
        }

        if (jqueryElement.is("body")) {
            return false;
        } else {
            return getBackgroundColor(jqueryElement.parent());
        }
    }

    let mydom = $("#" + dom_id);
    let orjhei = mydom.height();
    let mydomsbgcolor = getBackgroundColor(mydom)
    mydom.css("overflow", "hidden");
    mydom.css("outline", "none");
    mydom.css("max-height", maxheight_px + "px");

    let devaminiokuBlurbox = $("<div>")
        //.attr("id",myrandomid)
        .css("background-image", "linear-gradient(transparent," + mydomsbgcolor + ")")
        .css("height", "80px")
        .css("margin-top", "-80px");

    let devaminiokulink = $("<button>")
        .html("<<    Devamını Oku    >>")
        .css("border", "none")
        .css("background-color", "transparent")
        .css("padding", "0.6em")
        .css("margin-bottom", "1em")
        .css("margin-top", "-1em")
        .click(function () {

            if (mydom.css("max-height") == maxheight_px + "px") {
                mydom.animate({ "max-height": orjhei + "px" }, 'fast');
                $(this).html("<<      Kapat       >>");
                //$("#" + myrandomid).hide();
                devaminiokuBlurbox.hide();
            }
            else {
                mydom.animate({ "max-height": maxheight_px + "px" }, 'fast');
                $(this).html("<<    Devamını Oku    >>");
                if (closeit_dom_id) {
                    GoToByScroll(closeit_dom_id);
                }
                else {
                    GoToByScroll(dom_id);
                }

                //$("#" + myrandomid).show();
                devaminiokuBlurbox.show();
            }
        });

    mydom.parent().append(devaminiokuBlurbox);
    mydom.parent().append(devaminiokulink);
}
// #endregion


function GetCities(cityBox, oncompleteCallBack) {
    new OzAjax("/Ortak/GetCities", GET, null, function (data) {
        let dataparsed = JSON.parse(data);
        for (var i = 0; i < dataparsed.length; i++) {
            $(cityBox).append(new Option(dataparsed[i].name, dataparsed[i].id));
        }
        if (oncompleteCallBack instanceof Function)
            oncompleteCallBack();
    }).Send();

}
function GetDistrictsOfCity(id, districtBox, oncompleteCallBack) {
    $(districtBox).empty();
    new OzAjax("/Ortak/GetDistrictsOfCity", GET, { id: id }, function (data) {
        let dataparsed = JSON.parse(data);
        for (var i = 0; i < dataparsed.length; i++) {
            $(districtBox).append(new Option(dataparsed[i].name, dataparsed[i].id));
        }
        if (oncompleteCallBack instanceof Function)
            oncompleteCallBack();
    }).Send();

}


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
var ValidTcNum = function (value) {
    value = value.toString();
    var isEleven = /^[0-9]{11}$/.test(value);
    var totalX = 0;
    for (var i = 0; i < 10; i++) {
        totalX += Number(value.substr(i, 1));
    }
    var isRuleX = totalX % 10 == value.substr(10, 1);
    var totalY1 = 0;
    var totalY2 = 0;
    for (var i = 0; i < 10; i += 2) {
        totalY1 += Number(value.substr(i, 1));
    }
    for (var i = 1; i < 10; i += 2) {
        totalY2 += Number(value.substr(i, 1));
    }
    var isRuleY = ((totalY1 * 7) - totalY2) % 10 == value.substr(9, 0);
    return isEleven && isRuleX && isRuleY;
};

var IsEmail = function (email) {
    let regex = /^([a-zA-Z0-9_\.\-\+])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
    return regex.test(email);
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