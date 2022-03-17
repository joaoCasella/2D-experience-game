using Runner.Scripts.Manager;
using UnityEngine;

namespace Runner.Scripts.Controller
{
    public class BackgroundController : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer _backgroundSprite = null;

        // Start is called before the first frame update
        void Start()
        {
            ResizeBackgroundToFillScreen();
        }

        // Solution based on the thread: https://answers.unity.com/questions/620699/scaling-my-background-sprite-to-fill-screen-2d-1.html
        // Acessed in: 22/08/2020
        private void ResizeBackgroundToFillScreen()
        {
            var sprite = _backgroundSprite.sprite.bounds.size;
            var expandScale = 2f * Mathf.Max(GameManager.halfHorizontalScreenSize / sprite.x, GameManager.halfVerticalScreenSize / sprite.y);
            _backgroundSprite.transform.localScale = Vector2.one * expandScale;
        }
    }
}
