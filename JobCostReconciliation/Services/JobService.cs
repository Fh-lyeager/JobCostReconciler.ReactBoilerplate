using JobCostReconciliation.Interfaces.Repositories;
using JobCostReconciliation.Interfaces.Services;
using JobCostReconciliation.Data.Repositories;
using System;

namespace JobCostReconciliation.Services
{
    public class JobService : IJobService
    {
        private readonly IJobRepository _jobRepository;

        public JobService()
        {
            _jobRepository = new JobRepository();
        }

        public JobService(IJobRepository jobRepository)
        {
            _jobRepository = jobRepository;
        }

        public string FormatJobNumber(string jobNumber)
        {
            string communityCode = ParseCommunityCode(jobNumber);
            string productCode = ParseProductCode(jobNumber);
            string buildingCode = ParseBuildingCode(jobNumber);
            string unitCode = ParseUnitCode(jobNumber);

            return String.Format("{0}{1}/{2}/{3}",communityCode, productCode, buildingCode, unitCode);
        }

        private string ParseCommunityCode(string jobNumber)
        {
            return jobNumber.Substring(0, 3);
        }

        private string ParseProductCode(string jobNumber)
        {
            return jobNumber.Substring(3, 2);
        }

        private string ParseBuildingCode(string jobNumber)
        {
            return jobNumber.Substring(5, 3);
        }

        private string ParseUnitCode(string jobNumber)
        {
            return jobNumber.Substring(8, 4);
        }

        public string GetJobNumberByHomeRID(int homeRID)
        {
            return _jobRepository.GetJobNumberByHomeRID(homeRID);
        }
    }
}
