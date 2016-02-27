using System.Collections.Generic;
using nucs.Windows.Keyboard.Highlevel;


namespace nucs.Windows.Keyboard {
    public interface IKeylogTexualizer {
        /// <summary>
        ///     Outputs the history to a string for a readable format.
        /// </summary>
        /// <returns></returns>
        string Output();

        /// <summary>
        ///     Clear logs history.
        /// </summary>
        void Clear();

        /// <summary>
        ///     The processes that log or have logged in the past history.
        /// </summary>
        
        List<ProcessLog> ActiveLogs { get; }
    }
}