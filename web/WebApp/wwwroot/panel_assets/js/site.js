if (typeof Dropzone != "undefined") {
    Dropzone.autoDiscover = false;
}

$(document).ready(function () {
    let dropzoneOptions = {
        maxFilesize: 1,
        dictRemoveFile: 'حذف',
        addRemoveLinks: true,
        success: function (e) {
            var dropZoneElement = $(this).attr('element');
            var input = $(dropZoneElement).children('input.address-holder');
            let fileAddress = e.xhr.response.replaceAll('"', '');
            input.val(fileAddress);
        },
        init: function () {
            var dropZoneElement = $(this).attr('element');
            var input = $(dropZoneElement).children('input.address-holder');
            var fileAddress = $(input).val();
            if (fileAddress !== '') {
                let myDropzone = this;

                let mockFile = { name: "Filename", size: 12345 };
                let callback = null;
                let crossOrigin = null;
                let resizeThumbnail = false;
                myDropzone.displayExistingFile(mockFile, $(input).val(), callback, crossOrigin, resizeThumbnail);
                $('.dz-image').addClass('initial');
            }
        },
        removedfile: function (file) {
            var dropZoneElement = $(this).attr('element');
            var input = $(dropZoneElement).children('input.address-holder');
            $(input).val('');
            var _ref;
            return (_ref = file.previewElement) != null ? _ref.parentNode.removeChild(file.previewElement) : void 0;
        }
    }

    if ($(".dropzone-file-area").length > 0) {
        $.each($(".dropzone-file-area"), function (index, elem) {
            dropzoneOptions.url = $(elem).data('upload-address');
            $(elem).dropzone(dropzoneOptions);
        });
    }

    toastr.options.rtl = true;
    toastr.options.positionClass = 'toast-top-left';

    init();
    openSearchboxIfNeeded();
    markCurrentSort();

    var dateSelectors = $("#AdvancedSearchBox").find("[data-type='datetime']").find('input[type="text"]');
    $.each(dateSelectors, (index, item) => {
        var id = $(item).attr('id');
        $(item).MdPersianDateTimePicker({
            Trigger: 'click',
            targetTextSelector: `#${id}`,
            targetDateSelector: `#${id}_Val`,
            ToDate: true,
            EnableTimePicker: false,
            Placement: 'right',
            Format: "yyyy/MM/dd",
            GroupId: '',
            FromDate: false,
            DisableBeforeToday: false,
            Disabled: false,
            IsGregorian: false,
            EnglishNumber: false,
        });
    });

    if ($('.summernote').length > 0) {
        var myElement = $('.summernote');
        myElement.summernote({
            toolbar: [
                ['style', ['style']],
                ['font', ['bold', 'underline', 'clear']],
                ['color', ['color']],
                ['para', ['ul', 'ol', 'paragraph']],
                ['table', ['table']],
                ['insert', ['link', 'picture']],
                ['view', ['codeview', 'help']],
            ],
            callbacks: {
                onChange: function (contents, $editable) {
                    myElement.val(myElement.summernote('isEmpty') ? "" : contents);
                    $("form").validate().element(myElement);
                }
            }
        });
    }

    if ($('.select2').length > 0) {
        $.each($('.select2.ajax-load'), function (i, elem) {
            var url = $(this).data('url');
            $.ajax({
                url: url,
                type: 'get',
                dataType: 'json',
                success: function (jsonObject) {
                    $(elem).select2({ dir: "rtl", data: jsonObject });
                }
            });
        });
    }

   
});

const init = () => {
    var $loading = $('#loading-div').hide();
    $(document).ajaxStart(function () {
        $loading.show();
    }).ajaxStop(function () {
        $loading.hide();
    }).ajaxError(function (event, jqxhr, settings, thrownError) {
        toastr.error("خطایی رخ داده است.");
    });
};

$(document).on('click', 'ul.pagination li a', function () {
    if ($(this).parent().hasClass('active') || $(this).parent().hasClass('disabled'))
        return false;
    var page = $(this).attr('href').toLowerCase().match(/current=(.*)/)[1];
    $('#ThisPageIndex').val(page);
    SubmitAdvancedSearch();
    return false;
});

$('.btn-delete-row').click(function (e) {
    e.preventDefault();
    var id = $(this).data('id');
    $('#delete-id').val(id);
    $('#modal-delete').modal('show');
});

function DoAdvancedSearch() {
    $('#ThisPageIndex').val('1');
    SubmitAdvancedSearch();
}

function SubmitAdvancedSearch() {
    $('#AdvancedSearchForm').submit();
}


function ClearFilters() {
    $('#AdvancedSearchForm .row input').val('');
    $("#AdvancedSearchForm .row select option:selected").prop("selected", false);
    $('#AdvancedSearchBox .card-header .card-tools button').trigger('click');
    DoAdvancedSearch();
}

$('#AdvancedSearchBox input[type="text"],#AdvancedSearchBox input[type="number"]').keypress(function (e) {
    if (e.which == 13) {
        SubmitAdvancedSearch();
        return false;
    }
});

function openSearchboxIfNeeded() {
    var texts = $('#AdvancedSearchBox').find('input[type="text"]').filter(function () { return $(this).val().trim() != ""; }).length;
    var numbers = $('#AdvancedSearchBox').find('input[type="numbers"]').filter(function () { return $(this).val().trim() != ""; }).length;
    var selects = $('#AdvancedSearchBox').find('select:enabled').filter(function () { return $(this).val().trim() != ""; }).length;
    var filledInputs = texts + selects + numbers;
    if (filledInputs > 0) {
        $("#AdvancedSearchBox").removeClass('collapsed-card');
        $("#AdvancedSearchBox .card-header .card-tools button i").removeClass('fa-plus').addClass('fa-minus');
        $("#AdvancedSearchBox .card-body").show();
    }
}

function markCurrentSort() {
    var currentOrder = $('#PageOrder').val();
    var currentOrderBy = $('#PageOrderBy').val();
    var icon = `<i class="fa fa-sort-${currentOrder == "asc" ? "up" : "down"} mr-2"></i>`

    $(`.AdvancedGrid label[for=searchModel_${currentOrderBy}]`).append(icon);
}

$(document).on('click', '.AdvancedGrid th.sortable',function () {
    var currentOrder = $('#PageOrder').val();
    var currentOrderBy = $('#PageOrderBy').val();
    var split = $(this).find('label').attr('for').split('_');
    var thisOrderBy = split[1];
    if (thisOrderBy == currentOrderBy) {
        $('#PageOrder').val(currentOrder === "asc" ? "desc" : "asc");
    }
    else {
        $('#PageOrderBy').val(thisOrderBy);
        $('#PageOrder').val("desc");
    }
    SubmitAdvancedSearch();
});

$(document).on('click', '.StatusBtn', function () {
    var data = {
        id: $(this).data('id')
    };
    var handler = $(this).data('handler');
    AjaxSubmit(handler, data, changeStatusHtml);
    return false;
});

function changeStatusHtml(e, handler) {
    if (e.success) {
        SubmitAdvancedSearch();
    }
    else {
        toastr.error(e.message);
    }
}


$('#delete-row-button').click(function () {
    var data = {
        id : $('#delete-id').val()    
    };
    AjaxSubmit('Delete', data, removeRowHtml);
});

function removeRowHtml(e, handler) {
    if (e.success) {
        $(`.AdvancedGrid tr[data-id="${e.id}"]`).fadeOut(300, function () { $(this).remove(); });
    }
    else {
        toastr.error(e.message);
    }
    $('#modal-delete').modal('hide');
}

function AjaxSubmit(handler, data, successCallback) {
    url = `?handler=${handler}`;
    $.ajax({
        type: 'POST',
        url: url,
        headers:
        {
            "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val()
        },
        data: data
    }).fail(function (e) {
        console.log('fail: ', e);
    }).done(function (e) {
        successCallback(e, handler);
    });
}

ajaxOnSuccess = (response) => {
    if (!response.success) {
        toastr.error(response.message);
        return;
    }

    var message = response.message;
    if (message == undefined || message == null || message == '')
        message = 'عملیات با موفقیت انجام شد.';
    toastr.success(message);
    setTimeout(function () {
        location.href = response.url;
    }, 500);
};