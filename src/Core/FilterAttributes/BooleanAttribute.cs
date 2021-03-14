using System;

namespace Core.FilterAttributes
{
    public class BooleanAttribute : Attribute
    {
        public BooleanAttribute(string TrueText = "فعال", string FalseText = "غیر فعال", string TrueBadge = "badge-success", string FalseBadge = "badge-secondary")
        {
            this.TrueText = TrueText;
            this.FalseText = FalseText;
            this.TrueBadge = TrueBadge;
            this.FalseBadge = FalseBadge;
        }

        public string TrueText { get; private set; }
        public string FalseText { get; private set; }
        public string TrueBadge { get; private set; }
        public string FalseBadge { get; private set; }
    }
}
