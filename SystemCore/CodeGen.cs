using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading;
using Microsoft.CSharp;
using nucs.Collections.Extensions;

namespace nucs.SystemCore {
    public static class CodeGen {
        
        /// <summary>
        /// Generates code, into a dll or exe -
        /// </summary>
        /// <param name="code"></param>
        /// <param name="filename"></param>
        public static CodeGenerated Generate(string code, string filename, FileOutputExtension extension = FileOutputExtension.DLL) {
            var provider = new CSharpCodeProvider();
            var parameters = new CompilerParameters 
                { GenerateInMemory = true
                , GenerateExecutable = extension == FileOutputExtension.EXE
                , OutputAssembly = filename+extension.GetCustomAttributeDescription()
                , CompilerOptions = "/optimize"
                ,};
            parameters.ReferencedAssemblies.AddRange(
                AppDomain.CurrentDomain.GetAssemblies()
                .Select(asm=>asm.Location)
                .Where(asmloc=>!string.IsNullOrEmpty(asmloc)).ToArray()
            );

            #if DEBUG
            try {
                code = CodeGen.PrepareCode(code);
                code.SaveAs("D:/coderunner-genned.txt");
            } catch {} //safecatch
            #endif

            CompilerResults results = provider.CompileAssemblyFromSource(parameters, code);
            return new CodeGenerated(results);
        }

        private const string NL = "\n\r"; //newline:
        private const int NAMESPACE_LENGHT_LIMIT = 10; //newline:
        /// <summary>
        /// Prepares the given <param name="code"> for runtime compilation</param>
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        private static string PrepareCode(string code) {
            code = PrepareNamespaces() + NL + PrepareMainAttribute() + NL + code;
            if (!code.Contains("main()", StringComparison.InvariantCultureIgnoreCase)) {
                code += NL + @"namespace {0} {
	                                 static class {1} {
		                                 static void Main() {}
	                                 }
                                 }".Replace("{0}",Randomizer.RandomString(10)).Replace("{1}",Randomizer.RandomString(10));

            }
            return code;
        }

        private static string PrepareMainAttribute() {
            return 
                  @"[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
                  public sealed class MainAttribute : Attribute {}";
        }

        /// <summary>
        /// Returns the prepared namespaces for the runtime gen.
        /// </summary>
        /// <returns></returns>
        private static string PrepareNamespaces() {
            return AppDomain.CurrentDomain.GetAssemblies().SelectMany(asm => asm.GetTypes())
                       .Select(_filter_namespaces)
                       .Distinct().Where(names => !(string.IsNullOrEmpty(names) || names.ContainsAny("<", ">")))
            .Select(ns => "using " + ns + ";" + Environment.NewLine).StringJoin("").TrimEnd('\n', '\r');

        }

        private static string _filter_namespaces(Type t) {
            string ns = t.Namespace ?? "";
            if (ns == "") return null;

            int i=0;
            var l = new List<int>();
            while (l.Count != NAMESPACE_LENGHT_LIMIT) {
                var n = ns.IndexOf('.', i);
                if (n == -1) break;
                i = n+1;
                l.Add(n);
            }
            if (l.Count == 0)
                return ns;
            var a = l.Take(l.Count >= NAMESPACE_LENGHT_LIMIT ? NAMESPACE_LENGHT_LIMIT : l.Count).Last();
            return ns.Substring(0, a);
        }
        
    }

    /// <summary>
    /// Represents a compiled code.
    /// </summary>
    public class CodeGenerated : IDisposable {

        /// <summary>
        /// The appdomain that contains the assembly, unload the appdomain inorder to unload the assembly.
        /// </summary>
        public AppDomain AppDomain { get; private set; }

        /// <summary>
        /// The assembly that was generated from the given code. always null when compilation failed.
        /// </summary>
        public Assembly Assembly { get; private set; }

        /// <summary>
        /// Did the code compile currectly or are there any errors?
        /// </summary>
        public bool CompiledSuccessfully {
            get { return Errors.Count == 0; }
        }

        /// <summary>
        /// Errors that were produced during compilation and after it.
        /// </summary>
        public List<CompilerError> Errors { get; private set; }

        private readonly MethodInfo main = null;
        
        internal CodeGenerated(CompilerResults res) {
           
            Errors = res.Errors.Cast<CompilerError>().Where(err => err.IsWarning == false).ToList();
            if (res.Errors.HasErrors) { //has failed compiling
                return;
            }
            Assembly = res.CompiledAssembly;
            AppDomain = AppDomain.CreateDomain(Randomizer.RandomString(10));
            var assemblyName = new AssemblyName();
            assemblyName.CodeBase = Assembly.CodeBase;
            Assembly assembly = AppDomain.Load(assemblyName);

            //post compiling:

            var mains = Assembly.ToEnumerable().FindAttributedMethod("MainAttribute");
            if (mains.Length > 1) {
                Errors.Add(new PostCompilationException("Too many main methods were found with the attribute [Main]"));
                return;
            }
            if (mains.Length == 0) {
                Errors.Add(new PostCompilationException("no main methods were found with the attribute [Main]"));
                return;
            }

            main = mains[0]; //main method validation.
            if (main.ReturnType != typeof(void) || main.GetParameters().Length != 0) { //todo test
                Errors.Add(new PostCompilationException("Main method must return void return and take no parameters"));
                return;
            }
        }

        public void Invoke() {
            if (!CompiledSuccessfully) Errors.Add(new PostCompilationException("Too many main methods were found with the attribute [Main]"));
            main.Invoke(null, null);

        }

        public Thread InvokeAsync() {
            if (!CompiledSuccessfully) Errors.Add(new PostCompilationException("Too many main methods were found with the attribute [Main]"));
            return null;

        }

        public void Dispose() {
            AppDomain.Unload(AppDomain);
            GC.Collect(); // collects all unused memory
            GC.WaitForPendingFinalizers(); // wait until GC has finished its work
            GC.Collect();
        }
    }

    /// <summary>
    /// Used to define which file extension the output will have.
    /// </summary>
    public enum FileOutputExtension {
        [Description(".dll")]
        DLL,
        [Description(".exe")]
        EXE
    }

    [Serializable]
    public class PostCompilationException : CompilerError {
        public PostCompilationException() { }
        public PostCompilationException(string message) : base("",0,0,"None", message) { }
    }

}
