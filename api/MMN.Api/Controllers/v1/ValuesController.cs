using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace MMN.Api.Controllers.v1
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            string[] fruits = { "Apple", "Mango", "Peach", "Banana", "Orange", "Grapes", "Watermelon", "Tomato" };

            Random rnd = new Random();
            string[] random = fruits.OrderBy(x => rnd.Next()).ToArray();


            //DbContextOptionsBuilder options = new DbContextOptionsBuilder();
            //var connection = ConexaoDataBase.Connection();
            //options.UseSqlServer(connection);
            //var ctx = new DatabaseContext(options.Options);

            //var admin = ctx.Usuario.FirstOrDefault(x => x.Login == "administrador");

            //var salt = Hash.Get_SALT();
            //var hash = Hash.Get_HASH_SHA512("Pro@ire2020", admin.Email, salt);

            //admin.SaltKey = salt;
            //admin.Senha = hash;

            //ctx.SaveChanges();

            return Ok(random);
        }
    }
}
