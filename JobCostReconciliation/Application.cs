using JobCostReconciliation.Interfaces;
using JobCostReconciliation.Interfaces.Services;

namespace JobCostReconciliation
{
    public class Application
    {
        private readonly IConsoleLogger _logger;
        private readonly IReconciler _reconciler;
        private readonly IServiceLog _serviceLog;

        public Application(IConsoleLogger logger, IReconciler reconciler, IServiceLog serviceLog)
        {
            _logger = logger;
            _reconciler = reconciler;
            _serviceLog = serviceLog;
        }

        public void Run()
        {
            _logger.Info(nameof(Application) + " started.");

            //_reconciler.RunApp();

            _logger.Info(nameof(Application) + " finished.");
        }
    }
}
