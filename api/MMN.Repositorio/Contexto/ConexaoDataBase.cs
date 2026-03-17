using System;
using Microsoft.AspNetCore.Http;

namespace MMN.Repositorio.Contexto
{
    public static class ConexaoDataBase
    {
        private static HttpContextAccessor _httpContextAccessor = new HttpContextAccessor();
        public static string Connection()
        {
            //return "Server=191.235.240.126\\MSSQLSERVER,14330;Initial Catalog=QuantaShop_03042024;Persist Security Info=False;User ID=QuantaDev;Password=P3985jmMqY**;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Connection Timeout=30;MultipleActiveResultSets=True;";
            //return "Server=tcp:bigcash.database.windows.net,1433;Initial Catalog=Bigcash;Persist Security Info=False;User ID=bigcash;Password=Fendizola88;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;MultipleActiveResultSets=True;";
            var cs = Environment.GetEnvironmentVariable("SQL_CONNECTION_STRING")
                  ?? Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");

            return cs;
        }
    }
}