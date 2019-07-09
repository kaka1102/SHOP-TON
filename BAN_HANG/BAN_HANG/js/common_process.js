



function LoadPartialView(url, container, data) {
    $.ajax({
        url: url,
        data: data,
        type: "GET",
        dataType: "html",
        success: function (data) {

            container.html(data);
        },
        complete: function () {
            $(".back-loading").remove();
        },
        beforeSend: function () {
            var loader = $('<div class="back-loading"><img class="img-content"/></div>');
            $("body").prepend(loader);
        }
    });
}


function LoadPartialViewViewCS(url, container, data) {
    $.ajax({
        url: url,
        data: data,
        type: "POST",
        contentType: "application/json",
        dataType: "json",
        success: function (data) {

            container.html(data);
        },
        complete: function () {
            $(".back-loading").remove();
        },
        beforeSend: function () {
            var loader = $('<div class="back-loading"><img class="img-content"/></div>');
            $("body").prepend(loader);
        }
    });
}

function LoadPartialViewCallBack(url, container, data, callbackSuccess) {
    $.ajax({
        url: url,
        data: data,
        type: "GET",
        dataType: "html",
        success: function (data) {
            container.html(data);
            callbackSuccess();
        },
        beforeSend: function () {
            //            container.append($('<div class="loaderactive"><div id="loader"></div></div>'));
        }
    });
}

function FormatDate(inputDate) {
    var date = new Date(inputDate);
    if (!isNaN(date.getTime())) {
        var day = date.getDate().toString();
        var month = (date.getMonth() + 1).toString();
        // Months use 0 index.
        return (day[1] ? day : '0' + day[0]) +
            '/' +
            (month[1] ? month : '0' + month[0]) +
            '/' +
            date.getFullYear();
    }
};
//vá tạm lỗi
function FormatDateTmp(inputDate) {
    console.log(inputDate);
    var tmp = inputDate.split(' ');
    return tmp[0];
};
function FormatDateTime(inputDateTime) {
    var d = new Date(inputDateTime),
        dformat = [
            d.getDate(), d.getMonth() + 1,
            d.getFullYear()
        ].join('/') +
            ' ' +
            [
                d.getHours(),
                d.getMinutes()
            ].join(':');
    return dformat;
};
function FormatOnlyTime(inputDateTime) {
    if (inputDateTime == null) {
        return dformat = "-";
    } else {
        var d = new Date(inputDateTime),
            dformat = [

                [
                    d.getHours(),
                    d.getMinutes()
                ].join(':')];


        var abc = dformat[0].split(":");

        if (abc[0].length < 2) {
            dformat = "0" + abc[0] + ":" + abc[1];
        }

        if (abc[1].length < 2) {
            dformat = abc[0] + ":" + "0" + abc[1];
        }
        return dformat;
    }

};
function BoostrapDialogConfirm(title, message, type, callback, dataObj) {
    BootstrapDialog.confirm({
        title: title,
        message: message,
        type: type, // <-- Default value is BootstrapDialog.TYPE_PRIMARY
        closable: true, // <-- Default value is false
        draggable: true, // <-- Default value is false
        btnCancelLabel: 'CANCEL', // <-- Default value is 'Cancel',
        btnOKLabel: 'OK', // <-- Default value is 'OK',
        btnOKClass: 'btn-warning', // <-- If you didn't specify it, dialog type will be used,
        callback: function (result) {
            if (result) {
                callback(dataObj);
            }
        }
    }
    );
}

function BoostrapDialogAlert(title, message, type) {
    BootstrapDialog.alert({
        title: title,
        message: message,
        type: type, // <-- Default value is BootstrapDialog.TYPE_PRIMARY
        closable: true, // <-- Default value is false
        draggable: true, // <-- Default value is false
        btnOKLabel: 'OK', // <-- Default value is 'OK',
        btnOKClass: 'btn-warning' // <-- If you didn't specify it, dialog type will be used,
    }
    );
}

function CallAjax(type, url, data, callback) {
    $.ajax({
        type: type,
        url: url,
        data: data,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            callback(data);
        },
        complete: function () {
            $(".back-loading").remove();
        },
        beforeSend: function () {
            var loader = $('<div class="back-loading"><img class="img-content"/></div>');
            $("body").prepend(loader);
        },
        failure: function () {
            alert("fail");
        }
    });
}
function CallAjax_WithInputObj(type, url, data, callback, dataObj) {
    $.ajax({
        type: type,
        url: url,
        data: data,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            callback(data, dataObj);
        },
        complete: function () {
            $(".back-loading").remove();
        },
        beforeSend: function () {
            var loader = $('<div class="back-loading"><img class="img-content"/></div>');
            $("body").prepend(loader);
        },
        failure: function () {
            alert("fail");
        }
    });
}
function CallAjax_FormData(url, data, callback) {
    $.ajax({
        type: "POST",
        url: url,
        data: data,
        processData: false,
        contentType: false,
        success: function (data) {
            callback(data);
        },
        complete: function () {
            $(".back-loading").remove();
        },
        beforeSend: function () {
            var loader = $('<div class="back-loading"><img class="img-content"/></div>');
            $("body").prepend(loader);
        },
        failure: function () {
            alert("fail");
        }
    });
}

function ConvertMoneyString(data) {
    var value = parseInt(ConvertMoneyStringToFloat(data));
    return value.toString().replace(new RegExp(',', 'g'), "").replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
}

function ConvertMoneyStringToFloat(data) {

    var result = data.toString().replace(new RegExp(',', 'g'), "");
    return parseFloat(result);
}

function validate(evt, self) {
    var maxVal = $(self).attr("max");
    var thisVal = $(self).val();

    var theEvent = evt || window.event;
    if ((evt.keyCode < 48 && evt.keyCode > 57) && (evt.keyCode < 96 && evt.keyCode > 105)) {
        theEvent.preventDefault();
        if (thisVal.length <= 1) {
            $(self).val(0);
        } else {
            var curr = thisVal.substring(0, thisVal.length - 1);
            $(self).val(curr);
        }

    } else {

        if (ConvertMoneyStringToFloat(thisVal) >= ConvertMoneyStringToFloat(maxVal)) {
            $(self).val(ConvertMoneyString(maxVal));
        }
        else if (ConvertMoneyStringToFloat(thisVal) <= 0) {
            $(self).val(0);
        } else {
            $(self).val(ConvertMoneyString(thisVal));
        }
    }
    if (thisVal.length === 0) {
        $(self).val(0);
    }
}

function ShowPopup(selector, url, width, height) {
    selector.colorbox({
        iframe: true,
        innerWidth: width,
        innerHeight: height,
        href: url
    });
}
function ReloadDatatableAjax(selector) {
    var _table = $(selector).DataTable();
    _table.ajax.reload(null, false);
}
function ReloadDatatableAjaxFromPopup(selector) {
    var _table = window.parent.$(selector).DataTable();
    _table.ajax.reload(null, false);
}

function AppendToToastr(title, content) {
    setTimeout(function () {
        toastr.options = {
            closeButton: true,
            progressBar: true,
            showMethod: 'slideDown',
            timeOut: 4000
        }
        toastr.success(content, title);

    }, 1300);
}

function AppendToToastr_survey(title, content, time, showtime) {
    setTimeout(function () {
        toastr.options = {
            closeButton: true,
            progressBar: true,
            showMethod: 'slideDown',
            timeOut: showtime
        }
        toastr.success(content, title);

    }, time);
}

function autoAdjustTextArea_FormShow(o) {
    o.style.height = o.scrollHeight + 'px';
}


function autoAdjustTextArea(o) {
    o.style.height = '1px';
    o.style.height = o.scrollHeight + 'px';
}
var chars = '\n abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ'.split('');
var randRange = function (min, max) { return max == null ? randRange(0, min) : ~~(Math.random() * (max - min) + min); }
var randChars = function (chrs, len) { return len > 0 ? chrs[randRange(chrs.length)] + randChars(chrs, len - 1) : ''; }
var txtAra = document.getElementsByClassName('noscrollbars')[0];

function checkTime(i) {
    if (i < 10) { i = "0" + i };  // add zero in front of numbers < 10
    return i;
}

function HidePhone(phone) {
    var length = phone.length;
    if (length === 0) {
        return phone;
    } else if (length === 10) {
        //  return phone = phone.substring(5,9) + "*****";
        return phone = "*****" + phone.substring(5, 10);
    } else {
        // return phone = phone.substring(6,10) + "******";
        return phone = "*****" + phone.substring(6, 11);
    }
}
