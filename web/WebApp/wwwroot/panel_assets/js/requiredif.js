jQuery.validator.unobtrusive.adapters.add("requiredif", ["otherPropertyName", "otherPropertyValue"], function (options) {
    options.rules["requiredif"] = options.params;
    if (options.message) {
        options.messages["requiredif"] = options.message;
    }
});

jQuery.validator.addMethod("requiredif", function (value, element, params) {

    var currentElementValue = $(element).val();
    var checkcurrentElementValue = Array.isArray(currentElementValue);

    var $form = $(element).closest("form");

    var name = params.otherPropertyName;

    if (element.name.lastIndexOf('.') > -1) {
        //when form has a multipart name seperated by dots
        name = element.name.substring(0, element.name.lastIndexOf('.')) + "." + params.otherPropertyName
    }

    var $otherElement = $form.find("[name='" + name + "']");

    var otherElementValue = $($otherElement).val();

    if ($otherElement.length > 0) {
        var $item = $($otherElement[0]);
        if ($item.attr("type") == "checkbox") {
            otherElementValue = $item.is(":checked").toString();
        }
    }

    if (checkcurrentElementValue == true && currentElementValue.length == 0 && otherElementValue === params.otherPropertyValue.toLowerCase()) {
        return false;
    }

    else if (currentElementValue === "" && otherElementValue === params.otherPropertyValue.toLowerCase()) {
        return false;
    }

    return true;
}, '');