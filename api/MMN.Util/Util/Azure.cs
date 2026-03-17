using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using ImageProcessor;
using ImageProcessor.Imaging.Formats;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using MMN.Util.Enum;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace MMN.Util.Util
{
    public class AzureStorage
    {

        public static async Task<List<string>> GetImages(string cnn, string folder, int? segmentSize = null)
        {
            var result = new List<string>();

            var blobContainerClient = new BlobContainerClient(cnn, folder);

            // Call the listing operation and return pages of the specified size.
            var resultSegment = blobContainerClient.GetBlobsAsync()
                .AsPages(default, segmentSize);

            // Enumerate the blobs returned for each page.
            await foreach (Azure.Page<BlobItem> blobPage in resultSegment)
            {
                foreach (BlobItem blobItem in blobPage.Values)
                {
                    result.Add(blobContainerClient.Uri + "/" + blobItem.Name);
                }
            }

            return result;
        }

        public static async Task<string> UploadImagem(string cnn, string folder, string completeNameFile, MemoryStream memoryStream)
        {
            try
            {
                var storageAccount = CloudStorageAccount.Parse(cnn);
                var blobClient = storageAccount.CreateCloudBlobClient();
                var container = blobClient.GetContainerReference(folder);

                var permissions = await container.GetPermissionsAsync();
                permissions.PublicAccess = BlobContainerPublicAccessType.Container;
                await container.SetPermissionsAsync(permissions);

                await container.CreateIfNotExistsAsync();

                if (!await container.ExistsAsync()) return "Erro";

                var cloudBlockBlob = container.GetBlockBlobReference(completeNameFile);

                var ms = new MemoryStream(memoryStream.ToArray());

                await cloudBlockBlob.UploadFromStreamAsync(ms);

                var end = cloudBlockBlob.Uri.AbsoluteUri;

                return end;

            }
            catch (Exception e)
            {
                return $"Falhou {e.Message}";
            }
        }

        public object CreateBlob(object base64, Guid guid, object storageAccountConnectionString, object diretorio, object nome, bool v)
        {
            throw new NotImplementedException();
        }

        public static void Resize(int width, MemoryStream input, MemoryStream output)
        {
            int quality = 70;
            var size = new Size(width, 0);

            using var factory = new ImageFactory(preserveExifData: true);
            using var memory = new MemoryStream();
            factory.Load(input).Resize(size).Format(new PngFormat()).Quality(quality).Save(memory);
            memory.CopyTo(output);
            factory.Dispose();
        }

        private static readonly string[] separator = ["base64,"];

        public static async Task<string> CreateBlob(string files, Guid idUsaurio, string cnn)
        {
            try
            {
                var base64 = files.Split(separator, StringSplitOptions.None);

                var extensao = GetFileExtensionFromBase64(base64.Length > 1 ? base64[1] : base64[0]);
                var retorno = string.Empty;

                if (extensao == "") return "Falhou - Extensão inválida";

                var idUsuario = idUsaurio;
                var outImage = new MemoryStream();

                var bytes = Convert.FromBase64String(base64.Length > 1 ? base64[1] : base64[0]);

                foreach (var item in System.Enum.GetValues(typeof(ImageBlobEnum)))
                {
                    var ms = new MemoryStream(bytes);

                    AzureStorage.Resize(ImageBlobEnum.G.GetHashCode(), ms, outImage);

                    if (string.IsNullOrEmpty(retorno))
                    {
                        retorno = await AzureStorage.UploadImagem(cnn, "imagens-perfil", idUsuario + "_" + item + "." + extensao, new MemoryStream(outImage.ToArray()));

                        //ms.SetLength(0);
                        ms.Close();
                        ms.Dispose();
                    }

                }

                return retorno;
            }
            catch (Exception ex)
            {
                return "Falhou -" + ex.Message;
            }
        }

        private static string GetFileExtensionFromBase64(string base64)
        {
            var data = base64[..5];

            return data.ToUpper() switch
            {
                "IVBOR" => "png",
                "/9J/4" => "jpeg",
                "JVBER" => "pdf",
                _ => "",
            };
        }

        public static async Task<string> CreateBlob(string files, Guid idUsuario, string cnn, string folder, string nomeArquivo, bool comprovante = false)
        {
            try
            {
                var base64 = files.Split(separator, StringSplitOptions.None);

                var extensao = GetFileExtensionFromBase64(base64.Length > 1 ? base64[1] : base64[0]);
                var retorno = string.Empty;

                if (extensao == "") return "Falhou - Extensão inválida";

                var outImage = new MemoryStream();

                var bytes = Convert.FromBase64String(base64.Length > 1 ? base64[1] : base64[0]);

                if (!comprovante)
                {
                    foreach (var item in System.Enum.GetValues(typeof(ImageBlobEnum)))
                    {
                        var ms = new MemoryStream(bytes);

                        AzureStorage.Resize(ImageBlobEnum.G.GetHashCode(), ms, outImage);
                        retorno = await AzureStorage.UploadImagem(cnn, folder, NormalizeString(nomeArquivo) + item + "." + extensao, new MemoryStream(outImage.ToArray()));

                        //ms.SetLength(0);
                        ms.Close();
                        ms.Dispose();
                    }
                }
                else
                {
                    var ms = new MemoryStream(bytes);
                    retorno = await AzureStorage.UploadImagem(cnn, folder, NormalizeString(nomeArquivo) + "." + extensao, ms);
                }

                return retorno;
            }
            catch (Exception ex)
            {
                throw new Exception("Falhou -" + ex.Message);
            }
        }

        private static string NormalizeString(string name)
        {
            if (name == null) return "";

            var stringBuilder = new StringBuilder();
            var prevdash = false;

            foreach (char c in name)
            {
                switch (CharUnicodeInfo.GetUnicodeCategory(c))
                {
                    case UnicodeCategory.LowercaseLetter:
                    case UnicodeCategory.UppercaseLetter:
                    case UnicodeCategory.DecimalDigitNumber:
                        if (c < 128)
                            stringBuilder.Append(c);
                        else
                            stringBuilder.Append(CharToAscii(c));

                        prevdash = false;
                        break;
                    // Check if the character is to be replaced by a hyphen but only if the last character wasn't
                    case UnicodeCategory.SpaceSeparator:
                    case UnicodeCategory.ConnectorPunctuation:
                    case UnicodeCategory.DashPunctuation:
                    case UnicodeCategory.OtherPunctuation:
                    case UnicodeCategory.MathSymbol:
                        if (!prevdash)
                        {
                            stringBuilder.Append('-');
                            prevdash = true;
                        }
                        break;
                }
            }

            return stringBuilder.ToString().Trim('-');
        }

        public static string CharToAscii(char c)
        {
            string s = c.ToString().ToLowerInvariant();
            if ("àåáâäãåą".Contains(s))
            {
                return "a";
            }
            else if ("èéêëę".Contains(s))
            {
                return "e";
            }
            else if ("ìíîïı".Contains(s))
            {
                return "i";
            }
            else if ("òóôõöøőð".Contains(s))
            {
                return "o";
            }
            else if ("ùúûüŭů".Contains(s))
            {
                return "u";
            }
            else if ("çćčĉ".Contains(s))
            {
                return "c";
            }
            else if ("żźž".Contains(s))
            {
                return "z";
            }
            else if ("śşšŝ".Contains(s))
            {
                return "s";
            }
            else if ("ñń".Contains(s))
            {
                return "n";
            }
            else if ("ýÿ".Contains(s))
            {
                return "y";
            }
            else if ("ğĝ".Contains(s))
            {
                return "g";
            }
            else if (c == 'ř')
            {
                return "r";
            }
            else if (c == 'ł')
            {
                return "l";
            }
            else if (c == 'đ')
            {
                return "d";
            }
            else if (c == 'ß')
            {
                return "ss";
            }
            else if (c == 'þ')
            {
                return "th";
            }
            else if (c == 'ĥ')
            {
                return "h";
            }
            else if (c == 'ĵ')
            {
                return "j";
            }
            else
            {
                return "";
            }
        }

        public static async Task<bool> Delete(string folder, string nomeArquivo, string cnn)
        {
            try
            {
                var storageAccount = CloudStorageAccount.Parse(cnn);
                var blobClient = storageAccount.CreateCloudBlobClient();
                var container = blobClient.GetContainerReference(folder);

                var blobArquivo = container.GetBlockBlobReference(nomeArquivo);

                await blobArquivo.DeleteAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
