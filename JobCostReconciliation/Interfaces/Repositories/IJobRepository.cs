namespace JobCostReconciliation.Interfaces.Repositories
{
    public interface IJobRepository
    {
        int GetHomeRIDByJobNumber(string jobNumber);
    }
}