using System;

namespace Core.FilterAttributes
{
    public class SearchAttribute : Attribute
    {
        public SearchAttribute(SearchFieldType type = SearchFieldType.String)
        {
            Type = type;
        }
        public SearchFieldType Type { get; private set; }
    }
}
