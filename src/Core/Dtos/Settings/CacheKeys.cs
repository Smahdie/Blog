namespace Core.Dtos.Settings
{
    public static class CacheKeys
    {
        public static string ContactInfo() => $"ContactInfo";

        public static string Page(string keyword) => $"Page_{keyword}";

        public static string Menu(string keyword, bool nested) => $"Menu_{keyword}_{nested}";
    }
}
