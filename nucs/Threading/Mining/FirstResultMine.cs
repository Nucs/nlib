using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace nucs.Threading {
    public class FirstResultMine<IN, OUT> {
        public FirstResultMine(MiningFactory<IN, OUT> factory) {
            Factory = factory;
        }

        public MiningFactory<IN, OUT> Factory { get; set; }

        public Syncer<OUT> StartMining(CancellationTokenSource src) {
            var m = new Syncer<OUT>();

            var _funcs = Factory.CreateMiners(src?.Token ?? CancellationToken.None).ToArray();
            long c = _funcs.Length;
            foreach (var func in _funcs)
                System.Threading.Tasks.Task.Factory.StartNew(() => {
                    var res = default(OUT);
                    try {
                        src?.Token.ThrowIfCancellationRequested();
                        res = func();
/*#if DEBUG*/
                        } catch (Exception e) {
                            Console.WriteLine(e);
/*#else
                        } catch {
#endif*/
                    } finally {
                        Interlocked.Decrement(ref c);
                        if (!m.IsSet && (src?.IsCancellationRequested==true || Interlocked.Read(ref c) == 0)) {
                            m.Set(default(OUT));
                            src?.Cancel();
                        }
                    }
                    lock (m.Root)
                        if (!m.IsSet && !(res != null && res.Equals(default(OUT)))) {
                            m.Set(res);
                            src?.Cancel();
                        }
                }, src?.Token ?? CancellationToken.None);
            return m;
        }
    }
}