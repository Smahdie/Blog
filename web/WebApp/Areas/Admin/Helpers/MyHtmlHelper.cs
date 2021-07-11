using Core.Dtos.CommonDtos;
using Core.FilterAttributes;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Admin.Helpers
{
    public static class MyHtmlHelper
    {
        public static Func<object, string> GetDisplayName = o =>
        {
            var result = null as string;
            var display = o.GetType().GetMember(o.ToString()).First().GetCustomAttributes(false).OfType<DisplayAttribute>().LastOrDefault();
            if (display != null)
                result = display.GetName();
            return result ?? o.ToString();
        };

        public static string EnumDisplayName(this Enum item)
        {
            var type = item.GetType();
            var member = type.GetMember(Enum.GetName(type, item));
            DisplayAttribute displayName = (DisplayAttribute)member[0].GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault();

            if (displayName != null)
            {
                return displayName.Name;
            }

            return item.ToString();
        }

        public static IHtmlContent StatusBadge(this IHtmlHelper htmlHelper, string id, bool? value, string handler= "ChangeStatus", string yesText = "فعال",string noText="غیر فعال", string yesBadge = "badge-success", string noBadge = "badge-secondary")
        {
            var template = "<a class=\"StatusBtn\" href=\"#\" data-id=\"{2}\" data-handler=\"{3}\" ><span class=\"badge {0}\">{1}</span></a>";
            if (value == true)
                return new HtmlString(string.Format(template, yesBadge, yesText, id, handler));
            if (value == false)
                return new HtmlString(string.Format(template, noBadge, noText, id, handler));
            return new HtmlString(string.Format(template, "badge-light", "نامشخص", id));
        }

        public static IHtmlContent RemoveLink(this IHtmlHelper htmlHelper, string id)
        {
            return new HtmlString($"<a href=\"#\" data-id=\"{id}\" title=\"حذف\" class=\"btn btn-danger btn-sm btn-delete-row\"><i class=\"fas fa-trash\"></i></a>");
        }

        public static IHtmlContent DetailsLink(this IHtmlHelper htmlHelper, string href)
        {
            return new HtmlString($"<a title=\"مشاهده\" class=\"btn btn-primary btn-sm\" href=\"{href}\"><i class=\"fas fa-folder\"></i></a>");
        }

        public static IHtmlContent EditLink(this IHtmlHelper htmlHelper, string href)
        {
            return new HtmlString($"<a title=\"ویرایش\" class=\"btn btn-info btn-sm\" href=\"{href}\"><i class=\"fas fa-pencil-alt\"></i></a>");
        }

        public static IHtmlContent MakeSearchForm(this IHtmlHelper htmlHelper, object o)
        {
            var baseModel = (BaseGridDto)o;

            var properties = o.GetType().GetProperties().Where(prop => prop.IsDefined(typeof(SearchAttribute), false))
                                .OrderBy(prop => prop.GetCustomAttributes(typeof(SearchAttribute), false).Cast<SearchAttribute>().SingleOrDefault().Type);
            var builder = new StringBuilder();

            var generalMarkupBegin = "<div class=\"col-md-4 mb-2\"><div class=\"form-group\" data-type=\"{0}\"><label class=\"col-md-4 col-form-label\" for=\"{1}\">{2}</label><div class=\"col-md-8\">";
            var generalMarkupEnd = "</div></div></div>";

            foreach (var myProperty in properties)
            {
                try
                {
                    var searchAttribute = myProperty.GetCustomAttributes(typeof(SearchAttribute), false).Cast<SearchAttribute>().SingleOrDefault();
                    if (searchAttribute == null)
                    {
                        continue;
                    }
                    var displayAttribute = myProperty.GetCustomAttributes(typeof(DisplayAttribute), false).Cast<DisplayAttribute>().SingleOrDefault();
                    if (displayAttribute == null)
                    {
                        continue;
                    }

                    var propertyName = myProperty.Name;
                    var propertyDisplayName = displayAttribute.Name;
                    string dataType = searchAttribute.Type.ToString().ToLower();
                    object propertyValue = myProperty.GetValue(o);


                    switch (dataType)
                    {
                        case "enum":
                            var t = myProperty.PropertyType;
                            if (t.IsGenericType)
                            {
                                t = myProperty.PropertyType.GenericTypeArguments[0];
                            }
                            var values = Enum.GetValues(t).Cast<object>()
                             .Select(v => new SelectListItem
                             {
                                 Selected = v.Equals(propertyValue),
                                 Text = GetDisplayName(v),
                                 Value = v.ToString()
                             });
                            builder
                                .Append(string.Format(generalMarkupBegin, dataType, propertyName, propertyDisplayName))
                                .Append("<select class=\"form-control\" name=\"")
                                .Append(propertyName)
                                .Append("\"><option value=\"\">همه</option>");
                            foreach (var v in values)
                            {
                                builder
                                    .Append("<option value='")
                                    .Append(v.Value)
                                    .Append("' ")
                                    .Append(v.Selected ? "selected" : string.Empty)
                                    .Append(" >")
                                    .Append(v.Text)
                                    .Append("</option>");
                            }
                            builder
                                .Append("</select>")
                                .Append(generalMarkupEnd); 
                            break;

                        case "boolean":
                            string trueText = "فعال", falseText = "غیر فعال";

                            if (myProperty.IsDefined(typeof(BooleanAttribute), false))
                            {
                                var booleanAttribute = myProperty.GetCustomAttributes(typeof(BooleanAttribute), false).Cast<BooleanAttribute>().SingleOrDefault();
                                trueText = booleanAttribute.TrueText;
                                falseText = booleanAttribute.FalseText;
                            }

                            builder
                                .Append(string.Format(generalMarkupBegin, dataType, propertyName, propertyDisplayName))
                                .Append("<select class=\"form-control\" name=\"")
                                .Append(propertyName)
                                .Append("\"><option value=\"\">همه</option><option value=\"true\"")
                                .Append((bool?)propertyValue == true ? " selected" : string.Empty)
                                .Append(">")
                                .Append(trueText)
                                .Append("</option><option value=\"false\"")
                                .Append((bool?)propertyValue == false ? " selected" : string.Empty)
                                .Append(">")
                                .Append(falseText)
                                .Append("</option></select>")
                                .Append(generalMarkupEnd);
                            break;

                        case "number":
                        case "string":
                            builder
                                .Append(string.Format(generalMarkupBegin, dataType, propertyName, propertyDisplayName))
                                .Append("<input type=\"")
                                .Append(dataType == "number" ? dataType : "text")
                                .Append("\" class=\"form-control\" id=\"")
                                .Append(propertyName)
                                .Append("\" name=\"")
                                .Append(propertyName)
                                .Append("\" placeholder=\"")
                                .Append(displayAttribute.Name)
                                .Append("\" value=\"")
                                .Append(propertyValue)
                                .Append("\" />")
                                .Append(generalMarkupEnd);
                            break;

                        case "datetime":
                            builder
                                .Append(string.Format(generalMarkupBegin, dataType, propertyName, propertyDisplayName))
                                .Append("<div class=\"input-group\"><input type=\"text\" class=\"form-control\" id=\"")
                                .Append(propertyName)
                                .Append("\" placeholder=\"")
                                .Append(displayAttribute.Name)
                                .Append("\" /><input type=\"hidden\" name=\"")
                                .Append(propertyName)
                                .Append("\" id=\"")
                                .Append(propertyName)
                                .Append("_Val\" value=\"")
                                .Append(propertyValue)
                                .Append("\" /></div>")
                                .Append(generalMarkupEnd);
                            break;
                        default:
                            break;
                    }
                }
                catch { }
            }
            return new HtmlString(builder.ToString());
        }
    }
}
