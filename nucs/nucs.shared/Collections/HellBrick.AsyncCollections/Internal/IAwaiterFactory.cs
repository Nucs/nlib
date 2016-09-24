#if !NET4
namespace nucs.Collections.Concurrent.Internal
{
	internal interface IAwaiterFactory<T>
	{
		IAwaiter<T> CreateAwaiter();
	}
}
#endif