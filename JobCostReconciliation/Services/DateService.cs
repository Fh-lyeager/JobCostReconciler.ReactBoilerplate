using JobCostReconciliation.Interfaces.Services;
using System;

namespace JobCostReconciliation.Services
{
    public class DateService : IDateService
    {
        private readonly IServiceLog _serviceLog;

        public DateService()
        {
            _serviceLog = new ServiceLog();
        }

        public DateService(IServiceLog serviceLog)
        {
            _serviceLog = serviceLog;
        }

        public bool DatesAreEqual(object sapphireDate, object pervasiveDate, bool compareTimes = true)
        {
            try
            {
                string sapphireDateStringToCompare = FormatDateForCompare(sapphireDate, compareTimes);
                string pervasiveDateStringToCompare = FormatDateForCompare(pervasiveDate, compareTimes);

                if (String.IsNullOrEmpty(sapphireDateStringToCompare) && String.IsNullOrEmpty(pervasiveDateStringToCompare)) return true;
                if (sapphireDateStringToCompare.Equals(pervasiveDateStringToCompare)) return true;

                return false;
            }
            catch (Exception ex)
            {
                _serviceLog.AppendLog(ex.Message, "", ex);
                return false;
            }
        }

        public string FormatDateForCompare(object dateObject, bool compareTimes = false)
        {
            try
            {
                DateTime formattedDate = (dateObject is null ? new DateTime(1900, 1, 1) : (DateTime)dateObject);
                return compareTimes ? formattedDate.ToString("yyyy-MM-dd H:mm:ss") : formattedDate.ToString("yyyy-MM-dd");
            }
            catch (Exception ex)
            {
                _serviceLog.AppendLog(ex.Message, "", ex);
                return null;
            }
        }

        public bool IsNullSapphireDate(object dateValue)
        {
            DateTime date = (DateTime)dateValue;
            if (date.ToString() == new DateTime(1900, 1, 1).ToString()) return true;

            return false;
        }

        public string FormatDateForPervasive(object dateValue)
        {
            DateTime date = (DateTime)dateValue;
            if (date.ToString() == new DateTime(1900, 1, 1).ToString()) return null;
            return date.ToString("yyyy-MM-dd");
        }


        public string FormatTimeForPervasive(object dateValue)
        {
            DateTime date = (DateTime)dateValue;
            if (date.ToString() == new DateTime(1900, 1, 1).ToString()) return null;
            return date.ToString("HH:mm:ss");
        }
    }
}
