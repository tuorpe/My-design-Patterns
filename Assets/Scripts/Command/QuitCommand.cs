namespace Command
{
    public class QuitCommand : ICommand
    {
        public void Execute()
        {
            // Quit the application
            GameManager.Instance.OnReturnToMenu();
        }

        public void Undo()
        {
            // Do absolutely nothing
        }
    }
}
