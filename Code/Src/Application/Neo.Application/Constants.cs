namespace Neo.Application
{
    internal static class Constants
    {
        public static IReadOnlyCollection<string> InvalidCharacters
            => new List<string> { "$", "&", "!", "@", "#", "*", "%", "|", "{", "}",
                "(", ")", "[", "]", ";", "," };
    }
}
