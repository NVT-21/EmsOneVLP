/*const { debug } = require("console");*/

var AttributeSelect2 = {
    selectOnClose: true,
    width: '100%',
    language: {
        noResults: function () {
            return 'Không tìm thấy dữ liệu.';
        },
    },
};

function OnChangeValidateRegex(ID, errID, text, type) {
    $(ID).on('change', function () {
        var value = $(ID).val().trim();
        if (value != "") {
            if (!validate(type, value)) {
                $(errID).text(text);
            } else {
                $(errID).text("");
            }
        } else {
            $(errID).text("");
        };
    });
}

//function validate(type, value) {
//    var regex;
//    if (type == "MobileNumber") {
//        regex = /((03|05|07|08|09)+([0-9]{8})\b)/g;
//    } else if (type == "Email") {
//        regex = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
//    } else if (type == "UserName") {
//        regex = /^[a-zA-Z0-9- ]*$/;
//    } else if (type == "NotSpecialCharButVietnamese") {
//        regex = /[!@#$%^&*()_+\-=\[\]{};':"\\|,.<>\/?]+/;
//    }
//    return regex.test(value);
//}

function OnChangeValidate(ID, errID, text) {
    $(ID).on("change keyup", function () {
        if ($(ID).val() == "" || $(ID).val() == 0) {
            $(errID).text(text);
            $(ID).focus();
        } else {
            $(errID).text("");
        }
    });
}

function OnlyNumber(ID) {
    $(ID).on("keypress keyup blur", function (event) {
        $(this).val($(this).val().replace(/[^\d].+/, ""));
        if ((event.which < 48 || event.which > 57)) {
            event.preventDefault();
        }
    });
}

//function(e) {
//    if (!((e.keyCode > 95 && e.keyCode < 106)
//        || (e.keyCode > 47 && e.keyCode < 58)
//        || e.keyCode == 8)) {
//        return false;
//    }
//}

function checkSpecialChar_Spaces(value) {
    if (!value.match(/^[a-zA-Z0-9]+$/i)) {
        return 1;
    }
    return 0;
}

function c_AZ09(value) {
    if (!value.match(/^[a-zA-Z0-9]+$/i)) {
        return 1;
    }
    return 0;
}

function checkAZ_09(value) {
    if (!value.match(/^[a-zA-Z0-9_]+$/i)) {
        return 1;
    }
    return 0;
}

function checkByType(Type, value, errWhere, isError) {
    //Type Loại gàn buộc
    //value Giá trị cần kiểm tra
    //errWhere Nơi báo lỗi
    //isError trạng thái lỗi hiện tại của data args
    var regex;
    var errMessage = "";
    if (value != "") {
        switch (Type) {
            case "Code":
                regex = /^[a-zA-Z0-9-]+$/;
                errMessage = "Chỉ được phép nhập chữ cái [a-z], số [0-9] và ký tự - ";
                break;
            case "Name":
                regex = /^[a-zA-ZÀÁÂÃÈÉÊÌÍÒÓÔÕÙÚĂĐĨŨƠàáâãèéêìíòóôõùúăđĩũơƯĂẠẢẤẦẨẪẬẮẰẲẴẶẸẺẼỀỀỂẾưăạảấầẩẫậắằẳẵặẹẻẽềềểếỄỆỈỊỌỎỐỒỔỖỘỚỜỞỠỢỤỦỨỪễệỉịọỏốồổỗộớờởỡợụủứừỬỮỰỲỴÝỶỸửữựỳỵýỷỹ0-9\s]+$/;
                errMessage = "Chỉ được phép nhập tiếng việt và số.";
                break;
        }
        //regex.test(value) = false là lỗi
        if (regex.test(value)) {
            $(errWhere).text('');
            return isError;
        }
        else {
            $(errWhere).text(errMessage);
            return true;
        }
    }
    else {
        $(errWhere).text('Trường này không được để trống.');
        return true;
    }
}

function RemoveErrorMessage(This) {
    $(This).parent().find('.error-message').text("");
}

function funcRequired(ID, isError) {
    var value = $('#' + ID).val();
    if (Array.isArray(value)) {
        if (value.length <= 0) {
            $('#err' + ID).text('Trường này không được để trống.');
            return true;
        }
        else {
            $('#err' + ID).text('');
            return isError;
        }
    }
    else {
        if (value == "" || value == "0") {
            $('#err' + ID).text('Trường này không được để trống.');
            return true;
        }
        else {
            $('#err' + ID).text('');
            return isError;
        }
    }
}

function checkNull(value, errWhere, isError) {
    //value Giá trị cần kiểm tra
    //errWhere Nơi báo lỗi
    //isError trạng thái lỗi hiện tại của data args

    if (Array.isArray(value)) {
        if (value.length <= 0) {
            $(errWhere).text('Trường này không được để trống.');
            return true;
        }
        else {
            $(errWhere).text('');
            return isError;
        }
    }
    else {
        if (value == "") {
            $(errWhere).text('Trường này không được để trống.');
            return true;
        }
        else if (value == "0" || value == "0.00") {
            $(errWhere).text('Trường này giá trị phải lớn hơn 0.');
            return true;
        }
        else {
            $(errWhere).text('');
            return isError;
        }
    }
}

function getCheckboxValue(ID) {
    let checked = document.getElementById(ID).checked;
    if (checked) {
        return "Y"
    }
    else {
        return "N";
    }
}

function checkboxHideShow(idInput) {
    // idInput - Là ID của input hiện hoặc ẩn khi chọn checkbox
    // ID của checkbox phải là: 'cd' + idInput
    var Checked = document.getElementById("cb" + idInput).checked;
    if (Checked) {
        $('#' + idInput).show();
    } else {
        $('#' + idInput).hide();
    }
}

function checkboxHideShowClass(Key) {
    // Key - Là class của view hiện hoặc ẩn khi chọn checkbox
    // ID của checkbox phải là: 'cd' + Key
    var Checked = document.getElementById("cb" + Key).checked;
    if (Checked) {
        $('.' + Key).show();
    } else {
        $('.' + Key).hide();
    }
}

//Convert stringDate (DD/MM/YYYY) to Date
function convertDate(strDate) {
    var parts = strDate.split("/");
    return new Date(parts[2], parts[1] - 1, parts[0]);
}

//Convert stringDateTime (DD/MM/YYYY H:s) to Date
function convertDateTime(strDate) {
    let parts = strDate.split(" ");
    let iDate = parts[0];
    let partDate = iDate.split("/");
    let h = 0, s = 0;
    if (parts.length > 1) {
        let iTime = parts[1];
        var partTime = iTime.split(":");
        h = partTime[0];
        s = partTime[1];
    }
    return new Date(partDate[2], partDate[1] - 1, partDate[0], h, s);
}

//Chặn ký tự đặc biệt và khoảng trắng
function blockSpecialCharAndSpace(e) {
    var k;
    document.all ? k = e.keyCode : k = e.which;
    return ((k > 64 && k < 91) || (k > 96 && k < 123) || k == 8 || (k >= 48 && k <= 57));
}

//Bắt ký tự đặc biệt nhưng được viết bỏ dấu
function OnChangeValidateRegexVersion3(ID, errID, text, type, isRequire) {
    $(ID).on('change keyup', function () {
        if ($(ID).val().trim() != "") {
            if (validate(type, $(ID).val().trim())) {
                $(errID).text("Không được phép có ký tự đặc biệt");
                $(ID).focus();
            } else {
                $(errID).text("");
            }
        } else {
            if (isRequire == "Y") {
                $(errID).text(text);
                $(ID).focus();
            } else {
                $(errID).text("");
            }

        };
    });
}

//Transport
function CheckFromRangeType(This) {
    var value = $(This).val();
    if (value == "1") {
        $(This).parent().parent().find('.FromRegion').show();
        $(This).parent().parent().find('.FromProvince').hide();
    }
    else if (value == "2") {
        $(This).parent().parent().find('.FromRegion').hide();
        $(This).parent().parent().find('.FromProvince').show();
    }
};

function CheckToRangeType(This) {
    var value = $(This).val();
    if (value == "1") {
        $(This).parent().parent().find('.ToRegion').show();
        $(This).parent().parent().find('.ToProvince').hide();
    }
    else if (value == "2") {
        $(This).parent().parent().find('.ToRegion').hide();
        $(This).parent().parent().find('.ToProvince').show();
    }

};

function AddItemPostageWeight(This) {
    let strHtml = '', isError = false;
    let list = $(This).parents('table').find('#Weight').find('tr');
    if (list.length == 0) {
        strHtml = '<tr>' +
            '<td class="vertical-top"><input class="form-control form-control-sm w-100 mb-1" type="text" name="FromWeight" onkeypress="RemoveErrorMessage(this)" value="0" disabled placeholder="Nhập khối lượng" data-inputmask="' + "'alias': 'currency'" + '"/><span class="error-message" name="errFromWeight"></span></td>' +
            '<td class="vertical-top"><input class="form-control form-control-sm w-100 mb-1" type="text" name="ToWeight" onkeypress="RemoveErrorMessage(this)" placeholder="Nhập khối lượng" data-inputmask="' + "'alias': 'currency'" + '"/><span class="error-message" name="errToWeight"></span></td>' +
            '<td class="vertical-top"><input class="form-control form-control-sm w-100 mb-1" type="text" name="Value" onkeypress="RemoveErrorMessage(this)" placeholder="Nhập mức giá" data-inputmask="' + "'alias': 'currency'" + '"/><span class="error-message" name="errValue"></span></td>' +
            '<td class="text-center vertical-top"><button type="button" class="btn btn-sm btn-warning form-control-sm" tabindex="-1" onclick="RemoveItemPostageWeight(this)"><i class="fa fa-minus"></i></button></td>' +
            '</tr>'
    }
    else {
        let FromWeight = $(list[list.length - 1]).find('[name=FromWeight]').val();
        let ToWeight = $(list[list.length - 1]).find('[name=ToWeight]').val();
        let Value = $(list[list.length - 1]).find('[name=Value]').val();
        if (FromWeight == "") {
            $(list[list.length - 1]).find('[name=errFromWeight]').text('Trường này không được để trống.');
            isError = true;
        }
        if (ToWeight == "") {
            $(list[list.length - 1]).find('[name=errToWeight]').text('Trường này không được để trống.');
            isError = true;
        }
        if (FromWeight != "" && ToWeight != "") {
            if (Number(FromWeight) >= Number(ToWeight)) {
                $(list[list.length - 1]).find('[name=errToWeight]').text('Giá trị đến phải lớn hơn giá trị từ.');
                isError = true;
            }
        }

        if (Value == "") {
            $(list[list.length - 1]).find('[name=errValue]').text('Trường này không được để trống.');
            isError = true;
        }
        if (isError) {
            return;
        }

        let FromWeightNext = Number(ToWeight.replaceAll(',', '')) + 1;
        strHtml = '<tr>' +
            '<td class="vertical-top"><input class="form-control form-control-sm w-100 mb-1" type="text" name="FromWeight" onkeypress="RemoveErrorMessage(this)" disabled placeholder="Nhập khối lượng" value="' + FromWeightNext + '" data-inputmask="' + "'alias': 'currency'" + '"/><span class="error-message" name="errFromWeight"></span></td>' +
            '<td class="vertical-top"><input class="form-control form-control-sm w-100 mb-1" type="text" name="ToWeight" onkeypress="RemoveErrorMessage(this)" placeholder="Nhập khối lượng" data-inputmask="' + "'alias': 'currency'" + '"/><span class="error-message" name="errToWeight"></span></td>' +
            '<td class="vertical-top"><input class="form-control form-control-sm w-100 mb-1" type="text" name="Value" onkeypress="RemoveErrorMessage(this)" placeholder="Nhập mức giá" data-inputmask="' + "'alias': 'currency'" + '"/><span class="error-message" name="errValue"></span></td>' +
            '<td class="text-center vertical-top"><button type="button" class="btn btn-sm btn-warning form-control-sm" tabindex="-1" onclick="RemoveItemPostageWeight(this)"><i class="fa fa-minus"></i></button></td>' +
            '</tr>'

    }
    $(This).parents('table').find('#Weight').append(strHtml);

    let ROOT = window.location.origin;
    loadjs(ROOT + '/Assets/js/inputmask.js');
}

function loadjs(file) {
    var script = document.createElement("script");
    script.type = "text/javascript";
    script.src = file;
    document.body.appendChild(script);
}

function RemoveItemPostageWeight(This) {
    $(This).parent().parent().remove();
}

function SurchargeUnit(This) {
    var val = $(This).val();
    var Parent = $(This).parent().parent();
    $(Parent).find('.DefaultValue').val("0");
    if (val == "2") {
        $(Parent).find('.SurchargePercent').show();
        $(Parent).find('.SurchargeValue').css('display', 'none');
        //$(This).parent().parent().find('[name=Value]').attr('');
    }
    else {
        $(Parent).find('.SurchargePercent').hide();
        $(Parent).find('.SurchargeValue').css('display', 'block');
        //$(This).parent().parent().find('[name=Value]').hide();
    }
};

function NoNegativeNumbers(e) {
    if (!((e.keyCode > 95 && e.keyCode < 106)
        || (e.keyCode > 47 && e.keyCode < 58)
        || e.keyCode == 8 || e.keyCode == 9)) {
        return false;
    }
}

function CheckUnit(This) {
    var val = $(This).val();
    if (val == "2") {
        $(This).parent().parent().find('.SurchargePercent').show();
        $(This).parent().parent().find('[name=Value]').attr("data-inputmask", "'alias': 'percentage'");
    }
    else {
        $(This).parent().parent().find('.SurchargePercent').hide();
        $(This).parent().parent().find('[name=Value]').attr("data-inputmask", "'alias': 'currency'");
    }
    let ROOT = window.location.origin;
    loadjs(ROOT + '/Assets/js/inputmask.js');
};

//function checkCode(This, value) {
//    $('#errCode').text("");
//    $(This).removeClass('border-danger');
//    if (value != "") {
//        var check = c_AZ09(value);
//        if (check == 1) {
//            $('#errCode').text("Chỉ được phép nhập chữ cái (a-z), số (0-9)");
//            $(This).addClass('border-danger');
//            $(This).focus();
//            return;
//        }
//    }
//}

function checkAZ09(value, errWhere, isError) {
    //value Giá trị cần kiểm tra
    //errWhere Nơi báo lỗi
    //isError trạng thái lỗi hiện tại của data args
    if (value != "") {
        if (!value.match(/^[a-zA-Z0-9]+$/i)) {
            $(errWhere).text("Chỉ được phép nhập chữ cái (a-z), số (0-9)");
            return true;
        }
        else {
            $(errWhere).text('');
            return isError;
        }
    }
    else {
        $(errWhere).text('Trường này không được để trống.');
        return true;
    }
}

function checkNumber(value, errWhere, isError) {
    //value Giá trị cần kiểm tra
    //errWhere Nơi báo lỗi
    //isError trạng thái lỗi hiện tại của data args
    if (value != "") {
        if (!value.match(/^[0-9]+$/i)) {
            $(errWhere).text("Chỉ được phép nhập số");
            return true;
        }
        else {
            $(errWhere).text('');
            return isError;
        }
    }
    $(errWhere).text('');
    return isError;
}

function checkCode(value, errWhere, isError) {
    //value Giá trị cần kiểm tra
    //errWhere Nơi báo lỗi
    //isError trạng thái lỗi hiện tại của data args
    if (value != "") {
        if (!value.match(/^[a-zA-Z0-9-]+$/i)) {
            $(errWhere).text("Giá trị hợp lệ chỉ bao gồm chữ cái (a-z), số (0-9) và dấu -");
            return true;
        }
        else {
            $(errWhere).text('');
            return isError;
        }
    }
    else {
        $(errWhere).text('Trường này không được để trống.');
        return true;
    }
}

function checkMobileNumber(value, errWhere, isError) {
    //value Giá trị cần kiểm tra
    //errWhere Nơi báo lỗi
    //isError trạng thái lỗi hiện tại của data args
    var regex;
    if (value.length <= "10") {
        regex = /((03|05|07|08|09)+([0-9]{8})\b)/g;
    } else {
        regex = /((02)+([0-9]{9})\b)/g;
    }
    if (value != "") {
        if (!regex.test(value)) {
            $(errWhere).text("Số điện thoại không hợp lệ.");
            return true;
        } else {
            $(errWhere).text('');
            return isError;
        }
    }
    else {
        $(errWhere).text('Trường này không được để trống.');
        return true;
    }
}

function checkEmail(value, errWhere, isError) {
    //value Giá trị cần kiểm tra
    //errWhere Nơi báo lỗi
    //isError trạng thái lỗi hiện tại của data args
    var regex = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    if (value != "") {
        if (!regex.test(value)) {
            $(errWhere).text("Email không hợp lệ.");
            return true;
        } else {
            $(errWhere).text('');
            return isError;
        }
    }
    else {
        $(errWhere).text('Trường này không được để trống.');
        return true;
    }
}

function checkAddress(value, errWhere, isError) {
    //value Giá trị cần kiểm tra
    //errWhere Nơi báo lỗi
    //isError trạng thái lỗi hiện tại của data args
    regex = /^[a-zA-ZÀÁÂÃÈÉÊÌÍÒÓÔÕÙÚĂĐĨŨƠàáâãèéêìíòóôõùúăđĩũơƯĂẠẢẤẦẨẪẬẮẰẲẴẶẸẺẼỀỀỂẾưăạảấầẩẫậắằẳẵặẹẻẽềềểếỄỆỈỊỌỎỐỒỔỖỘỚỜỞỠỢỤỦỨỪễệỉịọỏốồổỗộớờởỡợụủứừỬỮỰỲỴÝỶỸửữựỳỵýỷỹ0-9\-\/.,\s]+$/;
    if (value != "") {
        if (!regex.test(value)) {
            $(errWhere).text('Giá trị hợp lệ không bao gồm ký tự đặc biệt.');
            return true;
        } else {
            $(errWhere).text('');
            return isError;
        }
    }
    else {
        $(errWhere).text('Trường này không được để trống.');
        return true;
    }
}

function checkVietnamese(value, errWhere, isError) {
    //value Giá trị cần kiểm tra
    //errWhere Nơi báo lỗi
    //isError trạng thái lỗi hiện tại của data args
    regex = /^[a-zA-ZÀÁÂÃÈÉÊÌÍÒÓÔÕÙÚĂĐĨŨƠàáâãèéêìíòóôõùúăđĩũơƯĂẠẢẤẦẨẪẬẮẰẲẴẶẸẺẼỀỀỂẾưăạảấầẩẫậắằẳẵặẹẻẽềềểếỄỆỈỊỌỎỐỒỔỖỘỚỜỞỠỢỤỦỨỪễệỉịọỏốồổỗộớờởỡợụủứừỬỮỰỲỴÝỶỸửữựỳỵýỷỹ0-9\s]+$/;
    if (value != "") {
        if (!regex.test(value)) {
            $(errWhere).text('Giá trị hợp lệ không bao gồm ký tự đặc biệt.');
            return true;
        } else {
            $(errWhere).text('');
            return isError;
        }
    }
    else {
        $(errWhere).text('Trường này không được để trống.');
        return true;
    }
}

function checkVietnameseOtherNull(value, errWhere, isError) {
    //value Giá trị cần kiểm tra
    //errWhere Nơi báo lỗi
    //isError trạng thái lỗi hiện tại của data args
    regex = /^[a-zA-ZÀÁÂÃÈÉÊÌÍÒÓÔÕÙÚĂĐĨŨƠàáâãèéêìíòóôõùúăđĩũơƯĂẠẢẤẦẨẪẬẮẰẲẴẶẸẺẼỀỀỂẾưăạảấầẩẫậắằẳẵặẹẻẽềềểếỄỆỈỊỌỎỐỒỔỖỘỚỜỞỠỢỤỦỨỪễệỉịọỏốồổỗộớờởỡợụủứừỬỮỰỲỴÝỶỸửữựỳỵýỷỹ0-9\s]+$/;
    if (value != "") {
        if (!regex.test(value)) {
            $(errWhere).text('Giá trị hợp lệ không bao gồm ký tự đặc biệt.');
            return true;
        } else {
            $(errWhere).text('');
            return isError;
        }
    }
    else {
        $(errWhere).text('');
        return isError;
    }
}

function CurrencyFormatVND(number) {
    var fNumber = new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(number);
    return (fNumber.replaceAll(".", ","));
}

function checkEng(value, errWhere, isError, isCheckNull) {
    //value Giá trị cần kiểm tra
    //errWhere Nơi báo lỗi
    //isError trạng thái lỗi hiện tại của data args
    if (value != "" && value != undefined) {
        if (!value.match(/^[a-zA-Z0-9,]+$/i)) {
            $(errWhere).text("Chỉ được phép nhập chữ cái (a-z), số (0-9) và dấu phẩy");
            return true;
        }
        else {
            $(errWhere).text('');
            return isError;
        }
    }
    else if (isCheckNull){
        $(errWhere).text('Trường này không được để trống.');
        return true;
    }
    return isError;
}

function checkCode_V2(value, errWhere, isError, isCheckNull) {
    //value Giá trị cần kiểm tra
    //errWhere Nơi báo lỗi
    //isError trạng thái lỗi hiện tại của data args
    if (value != "" && value != undefined) {
        if (!value.match(/^[a-zA-Z0-9-,]+$/i)) {
            $(errWhere).text("Giá trị hợp lệ chỉ bao gồm chữ cái (a-z), số (0-9), dấu phẩy và dấu -");
            return true;
        }
        else {
            $(errWhere).text('');
            return isError;
        }
    }
    else if (isCheckNull){
        $(errWhere).text('Trường này không được để trống.');
        return true;
    }
    return isError;
}

////Lấy danh sách Tỉnh/ Thành phố
//function LoadProvinces() {
//    $.ajax({
//        type: 'POST',
//        url: '/Handle/GetListProvinces',
//        loading: true,
//        dataType: 'json',
//        success: function (data) {
//            $.each(data, function (index, value) {
//                $("<option/>", {
//                    value: value.Value,
//                    text: value.Text
//                }).appendTo("#ProvinceID");
//            });
//        },
//        error: function (xhr, status, error) {
//            var err = eval("(" + xhr.responseText + ")");
//            swal({
//                icon: 'error',
//                title: 'Lỗi',
//                text: err.Message,
//                button: {
//                    text: "Đóng",
//                    value: true,
//                    visible: true,
//                    className: "btn btn-primary"
//                }
//            });
//            return;
//        }
//    });
//}

////Lấy danh sách Quận/ Huyện
//$("select.ProvinceID").change(function () {
//    var ProvinceID = $(this).children("option:selected").val();
//    $.ajax({
//        type: 'GET',
//        url: '/Handle/GetDistrictByProvinceID?ProvinceID=' + ProvinceID,
//        loading: true,
//        async: false,
//        dataType: 'json',
//        success: function (data) {
//            $('#DistrictID').empty();
//            $("<option/>", {
//                value: "",
//                text: "--- Quận/ Huyện ---"
//            }).appendTo("#DistrictID");
//            $('#WardID').empty();
//            $("<option/>", {
//                value: "",
//                text: "--- Phường/ Xã ---"
//            }).appendTo("#WardID");
//            $.each(data, function (index, value) {
//                $("<option/>", {
//                    value: value.Value,
//                    text: value.Text
//                }).appendTo("#DistrictID");
//            });
//        },
//        error: function (xhr, status, error) {
//            var err = eval("(" + xhr.responseText + ")");
//            swal({
//                icon: 'error',
//                title: 'Lỗi',
//                text: err.Message,
//                button: {
//                    text: "Đóng",
//                    value: true,
//                    visible: true,
//                    className: "btn btn-primary"
//                }
//            });
//            return;
//        }
//    });
//});

////Lấy danh sách Phường/ Xã
//$("select.DistrictID").change(function () {
//    var DistrictID = $(this).children("option:selected").val();
//    $.ajax({
//        type: 'GET',
//        url: '/Handle/GetWardByDistrictID?DistrictID=' + DistrictID,
//        loading: true,
//        dataType: 'json',
//        async: false,
//        success: function (data) {
//            $('#WardID').empty();
//            $("<option/>", {
//                value: "",
//                text: "--- Phường/ Xã ---"
//            }).appendTo("#WardID");
//            $.each(data, function (index, value) {
//                $("<option/>", {
//                    value: value.Value,
//                    text: value.Text
//                }).appendTo("#WardID");
//            });
//        },
//        error: function (xhr, status, error) {
//            var err = eval("(" + xhr.responseText + ")");
//            swal({Handle/VNPMapsAutocomplete
//                icon: 'error',
//                title: 'Lỗi',
//                text: err.Message,
//                button: {
//                    text: "Đóng",
//                    value: true,
//                    visible: true,
//                    className: "btn btn-primary"
//                }
//            });
//            return;
//        }
//    });
//});

//VNPMapsAutocomplete
function VNPMapsAutocomplete(Address, Province, District, Ward) {
    $("#" + Address).autocomplete({
        source: function (request, response) {
            var Text = $("#" + Address).val().trim();
            $.ajax({
                type: 'GET',
                url: '/Handle/VNPMapsAutocomplete',
                dataType: 'json',
                loading: true,
                async: false,
                data: {
                    Text: Text,
                    Size: 10
                },
                success: function (res) {
                    if (res.Code == "00") {
                        var responseData = JSON.parse(res.Data);
                        var arr = [];
                        $.map(responseData.features, function (objet) {
                            var obj = {
                                label: objet.properties.label,
                                region_gid: objet.properties.region_gid,
                                locality_gid: objet.properties.locality_gid,
                                county_gid: objet.properties.county_gid,
                                name: objet.properties.name
                            };
                            arr.push(obj);
                        });
                        response(arr);
                    }
                }
            });
        },
        minLength: 10,
        select: function (event, ui) {
            $("#" + Address).val(ui.item.name);
            //Provicne
            $.ajax({
                type: 'POST',
                url: '/Handle/MapsGetByVMapCode',
                dataType: 'json',
                loading: true,
                async: false,
                data: {
                    ProvinceVMapCode: ui.item.region_gid,
                    DistrictVMapCode: ui.item.county_gid,
                    WardVMapCode: ui.item.locality_gid,
                },
                success: function (res) {
                    if (res.Code == "00") {
                        var resData = JSON.parse(res.Data);
                        $('#' + Province).val(resData.ProvinceID).trigger('change');
                        $('#' + District).val(resData.DistrictID).trigger('change');
                        $('#' + Ward).val(resData.WardID).trigger('change');
                    } else {
                        console.log("Lỗi lấy mã maps");
                        $('#' + Province).val("").trigger('change');
                        $('#' + District).val("").trigger('change');
                    }
                }
            });
            return false;
        },
        messages: {
            noResults: '',
            results: function () { }
        }
    });
}

function getDateDiff(time1, time2) {
    var str1 = time1.split('/');
    var str2 = time2.split('/');
    var t1 = new Date(str1[2], str1[1] - 1, str1[0]);
    var t2 = new Date(str2[2], str2[1] - 1, str2[0]);
    var diffMS = t2 - t1;
    var diffS = diffMS / 1000;
    var diffM = diffS / 60;
    var diffH = diffM / 60;
    var diffD = diffH / 24;
    return diffD;
}

//Lấy danh sách Tỉnh/ Thành phố - NEW
//function LoadProvinces_V2(sltProvince) {
//    $.ajax({
//        type: 'POST',
//        url: '/Handle/GetProvincesList',
//        loading: true,
//        async: false,
//        global: false,
//        dataType: 'json',
//        success: function (data) {
//            $.each(data, function (index, value) {
//                $("<option/>", {
//                    value: value.Value,
//                    text: value.Text
//                }).appendTo("#" + sltProvince);
//                //$("<option/>", {
//                //    value: value.Value,
//                //    text: value.Text
//                //}).attr("selected", "selected").appendTo("#" + sltProvince);
//                //$("<option/>", {
//                //    value: value.Value,
//                //    text: value.Text
//                //}).attr('data-name', value.Text).appendTo("#" + sltProvinceID);
//            });
//        },
//        error: function (xhr, status, error) {
//            var err = eval("(" + xhr.responseText + ")");
//            swal({
//                icon: 'error',
//                title: 'Lỗi',
//                text: err.Message,
//                button: {
//                    text: "Đóng",
//                    value: true,
//                    visible: true,
//                    className: "btn btn-primary"
//                }
//            });
//            return;
//        }
//    });
//}

//function LoadDistrict_V2(sltProvince, sltDistrict, sltWard) {
//    //Lấy danh sách Quận/ Huyện
//    var ProvinceID = $("#" + sltProvince).children("option:selected").val();
//    $.ajax({
//        type: 'GET',
//        url: '/Handle/GetDistrictByProvinceID?ProvinceID=' + ProvinceID,
//        loading: true,
//        global: false,
//        async: false,
//        dataType: 'json',
//        success: function (data) {
//            $('#' + sltDistrict).empty();
//            $("<option/>", {
//                value: "",
//                text: "--- Quận/ Huyện ---"
//            }).appendTo("#" + sltDistrict);
//            $('#' + sltWard).empty();
//            $("<option/>", {
//                value: "",
//                text: "--- Phường/ Xã ---"
//            }).appendTo("#" + sltWard);
//            $.each(data, function (index, value) {
//                $("<option/>", {
//                    value: value.Value,
//                    text: value.Text
//                }).appendTo("#" + sltDistrict);
//            });
//        },
//        error: function (xhr, status, error) {
//            var err = eval("(" + xhr.responseText + ")");
//            swal({
//                icon: 'error',
//                title: 'Lỗi',
//                text: err.Message,
//                button: {
//                    text: "Đóng",
//                    value: true,
//                    visible: true,
//                    className: "btn btn-primary"
//                }
//            });
//            return;
//        }
//    });
//}

////Lấy danh sách Phường/ Xã
//function LoadWardID_V2(sltDistrict, sltWard) {
//    var DistrictID = $("#" + sltDistrict).children("option:selected").val();
//    $.ajax({
//        type: 'GET',
//        url: '/Handle/GetWardByDistrictID?DistrictID=' + DistrictID,
//        loading: true,
//        global: false,
//        async: false,
//        dataType: 'json',
//        success: function (data) {
//            $('#' + sltWard).empty();
//            $("<option/>", {
//                value: "",
//                text: "--- Phường/ Xã ---"
//            }).appendTo("#" + sltWard);
//            $.each(data, function (index, value) {
//                $("<option/>", {
//                    value: value.Value,
//                    text: value.Text
//                }).appendTo("#" + sltWard);
//            });
//        },
//        error: function (xhr, status, error) {
//            var err = eval("(" + xhr.responseText + ")");
//            swal({
//                icon: 'error',
//                title: 'Lỗi',
//                text: err.Message,
//                button: {
//                    text: "Đóng",
//                    value: true,
//                    visible: true,
//                    className: "btn btn-primary"
//                }
//            });
//            return;
//        }
//    });
//}

//Ràng buộc dữ liệu
function Validate(type, value) {
    var regex;
    if (type == "MobileNumber") {
        if (value.length <= "10") {
            regex = /((03|05|07|08|09)+([0-9]{8})\b)/g;
        } else {
            regex = /((02)+([0-9]{9})\b)/g;
        }
    } else if (type == "Email") {
        //regex = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
        //regex = /^[a-zA-Z0-9.!#$%&'*+\/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$/;
        regex = /^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
    } else if (type == "UserName") {
        //regex = /^[a-zA-Z0-9- ]*$/;
        regex = /^[a-zA-Z0-9]+$/;
    } else if (type == "Password") {
        regex = /^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!#@\$%\^&\*]).{6,12}$/;
    } else if (type == "NotSpecialCharButVietnamese") {
        regex = /[!@#$%^&*()_+\-=\[\]{};':"\\|,.<>\/?]+/;
    } else if (type == "SpecialCharButNotVietnamese") {
        //Cho phép nhập: 
        // + a-z A-Z
        // + 0 - 9
        // + Ký tự đặc biệt
        regex = /^[a-zA-Z0-9!@#$%^&*()_+\-=\[\]{};':"\\|,.<>\/?]+$/;
    } else if (type == "Address") {
        //regex = /[!@#$%^&*()_+\-=\[\]{};':"\\|,.<>?]+/;
        //Cho phép nhập: 
        // + tiếng việt
        // + khoảng trắng
        // + 0 - 9
        // + -/.,
        regex = /^[a-zA-ZÀÁÂÃÈÉÊÌÍÒÓÔÕÙÚĂĐĨŨƠàáâãèéêìíòóôõùúăđĩũơƯĂẠẢẤẦẨẪẬẮẰẲẴẶẸẺẼỀỀỂẾưăạảấầẩẫậắằẳẵặẹẻẽềềểếỄỆỈỊỌỎỐỒỔỖỘỚỜỞỠỢỤỦỨỪễệỉịọỏốồổỗộớờởỡợụủứừỬỮỰỲỴÝỶỸửữựýỳỵỷỹ0-9\-\/.,\s]+$/;
    } else if (type == "Vietnamese") {
        //Cho phép nhập: 
        // + tiếng việt
        // + khoảng trắng
        // + 0 - 9
        regex = /^[a-zA-ZÀÁÂÃÈÉÊÌÍÒÓÔÕÙÚĂĐĨŨƠàáâãèéêìíòóôõùúăđĩũơƯĂẠẢẤẦẨẪẬẮẰẲẴẶẸẺẼỀỀỂẾưăạảấầẩẫậắằẳẵặẹẻẽềềểếỄỆỈỊỌỎỐỒỔỖỘỚỜỞỠỢỤỦỨỪễệỉịọỏốồổỗộớờởỡợụủứừỬỮỰỲỴÝỶỸửữựýỳỵỷỹ0-9\s]+$/;
    } else if (type == "Select") {
        //Cho phép nhập: 
        // + tiếng việt
        // + khoảng trắng
        // + 0 - 9
        regex = /^[a-zA-ZÀÁÂÃÈÉÊÌÍÒÓÔÕÙÚĂĐĨŨƠàáâãèéêìíòóôõùúăđĩũơƯĂẠẢẤẦẨẪẬẮẰẲẴẶẸẺẼỀỀỂẾưăạảấầẩẫậắằẳẵặẹẻẽềềểếỄỆỈỊỌỎỐỒỔỖỘỚỜỞỠỢỤỦỨỪễệỉịọỏốồổỗộớờởỡợụủứừỬỮỰỲỴÝỶỸửữựýỳỵỷỹ0-9\s]+$/;
    } else if (type == "Note") {
        //Cho phép nhập: 
        // + tiếng việt
        // + khoảng trắng
        // + 0 - 9
        // + .,
        regex = /^[a-zA-ZÀÁÂÃÈÉÊÌÍÒÓÔÕÙÚĂĐĨŨƠàáâãèéêìíòóôõùúăđĩũơƯĂẠẢẤẦẨẪẬẮẰẲẴẶẸẺẼỀỀỂẾưăạảấầẩẫậắằẳẵặẹẻẽềềểếỄỆỈỊỌỎỐỒỔỖỘỚỜỞỠỢỤỦỨỪễệỉịọỏốồổỗộớờởỡợụủứừỬỮỰỲỴÝỶỸửữựýỳỵỷỹ0-9.,\s]+$/;
    } else if (type == "Code") {
        //Cho phép nhập: 
        // + Chữ không dấu
        // + 0 - 9
        // + -
        regex = /^[a-zA-Z0-9-]+$/;
    } else if (type == "Number") {
        //Cho phép nhập: 
        // + 0 - 9
        regex = /^[0-9]+$/;
    } else if (type == "Money") {
        //Cho phép nhập: 
        // + 0 - 9
        // + ,
        regex = /^[0-9,]+$/;
    } else if (type == "ProductCode") {
        //Cho phép nhập: 
        // + Chữ không dấu
        // + 0 - 9
        // + _
        regex = /^[a-zA-Z0-9_]+$/;
    } else if (type == "SKU") {
        //Cho phép nhập: 
        // + Chữ không dấu
        // + 0 - 9
        // + _
        regex = /^[a-zA-Z0-9_]+$/;
    } else if (type == "BARCODE") {
        //Cho phép nhập: 
        // + Chữ không dấu
        // + 0 - 9
        // + _
        regex = /^[a-zA-Z0-9_]+$/;
    } else if (type == "ChildCustomerCode") {
        //Cho phép nhập: 
        // + Chữ không dấu
        // + 0 - 9
        // + -,
        regex = /^[a-zA-Z0-9-,]+$/;
    } else if (type == "TransportServiceCode") {
        //Cho phép nhập: 
        // + Chữ không dấu
        // + 0 - 9
        // + _
        regex = /^[a-zA-Z0-9_]+$/;
    }
    return regex.test(value);
}

//function validate(type, value) {
//    var regex;
//    if (type == "MobileNumber") {
//        if (value.length <= "10") {
//            regex = /((03|05|07|08|09)+([0-9]{8})\b)/g;
//        } else {
//            regex = /((02)+([0-9]{9})\b)/g;
//        }
//    } else if (type == "Email") {
//        //regex = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
//        //regex = /^[a-zA-Z0-9.!#$%&'*+\/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$/;
//        regex = /^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
//    } else if (type == "UserName") {
//        //regex = /^[a-zA-Z0-9- ]*$/;
//        regex = /^[a-zA-Z0-9]+$/;
//    } else if (type == "NotSpecialCharButVietnamese") {
//        regex = /[!@#$%^&*()_+\-=\[\]{};':"\\|,.<>\/?]+/;
//    } else if (type == "SpecialCharButNotVietnamese") {
//        //Cho phép nhập: 
//        // + a-z A-Z
//        // + 0 - 9
//        // + Ký tự đặc biệt
//        regex = /^[a-zA-Z0-9!@#$%^&*()_+\-=\[\]{};':"\\|,.<>\/?]+$/;
//    } else if (type == "Address") {
//        //regex = /[!@#$%^&*()_+\-=\[\]{};':"\\|,.<>?]+/;
//        //Cho phép nhập: 
//        // + tiếng việt
//        // + khoảng trắng
//        // + 0 - 9
//        // + -/.,
//        regex = /^[a-zA-ZÀÁÂÃÈÉÊÌÍÒÓÔÕÙÚĂĐĨŨƠàáâãèéêìíòóôõùúăđĩũơƯĂẠẢẤẦẨẪẬẮẰẲẴẶẸẺẼỀỀỂẾưăạảấầẩẫậắằẳẵặẹẻẽềềểếỄỆỈỊỌỎỐỒỔỖỘỚỜỞỠỢỤỦỨỪễệỉịọỏốồổỗộớờởỡợụủứừỬỮỰỲỴÝỶỸửữựỳỵýỷỹ0-9\-\/.,\s]+$/;
//    } else if (type == "Vietnamese") {
//        //Cho phép nhập: 
//        // + tiếng việt
//        // + khoảng trắng
//        // + 0 - 9
//        regex = /^[a-zA-ZÀÁÂÃÈÉÊÌÍÒÓÔÕÙÚĂĐĨŨƠàáâãèéêìíòóôõùúăđĩũơƯĂẠẢẤẦẨẪẬẮẰẲẴẶẸẺẼỀỀỂẾưăạảấầẩẫậắằẳẵặẹẻẽềềểếỄỆỈỊỌỎỐỒỔỖỘỚỜỞỠỢỤỦỨỪễệỉịọỏốồổỗộớờởỡợụủứừỬỮỰỲỴÝỶỸửữựỳỵýỷỹ0-9\s]+$/;
//    } else if (type == "Select") {
//        //Cho phép nhập: 
//        // + tiếng việt
//        // + khoảng trắng
//        // + 0 - 9
//        regex = /^[a-zA-ZÀÁÂÃÈÉÊÌÍÒÓÔÕÙÚĂĐĨŨƠàáâãèéêìíòóôõùúăđĩũơƯĂẠẢẤẦẨẪẬẮẰẲẴẶẸẺẼỀỀỂẾưăạảấầẩẫậắằẳẵặẹẻẽềềểếỄỆỈỊỌỎỐỒỔỖỘỚỜỞỠỢỤỦỨỪễệỉịọỏốồổỗộớờởỡợụủứừỬỮỰỲỴÝỶỸửữựỳỵýỷỹ0-9\s]+$/;
//    } else if (type == "Note") {
//        //Cho phép nhập: 
//        // + tiếng việt
//        // + khoảng trắng
//        // + 0 - 9
//        // + .,
//        regex = /^[a-zA-ZÀÁÂÃÈÉÊÌÍÒÓÔÕÙÚĂĐĨŨƠàáâãèéêìíòóôõùúăđĩũơƯĂẠẢẤẦẨẪẬẮẰẲẴẶẸẺẼỀỀỂẾưăạảấầẩẫậắằẳẵặẹẻẽềềểếỄỆỈỊỌỎỐỒỔỖỘỚỜỞỠỢỤỦỨỪễệỉịọỏốồổỗộớờởỡợụủứừỬỮỰỲỴÝỶỸửữựỳỵýỷỹ0-9.,\s]+$/;
//    } else if (type == "Code") {
//        //Cho phép nhập: 
//        // + Chữ không dấu
//        // + 0 - 9
//        // + -
//        regex = /^[a-zA-Z0-9-]+$/;
//    } else if (type == "Number") {
//        //Cho phép nhập: 
//        // + 0 - 9
//        regex = /^[0-9]+$/;
//    } else if (type == "Money") {
//        //Cho phép nhập: 
//        // + 0 - 9
//        // + ,
//        regex = /^[0-9,]+$/;
//    } else if (type == "ProductCode") {
//        //Cho phép nhập: 
//        // + Chữ không dấu
//        // + 0 - 9
//        // + _
//        regex = /^[a-zA-Z0-9_]+$/;
//    } else if (type == "SKU") {
//        //Cho phép nhập: 
//        // + Chữ không dấu
//        // + 0 - 9
//        // + _
//        regex = /^[a-zA-Z0-9_]+$/;
//    } else if (type == "BARCODE") {
//        //Cho phép nhập: 
//        // + Chữ không dấu
//        // + 0 - 9
//        // + _
//        regex = /^[a-zA-Z0-9_]+$/;
//    } else if (type == "ChildCustomerCode") {
//        //Cho phép nhập: 
//        // + Chữ không dấu
//        // + 0 - 9
//        // + -,
//        regex = /^[a-zA-Z0-9-,]+$/;
//    }
//    return regex.test(value);
//}

//Check data trước khi thực hiện action
//function isValidCharacter(ID, text, type, isRequire) {
//    $("#" + ID).on('change', function () {
//        var data = $("#" + ID).val();
//        if (data == "" || data == undefined) {
//            if (isRequire == "Y") {
//                $("#err" + ID).text(text);
//                $("#" + ID).focus();
//            } else {
//                $("#err" + ID).text("");
//            }
//        }
//        else {
//            if (type != "") {
//                if (Validate(type, type == "Number" ? data.trim().replaceAll(",", "") : data.trim()) == true) {
//                    $("#err" + ID).text("");
//                } else {
//                    var msg = "";
//                    switch (type) {
//                        case "MobileNumber":
//                            msg = "Số điện thoại không hợp lệ.";
//                            break;
//                        case "Email":
//                            msg = "Email không hợp lệ.";
//                            break;
//                        case "TaxCode":
//                            msg = "Mã số thuế không hợp lệ.";
//                            break;
//                        case "Number":
//                            msg = "Giá trị hợp lệ chỉ bao gồm ký tự số.";
//                            break;
//                        case "ChildCustomerCode":
//                            msg = "Giá trị hợp lệ chỉ bao gồm chữ cái (a-z), số (0-9), dấu phẩy và dấu -";
//                            break;
//                        case "Code":
//                            msg = "Giá trị hợp lệ chỉ bao gồm chữ cái (a-z), số (0-9) và dấu -";
//                            break;
//                        default:
//                            msg = "Giá trị hợp lệ không bao gồm ký tự đặc biệt.";
//                            break;
//                    }
//                    $("#err" + ID).text(msg);
//                    $("#" + ID).focus();
//                }
//            } else {
//                $("#err" + ID).text("");
//            }
//        }
//    });
//}

function RedirectUsingAjax(URL) {
    $.ajax({
        type: 'POST',
        url: '/Handle/RedirectUsingAjax?URL=' + URL,
        dataType: 'json',
        loading: true,
        global: false,
        beforeSend: function () {
            $('#ajax_loader').show();
        },
        success: function (data) {
            window.location.href = data.RedirectURL;
        },
        error: function (xhr, status, error) {
            var err = eval("(" + xhr.responseText + ")");
            swal({
                title: 'Lỗi',
                text: err.Message,
                button: {
                    text: "ĐÓNG"
                }
            });
            return;
        }
    });
}

function CreateDatePicker(DivID) {
    $(DivID).datepicker({
        format: 'dd/mm/yyyy',
        language: "vi",
        autoclose: true,
        endDate: moment().format('DD/MM/YYYY HH:mm:ss'),
        orientation: "bottom auto"
    });
}

function CreateDateTimePicker(DivID) {
    $(DivID).datetimepicker({
        format: 'd/m/Y H:i',
        lang: "vi",
        autoclose: true,
        minDate: new Date(),
        endDate: moment().format('DD/MM/YYYY HH:mm:ss'),
        step: 10,
        validateOnBlur: true
    });
}

function CreateSelectPicker(DivID) {
    $(DivID).selectpicker();
}

function MyCreateDataTable(TableID) {
    $("#" + TableID).DataTable({
        "aLengthMenu": [
            [20, 30, 50, 100, 500, -1],
            [20, 30, 50, 100, 500, "Tất cả"]
        ],
        "iDisplayLength": 20,
        "language": {
            "sSearch": "",
            "searchPlaceholder": "Tìm kiếm nhanh",
            "emptyTable": "Không có dữ liệu",
            "zeroRecords": "Không có dữ liệu",
            "sInfo": 'Hiển thị từ _START_ đến _END_ của _TOTAL_ bản ghi',
            "infoEmpty": "Hiển thị từ _START_ đến _END_ của _TOTAL_ bản ghi",
            "infoFiltered": "(Tìm kiếm từ _MAX_ bản ghi)",
            "oPaginate": {
                "sFirst": "Trang đầu",
                "sPrevious": "Trang trước",
                "sNext": "Trang sau",
                "sLast": "Trang cuối"
            },
            'lengthMenu': "Hiển thị _MENU_ bản ghi"
        },
        "ordering": false,
        "stateSave": true,
    });
};

//Tự tạo style thông báo swal
function CreateCustomSwal(Type, Message, Link) {
    //var html = "";
    //const wrapper = document.createElement('div');
    //wrapper.className = "row";
    //html += '<svg width="24" height="24" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">';
    //html += '<path fill-rule="evenodd" clip-rule="evenodd" d="M12 1.75C6.33908 1.75 1.75 6.33908 1.75 12C1.75 17.6609 6.33908 22.25 12 22.25C17.6609 22.25 22.25 17.6609 22.25 12C22.25 6.33908 17.6609 1.75 12 1.75ZM0.25 12C0.25 5.51065 5.51065 0.25 12 0.25C18.4893 0.25 23.75 5.51065 23.75 12C23.75 18.4893 18.4893 23.75 12 23.75C5.51065 23.75 0.25 18.4893 0.25 12ZM12 6.25C12.4142 6.25 12.75 6.58579 12.75 7V8C12.75 8.41421 12.4142 8.75 12 8.75C11.5858 8.75 11.25 8.41421 11.25 8V7C11.25 6.58579 11.5858 6.25 12 6.25ZM12 10.25C12.4142 10.25 12.75 10.5858 12.75 11V17C12.75 17.4142 12.4142 17.75 12 17.75C11.5858 17.75 11.25 17.4142 11.25 17V11C11.25 10.5858 11.5858 10.25 12 10.25Z" fill="white"';
    //html += '</svg>';
    //html += '<span class="ms-2 mt-1">' + Message + '</span>';
    //wrapper.innerHTML = html;
    if (Type == "success") {
        swal({
            //content: wrapper,
            //className: "custom-background-swal",
            icon: "success",
            title: "Thông báo",
            text: Message,
            button: false,
            html: true,
            timer: 2500
        }).then(function () {
            if (Link) {
                window.location.href = Link;
            } else {
                window.location.reload();
            }
        });
    } else if (Type == "error") {
        swal({
            //content: wrapper,
            //className: "custom-background-swal",
            icon: "error",
            title: "Thông báo",
            text: Message,
            button: false,
            html: true,
            timer: 3000
        });
    }
};

//Lấy danh sách Tỉnh/ Thành phố
function SearchListProvince() {
    return $.ajax({
        type: 'POST',
        url: '/Handle/SearchListProvince',
        loading: true,
        async: false,
        global: false,
        dataType: 'json',
    });
};

function LoadListProvince(sltProvince) {
    var Data = SearchListProvince();

    var JSON = Data.responseJSON;

    if (JSON.length > 0) {
        $.each(JSON, function (index, value) {
            $("<option/>", {
                value: value.ID,
                text: value.Name
            }).appendTo("#" + sltProvince);
            //$("<option/>", {
            //    value: value.Value,
            //    text: value.Text
            //}).attr("selected", "selected").appendTo("#" + sltProvince);
            //$("<option/>", {
            //    value: value.Value,
            //    text: value.Text
            //}).attr('data-name', value.Text).appendTo("#" + sltProvinceID);
        });
    }
};

//Lấy danh sách Quận/ Huyện
function SearchListDistrict(ProvinceID) {
    return $.ajax({
        type: 'POST',
        url: '/Handle/SearchListDistrictByProvinceID?ProvinceID=' + ProvinceID,
        loading: true,
        global: false,
        async: false,
        dataType: 'json'
    });
};

function LoadListDistrict(sltProvince, sltDistrict, sltWard) {
    var ProvinceID = $("#" + sltProvince).children("option:selected").val();

    var Data = SearchListDistrict(ProvinceID);

    var JSON = Data.responseJSON;

    $('#' + sltDistrict).empty();
    $("<option/>", {
        value: "",
        text: "--- Quận/ Huyện ---"
    }).appendTo("#" + sltDistrict);

    $('#' + sltWard).empty();
    $("<option/>", {
        value: "",
        text: "--- Phường/ Xã ---"
    }).appendTo("#" + sltWard);

    if (JSON.length > 0) {
        $.each(JSON, function (index, value) {
            $("<option/>", {
                value: value.ID,
                text: value.Name
            }).appendTo("#" + sltDistrict);
        });
    }
};

function SearchDistrictIDByDistrictName(ProvinceID, DistrictName) {
    return $.ajax({
        type: 'POST',
        url: '/Handle/SearchDistrictIDByDistrictName?ProvinceID=' + ProvinceID + "&DistrictName=" + DistrictName,
        loading: true,
        global: false,
        async: false,
        dataType: 'json',
    });
}

//Lấy danh sách Phường/ Xã
function SearchListWard(DistrictID, Type = 0) {
    return $.ajax({
        type: 'POST',
        url: '/Handle/SearchListWardByDistrictID?DistrictID=' + DistrictID + '&Type=' + Type,
        loading: true,
        global: false,
        async: false,
        dataType: 'json'
    });
};

function LoadListWard(sltDistrict, sltWard) {
    var DistrictID = $("#" + sltDistrict).children("option:selected").val();

    var Data = SearchListWard(DistrictID);

    var JSON = Data.responseJSON;

    $('#' + sltWard).empty();
    $("<option/>", {
        value: "",
        text: "--- Phường/ Xã ---"
    }).appendTo("#" + sltWard);

    if (JSON.length > 0) {
        $.each(JSON, function (index, value) {
            $("<option/>", {
                value: value.ID,
                text: value.Name
            }).appendTo("#" + sltWard);
        });
    }
};

function LoadListWardV2(sltProvince, sltWard) {
    var ProvinceID = $("#" + sltProvince).children("option:selected").val();
    $('#' + sltWard).empty();
    $("<option/>", {
        value: "",
        text: "--- Phường/ Xã ---"
    }).appendTo("#" + sltWard);

    var Data = SearchListWard(ProvinceID, 1);

    var JSON = Data.responseJSON;



    if (JSON.length > 0) {
        $.each(JSON, function (index, value) {
            $("<option/>", {
                value: value.ID,
                text: value.Name
            }).appendTo("#" + sltWard);
        });
    }

};

function SearchWardIDByWardName(DistrictID, WardName) {
    return $.ajax({
        type: 'POST',
        url: '/Handle/SearchWardIDByWardName?DistrictID=' + DistrictID + "&WardName=" + WardName,
        loading: true,
        global: false,
        async: false,
        dataType: 'json',
    });
}

//Kiểm tra dữ liệu khi bị thay đổi
function CheckChangeValue(ID, Text, Type, IsRequire) {
    $("#" + ID).on('change', function () {
        var data = $("#" + ID).val();
        if (data == "" || data == undefined) {
            if (IsRequire == "Y") {
                $("#err" + ID).text(Text);
                $("#" + ID).focus();
            } else {
                $("#err" + ID).text("");
            }
        }
        else {
            if (Type == "") {
                $("#err" + ID).text("");
            } else {
                if (Validate(Type, Type == "Number" ? data.trim().replaceAll(",", "") : data.trim()) == true) {
                    $("#err" + ID).text("");
                } else {
                    var msg = "";
                    switch (Type) {
                        case "MobileNumber":
                            msg = "Số điện thoại không hợp lệ.";
                            break;
                        case "Email":
                            msg = "Email không hợp lệ.";
                            break;
                        case "TaxCode":
                            msg = "Mã số thuế không hợp lệ.";
                            break;
                        case "Number":
                            msg = "Giá trị hợp lệ chỉ bao gồm ký tự số.";
                            break;
                        case "ChildCustomerCode":
                            msg = "Giá trị hợp lệ chỉ bao gồm chữ cái (a-z), số (0-9), dấu phẩy và dấu -";
                            break;
                        case "Code":
                            msg = "Giá trị hợp lệ chỉ bao gồm chữ cái (a-z), số (0-9) và dấu -";
                            break;
                        case "Password":
                            msg = "Mật khẩu phải có độ dài từ 6 đến 12 ký tự, bao gồm ký tự thường, ký tự in hoa, số và ký tự đặc biệt !#@$%^&";
                            break;
                        default:
                            msg = "Giá trị hợp lệ không bao gồm ký tự đặc biệt.";
                            break;
                    }
                    $("#err" + ID).text(msg);
                    $("#" + ID).focus();
                }
            }
        }
    });
}

//Kiểm tra dữ liệu trống
function CheckNullValue(Data, DivID, Message, IsError) {
    if (Data) {
        $("#err" + DivID).text("");
    } else {
        $("#err" + DivID).text(Message);
        $(DivID).focus();
        return true;
    }
    return IsError;
}

//Kiểm tra ràng buộc dữ liệu
function CheckRegexValue(Data, Type, DivID, Message, IsError) {
    if (Data) {
        if (Validate(Type, Data) == false) {
            $("#err" + DivID).text(Message);
            $(DivID).focus();
            return true;
        } else {
            $("#err" + DivID).text("");
        }
    }
    return IsError;
}

//Kiểm tra dữ liệu trống và sau đó là ràng buộc
function CheckNullAndRegexValue(Data, Type, DivID, MessageNull, MessageRegex, IsError) {
    var IsNull = CheckNullValue(Data, DivID, MessageNull, IsError);
    if (IsNull == true) {
        return true;
    } else {
        var IsRegex = CheckRegexValue(Data, Type, DivID, MessageRegex, IsError);
        if (IsRegex == true) {
            return true;
        }
    }
    return IsError;
}

//Thay thế dấu phân cách số
function NumberReplace(Number) {
    var Data = 0;
    Number = Number.toString().trim();
    if (Number) {
        if (Number.indexOf(',') > 0) {
            Data = Number.replace(/,/g, '');
        } else if (Number.indexOf('.') > 0) {
            Data = Number.replace(/\./g, '');
        } else {
            Data = Number;
        }
    }
    return parseInt(Data);
}

//Định dạng số
function NumberFormat(number) {
    return Intl.NumberFormat('en').format(number);
}

function ImageZoomAndSlider(this1) {
    // Get the modal
    var modal = document.getElementById("ImageZoomModal");
    modal.style.display = "block";

    var ListImage = $(this1).attr("data-ListImage");
    var ActiveImage = $(this1).attr("src");

    $.ajax({
        type: 'POST',
        url: '/Handle/ImageZoomAndSliderView',
        dataType: 'html',
        loading: true,
        global: false,
        async: false,
        data: {
            ListImage: JSON.parse(ListImage),
            ActiveImage: ActiveImage
        },
        success: function (htmlString) {
            $("#ImageZoom").html(htmlString);
        },
    });

    // Get the <span> element that closes the modal
    var span = document.getElementsByClassName("image-zoom-close")[0];
    // When the user clicks on <span> (x), close the modal
    span.onclick = function () {
        modal.style.display = "none";
    }
}

function ImageError(this1) {
    this1.onerror = "";
    this1.src = "/Assets/images/image-default.png";
    return true;
}

