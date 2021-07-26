$('document').ready(function () {
    $.ajax({
        url: '/admin/languages?handler=list',
        type: 'get',
        dataType: 'json',
        success: function (resultObj) {
            $.each(resultObj, function (index, elem) {
                let source = document.getElementById("entry-template").innerHTML;
                let template = Handlebars.compile(source);
                $('#member-container').append(template(elem));
            });

            if (prevTranslations != null) {
                $.each(prevTranslations, function (index, elem) {
                    $(`#member-container tr[data-lang="${elem.language}"]`).find('input').val(elem.name);
                });
            }
        }        
    });
});

function save() {
    var items = [];
    $.each($('#member-container tr'), function (index, elem) {
        let language = $(elem).data('lang');
        let name = $(elem).find('input').first().val();

        items.push({
            language,
            name
        });
    });
    $('#Input_Names').val(JSON.stringify(items));
}