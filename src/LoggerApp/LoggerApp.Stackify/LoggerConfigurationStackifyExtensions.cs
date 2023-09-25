using System;
using LoggerApp.Stackify;
using Serilog.Configuration;
using Serilog.Events;

namespace Serilog
{
    /// <summary>
    /// Adds the WriteTo.Stackify() extension method to <see cref="LoggerConfiguration"/>.
    /// </summary>
    public static class LoggerConfigurationStackifyExtensions
    {
        /// <summary>
        /// Adds a sink that writes log events to the elmah.io webservice.
        /// </summary>
        /// <param name="loggerConfiguration">The logger configuration.</param>
        /// <param name="formatProvider">Supplies culture-specific formatting information, or null.</param>
        /// <param name="restrictedToMinimumLevel">The minimum log event level required in order to write an event to the sink. Set to Verbose by default.</param>
        /// <returns>Logger configuration, allowing configuration to continue.</returns>
        /// <exception cref="ArgumentNullException">A required parameter is null.</exception>
        public static LoggerConfiguration Stackify(
            this LoggerSinkConfiguration loggerConfiguration,
            string apiKey,
            IFormatProvider formatProvider = null,
            LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum
            )
        {
            if (loggerConfiguration == null) throw new ArgumentNullException("loggerConfiguration");

            return loggerConfiguration
                .Sink(new StackifySink(apiKey, formatProvider), restrictedToMinimumLevel);
        }
    }
}
