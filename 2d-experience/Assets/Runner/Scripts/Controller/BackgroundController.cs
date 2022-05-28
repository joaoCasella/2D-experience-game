using Runner.Scripts.Manager;
using UnityEngine;

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
            SoundManager.Instance.RegisterMusicSource(Domain.MusicType.Gameplay, AudioSource);
        }

        public void SetupBackgroundSize()
        {
            var sprite = BackgroundSprite.sprite.bounds.size;
            var expandScale = 2f * Mathf.Max(GameManager.Instance.HalfHorizontalScreenSize / sprite.x, GameManager.Instance.HalfVerticalScreenSize / sprite.y);
            BackgroundSprite.transform.localScale = Vector2.one * expandScale;
        }

        private void OnDestroy()
        {
            SoundManager.Instance.DeregisterMusicSource(Domain.MusicType.Gameplay, AudioSource);
        }
    }
}
