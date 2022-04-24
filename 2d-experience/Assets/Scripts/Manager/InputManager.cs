using Runner.Scripts.Inputter;
using UnityEngine;

namespace Runner.Scripts.Manager
{
    public class InputManager : MonoBehaviour
    {
        // Delegates
        public delegate void OnInputDetected();
        public event OnInputDetected OnInputAction;
        public event OnInputDetected OnInputPause;

        private static InputManager _instance;
        public static InputManager Instance => _instance;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (_instance != this)
            {
                Destroy(gameObject);
            }
        }

        public void RegisterInputter(Inputter.Inputter inputter)
        {
            inputter.OnInput += OnInput;
        }

        private void OnInput(InputAction inputAction)
        {
            switch (inputAction)
            {
                case InputAction.Action:
                    OnInputAction();
                    break;
                case InputAction.Pause:
                    OnInputPause();
                    break;
                default:
                    break;
            }
        }

        public void DeregisterInputter(Inputter.Inputter inputter)
        {
            inputter.OnInput -= OnInput;
        }
    }
}
