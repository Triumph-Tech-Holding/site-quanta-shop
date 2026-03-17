using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace MMN.Api.Hubs
{
    public class BuyConfirmationHub : Hub
    {
        public async Task SendBuyConfirmation(Guid idComercianteVenda)
        {
            await Clients.All.SendAsync("SellConfirmation", idComercianteVenda);
        }
    }
}
