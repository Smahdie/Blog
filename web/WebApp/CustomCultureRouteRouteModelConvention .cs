using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System.Collections.Generic;
using System.Linq;

namespace WebApp
{
    public class CustomCultureRouteRouteModelConvention : IPageRouteModelConvention
    {
        public void Apply(PageRouteModel model)
        {
            var selectorModels = new List<SelectorModel>();
            var toRemove = new List<SelectorModel>();

            foreach (var selector in model.Selectors.ToList())
            {
                var template = selector.AttributeRouteModel.Template;

                if (!template.Contains("Admin"))
                {
                    if (template != "Index" && template != string.Empty)
                    {
                        toRemove.Add(selector);
                    }

                    selectorModels.Add(new SelectorModel()
                    {
                        AttributeRouteModel = new AttributeRouteModel
                        {
                            Template = "/{culture}/" + template
                        }
                    });
                }                
            }

            foreach (var item in toRemove)
            {
                model.Selectors.Remove(item);
            }

            foreach (var m in selectorModels)
            {
                model.Selectors.Add(m);
            }
        }
    }
}
