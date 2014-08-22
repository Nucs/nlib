using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nucs.Collections {
    
    //Tallseth's method
    public static class EnumerationPartitioner {

        /// <summary>
        /// Pulls <paramref name="batchSize"/> every loop request.
        /// </summary>
        public static IEnumerable<IEnumerable<T>> PartitionInto<T>(this IEnumerable<T> source, int batchSize) {
            List<T> batch = null;
            foreach (var item in source)
            {
                if (batch == null)
                    batch = new List<T>();

                batch.Add(item);

                if (batch.Count != batchSize)
                    continue;

                yield return batch;
                batch = null;
            }

            if (batch != null)
                yield return batch;
        }
    }


    //MarcinJuraszek's method
    public class EnumerationPartitioner<T> : IEnumerable<T> {

        /// <summary>
        /// Has the enumeration ended?
        /// </summary>
        public bool Ended {
            get { return over; }
        }

        public IEnumerator<T> Enumerator { get; private set; }

        public EnumerationPartitioner(IEnumerator<T> _enum) {
            Enumerator = _enum;
        }

        /// <summary>
        /// Has the enumeration ended
        /// </summary>
        private bool over = false;

        /// <summary>
        /// Items that were pulled from the <see cref="Enumerator"/>
        /// </summary>
        private int n = 0;

        /// <summary>
        /// Pulls <paramref name="count"/> items out of the <see cref="Enumerator"/>.
        /// </summary>
        /// <param name="count">Number of items to pull out the <see cref="Enumerator"/></param>
        public List<T> Pull(int count) {
            var l = new List<T>();
            if (over) return l;
            for (int i = 0; i < count; i++, n++) {
                if ((Enumerator.MoveNext()) == false) {
                    over = true;
                    return l;
                }
                l.Add(Enumerator.Current);
            }
            return l;
        }

        /// <summary>
        /// Resets the Enumerator and clears internal counters, use this over manual reset
        /// </summary>
        public void Reset() {
            n = 0;
            over = false;
            Enumerator.Reset();
        }


        public IEnumerator<T> GetEnumerator() {
            return Enumerator;
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return Enumerator;
        }
    }
}
