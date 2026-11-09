(function ($) {
    'use strict';
    $(function () {
        $('#data-table').DataTable({
            aLengthMenu: [
                [20, 30, 50, 100, -1],
                [20, 30, 50, 100, "Tất cả"]
            ],
            info: true,
            searching: true,
            ordering: false,
            paging: true,
            iDisplayLength: 20,
            bLengthChange: true,
            language: {
                sSearch: "Tìm kiếm",
                sInfo: 'Hiển thị từ _START_ đến _END_ của _TOTAL_ dòng',
                oPaginate: {
                    sFirst: 'Trang đầu',
                    sPrevious: 'Trang trước',
                    sNext: 'Trang sau',
                    sLast: 'Trang cuối'
                },
                emptyTable: "Không có dữ liệu",
                infoEmpty: "Hiển thị 0 đến 0 của 0 dòng",
                zeroRecords: "Không tìm thấy dòng phù hợp",
                infoFiltered: "(tìm kiếm từ _MAX_ dòng)",
                loadingRecords: "Đang tải...",
                processing: "Đang xử lý...",
                lengthMenu: "Hiển thị _MENU_ bản ghi"
            }
        });
        $('#data-table').each(function () {
            var datatable = $(this);
            // SEARCH - Add the placeholder for Search and Turn this into in-line form control
            var search_input = datatable.closest('.dataTables_wrapper').find('div[id$=_filter] input');
            search_input.attr('placeholder', 'Tìm kiếm nhanh');
            search_input.removeClass('form-control-sm');
            // LENGTH - Inline-Form control
            var length_sel = datatable.closest('.dataTables_wrapper').find('div[id$=_length] select');
            length_sel.removeClass('form-control-sm');
            length_sel.css('color', 'black');
        });
    });
})(jQuery);