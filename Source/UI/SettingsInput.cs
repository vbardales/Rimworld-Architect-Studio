namespace ArchitectStudio
{
    /// <summary>Validation for player-entered group/category names.</summary>
    public static class SettingsInput
    {
        public static string NormalizeName(string value) => (value ?? "").Trim();
    }
}
