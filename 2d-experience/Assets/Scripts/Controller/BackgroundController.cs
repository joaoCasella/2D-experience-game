using Runner.Scripts.Manager;
using UnityEngine;

namespace Runner.Scripts.Controller
{
    public class BackgroundController : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer _backgroundSprite = null;

        public void SetupBackgroundSize()
        {
            var sprite = _backgroundSprite.sprite.bounds.size;
            var expandScale = 2f * Mathf.Max(GameManager.halfHorizontalScreenSize / sprite.x, GameManager.halfVerticalScreenSize / sprite.y);
            _backgroundSprite.transform.localScale = Vector2.one * expandScale;
        }
    }
}
