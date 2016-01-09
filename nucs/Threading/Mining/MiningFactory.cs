using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace nucs.Threading {
    public abstract class MiningFactory<IN, OUT> {
        protected abstract OUT Mine(IN _o, CancellationToken token);
        /*
                public OUT Mine(CancellationToken token) {
                    return Mine(_o, token);
                }
        */

        public delegate OUT Miner(IN _in, CancellationToken token);
        public delegate OUT ReadyMiner();

        /// <summary>
        ///     Fetches the mines that are possible to mine in
        /// </summary>
        /// <returns></returns>
        public abstract IEnumerable<IN> Mines();

        /// <summary>
        ///     Called before mining, return true to start mining or false to abort mning and return default(OUT)
        /// </summary>
        /// <returns></returns>
        public virtual bool PriorToMining() {
            return true;
        }

        public IEnumerable<ReadyMiner> CreateMiners(CancellationToken token) {
            return Mines()?.Select(@in => new ReadyMiner(()=>Mine(@in, token))) ?? new ReadyMiner[0];
        }

        public OUT MineFirstOrDefault(int timeout = -1, CancellationTokenSource src=null) {
            return new FirstResultMine<IN,OUT>(this).StartMining(src).Wait(timeout, src?.Token ?? CancellationToken.None);
        }

    }
}