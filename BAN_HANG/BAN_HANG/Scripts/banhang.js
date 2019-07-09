
//price
function LoadPrice_ByID_Product() {
    $('#cbx-product').change(function () {
        $("#product-price-container").removeClass("hidden");
        var id_product = $(this).find('option:selected').val();
        var data = {
            id_product: id_product,
            id: "cbx-chose-price",
            idSelected: -1,
            isDisable: false
        }
        LoadPartialViewCallBack("/Common/DS_Gia_SP_TheoID", $("#content-price"), data, LoadPrice_ByID_ProductCallBack);
    });
}
function LoadPrice_ByID_ProductCallBack(data) {
    var id_price = $('#cbx-chose-price').find('option:selected').attr('data-id');
    var gia_xuat = $('#cbx-chose-price').find('option:selected').attr('data-gia-xuat');
    var button = $("#btn-add-product");
    button.attr({ "data-id-price": id_price, "data-gia-xuat": gia_xuat });
}

// đơn vị 
function LoadUnit_ByID_Product() {
    $('#cbx-product').change(function () {
        $("#product-unit-container").removeClass("hidden");
        var id_product = $(this).find('option:selected').val();
        var data = {
            id_product: id_product
        }
        CallAjax("GET", "/Common/LoadUnit_ByID_ProductCallBack", data, LoadUnit_ByID_ProductCallBack);
    });
}

function LoadUnit_ByID_ProductCallBack(data) {
    $('#txt-unit').val(data.result.unit_name);
    document.getElementById('txt-unit').setAttribute('unit-id', data.result.id_unit);

    var button = $("#btn-add-product");
    button.attr({ "data-unit-id": data.result.id_unit, "data-unit-name": data.result.unit_name });
}

// san pham
function LoadNhomSP_TheoIDSP() {
    $('#cbx-product').change(function () {
        $("#group-product").removeClass("hidden");
        $('#auto-product').val('');
        var button = $("#btn-add-product");
        button.attr({ "data-unit-id": "", "data-unit-name": "", "data-id-product": "", "data-name-product": "", "data-group-code-product": "", "data-quantity": "", "data-id-price": "", "data-gia-xuat": "" });

        var id_p = $(this).find('option:selected').val();
        var data = {
            id_p: id_p,
            id: "cbx-nhom-sp-tonkho",
            idSelected: -1,
            isDisable: false
        }
        CallAjax("GET", "/Common/DS_Nhom_SP_TonKho_TheoID_Json", data, LoadAllProductCallBack);
    });
}

function LoadAllProductCallBack(data) {
    BindProductAutoComplete("#auto-product", data.result, ProductSelectCallBack);
}
function BindProductAutoComplete(selector, source, callback) {
    $(selector).autocomplete({
        source: function (req, responseFn) {
            var words = req.term.split(' ');
            var resultsOfSearchTitle = $.grep(source, function (data, index) {
                return words.every(function (word) {
                    if (data.ngaynhap === null) {
                        if (data.tennguon.indexOf(word) !== -1) {
                            return true;
                        }
                    } else {
                        if (data.tennguon.indexOf(word) !== -1 || data.ngaynhap.toString().indexOf(word) !== -1) {
                            return true;
                        }
                    }

                    return false;
                });
            });
            responseFn(resultsOfSearchTitle);
        },
        minLength: 0,
        select: function (event, ui) {
            $(selector).val(ui.item.tensp + " - " + ui.item.group_code_product);
            callback(event, ui);
            return false;
        }
    }).data("ui-autocomplete")._renderItem = function (ul, item) {
        return $("<li>")
            .append("<span class='name-content'>Tên SP : " + item.tensp + "</span><span class='name-content'>Nguồn gốc : " + item.tennguon + "</span><span class='name-content'>Nhóm code :  " + item.group_code_product + "</span><span class='name-content'>Ngày nhập : " + item.ngaynhap + "</span><span class='price-content'>Số lượng còn : " + item.soluongcon + "</span>")
            .appendTo(ul);
    };
    $(selector).focus(function () {
        $(this).autocomplete("search", "");
    });
}

function ProductSelectCallBack(event, ui) {

    $("#product-price-container").removeClass("hidden");
    $("#product-quantity-container").removeClass("hidden");
    $("#product-button-container").removeClass("hidden");
    $("#product-unit-container").removeClass("hidden");
    $("#product-detail-container").addClass("hidden");

    if (ui.item.soluongcon > 0) {
        $('#txt-quantity').attr('max', ui.item.soluongcon);
    }
    $("#txt-quantity").val(1);
    var button = $("#btn-add-product");
    button.attr({ "data-id-product": ui.item.id_product, "data-name-product": ui.item.tensp, "data-group-code-product": ui.item.group_code_product, "data-quantity": 1 });

}
// số lượng

function ChangeQuantity() {
    $('#txt-quantity').change(function () {
        var button = $("#btn-add-product");
        button.attr({ "data-quantity": $(this).val() });
    });
}

// thêm sp 


var lstProduct = [];
function BindButtonBuyClick(self) {

    var id_product = $(self).attr("data-id-product");
    var productname = $(self).attr("data-name-product");
    var gia_xuat = $(self).attr("data-gia-xuat");
    var soluong = $(self).attr("data-quantity");
    var don_vi = $(self).attr("data-unit-name");
    var id_unit = $(self).attr("data-unit-id");
    var group_code = $(self).attr("data-group-code-product");
    var max_quantity = $('#txt-quantity').attr("max");

    var typeBonus = 2;
    var type = "haha";
    var container = $("#tbl-content-selected");


    var data = $("<tr><td>" +
        productname +
        "</td><td>" +
        group_code +
        "</td><td>" +
        ConvertMoneyString(gia_xuat) +
        "</td><td>" +
        don_vi +
        "</td>" +
        "<td><input style='width: 50%;' onkeyup='BindButtonQuantityKeyup(this,event)' max='" +
        max_quantity +
        "' min='1' class='form-control quantity-item bonusitem call-totalprice' data-price='" +
        gia_xuat +
        "'  type='text'   value = '" +
        soluong +
        "' data-type='" +
        type +
        "' data-unitname='" +
        don_vi +
        "' data-id-unit='" +
        id_unit +
        "' data-quantity='" +
        soluong +
        "' data-group-code-product='" +
        group_code +
        "' data-id='" +
        id_product +
        "'/></td>" +
        "<td style='    text-align: center;'><a data-id='" +
        id_product +
        "' data-price='" +
        gia_xuat +
        "' data-type='" +
        type +
        "' class='btn btn-danger' onclick='BindButtonRemoveClick(this)'><i class='fa fa-remove' style='margin-right: 5px;'></i>Xóa</a></td></tr>");

    if (group_code === "" || group_code === undefined) {
        AppendToToastr("Thông báo", "Chưa chọn nhóm sản phẩm hoặc sản phẩm đã hết");
    } else {
        var check = CheckExistedProduct(id_product, soluong, max_quantity, group_code);
        if (check === 0) {
            container.append(data);

        }
        CalcDetailTotalItemBonus();
    }
}

function CheckExistedProduct(idPro, quantity, max_quantity, group_code) {
    var result = 0;
    $(".bonusitem").each(function (i, elm) {

        var id = $(this).attr("data-id");
        var gr_code = $(this).attr("data-group-code-product");

        if (idPro === id && group_code === gr_code) {
            $(this).attr("max", max_quantity);
            $(this).attr("data-quantity", quantity);
            var quantityInput = $(this).parent().parent().find(".quantity-item");
            var currVal = parseInt(quantityInput.val());
            var thisQuan = currVal + parseInt(quantity);

            if (parseInt(thisQuan) <= parseInt(max_quantity)) {
                quantityInput.val(thisQuan);
                $(this).attr("data-quantity", thisQuan);
                result = 1;
                return false;
            } else {
                AppendToToastr("Thông báo", "Số lượng tồn kho đã hết");
                result = 1;
                return false;
            }
        }
    });
    return result;
}

function CalcDetailTotalItemBonus() {
    var isBonusBill = $('input[name=typebonus]:checked').val();

    var totalBonusCurrentValue = 0;
    var totalPriceCurr = 0;

    if ($('.call-totalprice').length === 0) {
        $("#txt-total").val(0);
        $("#txt-bonus").val(0);
        $("#txt-total-final").val(0);
    } else {
        $(".call-totalprice").each(function () {
            var price = ConvertMoneyStringToFloat($(this).attr("data-price"));
            var quantity = ConvertMoneyStringToFloat($(this).attr("data-quantity"));

            //if (isBonusBill !== "1") {
            //    var bonusDisplay = $("#txt-bonus");
            //    var bonusValue = 0;
            //    totalPriceCurr += (price * quantity);
            //    if (!isNaN(parseFloat($(this).val()))) {
            //        bonusValue = ConvertMoneyStringToFloat($(this).val());
            //    }
            //    if (bonusValue <= 100) {
            //        var thisPriceBonus = price * ConvertMoneyStringToFloat(bonusValue) * quantity / 100;
            //        totalBonusCurrentValue += thisPriceBonus;
            //        bonusDisplay.val(ConvertMoneyString(totalBonusCurrentValue));
            //    } else {
            //        totalBonusCurrentValue += bonusValue * quantity;
            //        bonusDisplay.val(ConvertMoneyString(totalBonusCurrentValue));
            //    }
            //} else {

            //}
            totalPriceCurr += (price * quantity);
        });
    }
    $("#txt-total").val(ConvertMoneyString(totalPriceCurr));
    CalFinalPrice();
}

function BindButtonQuantityKeyup(self, event) {
    validate(event, self);
    var value = $(self).val();
    var parent = $(self).parent();
    parent.parent().find(".bonusitem").attr("data-quantity", value);
    CalcDetailTotalItemBonus();
}



function BindButtonRemoveClick(self) {
    var id = $(self).attr("data-id");

    var price = $(self).attr("data-price");
    var total = ConvertMoneyStringToFloat($("#txt-total").val());
    var tmp = total - parseFloat(price);
    $("#txt-total").val(ConvertMoneyString(tmp));
    var td = $(self).parent();
    var tr = td.parent();
    tr.remove();
    $("#txt-bonus").val(0);
    var totalBonusCurrentValue = 0;
    CalcDetailTotalItemBonus(totalBonusCurrentValue);

}

function BindButtonBonusKeydown() {
    $("#txt-bonus").keyup(function () {
        CalFinalPrice();
    });
}
function CalFinalPrice() {
     
    var firstTotalPrice = ConvertMoneyStringToFloat($("#txt-total").val());
    $("#txt-bonus").attr("max", firstTotalPrice);
    var bonus = ConvertMoneyStringToFloat($("#txt-bonus").val());
    var totalFinal = $("#txt-total-final");
    if (bonus <= 100) {
        var calPercent = firstTotalPrice - firstTotalPrice * bonus / 100;
        totalFinal.val(ConvertMoneyString(calPercent));
    } else {
        var cal = firstTotalPrice - bonus;
        totalFinal.val(ConvertMoneyString(cal));
    }
}

function BindButtonRemoveProductOld(self) {
 
    BootstrapDialog.confirm({
        title: 'CHÚ Ý',
        message: 'Xác nhận xóa sản phẩm khỏi đơn hàng không ??',
        type: BootstrapDialog.TYPE_WARNING,
        closable: true,
        draggable: true,
        btnCancelLabel: 'CANCEL',
        btnOKLabel: 'OK',
        btnOKClass: 'btn-warning',
        callback: function (result) {
            if (result) {

                var fd = new FormData();
                fd.append('id_bill_dt', $(self).attr("data-idbill-detail"));

                CallAjax_FormData("/xuat_daily/_Dell_Bill_Detail", fd, RemoveProductInBillCallBack);
            }
        }
    });
}
//function RemoveProductInBillCallBack(data) {

//    if (data.result.IsSuccess) {
//        var self = document.querySelectorAll("[data-idbill-detail='" + data.result.id_bill+"']");

//        var id = $(self).attr("data-id");
//        var price = $(self).attr("data-price");
//        var total = ConvertMoneyStringToFloat($("#txt-total").val());
//        var tmp = total - parseFloat(price);
//        $("#txt-total").val(ConvertMoneyString(tmp));
//        var td = $(self).parent();
//        var tr = td.parent();
//        tr.remove();
//        $("#txt-bonus").val(0);
//        var totalBonusCurrentValue = 0;
//        CalcDetailTotalItemBonus(totalBonusCurrentValue);

//    }
//    AppendToToastr("Thông báo", data.result.Message);
//}

function RemoveProductInBillCallBack(data) {
    if (data.result.IsSuccess) {
        $("#message-result").css("color", "green");
    } else {
        $("#message-result").css("color", "red");
    }
    $("#message-result").text(data.result.Message);

    AppendToToastr("Thông báo", data.result.Message);

    setInterval(function () { window.location.reload(); }, 3000);
}
