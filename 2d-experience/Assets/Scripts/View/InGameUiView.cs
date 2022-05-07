using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Runner.Scripts.View
{
    public class InGameUiView : Inputter.Inputter
    {
        [field: SerializeField]
        private EventTrigger BackgroundButton { get; set; }

        [field: SerializeField]
        private Button PauseButton { get; set; }

        [field: SerializeField]
        private TextMeshProUGUI PontuationText { get; set; }

        public void Setup(int initialPontuation)
        {
            UpdatePontuationText(initialPontuation);
            PauseButton.onClick.AddListener(OnClickPauseButton);
            var entry = new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerDown
            };
            entry.callback.AddListener(OnClickActionButton);
            BackgroundButton.triggers.Add(entry);
        }

        private void OnClickPauseButton()
        {
            InputDetected(Inputter.InputAction.BackOrPause);
        }

        private void OnClickActionButton(BaseEventData baseEventData)
        {
            InputDetected(Inputter.InputAction.Action);
        }

        public void UpdatePontuationText(int pontuation)
        {
            PontuationText.text = $"<mspace=1em>Points: {pontuation}</mspace>";
        }

        private void OnDestroy()
        {
            PauseButton.onClick.RemoveListener(OnClickPauseButton);
            BackgroundButton.triggers.RemoveAt(0);
        }
    }
}
