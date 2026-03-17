using System;
using System.Threading;
using System.Threading.Tasks;

namespace MMN.Util.Util
{
    public class Throttler
    {
        private readonly int _maxCallbacksPerMinute = 15;
        private readonly object _throtleLock = new object();
        private DateTime _lastRequestMinute = DateTime.MinValue;
        private int _requestCount = 0;

        public Throttler(int maxCallbacksPerMinute)
        {
            _maxCallbacksPerMinute = maxCallbacksPerMinute;
        }

        private DateTime GetCurrentMinute()
        {
            var now = DateTime.Now;
            now = now.AddSeconds(-now.Second);
            now = now.AddMilliseconds(-now.Millisecond);

            return now;
        }

        private void Throttle()
        {
            lock (_throtleLock)
            {
                var currentMinute = GetCurrentMinute();
                if (_requestCount >= _maxCallbacksPerMinute)
                {
                    while (currentMinute <= _lastRequestMinute.AddMilliseconds(2))
                    {
                        Thread.Sleep((60 - DateTime.Now.Second) * 1000);
                        currentMinute = GetCurrentMinute();
                    }

                    _lastRequestMinute = currentMinute;
                    _requestCount = 0;
                }
                else if (currentMinute > _lastRequestMinute.AddMilliseconds(2))
                {
                    _lastRequestMinute = currentMinute;
                    _requestCount = 0;
                }

                _requestCount++;
            }
        }

        public Task<Out> ThrottleAsync<In1, In2, In3, In4, Out>(
            Func<In1, In2, In3, In4, Out> function,
            In1 input1,
            In2 input2,
            In3 input3,
            In4 input4)
        {
            return Task.Run(() =>
            {
                Throttle();

                return function(input1, input2, input3, input4);
            });
        }

        public Task<Out> ThrottleAsync<In1, In2, In3, Out>(
           Func<In1, In2, In3, Out> function,
           In1 input1,
           In2 input2,
           In3 input3)
        {
            return Task.Run(() =>
            {
                Throttle();

                return function(input1, input2, input3);
            });
        }

        public Task<Out> ThrottleAsync<In1, In2, Out>(
           Func<In1, In2, Out> function,
           In1 input1,
           In2 input2)
        {
            return Task.Run(() =>
            {
                Throttle();

                return function(input1, input2);
            });
        }

        public Task<Out> ThrottleAsync<In, Out>(Func<In, Out> function, In input)
        {
            return Task.Run(() =>
            {
                Throttle();

                return function(input);
            });
        }

        public Task<Out> ThrottleAsync<Out>(Func<Out> function)
        {
            return Task.Run(() =>
            {
                Throttle();

                return function();
            });
        }

        public Task<Out> ThrottleAsync<In1, In2, In3, In4, Out>(
            Func<In1, In2, In3, In4, Task<Out>> function,
            In1 input1,
            In2 input2,
            In3 input3,
            In4 input4)
        {
            return Task.Run(async () =>
            {
                Throttle();

                return await function(input1, input2, input3, input4);
            });
        }

        public Task<Out> ThrottleAsync<In1, In2, In3, Out>(
           Func<In1, In2, In3, Task<Out>> function,
           In1 input1,
           In2 input2,
           In3 input3)
        {
            return Task.Run(async () =>
            {
                Throttle();

                return await function(input1, input2, input3);
            });
        }
        public Task<Out> ThrottleAsync<In1, In2, Out>(
           Func<In1, In2, Task<Out>> function,
           In1 input1,
           In2 input2)
        {
            return Task.Run(async () =>
            {
                Throttle();

                return await function(input1, input2);
            });
        }

        public Task<Out> ThrottleAsync<In, Out>(Func<In, Task<Out>> function, In input)
        {
            return Task.Run(async () =>
            {
                Throttle();

                return await function(input);
            });
        }

        public Task<Out> ThrottleAsync<Out>(Func<Task<Out>> function)
        {
            return Task.Run(() =>
            {
                Throttle();

                return function();
            });
        }
    }
}
