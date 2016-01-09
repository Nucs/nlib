using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace nucs.Threading {
    public class MultiSourceSingleResultMiner<OUT> {
 /*       public List<Task<OUT>> _tasks = new List<Task<OUT>>();


        public MultiSourceSingleResultMiner(MiningFactory<> ) : this(Factory.CreateMiningFuncs()) {
            
        }

        public MultiSourceSingleResultMiner(IEnumerable<Func<OUT>> funcs) {
            var _funcs = funcs.ToArray();
            long c = _funcs.Length;
            foreach (var func in _funcs) {
                Task.Run(() => {
                    OUT res = default(OUT);
                    try {
                        res = func();
                    } catch {
                        
                    } finally {
                        Interlocked.Decrement(ref c);
                        if (Interlocked.Read(ref c)==0)
                            m.Set(default(OUT));
                    }
                lock (m.Root)
                    if (!m.IsSet && !(res != null && res.Equals(default(OUT)))) {
                        m.Set(res);
                    }
                });
            }
        }

        public OUT Result(int timeout = -1) {
            return m.Wait(timeout);
        }*/

    }
}