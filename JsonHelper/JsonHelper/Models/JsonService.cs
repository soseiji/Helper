using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonHelper.Models
{
    public class JsonService
    {
        public (bool, string) IsValidJson(string text)
        {
            if (string.IsNullOrEmpty(text)) return (false, string.Empty);
            try
            {
                // Json形式でない場合、シリアライザーが例外を返す
                JsonConvert.DeserializeObject<JObject>(text);
                return (true, string.Empty);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public string AddIndent(string text)
        {
            // 文字列をそのままシリアライズすると、escapeが入るだけ。文字列を一旦オブジェクトに戻す。
            //TextBoxOutput.Text = JsonConvert.SerializeObject(input, Formatting.Indented);
            var jObject = JsonConvert.DeserializeObject<JObject>(text);
            return JsonConvert.SerializeObject(jObject, Formatting.Indented);
        }

        public string DelIndent(string text)
        {
            var jObject = JsonConvert.DeserializeObject<JObject>(text);
            return JsonConvert.SerializeObject(jObject, Formatting.None);
        }

        public string AddEscapeStr(string text)
        {
            // シリアライズすれば、ダブルクォーテーションにエスケープ処理がかかる
            return JsonConvert.SerializeObject(text, Formatting.None);
        }

        public string DelEscapeStr(string text)
        {
            // エスケープされたJson文字列をオブジェクト化し、文字列で出力する
            var jToken = JsonConvert.DeserializeObject<JToken>(text);
            return jToken.ToString();
        }

        public string ConvertFromGzipHexStr(string text)
        {
            // 以下の処理を適用した文字列をJsonに変換する。
            // Json => Gzip => 16進数バイナリ文字列
            var byteList = new List<byte>();
            text = text.Replace("0x", "");
            for (int index = 0; index < text.Length / 2; index++)
            {
                byteList.Add(Convert.ToByte(text.Substring(startIndex: index * 2, length: 2), fromBase: 16));
            }
            return Encoding.UTF8.GetString(DecompressGZip(byteList.ToArray()));
        }

        public string ConvertFromGzipBase64(string text)
        {
            // 以下の処理を適用した文字列をJsonに変換する。
            // Json => Gzip => Base64           
            text = text.Replace(",", "").Replace(" ", "").Replace("　", "");  // Base64変換に不要な文字列を除外
            var decode = Convert.FromBase64String(text);
            return Encoding.UTF8.GetString(DecompressGZip(decode));
        }

        private byte[] DecompressGZip(byte[] data)
        {
            MemoryStream inputStream = null;
            MemoryStream outputStream = null;
            try
            {
                inputStream = new MemoryStream(data);
                outputStream = new MemoryStream();
                using (GZipStream gzipStream = new GZipStream(inputStream, CompressionMode.Decompress))
                {
                    byte[] buffer = new byte[1024];
                    while (true)
                    {
                        int readSise = gzipStream.Read(buffer, 0, buffer.Length);
                        if (readSise == 0) break;
                        outputStream.Write(buffer, 0, readSise);
                    }
                }
                return outputStream.ToArray();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (outputStream != null) outputStream.Dispose();
                if (inputStream != null) inputStream.Dispose();
            }
        }
    }
}
