using System.Threading;

namespace MMN.Api.Service
{
    public abstract class BaseService
    {
        public virtual int TickInterval { get; set; } = 30000;//60000;
        public virtual Timer Timer { get; set; }

        public abstract void TimerTick(object info);
    }
}
