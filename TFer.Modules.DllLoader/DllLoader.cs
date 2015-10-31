using System.IO;
using System.Reflection;
using System.Threading;

namespace TFer.Modules.DllLoader {

    /// <summary>
    ///     Loads an assembly and executes parallely the main task.
    /// </summary>
    public sealed class DllLoader {
        public FileInfo NetFile { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Assembly LoadedAssembly { get; }

        /// <summary>
        ///     The task of the dll
        /// </summary>
        public Thread Thread { get; private set; }

        /// <summary>
        ///     Has Main execution ended and yielded results.
        /// </summary>
        public bool Ended => _endevent.WaitOne(0);

        public int Result { get; private set; } = -1;
        /// <summary>
        /// If the return value wasn't a digit (int), 
        /// </summary>
        public object UndefinedResult { get; private set; } = null;

        private readonly ManualResetEvent _endevent = new ManualResetEvent(false);

        /// <summary>
        ///     Loads the file and runs the main method.
        /// </summary>
        /// <param name="dll"></param>
        /// <param name="args"></param>
        public DllLoader(FileInfo dll, string[] args=null) : this(ReadFileBytes(dll.FullName), args) {
            NetFile = dll;
        }
        
        /// <summary>
        ///     Loads the assmebly through data of the read exe and then runs the method.
        /// </summary>
        public DllLoader(byte[] filedata, string[] args = null) {
            NetFile = null;
            this.LoadedAssembly = Assembly.Load(filedata);
            Thread = new Thread(() => {
                var res = LoadedAssembly.EntryPoint.Invoke(null, new object[1] { args ?? new string[0] });
                int n;
                var isNumeric = int.TryParse(res.ToString(), out n);
                if (isNumeric) Result = n;
                else UndefinedResult = res;
                _endevent.Set();
            });
            Thread.Start();
        }

        /// <summary>
        ///     Wait for the execution of the dll to end.
        /// </summary>
        /// <param name="milliseconds"></param>
        /// <returns></returns>
        public bool WaitToEnd(int milliseconds = -1) {
            return _endevent.WaitOne(milliseconds);
        }

        /// <summary>
        ///     Reads data of a file
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private static byte[] ReadFileBytes(string file) {
            if (File.Exists(file)==false)
                return new byte[0];
            using (var reader = new FileStream(file, FileMode.Open, FileAccess.Read)) {
                var data = new byte[reader.Length];
                reader.Read(data, 0, data.Length);
                return data;
            }
        }
    }
}