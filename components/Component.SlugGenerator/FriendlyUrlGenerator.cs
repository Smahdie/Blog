using System;
using System.Text;

namespace Component.SlugGenerator
{
    public static class FriendlyUrlGenerator
    {
        public static string GetSlug(this string value, bool toLower = true)
        {
            if (value == null) return string.Empty;
            string normalised;
            try
            {
                normalised = value.Normalize(NormalizationForm.FormKD);
            }
            catch (Exception)
            {
                normalised = value;
            }

            const int maxlen = 80;
            int len = normalised.Length;
            bool prevDash = false;
            var sb = new StringBuilder(len);
            char c;

            for (int i = 0; i < len; i++)
            {
                c = normalised[i];
                if ((c >= 'ا' && c <= 'ی') || (c >= '۰' && c <= '۹') || (c >= '0' && c <= '9') || (c >= 'a' && c <= 'z'))
                {
                    if (prevDash)
                    {
                        sb.Append('-');
                        prevDash = false;
                    }
                    sb.Append(c);
                }
                else if (c >= 'A' && c <= 'Z')
                {
                    if (prevDash)
                    {
                        sb.Append('-');
                        prevDash = false;
                    }
                    
                    if (toLower)
                        sb.Append((char)(c | 32));
                    else
                        sb.Append(c);
                }
                else if (c == ' ' || c == ',' || c == '.' || c == '#' || c == '/' || c == '\\' || c == '-' || c == '_' || c == '=')
                {
                    if (!prevDash && sb.Length > 0)
                    {
                        prevDash = true;
                    }
                }
                else
                {
                    string swap = ConvertEdgeCases(c, toLower);

                    if (swap != null)
                    {
                        if (prevDash)
                        {
                            sb.Append('-');
                            prevDash = false;
                        }
                        sb.Append(swap);
                    }
                }

                if (sb.Length == maxlen) break;
            }

            return sb.ToString();
        }

        static string ConvertEdgeCases(char c, bool toLower)
        {
            string swap = null;
            switch (c)
            {
                case 'i':
                    swap = "i";
                    break;
                case 'l':
                    swap = "l";
                    break;
                case 'L':
                    swap = toLower ? "l" : "L";
                    break;
                case 'd':
                    swap = "d";
                    break;
                case 'ß':
                    swap = "ss";
                    break;
                case 'ø':
                    swap = "o";
                    break;
                case 'Þ':
                    swap = "th";
                    break;
                case 'ي':
                    swap = "ی";
                    break;
                case 'ك':
                    swap = "ک";
                    break;
            }
            return swap;
        }
    }
}
