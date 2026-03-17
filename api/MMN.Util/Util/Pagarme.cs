using Flurl;
using Flurl.Http;
using MMN.Util.Extensions;
using MMN.Util.Model;
using MMN.Util.Model.LionBit;
using MMN.Util.Util;
using MundiAPI.PCL;
using MundiAPI.PCL.Models;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Ocsp;
using PagarMe;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection.Metadata;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace MMN.Util
{
    public class Pagarme
    {
        const string url = "https://api.pagar.me/1/";
        private static readonly string DefaultApiKey = Environment.GetEnvironmentVariable("PAGARME_API_KEY");
        private static readonly string DefaultEncryptionKey = Environment.GetEnvironmentVariable("PAGARME_ENCRYPTION_KEY");
        private static readonly string PAGARME_V5_API_KEY = Environment.GetEnvironmentVariable("PAGARME_V5_API_KEY");
        private static readonly string PAGARME_ID_PLAN = Environment.GetEnvironmentVariable("PAGARME_ID_PLAN");

        const string ApiVersion = "2019-09-01";

        public static async Task<Transaction> GerarBoletoAsync(
            string observacao,
            decimal valor,
            string email,
            string documento,
            string nome,
            string ddd,
            string telefone,
            string logradouro,
            string numero,
            string complemento,
            string bairro,
            string cidade,
            string uf,
            string cep,
            string identificador,
            int parcela = 1,
            bool adicionarTaxa = false)
        {
            if (adicionarTaxa)
            {
                valor += (decimal)3.8;
            }

            cep = cep.Replace("-", "");
            documento = documento.Replace(".", "").Replace(",", "").Replace("-", "").Replace("/", "");

            PagarMeService.DefaultApiKey = DefaultApiKey;
            PagarMeService.DefaultEncryptionKey = DefaultEncryptionKey;

            Transaction transaction = new Transaction();

            transaction.Amount = Convert.ToInt32(valor * 100);
            transaction.Referer = identificador;
            transaction.PaymentMethod = PaymentMethod.Boleto;
            transaction.BoletoExpirationDate = parcela > 1 ? DateTime.Now.AddDays(8).AddMonths(parcela - 1) : DateTime.Now.AddDays(8);
            transaction.Customer = new PagarMe.Customer
            {
                ExternalId = documento,
                Name = nome,
                Type = documento.Length == 11 ? CustomerType.Individual : CustomerType.Corporation,
                Country = "br",
                Email = email,
                Documents = new[]
                {
                    new PagarMe.Document {
                        Type = documento.Length == 11 ? DocumentType.Cpf : DocumentType.Cnpj,
                        Number = documento
                    }
                },
                PhoneNumbers = new string[]
                {
                    $"+55{ddd}{telefone}"
                }
            };
            transaction.Billing = new Billing
            {
                Name = nome,
                Address = new PagarMe.Address()
                {
                    Country = "br",
                    State = uf,
                    City = cidade,
                    Neighborhood = bairro,
                    Street = logradouro,
                    StreetNumber = numero,
                    Zipcode = cep
                }
            };
            transaction.Item = new[]
            {
                new Item()
                {
                    Id = "1",
                    Title = observacao,
                    Quantity = 1,
                    Tangible = true,
                    UnitPrice = Convert.ToInt32(valor) * 100
                }
            };

            await transaction.SaveAsync();

            return transaction;
        }

        public static async Task<IEnumerable<PagarmeModel>> Extrato(TransactionStatus status)
        {
            PagarMeService.DefaultApiKey = DefaultApiKey;
            //PagarMeService.DefaultApiKey = DefaultApiKeyTest;
            var count = 1000;
            var page = 1;
            var statusPayment = "paid";
            var lista = new List<PagarmeModel>();

            PagarmeModel[] userData;
            do
            {
                var request = new HttpRequestMessage(HttpMethod.Get, url + "transactions" + "?count=" + count + "&page=" + page + "&status=" + statusPayment + "&api_key=" + DefaultApiKey);
                var client = new HttpClient();
                var userDataResponse = await client.SendAsync(request);
                var responseString = await userDataResponse.Content.ReadAsStringAsync();
                userData = JsonConvert.DeserializeObject<PagarmeModel[]>(responseString);
                lista.AddRange(userData);
                page++;
            } while (userData.Any());

            return lista;
        }

        public static async Task<string> ObterBoletos(string codigoBoleto = null)
        {
            var filtro = new Dictionary<string, object>
                {
                    { "api_key", DefaultApiKey},
                    { "payment_method", "boleto" },
                };

            if (!string.IsNullOrEmpty(codigoBoleto))
            {
                filtro.Add("id", codigoBoleto);
            }

            var client = new HttpClient();

            var request = new HttpRequestMessage();
            request.RequestUri = new Uri(url.AppendPathSegment("transactions"));
            request.Content = new StringContent(JsonConvert.SerializeObject(filtro));
            request.Method = new HttpMethod("Get");
            request.SetHeader("Content-Type", "application /json");

            var response = await client.SendAsync(request);

            client.Dispose();

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var result = await response.Content.ReadAsStringAsync();

                return result;

            }

            return null;
        }

        public static async void CriarPlano()
        {
            var cliente = new RestClient("https://api.pagar.me/core/v5/plans");
            var request = new RestRequest("");
            request.AddHeader("accept", "application/json");
            request.AddJsonBody("{\"interval\":\"month\",\"interval_count\":1,\"pricing_scheme\":{\"scheme_type\":\"Unit\"},\"quantity\":null,\"payment_methods\":[\"credit_card\"],\"name\":\"Hab Vision Plus\",\"description\":\"Cash Back em dobro\",\"minimum_price\":9900,\"statement_descriptor\":\"Hab Vision Plus\",\"currency\":\"BRL\",\"billing_type\":\"exact_day\",\"billing_days\":[28]}");
            var response = cliente.Post(request);

            Console.WriteLine("{0}", response.Content);
        }


        public static async Task<DadosRetorno<string>> CancelarAssinatura(string idAssinatura)
        {
            DadosRetorno<string> retorno = new DadosRetorno<string>();
            try
            {
                string authInfo = PAGARME_V5_API_KEY + ":" + "";
                authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));

                var options = new RestClientOptions($"https://api.pagar.me/core/v5/subscriptions/{idAssinatura}");
                var client = new RestClient(options);
                var request = new RestRequest("");
                request.AddHeader("accept", "application/json");
                request.AddHeader("authorization", "Basic " + authInfo);
                request.AddJsonBody("{\"cancel_pending_invoices\":true}", false);
                var response = await client.DeleteAsync(request);

                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    retorno.Success = false;
                    retorno.Message = response.Content.ToString();
                }
                else if (response.StatusCode == HttpStatusCode.OK)
                {
                    retorno.Success = true;
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return retorno;

        }
        public static async Task<DadosRetorno<string>> CriarAssinatura(string email, string nome, string card_number, string card_holder_name,
             string card_expiration_month, string card_expiration_year, string card_cvv, Guid idUsuario, string document_type, string document, string type, 
             string countryCode, string areaCode, string mobileNumber, dynamic usuarioEndereco)
        {

            string basicAuthUserName = PAGARME_V5_API_KEY;
            string basicAuthPassword = "";

            DadosRetorno<string> retorno = new DadosRetorno<string>();

            try
            {
                var client = new MundiAPIClient(basicAuthUserName, basicAuthPassword);

                string planId = PAGARME_ID_PLAN;

                var customer = new CreateCustomerRequest
                {
                    Name = nome,
                    Email = email,
                    Code = idUsuario.ToString(),
                    DocumentType = document_type,
                    Document = document,
                    Type = type,
                    Phones = new CreatePhonesRequest()
                    {
                        MobilePhone = new CreatePhoneRequest()
                        {
                            CountryCode = countryCode,
                            AreaCode = areaCode,
                            Number = mobileNumber
                        }
                    }
                };

                var card = new CreateCardRequest
                {
                    HolderName = card_holder_name,
                    Number = card_number,
                    ExpMonth = Convert.ToInt32(card_expiration_month),
                    ExpYear = Convert.ToInt32(card_expiration_year),
                    Cvv = card_cvv,
                    BillingAddress = new CreateAddressRequest()
                    {
                        Country = "BR",
                        State = usuarioEndereco.GetType().GetProperty("Estado").GetValue(usuarioEndereco, null),
                        City = usuarioEndereco.GetType().GetProperty("Cidade").GetValue(usuarioEndereco, null),
                        Neighborhood = usuarioEndereco.GetType().GetProperty("Bairro").GetValue(usuarioEndereco, null),
                        Street = usuarioEndereco.GetType().GetProperty("Rua").GetValue(usuarioEndereco, null),
                        Number = usuarioEndereco.GetType().GetProperty("Numero").GetValue(usuarioEndereco, null),
                        Complement = usuarioEndereco.GetType().GetProperty("Complemento").GetValue(usuarioEndereco, null),
                        ZipCode = usuarioEndereco.GetType().GetProperty("CEP").GetValue(usuarioEndereco, null)
                    }
                };

                var request = new CreateSubscriptionRequest
                {
                    PlanId = planId,
                    PaymentMethod = "credit_card",
                    Currency = "BRL",
                    Interval = "month",
                    IntervalCount = 1,
                    BillingType = "prepaid",
                    Installments = 1,
                    Customer = customer,
                    Card = card,
                };

                var response = client.Subscriptions.CreateSubscription(request);

                var isActive = response.Status.Contains("active") ? true : false;
                var idAssinatura = response.Id.ToString();

                if (isActive)
                {
                    retorno.Success = isActive;
                    retorno.Data = idAssinatura;
                    retorno.Response = response;
                }
                else
                {
                    retorno.Success = false;
                    retorno.Data = response.Status.ToString();
                }

            }
            catch (MundiAPI.PCL.Exceptions.ErrorException ex)
            {
                var msg = ex.InnerException is null ? ex.Message : ex.InnerException.Message;
                retorno.Data = msg;
                retorno.Success = false;

                await new EmailExceptionHandler().SendExceptionEmailAsync(ex, msg, email);
            }
            catch (Exception ex)
            {
                var msg = ex.InnerException is null ? ex.Message : ex.InnerException.Message;
                retorno.Data = msg;
                retorno.Success = false;

                await new EmailExceptionHandler().SendExceptionEmailAsync(ex, msg, email);
            }

            return retorno;
        }

        public static async Task<DadosRetorno<string>> CriarAssinatura(string planId, PaymentMethod paymentMethod, int? installments, string email, string nome, string card_number, string card_holder_name,
             string card_expiration_month, string card_expiration_year, string card_cvv, Guid idUsuario, string document_type, string document, string type)
        {
            string basicAuthUserName = PAGARME_V5_API_KEY;
            string basicAuthPassword = "";

            DadosRetorno<string> retorno = new DadosRetorno<string>();

            try
            {
                var client = new MundiAPIClient(basicAuthUserName, basicAuthPassword);

                var customer = new CreateCustomerRequest
                {
                    Name = nome,
                    Email = email,
                    Code = idUsuario.ToString(),
                    DocumentType = document_type,
                    Document = document,
                    Type = type
                };

                var request = new CreateSubscriptionRequest
                {
                    PlanId = planId,
                    PaymentMethod = paymentMethod.GetEnumValue(),
                    Installments = installments ?? 1,
                    Customer = customer,
                };

                if (paymentMethod == PaymentMethod.CreditCard)
                {
                    request.Card = new CreateCardRequest
                    {
                        HolderName = card_holder_name,
                        Number = card_number,
                        ExpMonth = Convert.ToInt32(card_expiration_month),
                        ExpYear = Convert.ToInt32(card_expiration_year),
                        Cvv = card_cvv,
                    };
                }

                var response = client.Subscriptions.CreateSubscription(request);

                var isActive = response.Status.Contains("active") ? true : false;
                var idAssinatura = response.Id.ToString();

                if (isActive)
                {
                    retorno.Success = isActive;
                    retorno.Data = idAssinatura;

                    if (paymentMethod == PaymentMethod.Boleto)
                    {
                        for (int i = 0; i < installments; i++)
                        {
                            //    string observacao,
                            //    decimal valor,
                            //    string email,
                            //    string documento,
                            //    string nome,
                            //    string ddd,
                            //    string telefone,
                            //    string logradouro,
                            //    string numero,
                            //    string complemento,
                            //    string bairro,
                            //    string cidade,
                            //    string uf,
                            //    string cep,
                            //    string identificador,
                            //    int parcela = 1,
                            //    bool adicionarTaxa = false

                            //await GerarBoletoAsync();


                            var dataVencimento = response.CurrentCycle.StartAt.AddMonths(i);
                        }

                        retorno.Success = false;
                        retorno.Data = idAssinatura;
                    }

                    if (paymentMethod == PaymentMethod.Pix)
                    {

                    }
                }
                else
                {
                    retorno.Success = false;
                    retorno.Data = response.Status.ToString();
                }

            }
            catch (Exception)
            {
                throw;
            }

            return retorno;
        }
    }
}
