using UnityEngine.Localization.Settings;
using UnityEngine.Localization.SmartFormat.Extensions;
using UnityEngine.Localization.SmartFormat.PersistentVariables;

namespace Runner.Scripts.Service
{
    public static class LocalizationService
    {
        // (14/05/2022) Based on solution available at: https://phrase.com/blog/posts/localizing-unity-games-official-localization-package/
        public static void ChangeLocalizationValue<VariableType, ValueType>(string key, ValueType value) where VariableType : Variable<ValueType>
        {
            var source = LocalizationSettings.StringDatabase.SmartFormatter.GetSourceExtension<PersistentVariablesSource>();
            var valueSaved = source["global"][key] as VariableType;
            valueSaved.Value = value;
        }
    }
}