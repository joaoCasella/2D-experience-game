using System;
using UnityEngine.Localization.Metadata;

namespace Runner.Scripts.Localization
{
    [Metadata(AllowedTypes = MetadataType.Locale)] // Hint to the editor to only show this type for a Locale
    [Serializable]
    public class LocaleMetadata : IMetadata
    {
        public string displayName;
    }
}