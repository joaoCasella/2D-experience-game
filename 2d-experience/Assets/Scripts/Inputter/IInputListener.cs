namespace Runner.Scripts.Inputter
{
    public interface IInputListener
    {
        public InputListenerPriority Priority { get; }

        bool ConsumeInput(InputAction inputAction);
    }
}