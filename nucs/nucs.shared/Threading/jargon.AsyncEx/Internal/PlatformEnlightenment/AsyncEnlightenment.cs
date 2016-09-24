using System;
using System.Threading.Tasks;

namespace Nito.AsyncEx.Internal.PlatformEnlightenment
{
    public static class AsyncEnlightenment
    {
        public static TaskCreationOptions AddDenyChildAttach(TaskCreationOptions options)
        {
#if NET4
            options &= ~TaskCreationOptions.AttachedToParent;
            return options;
#else
            return options | TaskCreationOptions.DenyChildAttach;
#endif
        }
    }
}
