using Runner.Scripts.Manager;
using UnityEngine;
using UnityEngine.Serialization;

namespace Runner.Scripts.Controller
{
    public class BackgroundController : MonoBehaviour
    {
        [field: SerializeField]
        private SpriteRenderer BackgroundSprite { get; set; }

        [field: SerializeField]
        private AudioSource AudioSource { get; set; }

        private void Awake()
        {
            SoundManager.Instance.RegisterMusicSource(AudioSource);
        }

        public void SetupBackgroundSize()
        {
            var sprite = BackgroundSprite.sprite.bounds.size;
            var expandScale = 2f * Mathf.Max(GameManager.halfHorizontalScreenSize / sprite.x, GameManager.halfVerticalScreenSize / sprite.y);
            BackgroundSprite.transform.localScale = Vector2.one * expandScale;
        }

        private void OnDestroy()
        {
            SoundManager.Instance.DeregisterMusicSource(AudioSource);
        }
    }
}
