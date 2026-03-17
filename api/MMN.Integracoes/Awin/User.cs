using System.Collections.Generic;

namespace MMN.Integracoes.Awin
{
    public class User
    {
        public int userId { get; set; }
        public List<Account> accounts { get; set; }
    }
}