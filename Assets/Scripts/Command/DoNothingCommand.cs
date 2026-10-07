namespace Command
{
    public class DoNothingCommand : ICommand
    {
        public void Execute()
        {
            // Do absolutely nothing
        }

        public void Undo()
        {
            // Do absolutely nothing
        }
    }
}
