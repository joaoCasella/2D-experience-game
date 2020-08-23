using UnityEngine;

namespace Runner.Scripts.Controller
{
    public class BackgroundController : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer _backgroundSprite = null;

        // Start is called before the first frame update
        void Awake()
        {
            ResizeBackgroundToFillScreen();
        }

        // Solution based on the thread: https://answers.unity.com/questions/620699/scaling-my-background-sprite-to-fill-screen-2d-1.html
        // Acessed in: 22/08/2020
        private void ResizeBackgroundToFillScreen()
        {
            var sprite = _backgroundSprite.sprite.bounds.size;
            var worldScreenHeight = Camera.main.orthographicSize * 2;
            var worldScreenWidth = worldScreenHeight * Screen.width / Screen.height;

            _backgroundSprite.transform.localScale = Vector2.one * Mathf.Max(worldScreenWidth / sprite.x, worldScreenHeight / sprite.y);
        }
    }
}
