using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nucs.Logger
{
    public interface IBaseLogging {

        /// <summary>
        ///     Merges the text with string.join("",text) and then prints it into the logger.
        /// </summary>
        /// <param name="text">The strings that will be joined without space</param>
        void Log(params string[] text);

        /// <summary>
        ///     Log with timestamp, calls <see cref="Log"/> after preparing a timestamp.
        ///     Recommanded format is [DATE] text
        /// </summary>
        /// <param name="text">The strings that will be joined without space</param>
        void LogTimed(params string[] text);


        /// <summary>
        ///     Clears the logs
        /// </summary>
        /// <returns>Number of logs cleared.</returns>
        int ClearLogs();
    }
}
