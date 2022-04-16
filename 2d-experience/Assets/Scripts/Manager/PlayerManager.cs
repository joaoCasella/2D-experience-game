using Runner.Scripts.Controller;
using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Runner.Scripts.Manager
{
    public class PlayerManager : MonoBehaviour
    {
        [field: SerializeField]
        private PlayerController Player { get; set; }

        private GameObject currentPlayer;

        public void SetupPlayerOnScene()
        {
            currentPlayer = Instantiate(
                Player.gameObject,
                new Vector2(
                    -GameManager.halfHorizontalScreenSize + FloorManager.floorSize.x - Player.PositionOffset.x,
                    -GameManager.halfVerticalScreenSize - Player.PositionOffset.y + FloorManager.floorSize.y + Player.Size.y * 0.5f
                ),
                Quaternion.identity
            );
        }

        public void KillPlayer(Action onComplete)
        {
            currentPlayer.GetComponent<PlayerController>().OnDeath(onComplete);
        }
    }
}
