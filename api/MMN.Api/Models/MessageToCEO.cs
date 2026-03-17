using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using System;

namespace MMN.Api.Models;

public class MessageToCEO
{
    public string From { get; set; }
    public string To { get; set; }
    public string Message { get; set; }
    public string Login { get; set; }
    public string Sender { get; set; }
    public string Phone { get; set; }

    public MessageToCEO()
    {
        To = "maurotriumph@souquantabank.com.br";
    }
}
