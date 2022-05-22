using Runner.Scripts.Manager;
using UnityEngine;

namespace Runner.Scripts.Inputter
{
    public class Inputter : MonoBehaviour
    {
        public delegate void OnInputDetected(InputAction inputAction);
        public event OnInputDetected OnInput;

        public void InputDetected(InputAction inputAction) => OnInput(inputAction);

        private void Start()
        {
            InputManager.Instance.RegisterInputter(this);
        }

        private void OnDestroy()
        {
            InputManager.Instance.DeregisterInputter(this);
        }
    }
}