namespace Selkie.Services.Monitor.Configuration
{
    public interface ISelkieProcess
    {
        bool IsUnknown { get; }
        void Start();
    }
}