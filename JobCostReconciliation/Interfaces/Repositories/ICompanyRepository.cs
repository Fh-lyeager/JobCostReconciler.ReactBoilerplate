namespace JobCostReconciliation.Interfaces.Repositories
{
    public interface ICompanyRepository
    {
        string GetCompanyByJob(string jobNumber);
    }
}