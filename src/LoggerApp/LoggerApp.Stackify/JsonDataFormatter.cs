using System;
using System.Collections.Generic;
using System.IO;
using Serilog.Events;
using Serilog.Formatting.Json;

namespace Serilog
{
    public class JsonDataFormatter : JsonFormatter
    {
        public void FormatData(LogEvent logEvent, TextWriter output)
        {
            if (output == null)
                throw new ArgumentNullException("output");

            output.Write("{");

            var delim = "";

            if (logEvent.Properties.Count != 0)
                WriteProperties(logEvent.Properties, ref delim, output);

            if (logEvent.Exception != null)
                WriteException(logEvent.Exception, ref delim, output);

            output.Write("}");
        }

        protected void WriteProperties(IReadOnlyDictionary<string, LogEventPropertyValue> properties, ref string delim, TextWriter output)
        {
            output.Write("\"{0}\":{{", "Properties");
            WritePropertiesValues(properties, output);
            output.Write("}");

            delim = ",";
        }
    }
}

