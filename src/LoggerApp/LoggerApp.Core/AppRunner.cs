using loggertest.Services;
using Microsoft.Extensions.Logging;

namespace LoggerApp.Core
{
    public class AppRunner : IAppRunner
    {
        private readonly IImportService _importService;
        private readonly ILogger<AppRunner> _logger;

        public AppRunner(IImportService importService, ILogger<AppRunner> logger)
        {
            _importService = importService;
            _logger = logger;
        }

        public void Run()
        {
            for (int i = 0; i < 1000; i++)
            {
                try
                {
                    _importService!.Import();
                }
                catch (Exception ex)
                {
                    _logger.LogCritical(ex, "Fatal error");
                }
            }
        }
    }
}
