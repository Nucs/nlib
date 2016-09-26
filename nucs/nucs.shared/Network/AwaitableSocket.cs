using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace nucs.shared.Network {
    public sealed class SocketAwaitable : INotifyCompletion {
        private readonly static Action SENTINEL = () => { };
        internal bool m_wasCompleted;
        internal Action m_continuation;
        internal SocketAsyncEventArgs m_eventArgs;

        public SocketAwaitable(SocketAsyncEventArgs eventArgs) {
            if (eventArgs == null)
                throw new ArgumentNullException(nameof(eventArgs))
                    ;
            m_eventArgs = eventArgs;
            eventArgs.Completed += delegate {
                var prev = m_continuation ?? Interlocked.CompareExchange(
                               ref m_continuation, SENTINEL, null);
                if (prev != null)
                    prev();
            };
        }

        internal void Reset() {
            m_wasCompleted = false;
            m_continuation = null;
        }

        public SocketAwaitable GetAwaiter() {
            return this;
        }

        public bool IsCompleted {
            get { return m_wasCompleted; }
        }

        public void OnCompleted(Action continuation) {
            if (m_continuation == SENTINEL ||
                Interlocked.CompareExchange(
                    ref m_continuation, continuation, null) == SENTINEL) {
                Task.Run(continuation);
            }
        }

        public void GetResult() {
            if (m_eventArgs.SocketError != SocketError.Success)
                throw new SocketException((int) m_eventArgs.SocketError);
        }
    }

    public static class SocketExtensions {
        public static Task<int> ReceiveAsync(this Socket socket, byte[] buffer, int offset, int size, SocketFlags socketFlags) {
            var tcs = new TaskCompletionSource<int>(socket);
            socket.BeginReceive(buffer, offset, size, socketFlags, iar => {
                var t = (TaskCompletionSource<int>) iar.AsyncState;
                var s = (Socket) t.Task.AsyncState;
                try {
                    t.TrySetResult(s.EndReceive(iar));
                } catch (Exception exc) {
                    t.TrySetException(exc);
                }
            }, tcs);
            return tcs.Task;
        }
        public static async Task ReadAsync(this Socket s)
        {
            // Reusable SocketAsyncEventArgs and awaitable wrapper 
            var args = new SocketAsyncEventArgs();
            args.SetBuffer(new byte[0x1000], 0, 0x1000);
            var awaitable = new SocketAwaitable(args);
            // Do processing, continually receiving from the socket 
            while (true)
            {
                await s.ReceiveAsync(awaitable);
                int bytesRead = args.BytesTransferred;
                if (bytesRead <= 0) break;
                Console.WriteLine(bytesRead);
            }
        }

        public static SocketAwaitable ReceiveAsync(this Socket socket,
            SocketAwaitable awaitable) {
            awaitable.Reset();
            if (!socket.ReceiveAsync(awaitable.m_eventArgs))
                awaitable.m_wasCompleted = true;
            return awaitable;
        }

        public static SocketAwaitable SendAsync(this Socket socket,
            SocketAwaitable awaitable) {
            awaitable.Reset();
            if (!socket.SendAsync(awaitable.m_eventArgs))
                awaitable.m_wasCompleted = true;
            return awaitable;
        }

        // … 
    }
}