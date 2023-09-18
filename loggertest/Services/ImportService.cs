using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace loggertest.Services
{
    internal class ImportService : IImportService
    {
        private readonly ILogger<ImportService> _logger;
        private readonly IConfiguration _configuration;

        public ImportService(ILogger<ImportService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public void Import()
        {
            var value = _configuration.GetSection("Stackify").GetValue<string>("ApiKey");
            var rand = new Random();

            var filename = $"file_{rand.Next(10000, 99000)}";

            _logger.LogInformation("Starting import of file {fileName}", filename);

            if (rand.Next(0, 10) < 3)
            {
                var failedrows = rand.Next(1, 30);

                _logger.LogInformation("Validation failed for {fileName}. Number of invalid entries: {invalidEntries}", filename, failedrows);
            }

            _logger.LogInformation("Validation of {fileName} is complete", filename);

            var nrOfItems = rand.Next(50, 1000);

            _logger.LogInformation("{fileName} contains {nrOfEntries}", filename, nrOfItems);

            _logger.LogInformation("Starting import {fileName} with {nrOfEntries} entries", filename, nrOfItems);

            if (rand.Next(0, 20) < 1)
            {
                var error = $"XH12345{rand.Next(10, 20)}";
                _logger.LogError("Import of {fileName} has failed with error {error}", filename, error);
            }

            if (rand.Next(0, 20) < 1)
            {
                throw new ArgumentNullException($"Could not parse the data of file {filename}");
            }

            var elasped = rand.Next(10000, 20000);

            _logger.LogInformation("Finished import {fileName} with {nrOfEntries} entries, elapsed {elapsed}", filename, nrOfItems, elasped);
        }
    }
}
