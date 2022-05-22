using Runner.Scripts.Controller;
using System;
using UnityEngine;

namespace Runner.Scripts.Manager
{
    public class PlayerManager : MonoBehaviour
    {
        [field: SerializeField]
        private PlayerController Player { get; set; }

        private GameObject CurrentPlayer { get; set; }

        public void SetupPlayerOnScene()
        {
            CurrentPlayer = Instantiate(
                Player.gameObject,
                new Vector2(
                    -GameManager.Instance.HalfHorizontalScreenSize + FloorManager.FloorSize.x - Player.PositionOffset.x,
                    -GameManager.Instance.HalfVerticalScreenSize - Player.PositionOffset.y + FloorManager.FloorSize.y + Player.Size.y * 0.5f
                ),
                Quaternion.identity
            );
        }

        public void KillPlayer(Action onComplete)
        {
            CurrentPlayer.GetComponent<PlayerController>().OnDeath(onComplete);
        }
    }
}
