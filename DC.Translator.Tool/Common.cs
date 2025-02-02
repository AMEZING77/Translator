using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DC.Translator.Tool
{
    internal class Common
    {

        public static readonly IReadOnlyDictionary<string,string> Languages = new Dictionary<string,string>{
            ["中文"] = "chinese",
            ["英语"] = "english",
            ["德语"] = "german",
            ["俄语"] = "russian",
            ["韩语"] = "korean",
            ["日语"] = "japanese",
            ["法语"] = "french",
            ["意大利语"] = "italian",
            ["越南语"] = "vietnamese",
            ["葡萄牙语"] = "portuguese",
            ["西班牙语"] = "spainish",
            ["中文-繁体"] = "traditional_chinese",
        };

        public static readonly IReadOnlyDictionary<string, string> BaiduLangMapping = new Dictionary<string, string>
        {
            ["english"] = "en",
            ["japanese"] = "jp",
            ["russian"] = "ru",
            ["german"] = "de",
            ["korean"] = "kor",
            ["french"] = "fra",
            ["italian"] = "it",
            ["vietnamese"] = "vie",
            ["portuguese"] = "pt",
            ["spanish"] = "spa",
            ["traditional_chinese"] = "cht",
        };
    }
}
