using System;
using System.Collections.Generic;
using MMN.Util.Extensions;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using Newtonsoft.Json;
using System.Xml;
using MMN.Util.Util;
using Newtonsoft.Json.Linq;

namespace MMN.Integracoes.Afilio
{
    public class AfilioBLL
    {
        public string AfiliadoId { get { return "167310"; } }
        public string Token { get { return "RO3fqD167310"; } }
        public string SiteId { get { return "16308"; } }
        public HttpClient Client { get { return new HttpClient(); } }
        public string UrlAfilio { get { return "https://data.afilio.com.br/"; } }
        public string Data { get { return DateTime.Now.ToString("yyyy-MM-dd"); } }

        private static readonly Throttler throttler = new Throttler(15);

        public async Task<List<Campanha>> GetCampanhas()
        {
            var url = UrlAfilio + $"/campaign?affid={AfiliadoId}&token={Token}";

            HttpResponseMessage response = await throttler.ThrottleAsync(Client.GetAsync, url);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            XNode node = JsonConvert.DeserializeXNode(responseBody, "Root");
            XmlDocument doc = new XmlDocument();

            doc.LoadXml(node.ToString());

            XmlNode root = doc.FirstChild;

            var campanhas = new List<Campanha>();

            if (root.HasChildNodes)
            {
                for (int i = 0; i < root.ChildNodes.Count; i++)
                {
                    XmlDocument xmlDoc = new XmlDocument();

                    xmlDoc.LoadXml($"<campanha>{root.ChildNodes[i].InnerXml}</campanha>");

                    string strCampanha = JsonConvert.SerializeXmlNode(xmlDoc);
                    var c = JsonConvert.DeserializeObject<Root>(strCampanha);

                    campanhas.Add(c.Campanha);
                }
            }

            return campanhas;
        }

        public async Task<List<Cupom>> GetCupons()
        {
            var url = UrlAfilio + $"/feedcupoms?token={Token}&mode=JSON&canalid={SiteId}&affid={AfiliadoId}";

            HttpResponseMessage response = await throttler.ThrottleAsync(Client.GetAsync, url);
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();

            var cupons = new List<Cupom>();

            JArray array = JArray.Parse(responseBody);
            foreach (JObject obj in array)
            {
                foreach (JProperty singleProp in obj.Properties())
                {
                    JObject item = (JObject)singleProp.Value;
                    try
                    {
                        var cupom = new Cupom
                        {
                            id_campaign = (string)item.SelectToken("id_campaign"),
                            campaign_name = (string)item.SelectToken("campaign_name"),
                            creative_id = (string)item.SelectToken("creative_id"),
                            title = (string)item.SelectToken("title"),
                            description = (string)item.SelectToken("description"),
                            category = (string)item.SelectToken("category"),
                            activation_date = (string)item.SelectToken("activation_date"),
                            expiration_date = (string)item.SelectToken("expiration_date"),
                            shortened = (string)item.SelectToken("shortened"),
                            site_id = (string)item.SelectToken("site_id"),
                            site_name = (string)item.SelectToken("site_name"),
                            type = (string)item.SelectToken("type"),
                            code = (string)item.SelectToken("code")

                        };

                        cupons.Add(cupom);

                    }
                    catch
                    {
                        continue;
                    }
                }
            }

            //for (int i = 1; i < array.Count(); i++)
            //{
            //    try
            //    {
            //        var itens = array[i].Split(";").Select(s => { s = s.Replace("\"", ""); return s; }).ToList();
            //        var cupom = new Cupom
            //        {
            //            id = itens[0],
            //            title = itens[1],
            //            startdate = itens[2],
            //            enddate = itens[3],
            //            url = itens[4] + ";" + itens[5],
            //            discount = itens[6],
            //            progid = itens[7],
            //            rule = itens[8],
            //            code = itens[9],
            //            description = itens[10],
            //            type = itens[11]
            //        };

            //        cupons.Add(cupom);
            //    }
            //    catch
            //    {
            //        continue;
            //    }
            //}

            return cupons;
        }

        public async Task<List<Venda>> GetCompras(string dataIni = "", string dataFim = "")
        {
            try
            {
                if (string.IsNullOrEmpty(dataIni))
                    dataIni = DateTime.UtcNow.HorarioBrasilia().AddDays(-1).ToString("yyyy-MM-dd");
                if (string.IsNullOrEmpty(dataFim))
                    dataFim = DateTime.UtcNow.HorarioBrasilia().ToString("yyyy-MM-dd");

                var url = UrlAfilio + $"//leadsale_api.php?mode=list" +
                    $"&token={Token}" +
                    $"&affid={AfiliadoId}" +
                    $"&dateType=transaction" +
                    $"&type=sale" +
                    $"&dateStart={dataIni}" +
                    $"&dateEnd={dataFim}" +
                    $"&format=JSON";

                HttpResponseMessage response = await throttler.ThrottleAsync(Client.GetAsync, url);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                XNode node = JsonConvert.DeserializeXNode(responseBody, "RootVenda");
                XmlDocument doc = new XmlDocument();

                doc.LoadXml(node.ToString());

                XmlNode root = doc.FirstChild;

                var vendas = new List<Venda>();
                if (root != null)
                {
                    if (root.HasChildNodes)
                    {
                        for (int i = 0; i < root.ChildNodes.Count; i++)
                        {
                            XmlDocument xmlDoc = new XmlDocument();

                            var filho = root.ChildNodes[i].FirstChild;
                            if (filho != null)
                            {
                                xmlDoc.LoadXml($"<venda>{filho.ChildNodes[0].InnerXml}</venda>");

                                string strCampanha = JsonConvert.SerializeXmlNode(xmlDoc);
                                var c = JsonConvert.DeserializeObject<RootVenda>(strCampanha);

                                vendas.Add(c.Venda);
                            }
                        }
                    }
                }

                return vendas;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
