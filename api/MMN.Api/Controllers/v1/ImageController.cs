using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Options;
using MMN.Util.Enum;
using MMN.Util.Model;
using MMN.Util.Util;

namespace MMN.Api.Controllers.v1
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ImageController : LoggedControllerBase
    {

        public readonly IWebHostEnvironment _environment;
        private readonly AppSettings _appSettings;

        public ImageController(IWebHostEnvironment environment, IOptions<AppSettings> appSettings)
        {
            _environment = environment;
            _appSettings = appSettings.Value;
        }

        [HttpPost]
        public async Task<string> Post(IFormFile files)
        {
            if (files.Length > 0)
            {
                var fileO = files;
                using (var ms = new MemoryStream())
                {
                    files.CopyTo(ms);

                    var ext = files.FileName.Split('.');
                    var outImage = new MemoryStream();
                    var retorno = string.Empty;

                    foreach (var item in Enum.GetValues(typeof(ImageBlobEnum)))
                    {
                        files.CopyTo(ms);
                        AzureStorage.Resize(item.GetHashCode(), new MemoryStream(ms.ToArray()), outImage);

                        retorno = await AzureStorage.UploadImagem(_appSettings.StorageAccountConnectionString, "imagens-perfil", item + "_" + item + "." + ext[1], new MemoryStream(outImage.ToArray()));
                    }

                    return retorno;
                }
            }

            return "Erro";
        }
    }
}