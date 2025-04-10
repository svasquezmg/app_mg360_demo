$(function (e) {

    // basic datatable
    $('#datatable-basic').DataTable({
        language: {
            searchPlaceholder: 'Filtrar...',
            sSearch: '',
        },
        "pageLength": 10,
        scrollX: true
        // scrollX: true
    });
    // basic datatable


    $('#datatable-basic2').DataTable({
        language: {
            searchPlaceholder: 'Filtrar...',
            sSearch: '',
        },
        "pageLength": 10,
        scrollX: true
        // scrollX: true
    });

    // basic datatable
    $('#datatable-basic-HistVentas').DataTable({
        language: {
            searchPlaceholder: 'Filtrar...',
            sSearch: '',
        },
        "pageLength": 10,
        scrollX: true
        // scrollX: true
    });
    // basic datatable

    // basic datatable
    $('#datatable-basic-KardexGeneral').DataTable({
        dom: 'lfrtip',
        language: {
            searchPlaceholder: 'Filtrar...',
            sSearch: '',
        },
        "pageLength": 100,
        scrollX: true
        // scrollX: true
    });
    // basic datatable

    // basic datatable
    $('#datatable-basic-Separacion').DataTable({
        language: {
            searchPlaceholder: 'Filtrar...',
            sSearch: '',
        },
        "pageLength": 25,
        scrollX: true,
        order: []
        // scrollX: true
    });
    // basic datatable


    // responsive datatable
    $('#responsiveDataTable2').DataTable({
        responsive: true,
        language: {
            searchPlaceholder: 'Buscar...',
            sSearch: '',
        },
        "pageLength": 10,
    });

    // responsive datatable
    /*$('#responsiveDataTableCrear').DataTable({
        responsive: true,
        language: {
            searchPlaceholder: 'Buscar...',
            sSearch: '',
        },
        "pageLength": 100,
    });*/
    // responsive datatable

    // responsive datatable
    $('#responsiveDataTable').DataTable({
        responsive: true,
        dom: 'Bfrtipl',
        buttons: [{
            extend: 'excel',
            text: 'Descargar Excel'
        }],
        language: {
            searchPlaceholder: 'Buscar...',
            sSearch: '',
        },
        "pageLength": 10,
    });
    // responsive datatable

    // responsive modal datatable
    $('#responsivemodal-DataTable').DataTable({
        responsive: {
            details: {
                display: $.fn.dataTable.Responsive.display.modal({
                    header: function (row) {
                        var data = row.data();
                        return data[0] + ' ' + data[1];
                    }
                }),
                renderer: $.fn.dataTable.Responsive.renderer.tableAll({
                    tableClass: 'table'
                })
            }
        },
        language: {
            searchPlaceholder: '3Search...',
            sSearch: '',
        },
        "pageLength": 10,
    });
    // responsive modal datatable

    // file export datatable
    $('#file-export').DataTable({
        dom: 'Bfrtip',
        buttons: [
            'copy', 'csv', 'excel', 'pdf', 'print'
        ],
        language: {
            searchPlaceholder: 'Buscar...',
            sSearch: '',
        },
        scrollX: true,
        "pageLength": 100,
        order: []
    });
    // file export datatable

    // delete row datatable
    var table = $('#delete-datatable').DataTable({
        language: {
            searchPlaceholder: '5Search...',
            sSearch: '',
        }
    });
    $('#delete-datatable tbody').on('click', 'tr', function () {
        if ($(this).hasClass('selected')) {
            $(this).removeClass('selected');
        }
        else {
            table.$('tr.selected').removeClass('selected');
            $(this).addClass('selected');
        }
    });
    $('#button').on("click", function () {
        table.row('.selected').remove().draw(false);
    });
    // delete row datatable

    // scroll vertical
    $('#scroll-vertical').DataTable({
        scrollY: '265px',
        scrollCollapse: true,
        paging: false,
        scrollX: true,
        language: {
            searchPlaceholder: '6Search...',
            sSearch: '',
        },
    });
    // scroll vertical

    // hidden columns
    $('#hidden-columns').DataTable({
        columnDefs: [
            {
                target: 2,
                visible: false,
                searchable: false,
            },
            {
                target: 3,
                visible: false,
            },
        ],
        language: {
            searchPlaceholder: '7Search...',
            sSearch: '',
        },
        "pageLength": 10,
        // scrollX: true
    });
    // hidden columns

    // add row datatable
    var t = $('#add-row').DataTable({

        language: {
            searchPlaceholder: '8Search...',
            sSearch: '',
        },
    });
    var counter = 1;
    $('#addRow').on('click', function () {
        t.row.add([counter + '.1', counter + '.2', counter + '.3', counter + '.4', counter + '.5']).draw(false);
        counter++;
    });
    // add row datatable

});
