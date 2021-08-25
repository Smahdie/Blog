$(function () {
    $('.tagsSelector').select2({
        tags: true,
        dir: "rtl",
        ajax: {
            url: '?handler=tags',
            dataType: 'json',
            data: function (params) {
                var query = {
                    search: params.term,
                    language: $('#content_lang').val(),
                    page: params.page || 1
                }
                return query;
            }
        },
        minimumInputLength: 2
    });

    $('#Input_Language').change(function () {
        var lang = $(this).val();
        $('#content_lang').val(lang);
    });

    if (typeof selectedTags != "undefined") {
        $('.tagsSelector').val(selectedTags).trigger('change')
    }

    $('#categories_tree').jstree({
        "plugins": ["checkbox"],
        'core': {
            'data': {
                'url': '?handler=categories'
            }
        }
    }).on('ready.jstree', function (e, data) {
        if (typeof selectedCats != "undefined") {
            data.instance.open_node(selectedCats);
            data.instance.select_node(selectedCats);
        }
    });
})

$('#categories_tree').on('changed.jstree', function (e, data) {
    $('#Input_Categories').val($("#categories_tree").jstree("get_selected"));
});