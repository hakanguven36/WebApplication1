
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
