#if !NET4
using System.Threading.Tasks;

namespace nucs.Collections.Concurrent.Internal
{
	internal static class TaskExtensions
	{
		public static async Task<T> WithYield<T>( this Task<T> task )
		{
			var result = await task.ConfigureAwait( false );
		    await Tasky.Yield();
			return result;
		}


	}
}
#endif