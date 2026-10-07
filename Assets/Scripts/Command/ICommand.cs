namespace Command
{
    public interface ICommand
    {
        // All classes that implement this interface must provide an implementation for the methods defined in the interface. 
        public void Execute();
        public void Undo();
    }
}
