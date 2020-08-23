using Runner.Scripts.Controller;
using UnityEngine;

namespace Runner.Scripts.Manager
{
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField]
        private Transform _player = null;

        private GameObject currentPlayer;

        public void SetupPlayerOnScene()
        {
            Vector2 playerSize = _player.GetComponent<BoxCollider2D>().size;
            currentPlayer = Instantiate(
                _player.gameObject,
                new Vector2(
                    -LevelManager.horizontalScreenSize + 9f * playerSize.x,
                    -LevelManager.verticalScreenSize + FloorManager.floorSize.y * 2.5f + playerSize.y * 12f
                ),
                Quaternion.identity
            );
        }

        public void KillPlayer()
        {
            currentPlayer.GetComponent<PlayerController>().OnDeath();
        }
    }
}
