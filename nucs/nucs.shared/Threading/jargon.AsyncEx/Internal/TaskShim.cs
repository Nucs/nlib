using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Nito.AsyncEx.Internal
{
    internal static class TaskShim
    {
        public static Task Run(Action func)
        {
            return Tasky.Run(func);
        }

        public static Task Run(Func<Task> func)
        {
            return Tasky.Run(func);
        }

        public static Task<T> Run<T>(Func<T> func)
        {
            return Tasky.Run(func);
        }

        public static Task<T> Run<T>(Func<Task<T>> func) {
            return func();
        }

        public static Task<T> FromResult<T>(T value)
        {
            return Tasky.FromResult(value);
        }
#if !NET4
        public static async Task<T[]> WhenAll<T>(IEnumerable<Task<T>> tasks) {
            var t = tasks as Task<T>[] ?? tasks.ToArray();
            await Tasky.WhenAll(t);

            return t.Select(tt=>tt.Result).ToArray();
        }
#else
        public static Task<T[]> WhenAll<T>(IEnumerable<Task<T>> tasks) {
            var t = tasks as Task<T>[] ?? tasks.ToArray();
            return Tasky.WhenAll(t).ContinueWith(tt=> t.Select(tas => tas.Result).ToArray());

        }
#endif
        public static Task WhenAll(params Task[] tasks)
        {
            return Tasky.WhenAll(tasks);
        }

        public static Task WhenAll(IEnumerable<Task> tasks)
        {
            return Tasky.WhenAll(tasks);
        }

        public static Task<Task<TResult>> WhenAny<TResult>(IEnumerable<Task<TResult>> tasks)
        {
            return Tasky.WhenAny(tasks);
        }

        public static Task<Task> WhenAny(IEnumerable<Task> tasks)
        {
            return Tasky.WhenAny(tasks);
        }

        public static Task<Task<TResult>> WhenAny<TResult>(params Task<TResult>[] tasks)
        {
            return Tasky.WhenAny(tasks);
        }

        public static Task<Task> WhenAny(params Task[] tasks)
        {
            return Tasky.WhenAny(tasks);
        }
    }
}
