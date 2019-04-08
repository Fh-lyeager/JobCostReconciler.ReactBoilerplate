namespace JobCostReconciliation.Interfaces.Services
{
    public interface IDateService
    {
        bool DatesAreEqual(object sapphireDate, object pervasiveDate, bool compareTimes = true);
        string FormatDateForCompare(object dateObject, bool compareTimes = false);
        string FormatDateForPervasive(object dateValue);
        string FormatTimeForPervasive(object dateValue);
        bool IsNullSapphireDate(object dateValue);
    }
}
