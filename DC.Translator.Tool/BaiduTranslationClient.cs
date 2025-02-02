using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DC.Translator.Tool
{
    public class BaiduTranslationClient
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        private static readonly string baseUrl = "http://api.fanyi.baidu.com/api/trans/vip/translate";
        private static readonly string appId = "20210618000866481";
        private static readonly string key = "Q0d16vT56KodRAq6YbO6";
        private readonly ILogger _logger;
        private readonly MD5 _md5;
        public BaiduTranslationClient(ILogger logger)
        {
            _logger = logger;
            _md5 = MD5.Create();
        }

        public async Task<string> Translate(string chinese, string lang)
        {
            var q = UrlEncoder.Default.Encode(chinese);
            var salt = Random.Shared.Next(1000000000);
            var sign = Sign();
            var response = await _httpClient.GetAsync($"{baseUrl}?q={q}&from=zh&to={Common.BaiduLangMapping[lang]}&appid={appId}&salt={salt}&sign={sign}");
            var content = await response.Content.ReadAsStringAsync();
            var json = JsonNode.Parse(content);
            if (json["error_code"] != null)
            {
                _logger.Information($"翻译时出现错误, 中文:{chinese}, 目标语言:{lang}, 错误:{content}");
                return string.Empty;
            }
            else
            {
                return json["trans_result"][0]["dst"].ToString();
            }

            string Sign()
            {
                var text=$"{appId}{chinese}{salt}{key}";
                var hash = _md5.ComputeHash(Encoding.UTF8.GetBytes(text));
                return Convert.ToHexString(hash).ToLower().Replace("-","");
            }
        }


    }
}
