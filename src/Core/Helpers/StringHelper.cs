namespace Core.Helpers
{
    public static class StringHelper
    {
        public static string ReplaceIfEmpty(this string src, string replacement = "-----")
        {
            if (string.IsNullOrWhiteSpace(src))
                return replacement;

            return src.Trim();
        }

        public static string SubString(this string src, int maxLength)
        {
            if (string.IsNullOrWhiteSpace(src))
                return src;

            if (src.Length <= maxLength)
                return src;

            src = src.Remove(maxLength);
            return src + "...";
        }

        public static int? GetNormalizedNumber(string number)
        {
            if (string.IsNullOrWhiteSpace(number))
                return null;

            number = number.Trim();

            number = WithEnglishDigits(number);

            if (int.TryParse(number, out int result))
            {
                return result;
            }

            return null;
        }

        public static string WithEnglishDigits(string src)
        {
            if (string.IsNullOrWhiteSpace(src))
            {
                return src;
            }
            string englishNumber = "";
            foreach (char ch in src)
            {
                englishNumber += char.GetNumericValue(ch);
            }
            return englishNumber;
        }

        public static string GetNormalizedString(string input)
        {
            if (input == null)
                return input;
            input = input.Trim();

            input = input.Replace("ي", "ی").Replace("ك", "ک");
            return input;
        }
    }
}
