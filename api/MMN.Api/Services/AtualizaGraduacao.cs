using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MMN.Repositorio.Contexto;
using MMN.Repositorio.Repositorio;
using MMN.Util.Extensions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MMN.Api.Service
{

    public class AtualizaGraduacao : BaseService, IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        public bool IsDev { get; set; }

        private IServiceScope _scope;

        public AtualizaGraduacao(IServiceProvider serviceProvider, IHostingEnvironment env)
        {
            _serviceProvider = serviceProvider;
            IsDev = env.IsDevelopment();
            _scope = _serviceProvider.CreateScope();
        }

        public void Dispose()
        {
            Timer?.Dispose();
            _scope.Dispose();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var task = Task.Factory.StartNew(() =>
            {
                Console.WriteLine("start");
                Timer = new Timer(TimerTick, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(TickInterval));
                Timer.Change(TickInterval, TickInterval);
            });
            return task;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            var task = Task.Factory.StartNew(() =>
            {
                Console.WriteLine("Stopped");
            });
            return task;
        }

        public override void TimerTick(object info)
        {
            try
            {
                if (DateTime.UtcNow.HorarioBrasilia().Hour == 02)
                {
                    Timer.Change(Timeout.Infinite, Timeout.Infinite);
                    AtualizarGraduacao();
                }
            }
            finally
            {
                Timer.Change(TickInterval, TickInterval);
            }
        }

        private void AtualizarGraduacao()
        {
            try
            {
                var context = _scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                var proceduresRepository = new ProceduresRepositorio(context);

                proceduresRepository.spc_AtualizaGraduacao();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
