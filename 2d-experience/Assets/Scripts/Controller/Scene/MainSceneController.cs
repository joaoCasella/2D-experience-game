using Runner.Scripts.Manager;
using UnityEngine;

namespace Runner.Scripts.Controller.Scene
{
    public class MainSceneController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private LevelManager _levelManager = null;

        private void Awake()
        {
            Camera.main.orthographicSize *= GameManager.cameraScaleFactor;
            _levelManager.Setup();
        }
    }
}
