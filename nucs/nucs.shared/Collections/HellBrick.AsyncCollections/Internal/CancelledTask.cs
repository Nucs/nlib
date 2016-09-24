#if !NET4
using System.Threading.Tasks;
using nucs.Collections.Concurrent.ValueTask;

namespace nucs.Collections.Concurrent.Internal
{
	internal static class CanceledValueTask<T>
	{
		public static readonly ValueTask<T> Value = CreateCanceledTask();

		private static ValueTask<T> CreateCanceledTask()
		{
			TaskCompletionSource<T> tcs = new TaskCompletionSource<T>();
			tcs.SetCanceled();
			return new ValueTask<T>( tcs.Task );
		}
	}
}
#endif