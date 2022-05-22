using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Runner.Scripts.View
{
    public class InGameUiView : Inputter.Inputter
    {
        [field: SerializeField]
        private AspectRatioFitter AspectRatioFitter { get; set; }

        [field: SerializeField]
        private EventTrigger BackgroundButton { get; set; }

        [field: SerializeField]
        private Button PauseButton { get; set; }

        public void Setup(float aspectRatio)
        {
            AspectRatioFitter.aspectRatio = aspectRatio;

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

        private void OnDestroy()
        {
            PauseButton.onClick.RemoveListener(OnClickPauseButton);
            BackgroundButton.triggers.RemoveAt(0);
        }
    }
}
