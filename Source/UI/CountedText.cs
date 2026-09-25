using Verse;

namespace ArchitectStudio
{
    /// <summary>
    /// A phrase that counts something is a family of keys chosen by the count (TRANSLATIONS.md, "Counts and
    /// plurals"): <c>&lt;key&gt;.Zero</c>, <c>&lt;key&gt;.One</c> and <c>&lt;key&gt;.Many</c>, the count being {0}.
    /// No suffix is added to a translated word and the engine's Pluralize is never called: the French worker
    /// pluralizes whatever the count, and French reads 0 as a singular, so each language writes its own forms.
    /// </summary>
    public static class CountedText
    {
        public static string KeyFor(string key, int count)
        {
            return key + (count == 0 ? ".Zero" : count == 1 ? ".One" : ".Many");
        }

        public static TaggedString Get(string key, int count)
        {
            return KeyFor(key, count).Translate(count);
        }

        /// <summary>The count is {0}, the extra argument {1}.</summary>
        public static TaggedString Get(string key, int count, NamedArgument arg1)
        {
            return KeyFor(key, count).Translate(count, arg1);
        }
    }
}
