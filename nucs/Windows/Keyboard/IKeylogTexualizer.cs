using System.Collections.Generic;
using nucs.Windows.Keyboard.Highlevel;
using ProtoBuf;

namespace nucs.Windows.Keyboard {
    [ProtoContract, ProtoInclude(100, typeof(KeyloggerTextualizer))]
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
        [ProtoMember(1)]
        List<ProcessLog> ActiveLogs { get; }
    }
}