namespace JobCostReconciliation.Interfaces.Services
{
    public interface IConsoleLogger
    {
        void Error(string message);
        void Info(string message);
    }
}
