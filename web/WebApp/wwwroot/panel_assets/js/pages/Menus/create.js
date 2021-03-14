$('document').ready(function () {
    $('#TargetType').change(function () {
        $('#CategoryId,#PageId').attr('disabled', 'disabled');
        var currentVal = parseInt($(this).val());
        if (currentVal === 4) {
            $('#CategoryId').removeAttr('disabled');
            return;
        }
        if (currentVal === 5) {
            $('#PageId').removeAttr('disabled');
        }
    });

    $('#save-btn').click(function () {
        let typeId = parseInt($("#TargetType").val());
        if (isNaN(typeId))
            return;
        let typeName = $("#TargetType option:selected").text();
        let categoryId = null;
        let categoryName = "---";
        let pageId = null;
        let pageTitle = "---";
        if (typeId == 4) {
            categoryId = parseInt($("#CategoryId").val());
            if (isNaN(categoryId)) {
                return;
            }
            categoryName = $("#CategoryId option:selected").text();
        }
        else if (typeId == 5) {
            pageId = parseInt($("#PageId").val());
            if (isNaN(pageId)) {
                return;
            }
            pageTitle = $("#PageId option:selected").text();
        }

        let resultObj = { typeId, typeName, categoryId, categoryName, pageId, pageTitle };
        let source = document.getElementById("entry-template").innerHTML;
        let template = Handlebars.compile(source);
        $('#member-container').append(template(resultObj));
    });

    $('#member-container').on('click', '.btn-remove-row', function (e) {
        e.preventDefault();
        $(this).closest('tr').remove();
        return false;
    });

    $('#member-container').sortable();
});

function save() {
    var items = [];
    $.each($('#member-container tr'), function (index, elem) {
        let TargetType = $(elem).data('type');
        let CategoryId = parseInt($(elem).data('category'));
        if (isNaN(CategoryId)) {
            CategoryId = null;
        }
        let PageId = parseInt($(elem).data('page'));
        if (isNaN(PageId)) {
            PageId = null;
        }

        let Id = parseInt($(elem).data('id'));

        items.push({
            Id,
            TargetType,
            CategoryId,
            PageId
        });
    });
    $('#Input_MenuMembers').val(JSON.stringify(items));
}