
//Thuộc tính chung cho select2
var AttributeSelect2 = {
    selectOnClose: true,
    width: '100%',
    language: {
        noResults: function () {
            return 'Không tìm thấy dữ liệu.';
        },
    },
};

var AttributeDataTable = {
    aLengthMenu: [
        [20, 30, 50, 100, 500, -1],
        [20, 30, 50, 100, 500, "Tất cả"]
    ],
    iDisplayLength: 20,
    language: {
        sSearch: "",
        searchPlaceholder: "Tìm kiếm nhanh",
        emptyTable: "Không có dữ liệu",
        zeroRecords: "Không có dữ liệu",
        sInfo: "_START_ - _END_ trên _TOTAL_",
        infoEmpty: "0 - 0 trên 0",
        infoFiltered: "(Tìm kiếm từ _MAX_ bản ghi)",
        oPaginate: {
            sFirst: "Trang đầu",
            sPrevious: "<",
            sNext: ">",
            sLast: "Trang cuối"
        },
        lengthMenu: "Hiển thị _MENU_ bản ghi"
    },
    ordering: false,
    searching: true,
    stateSave: true,
    autoWidth: false,
    //dom: "tr<'d-flex align-items-center justify-content-between flex-wrap'l <'row'<'col-6'i><'col-6'p>>>"
    dom: "tr<'row'<'col-4'l><'col-8 d-flex justify-content-end m-auto'<'mt-1 me-3'i>p>>"
};

//Cho phép scroll ngang bằng chuột
function ScrollX(className) {
    var scrollContainer = document.querySelector(className);
    if (scrollContainer) {
        scrollContainer.addEventListener("wheel", (evt) => {
            evt.preventDefault();
            scrollContainer.scrollLeft += evt.deltaY;
        });
    };
}


//const slider = document.querySelector('#Parent');
//let mouseDown = false;
//let startX, scrollLeft;

//let startDragging = function (e) {
//    mouseDown = true;
//    startX = e.pageX - slider.offsetLeft;
//    scrollLeft = slider.scrollLeft;
//};
//let stopDragging = function (event) {
//    mouseDown = false;
//};

//slider.addEventListener('mousemove', (e) => {
//    e.preventDefault();
//    if (!mouseDown) { return; }
//    const x = e.pageX - slider.offsetLeft;
//    const scroll = x - startX;
//    slider.scrollLeft = scrollLeft - scroll;
//});

//// Add the event listeners
//slider.addEventListener('mousedown', startDragging, false);
//slider.addEventListener('mouseup', stopDragging, false);
//slider.addEventListener('mouseleave', stopDragging, false);

//Tự tạo style thông báo swal
function CreateCustomSwal(Type, Message, Link) {
    var html = "";
    const wrapper = document.createElement('div');
    wrapper.className = "row";
    html += '<svg width="24" height="24" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">';
    html += '<path fill-rule="evenodd" clip-rule="evenodd" d="M12 1.75C6.33908 1.75 1.75 6.33908 1.75 12C1.75 17.6609 6.33908 22.25 12 22.25C17.6609 22.25 22.25 17.6609 22.25 12C22.25 6.33908 17.6609 1.75 12 1.75ZM0.25 12C0.25 5.51065 5.51065 0.25 12 0.25C18.4893 0.25 23.75 5.51065 23.75 12C23.75 18.4893 18.4893 23.75 12 23.75C5.51065 23.75 0.25 18.4893 0.25 12ZM12 6.25C12.4142 6.25 12.75 6.58579 12.75 7V8C12.75 8.41421 12.4142 8.75 12 8.75C11.5858 8.75 11.25 8.41421 11.25 8V7C11.25 6.58579 11.5858 6.25 12 6.25ZM12 10.25C12.4142 10.25 12.75 10.5858 12.75 11V17C12.75 17.4142 12.4142 17.75 12 17.75C11.5858 17.75 11.25 17.4142 11.25 17V11C11.25 10.5858 11.5858 10.25 12 10.25Z" fill="white"';
    html += '</svg>';
    html += '<span class="ms-2 mt-1">' + Message + '</span>';
    wrapper.innerHTML = html;
    if (Type == "success") {
        swal({
            content: wrapper,
            className: "custom-background-swal",
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
            content: wrapper,
            className: "custom-background-swal",
            button: false,
            html: true,
            timer: 3000
        });
    } else if (Type == "success-no-close") {
        swal({
            content: wrapper,
            className: "custom-background-swal",
            button: false,
            html: true,
            closeOnClickOutside: true
        }).then(function () {
            if (Link) {
                window.location.href = Link;
            } else {
                window.location.reload();
            }
        });
    } else if (Type == "success-no-reload") {
        swal({
            content: wrapper,
            className: "custom-background-swal",
            button: false,
            html: true,
            timer: 5000
        });
    }

};

//Tự tạo datatable không footer
function MyCreateDataTable(TableID) {
    $("#" + TableID).DataTable({
        "aLengthMenu": [
            [20, 30, 50, 100, 500, -1],
            [20, 30, 50, 100, 500, "Tất cả"]
        ],
        "iDisplayLength": 20,
        "language": {
            'sSearch': "",
            'searchPlaceholder': "Tìm kiếm nhanh",
            'emptyTable': "Không có dữ liệu",
            'zeroRecords': "Không có dữ liệu",
            'sInfo': "_START_ - _END_ trên _TOTAL_",
            'infoEmpty': "0 - 0 trên 0",
            'infoFiltered': "(Tìm kiếm từ _MAX_ bản ghi)",
            'oPaginate': {
                'sFirst': "Trang đầu",
                'sPrevious': "<",
                'sNext': ">",
                'sLast': "Trang cuối"
            },
            "lengthMenu": "Hiển thị _MENU_ bản ghi"
        },
        "ordering": false,
        "searching": true,
        "stateSave": false,
        "autoWidth": false,
        //"dom": "tr<'d-flex align-items-center justify-content-between flex-wrap'l <'row'<'col-6'i><'col-6'p>>>"
        "dom": "tr<'row'<'col-4'l><'col-8 d-flex justify-content-end m-auto'<'mt-1 me-3'i>p>>"
    });

    $("#" + TableID).removeClass('dataTable');
};

//function MyCreateDataTableTest(TableID, AttributeDataTable) {
//    $("#" + TableID).DataTable(AttributeDataTable);
//    $("#" + TableID).removeClass('dataTable');
//}

//Select all page datatable
function MySelectAllDataTable(this1, TableID, ColumnIndex) {
    $("#" + TableID).DataTable()
        .column(ColumnIndex)
        .nodes()
        .to$()
        .find('input[type=checkbox]')
        .prop('checked', this1.checked);
};

function MySelectAllDataTableV2(this1, TableID, ColumnIndex) {
    $("#" + TableID).find("tbody tr").each(function () {
        var $td = $(this).find("td").eq(ColumnIndex);
        $td.find('input[type="checkbox"]').prop("checked", this1.checked);
    });
};

function MySearchingDataTable(this1, TableID) {
    $("#" + TableID).DataTable().search(this1.value).draw();
}

//Lấy danh sách Tỉnh/ Thành phố
function SearchListProvince(Type = null) {

    if (Type == null) {
        Type = 0
    } else {
        Type = 1;
    }

    return $.ajax({
        type: 'POST',
        url: '/Handle/SearchListProvince?Type=' + Type,
        loading: true,
        async: false,
        global: false,
        dataType: 'json',
    });
};

function LoadListProvince(sltProvince, Type = null) {
    $("#" + sltProvince).empty();
    var Data = SearchListProvince(Type);

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

function CheckNewAddressLoad(sltDistrict, sltWard) {
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
}

// Thêm sltCheckNewAddress để load địa dư hành chính mới
function LoadListDistrict(sltProvince, sltDistrict, sltWard, sltCheckNewAddress) {
    var ProvinceID = $("#" + sltProvince).children("option:selected").val();
    var NewAddressCheck = $("#" + sltCheckNewAddress).is(':checked');

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

    if (!NewAddressCheck) {
        var Data = SearchListDistrict(ProvinceID);

        var JSON = Data.responseJSON;



        if (JSON.length > 0) {
            $.each(JSON, function (index, value) {
                $("<option/>", {
                    value: value.ID,
                    text: value.Name
                }).appendTo("#" + sltDistrict);
            });
        }
    } else {
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

//Load file JS
function LoadFileJS(FileName) {
    var script = document.createElement("script");
    script.type = "text/javascript";
    script.src = window.location.origin + FileName;
    document.body.appendChild(script);
};

//Tạo input date từ ngày - dên ngày
function CreateDateRangePicker(DivID, StartDate, EndDate) {
    $('#' + DivID).daterangepicker(
        {
            ranges: {
                'Hôm nay': [moment(), moment()],
                'Hôm qua': [moment().subtract(1, 'days'), moment().subtract(1, 'days')],
                'Tuần này': [moment().startOf('isoWeek'), moment()],
                'Tuần trước': [moment().subtract(1, 'isoWeek').startOf('isoWeek'), moment().subtract(1, 'isoWeek').endOf('isoWeek')],
                '7 Ngày trước': [moment().subtract(6, 'days'), moment()],
                '30 Ngày trước': [moment().subtract(29, 'days'), moment()],
                'Tháng này': [moment().startOf('month'), moment().endOf('month')],
                'Tháng trước': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')]
            },
            locale: {
                format: "DD/MM/YYYY",
                separator: " - ",
                applyLabel: 'Đồng ý',
                cancelLabel: "Hủy bỏ",
                fromLabel: 'Từ ngày',
                toLabel: 'Đến ngày',
                customRangeLabel: 'Tùy chọn',
                weekLabel: 'Tuần',
                daysOfWeek: ['CN', 'T2', 'T3', 'T4', 'T5', 'T6', 'T7'],
                monthNames: ['Tháng 1', 'Tháng 2', 'Tháng 3', 'Tháng 4', 'Tháng 5', 'Tháng 6', 'Tháng 7', 'Tháng 8', 'Tháng 9', 'Tháng 10', 'Tháng 11', 'Tháng 12'],
                firstDay: 1
            },
            startDate: StartDate,
            endDate: EndDate,
            maxDate: EndDate,
            //opens: 'left',
            alwaysShowCalendars: true,
            linkedCalendars: false,
            buttonClasses: ['btn', 'btn-sm'],
            applyClass: 'btn-danger',
            cancelClass: 'btn-inverse'
        }
    );

    //$('#' + DivID).attr('readonly', 'readonly');
}

//Tạo input date từ ngày - dên ngày có giờ
function CreateDateTimeRangePicker(DivID, StartDate, EndDate) {
    $('#' + DivID).daterangepicker(
        {
            ranges: {
                'Hôm nay': [moment(), moment()],
                'Hôm qua': [moment().subtract(1, 'days'), moment().subtract(1, 'days')],
                'Tuần này': [moment().startOf('isoWeek'), moment()],
                'Tuần trước': [moment().subtract(1, 'isoWeek').startOf('isoWeek'), moment().subtract(1, 'isoWeek').endOf('isoWeek')],
                '7 Ngày trước': [moment().subtract(6, 'days'), moment()],
                '30 Ngày trước': [moment().subtract(29, 'days'), moment()],
                'Tháng này': [moment().startOf('month'), moment().endOf('month')],
                'Tháng trước': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')]
            },
            locale: {
                format: "DD/MM/YYYY HH:mm:ss",
                separator: " - ",
                applyLabel: 'Đồng ý',
                cancelLabel: "Hủy bỏ",
                fromLabel: 'Từ ngày',
                toLabel: 'Đến ngày',
                customRangeLabel: 'Tùy chọn',
                weekLabel: 'Tuần',
                daysOfWeek: ['CN', 'T2', 'T3', 'T4', 'T5', 'T6', 'T7'],
                monthNames: ['Tháng 1', 'Tháng 2', 'Tháng 3', 'Tháng 4', 'Tháng 5', 'Tháng 6', 'Tháng 7', 'Tháng 8', 'Tháng 9', 'Tháng 10', 'Tháng 11', 'Tháng 12'],
                firstDay: 1
            },
            dateLimit: {
                days: 31
            },
            startDate: StartDate + " 00:00:00",
            endDate: EndDate + " 23:59:59",
            maxDate: EndDate + " 23:59:59",
            //opens: 'left',
            alwaysShowCalendars: true,
            linkedCalendars: false,
            timePicker: true,
            timePicker24Hour: true,
            buttonClasses: ['btn', 'btn-sm'],
            applyClass: 'btn-danger',
            cancelClass: 'btn-inverse'
        }
    );

    //$('#' + DivID).attr('readonly', 'readonly');
}

//Tạo input date từ ngày - đến ngày mặc định 1 tuần
function CreateDateRangePickerWeek(DivID, StartDate, EndDate) {
    //debugger
    $('#' + DivID).daterangepicker(
        {
            ranges: {
                'Hôm nay': [moment(), moment()],
                'Hôm qua': [moment().subtract(1, 'days'), moment().subtract(1, 'days')],
                'Tuần này': [moment().startOf('isoWeek'), moment()],
                'Tuần trước': [moment().subtract(1, 'isoWeek').startOf('isoWeek'), moment().subtract(1, 'isoWeek').endOf('isoWeek')],
                '7 Ngày trước': [moment().subtract(6, 'days'), moment()],
                '30 Ngày trước': [moment().subtract(29, 'days'), moment()],
                'Tháng này': [moment().startOf('month'), moment().endOf('month')],
                'Tháng trước': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')]
            },
            locale: {
                format: "DD/MM/YYYY",
                separator: " - ",
                applyLabel: 'Đồng ý',
                cancelLabel: "Hủy bỏ",
                fromLabel: 'Từ ngày',
                toLabel: 'Đến ngày',
                customRangeLabel: 'Tùy chọn',
                weekLabel: 'Tuần',
                daysOfWeek: ['CN', 'T2', 'T3', 'T4', 'T5', 'T6', 'T7'],
                monthNames: ['Tháng 1', 'Tháng 2', 'Tháng 3', 'Tháng 4', 'Tháng 5', 'Tháng 6', 'Tháng 7', 'Tháng 8', 'Tháng 9', 'Tháng 10', 'Tháng 11', 'Tháng 12'],
                firstDay: 1
            },
            startDate: moment().subtract(6, 'days'),
            endDate: EndDate,
            maxDate: EndDate,
            //opens: 'left',
            alwaysShowCalendars: true,
            linkedCalendars: false,
            buttonClasses: ['btn', 'btn-sm'],
            applyClass: 'btn-danger',
            cancelClass: 'btn-inverse'
        }
    );

    //$('#' + DivID).attr('readonly', 'readonly');
}

//Tạo input date từ ngày - dên ngày ở trang chủ
function CreateDateRangePickerHome(DivID, StartDate, EndDate) {
    var MinDate = new Date();
    var StartDate = new Date();
    var EndDate = new Date();
    MinDate.setDate(StartDate.getDate() - 364);
    StartDate.setDate(StartDate.getDate() - 6);
    EndDate.setDate(EndDate.getDate());
    MinDate = FormatDate(MinDate);
    StartDate = FormatDate(StartDate);
    EndDate = FormatDate(EndDate);
    $('#' + DivID).daterangepicker(
        {
            locale: {
                format: "DD/MM/YYYY",
                separator: " - ",
                applyLabel: 'Đồng ý',
                cancelLabel: "Hủy bỏ",
                fromLabel: 'Từ ngày',
                toLabel: 'Đến ngày',
                customRangeLabel: 'Tùy chọn',
                weekLabel: 'Tuần',
                daysOfWeek: ['CN', 'T2', 'T3', 'T4', 'T5', 'T6', 'T7'],
                monthNames: ['Tháng 1', 'Tháng 2', 'Tháng 3', 'Tháng 4', 'Tháng 5', 'Tháng 6', 'Tháng 7', 'Tháng 8', 'Tháng 9', 'Tháng 10', 'Tháng 11', 'Tháng 12'],
                firstDay: 1
            },
            startDate: StartDate,
            minDate: MinDate,
            endDate: EndDate,
            maxDate: EndDate,
            //opens: 'left',
            alwaysShowCalendars: true,
            linkedCalendars: false,
            buttonClasses: ['btn', 'btn-sm'],
            applyClass: 'btn-danger',
            cancelClass: 'btn-inverse'
        }
    );

    $('#' + DivID).attr('readonly', 'readonly');
}

//Tạo input ngày 
function CreateDatePicker(DivID) {
    $('#' + DivID).daterangepicker(
        {
            singleDatePicker: true,
            locale: {
                format: "DD/MM/YYYY",
                separator: " - ",
                applyLabel: 'Đồng ý',
                cancelLabel: "Hủy bỏ",
                fromLabel: 'Từ ngày',
                toLabel: 'Đến ngày',
                customRangeLabel: 'Tùy chọn',
                weekLabel: 'Tuần',
                daysOfWeek: ['CN', 'T2', 'T3', 'T4', 'T5', 'T6', 'T7'],
                monthNames: ['Tháng 1', 'Tháng 2', 'Tháng 3', 'Tháng 4', 'Tháng 5', 'Tháng 6', 'Tháng 7', 'Tháng 8', 'Tháng 9', 'Tháng 10', 'Tháng 11', 'Tháng 12'],
                firstDay: 1
            },
            maxDate: moment(),
            alwaysShowCalendars: true,
            linkedCalendars: false,
            buttonClasses: ['btn', 'btn-sm'],
            applyClass: 'btn-danger',
            cancelClass: 'btn-inverse'
        }
    );
    //$('#' + DivID).attr('readonly', 'readonly');
}

// Tạo input ngày và giờ
function CreateDatePickerAndTime(DivID, MaxDate) {
    $('#' + DivID).daterangepicker(
        {
            singleDatePicker: true,
            timePicker: true,
            timePicker24Hour: true,
            locale: {
                format: "DD/MM/YYYY HH:mm",
                separator: " - ",
                applyLabel: 'Đồng ý',
                cancelLabel: "Hủy bỏ",
                fromLabel: 'Từ ngày',
                toLabel: 'Đến ngày',
                customRangeLabel: 'Tùy chọn',
                weekLabel: 'Tuần',
                daysOfWeek: ['CN', 'T2', 'T3', 'T4', 'T5', 'T6', 'T7'],
                monthNames: ['Tháng 1', 'Tháng 2', 'Tháng 3', 'Tháng 4', 'Tháng 5', 'Tháng 6', 'Tháng 7', 'Tháng 8', 'Tháng 9', 'Tháng 10', 'Tháng 11', 'Tháng 12'],
                firstDay: 1
            },
            maxDate: MaxDate,
            alwaysShowCalendars: true,
            linkedCalendars: false,
            buttonClasses: ['btn', 'btn-sm'],
            applyClass: 'btn-danger',
            cancelClass: 'btn-inverse'
        }
    );
    //$('#' + DivID).attr('readonly', 'readonly');
}

// Tạo input tháng và năm
function CreateMonthYearPicker(DivID) {
    $.fn.datepicker.dates['vi'] = {
        days: ["Chủ nhật", "Thứ 2", "Thứ 3", "Thứ 4", "Thứ 5", "Thứ 6", "Thứ 7"],
        daysShort: ["CN", "T2", "T3", "T4", "T5", "T6", "T7"],
        daysMin: ["CN", "T2", "T3", "T4", "T5", "T6", "T7"],
        months: ["Tháng 1", "Tháng 2", "Tháng 3", "Tháng 4", "Tháng 5", "Tháng 6", "Tháng 7", "Tháng 8", "Tháng 9", "Tháng 10", "Tháng 11", "Tháng 12"],
        monthsShort: ["T1", "T2", "T3", "T4", "T5", "T6", "T7", "T8", "T9", "T10", "T11", "T12"],
        today: "Today",
        clear: "Clear",
        format: "mm/dd/yyyy",
        titleFormat: "MM yyyy", /* Leverages same syntax as 'format' */
        weekStart: 0
    };
    $('#' + DivID).datepicker({
        autoclose: true,
        format: "mm/yyyy",
        startView: "months",
        minViewMode: "months",
        language: "vi",
        startDate: moment().subtract(4, 'year').toDate(),
        endDate: new Date(),
    });
}

//Ràng buộc dữ liệu
function Validate(Type, Value) {
    //^[a-zA-ZÀÁÂÃÈÉÊÌÍÒÓÔÕÙÚĂĐĨŨƠàáâãèéêìíòóôõùúăđĩũơƯĂẠẢẤẦẨẪẬẮẰẲẴẶẸẺẼỀỀỂẾưăạảấầẩẫậắằẳẵặẹẻẽềềểếỄỆỈỊỌỎỐỒỔỖỘỚỜỞỠỢỤỦỨỪễệỉịọỏốồổỗộớờởỡợụủứừỬỮỰÝỲỴÝỶỸửữựýỳỵỷỹ0-9\s]+$
    var regex;
    if (Type == "MobileNumber") {
        // Bỏ khoảng trắng, dấu chấm, dấu gạch ngang trước khi kiểm tra
        const phone = Value.replace(/[\s.-]/g, "");

        regex = /^(?:(?:(?:\+84|84)(?:2\d{9}|(?:3|5|7|8|9)\d{8}|69\d{6,7}))|(?:0(?:2\d{9}|(?:3|5|7|8|9)\d{8}|69\d{6,7}))|(?:(?:1800|1900)\d{4,6}))$/;
    } else if (Type == "Email") {
        //regex = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
        //regex = /^[a-zA-Z0-9.!#$%&'*+\/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$/;
        regex = /^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
    } else if (Type == "UserName") {
        //regex = /^[a-zA-Z0-9- ]*$/;
        regex = /^[a-zA-Z0-9]+$/;
    } else if (Type == "Password") {
        regex = /^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!#@\$%\^&\*]).{6,20}$/;
    } else if (Type == "NotSpecialCharButVietnamese") {
        regex = /[!@#$%^&*()_+\-=\[\]{};':"\\|,.<>\/?]+/;
    } else if (Type == "SpecialCharButNotVietnamese") {
        //Cho phép nhập: 
        // + a-z A-Z
        // + 0 - 9
        // + Ký tự đặc biệt
        regex = /^[a-zA-Z0-9!@#$%^&*()_+\-=\[\]{};':"\\|,.<>\/?]+$/;
    } else if (Type == "Address") {
        //regex = /[!@#$%^&*()_+\-=\[\]{};':"\\|,.<>?]+/;
        //Cho phép nhập: 
        // + tiếng việt
        // + khoảng trắng
        // + 0 - 9
        // + -/.,
        regex = /^[a-zA-ZÀÁÂÃÈÉÊÌÍÒÓÔÕÙÚĂĐĨŨƠàáâãèéêìíòóôõùúăđĩũơƯĂẠẢẤẦẨẪẬẮẰẲẴẶẸẺẼỀỀỂẾưăạảấầẩẫậắằẳẵặẹẻẽềềểếỄỆỈỊỌỎỐỒỔỖỘỚỜỞỠỢỤỦỨỪễệỉịọỏốồổỗộớờởỡợụủứừỬỮỰÝỲỴỶỸửữựýỳỵỷỹ0-9\-\/.,\s]+$/;
    } else if (Type == "Vietnamese") {
        //Cho phép nhập: 
        // + tiếng việt
        // + khoảng trắng
        // + 0 - 9
        regex = /^[a-zA-ZÀÁÂÃÈÉÊÌÍÒÓÔÕÙÚĂĐĨŨƠàáâãèéêìíòóôõùúăđĩũơƯĂẠẢẤẦẨẪẬẮẰẲẴẶẸẺẼỀỀỂẾưăạảấầẩẫậắằẳẵặẹẻẽềềểếỄỆỈỊỌỎỐỒỔỖỘỚỜỞỠỢỤỦỨỪễệỉịọỏốồổỗộớờởỡợụủứừỬỮỰÝỲỴỶỸửữựýỳỵỷỹ0-9\s]+$/;
    } else if (Type == "Select") {
        //Cho phép nhập: 
        // + tiếng việt
        // + khoảng trắng
        // + 0 - 9
        regex = /^[a-zA-ZÀÁÂÃÈÉÊÌÍÒÓÔÕÙÚĂĐĨŨƠàáâãèéêìíòóôõùúăđĩũơƯĂẠẢẤẦẨẪẬẮẰẲẴẶẸẺẼỀỀỂẾưăạảấầẩẫậắằẳẵặẹẻẽềềểếỄỆỈỊỌỎỐỒỔỖỘỚỜỞỠỢỤỦỨỪễệỉịọỏốồổỗộớờởỡợụủứừỬỮỰÝỲỴỶỸửữựýỳỵỷỹ0-9\s]+$/;
    } else if (Type == "Note") {
        //Cho phép nhập: 
        // + tiếng việt
        // + khoảng trắng
        // + 0 - 9
        // + .,
        regex = /^[a-zA-ZÀÁÂÃÈÉÊÌÍÒÓÔÕÙÚĂĐĨŨƠàáâãèéêìíòóôõùúăđĩũơƯĂẠẢẤẦẨẪẬẮẰẲẴẶẸẺẼỀỀỂẾưăạảấầẩẫậắằẳẵặẹẻẽềềểếỄỆỈỊỌỎỐỒỔỖỘỚỜỞỠỢỤỦỨỪễệỉịọỏốồổỗộớờởỡợụủứừỬỮỰÝỲỴỶỸửữựýỳỵỷỹ0-9.,\s]+$/;
    } else if (Type == "Code") {
        //Cho phép nhập: 
        // + Chữ không dấu
        // + 0 - 9
        // + -
        regex = /^[a-zA-Z0-9-]+$/;
    } else if (Type == "Number") {
        //Cho phép nhập: 
        // + 0 - 9
        regex = /^[0-9]+$/;
    } else if (Type == "Money") {
        //Cho phép nhập: 
        // + 0 - 9
        // + ,
        regex = /^[0-9,]+$/;
    } else if (Type == "ProductCode") {
        //Cho phép nhập: 
        // + Chữ không dấu
        // + 0 - 9
        // + _
        regex = /^[a-zA-Z0-9_]+$/;
    } else if (Type == "SKU") {
        //Cho phép nhập: 
        // + Chữ không dấu
        // + 0 - 9
        // + _
        regex = /^[a-zA-Z0-9_]+$/;
    } else if (Type == "BARCODE") {
        //Cho phép nhập: 
        // + Chữ không dấu
        // + 0 - 9
        // + _
        regex = /^[a-zA-Z0-9_]+$/;
    } else if (Type == "Date") {
        regex = /^(0[1-9]|1\d|2\d|3[01])\/(0[1-9]|1[0-2])\/(19|20)\d{2}$/;

        //dd.MM.yyyy
        //var date_regex = /^(0[1-9]|1\d|2\d|3[01])\.(0[1-9]|1[0-2])\.(19|20)\d{2}$/;
        //alert(date_regex.test("02.02.1991"));

        //      //dd/mm/yyyy
        //      var date_regex = /^(0[1-9]|1\d|2\d|3[01])\/(0[1-9]|1[0-2])\/(19|20)\d{2}$/;
        //          alert(date_regex.test("02/12/1991"));  

        //      //dd-mm-yyyy
        //      var date_regex = /^(0[1-9]|1\d|2\d|3[01])\-(0[1-9]|1[0-2])\-(19|20)\d{2}$/;
        //      alert(date_regex.test("02-12-1991")); 

        //      //mm/dd/yyyy
        //      var date_regex = /^(0[1-9]|1[0-2])\/(0[1-9]|1\d|2\d|3[01])\/(19|20)\d{2}$/;
        //      alert(date_regex.test("12/02/1991")); 


        //      //yyyy.MM.dd
        //      var date_regex = /^((19|20)\d{2})\.(0[1-9]|1[0-2])\.(0[1-9]|1\d|2\d|3[01])$/;
        //      alert(date_regex.test("1991.12.02")); 

        //      //yyyy/MM/dd
        //      var date_regex = /^((19|20)\d{2})\/(0[1-9]|1[0-2])\/(0[1-9]|1\d|2\d|3[01])$/;
        //      alert(date_regex.test("1991/12/02")); 

        //      //yyyy-MM-dd
        //      var date_regex = /^((19|20)\d{2})\-(0[1-9]|1[0-2])\-(0[1-9]|1\d|2\d|3[01])$/;
        //      alert(date_regex.test("1991-12-02"));
    }
    return regex.test(Value);
}

//Kiểm tra dữ liệu khi bị thay đổi
function CheckChangeValue(ID, Text, Type, IsRequire) {
    $("#" + ID).on('change', function () {
        var data = $("#" + ID).val().trim();
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
                if (Validate(Type, Type == "Number" ? NumberReplace(data) : data) == true) {
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
                        case "Password":
                            msg = "Mật khẩu phải có độ dài từ 6 đến 20 ký tự, bao gồm ký tự thường, ký tự in hoa, số và ký tự đặc biệt !#@$%^&";
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

//Chỉ cho phép nhập số
function OnlyNumber(ID) {
    $(ID).on("keypress keyup blur", function (event) {
        $(this).val($(this).val().replace(/[^\d].+/, ""));
        if ((event.which < 48 || event.which > 57)) {
            event.preventDefault();
        }
    });
}

//Lấy Tỉnh/ Thành phố || Quận/ Huyện || Phường/ Xã theo địa chỉ nhập vào (từ gợi ý địa chỉ (Autocomplete) của VNP)
function PWDGetByVNPMapsAutocomplete(Address) {
    return $.ajax({
        type: 'POST',
        url: '/Handle/SearchPWDByVNPMapsAutocomplete',
        dataType: 'json',
        loading: true,
        global: false,
        async: false,
        data: {
            Address: Address
        }
    });
}

//Lấy Tỉnh/ Thành phố || Quận/ Huyện || Phường/ Xã theo địa chỉ nhập vào (từ gợi ý địa chỉ (Search) của VNP)
function PWDGetByVNPMapsSearch(Address) {
    return $.ajax({
        type: 'POST',
        url: '/Handle/SearchPWDByVNPMapsSearch',
        dataType: 'json',
        loading: true,
        global: false,
        async: false,
        data: {
            Address: Address
        }
    });
}

//Lấy Tỉnh/ Thành phố || Quận/ Huyện || Phường/ Xã theo địa chỉ nhập vào (từ gợi ý địa chỉ (Search) của EMS)
function PWDGetByEMSMapsSearch(Address) {
    return $.ajax({
        type: 'POST',
        url: '/Handle/SearchPWDByEMSMapsSearch',
        dataType: 'json',
        loading: true,
        global: false,
        async: false,
        data: {
            Address: Address
        }
    });
}

function PWDGetByEMSVMapsSearch(Address) {
    return $.ajax({
        type: 'POST',
        url: '/Handle/SearchPWDByEMSVMapsSearch',
        dataType: 'json',
        loading: true,
        global: false,
        async: false,
        data: {
            Address: Address
        }
    });
}

//Lấy Tỉnh/ Thành phố || Quận/ Huyện || Phường/ Xã theo VMapCode (từ gợi ý địa chỉ của VNP)
function PWDGetByVMapCode(ProvinceVMapCode, DistrictVMapCode, WardVMapCode) {
    return $.ajax({
        type: 'POST',
        url: '/Handle/SearchPWDByVMapCode',
        dataType: 'json',
        loading: true,
        global: false,
        async: false,
        data: {
            ProvinceVMapCode: ProvinceVMapCode,
            DistrictVMapCode: DistrictVMapCode,
            WardVMapCode: WardVMapCode,
        }
    });
}

// Gợi ý địa chỉ 
function VNPMapsAutocomplete(Address, sltProvince, sltDistrict, sltWard) {
    $("#" + Address).autocomplete({
        delay: 1000,
        source: function (request, response) {
            var Text = $("#" + Address).val().trim();
            $.ajax({
                type: 'POST',
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
            $.when(PWDGetByVMapCode(ui.item.region_gid, ui.item.county_gid, ui.item.locality_gid))
                .then(function (res) {
                    // Resolve 
                    $("#" + Address).val(ui.item.name);
                    if (res.Code == "00") {
                        var data = JSON.parse(res.Data);
                        $("#" + sltProvince).val(data.ProvinceID).change();
                        $("#" + sltDistrict).val(data.DistrictID).change();
                        $("#" + sltWard).val(data.WardID).change();
                    } else {
                        console.log("Lỗi lấy mã maps");
                    }
                }, function () {
                    // Reject!
                    console.log('Something broke!');
                });
        },
        messages: {
            noResults: '',
            results: function () { }
        }
    });
}

function CustomerAutocomplete(DivID) {
    $("#" + DivID).autocomplete({
        delay: 1000,
        source: function (request, response) {
            $.ajax({
                type: 'POST',
                url: '/Handle/SearchListCustomer',
                dataType: 'json',
                loading: true,
                async: false,
                data: {
                    SearchKey: request.term
                },
                success: function (data) {
                    var arr = [];
                    $.map(data, function (value, index) {
                        var obj = {
                            label: value.FullName + " - " + value.MobileNumber,
                            value: value.MobileNumber,
                            //MobileNumber: value.MobileNumber,
                            //FullName: value.FullName,
                        };
                        arr.push(obj);
                    });
                    response(arr);
                },
            });
        },
        minLength: 2,
        //select: function (event, ui) {
        //    $("#" + DivID).val(ui.item.value).change();
        //},
        //messages: {
        //    noResults: 'Không tìm thấy dữ liệu.',
        //}
    });
}

/* Check Date Diff */
function padTo2Digits(num) {
    return num.toString().padStart(2, '0');
}

function FormatDate(date) {
    return [
        padTo2Digits(date.getDate()),
        padTo2Digits(date.getMonth() + 1),
        date.getFullYear(),
    ].join('/');
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

/* End - Check Date Diff */

function CurrencyFormatVND(number) {
    var fNumber = new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(number);
    return (fNumber.replaceAll(".", ","));
}

/* Check null */
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
        if (value == "" || value == undefined) {
            $(errWhere).text('Trường này không được để trống.');
            return true;
        }
        else if (Number(value) == 0) {
            $(errWhere).text('Trường này phải > 0.');
            return true;
        }
        else {
            $(errWhere).text('');
            return isError;
        }
    }
}
/* End - Check null */

//Chặn ký tự đặc biệt và khoảng trắng
function blockSpecialCharAndSpace(e) {
    var k;
    document.all ? k = e.keyCode : k = e.which;
    return ((k > 64 && k < 91) || (k > 96 && k < 123) || k == 8 || (k >= 48 && k <= 57));
}

//Ghép địa chỉ
function ConvertAddress(street, ward, province) {
    if (ward != "") {
        address = street + ", " + ward + ", ";
    } else {
        address = street + ", ";
    }

    if (province != "") {
        address = address + province + ".";
    }

    return address;
}

//Check value khi import excel
function CheckDataJSpreadSheet(Data, ErrWhere, Title, MaxLength, Type, IsRequire) {
    var Comment = "";
    //Data = Data.toString();
    if (Data != "" && Data != "0" && Data != null) {
        if (Data.length > MaxLength) {
            Comment = Title + " độ dài tối đa " + MaxLength + " ký tự.\n";
        } else if (Type) {
            if (Validate(Type, Data) == false) {
                if (Type == "MobileNumber") {
                    Comment = 'Số điện thoại không hợp lệ.\n';
                } else if (Type == "SpecialCharButNotVietnamese") {
                    Comment = Title + ' không được chứa ký tự bỏ dấu.\n';
                } else if (Type == "Email") {
                    Comment = Title + ' không hợp lệ.\n';
                } else {
                    Comment = Title + ' không được chứa ký tự đặc biệt.\n';
                }
            }
        }
    } else {
        if (IsRequire == "Y") {
            if (Data == "" || Data == undefined) {
                Comment = Title + ' không được để trống.\n';
            } else if (Data == "0") {
                Comment = Title + ' phải > 0.\n';
            }
        }
    }

    showJSpreadSheet[0].setComments(ErrWhere, Comment);
    return Comment;
}

function NumberFormat(number) {
    return Intl.NumberFormat('en').format(number);
}

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

//Danh sách sản phẩm 
function SearchListProductVariant(WareHouseID) {
    return $.ajax({
        type: 'POST',
        url: '/Handle/SearchListProductVariantByWareHouseID?WareHouseID=' + WareHouseID,
        loading: true,
        global: false,
        async: false,
        dataType: 'json'
    });
}

//Danh sách kho
function SearchListWareHouse() {
    return $.ajax({
        type: 'POST',
        url: '/Handle/SearchListWareHouse',
        loading: true,
        global: false,
        async: false,
        dataType: 'json'
    });
}

//Danh sách chức năng
function SearchListFunctional(GroupPermissionID) {
    return $.ajax({
        type: 'POST',
        url: '/Handle/SearchListFunctionalByGroupPermissionID?GroupPermissionID=' + GroupPermissionID,
        loading: true,
        global: false,
        async: false,
        dataType: 'json'
    });
}

//Hiển thị số ký tự nhập vào
function ShowInputLength(InputID, OutputID, MaxLength) {
    $("#" + InputID).on("keyup keydown", function () {
        var NoteLength = $("#" + InputID).val().length;
        if (NoteLength <= MaxLength) {
            $("#" + OutputID).text(NoteLength);
        } else {
            var SubString = $("#" + InputID).val().substring(0, MaxLength);
            $("#" + InputID).val(SubString);
        }
    });
}

//function ImageZoom(this1) {
//    // Get the modal
//    var modal = document.getElementById("ImageZoomModal");
//    var modalImg = document.getElementById("ImageZoom");
//    var captionText = document.getElementById("ImageZoomCaption");

//    modal.style.display = "block";
//    modalImg.src = this1.src;
//    captionText.innerHTML = this1.alt;

//    // Get the <span> element that closes the modal
//    var span = document.getElementsByClassName("image-zoom-close")[0];

//    // When the user clicks on <span> (x), close the modal
//    span.onclick = function () {
//        modal.style.display = "none";
//    }
//}

function SetupImageZoomControls() {
    let currentRotation = 0;
    let currentScale = 1;
    let offsetX = 0;
    let offsetY = 0;
    let isDragging = false;
    let startX, startY;

    function getActiveImage() {
        return document.querySelector('#carouselExampleIndicators .carousel-item.active .image-zoom-modal-content');
    }

    window.rotateImage = function (degrees) {
        currentRotation = (currentRotation + degrees) % 360;
        applyTransform();
    }

    window.zoomImage = function (factor) {
        currentScale *= factor;
        applyTransform();
    }

    window.resetTransform = function () {
        currentRotation = 0;
        currentScale = 1;
        offsetX = 0;
        offsetY = 0;
        applyTransform();
    }

    function applyTransform() {
        const img = getActiveImage();
        if (img) {
            img.style.transform = `translate(${offsetX}px, ${offsetY}px) rotate(${currentRotation}deg) scale(${currentScale})`;
        }
    }

    // Drag logic
    const imageContainer = document.getElementById('carouselExampleIndicators');

    // Quan trọng: lắng nghe trực tiếp trên image, không phải container
    imageContainer.addEventListener('mousedown', function (e) {
        const img = getActiveImage();
        if (currentScale > 1 && img && img.contains(e.target)) {
            isDragging = true;
            startX = e.clientX - offsetX;
            startY = e.clientY - offsetY;

            // Ngăn chặn chọn text khi kéo
            e.preventDefault();
        }
    });

    // Gắn vào window để luôn nhận được mouseup dù chuột ở đâu
    window.addEventListener('mousemove', function (e) {
        if (isDragging) {
            offsetX = e.clientX - startX;
            offsetY = e.clientY - startY;
            applyTransform();
        }
    });

    window.addEventListener('mouseup', function () {
        isDragging = false;
    });

    // Reset khi đổi slide
    document.querySelectorAll('[data-bs-slide-to], .carousel-control-prev, .carousel-control-next').forEach(btn => {
        btn.addEventListener('click', function () {
            setTimeout(() => resetTransform(), 300);
        });
    });
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

            // Sau khi HTML render xong thì gắn script zoom/rotate
            SetupImageZoomControls();
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
    this1.src = "/Assets/images/default/avatar.jpg";
    return true;
}

//Tính cước
function TransportPostageCharged(req) {
    return $.ajax({
        type: 'POST',
        url: '/Handle/TransportPostageCharged',
        loading: true,
        global: false,
        async: false,
        dataType: 'json',
        data: req,
    });
}

function TranslateAPI(inputLanguage, outputLanguage, inputText) {
    var outputText = "";
    var url = `https://translate.googleapis.com/translate_a/single?client=gtx&sl=${inputLanguage}&tl=${outputLanguage}&dt=t&q=${encodeURI(inputText)}`;

    $.ajax({
        type: 'POST',
        url: url,
        loading: true,
        global: false,
        async: false,
        dataType: 'json',
        success: function (res) {
            outputText = res[0].map((item) => item[0]).join("");
        }
    });
    return outputText;
}

//Delay khi nhập ký tự dùng cho keyup keydown
function DelayKeyboard(callback, ms) {
    var timer = 0;
    return function () {
        var context = this, args = arguments;
        clearTimeout(timer);
        timer = setTimeout(function () {
            callback.apply(context, args);
        }, ms || 0);
    };
}

function FillProvince(provinceCode, isLoading, element) {
    if (!isLoading) {
        $(element).val(provinceCode).trigger('change');  // set value cho Tỉnh
    }

}

function FillWard(wardCode, isLoading, element) {
    if (!isLoading) {
        $(element).val(wardCode).trigger('change');  // set value cho Phường
    }
}

// Hàm debounce để trì hoãn việc gọi API
function Debounce(fn, delay = 500) {
    let timer;
    return function () {
        clearTimeout(timer);
        timer = setTimeout(() => fn.apply(this, arguments), delay);
    };
}