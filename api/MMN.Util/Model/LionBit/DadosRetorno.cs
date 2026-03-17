using Microsoft.WindowsAzure.Storage.Blob.Protocol;
using MundiAPI.PCL.Models;

namespace MMN.Util.Model.LionBit
{
    public class DadosRetorno<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public GetSubscriptionResponse Response { get; set; }

        public DadosRetorno(bool sucesso = default, string mensagem = default, T data = default(T))
        {
            Success = sucesso;
            Message = mensagem;
            Data = data;
        }
    }
}