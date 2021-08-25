namespace Core.Dtos.Settings
{
    public static class CacheKeys
    {
        public static string ContactInfo(string lang) => $"ContactInfo_{lang}";

        public static string Page(string keyword, string lang) => $"Page_{keyword}_{lang}";

        public static string Menu(string keyword, string lang, bool nested) => $"Menu_{keyword}_{lang}_{nested}";
    }
}
