using Runner.Scripts.Inputter;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Runner.Scripts.Manager
{
    public class InputManager : MonoBehaviour
    {
        private List<IInputListener> InputListeners { get; } = new List<IInputListener>();

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
            inputter.OnInput += ConsumeOnInput;
        }

        public void DeregisterInputter(Inputter.Inputter inputter)
        {
            inputter.OnInput -= ConsumeOnInput;
        }

        public void ConsumeOnInput(InputAction inputAction)
        {
            for (int listenerIndex = 0; listenerIndex < InputListeners.Count && !InputListeners[listenerIndex].ConsumeInput(inputAction); listenerIndex++);
        }

        public void RegisterInputListener(IInputListener inputListener)
        {
            if (InputListeners.Count == 0)
            {
                InputListeners.Add(inputListener);
                return;
            }
            var countItemsMoreImportant = InputListeners.TakeWhile(il => il.Priority > inputListener.Priority).Count();
            InputListeners.Insert(countItemsMoreImportant, inputListener);
        }

        public void DeregisterInputListener(IInputListener inputListener)
        {
            if (InputListeners.Contains(inputListener))
                InputListeners.Remove(inputListener);
        }
    }
}
