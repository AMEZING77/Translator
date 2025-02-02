using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Collections;
using System.Net.Http;
using System.Security.Cryptography;
using System.Web;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Xml;
using System.Data;
using System.Windows.Forms;
using Serilog;
using static System.Net.Mime.MediaTypeNames;

namespace DC.Translator
{
    /// <summary>
    /// 三维语言翻译器
    /// </summary>
    public partial class MultiLangTranslator
    {

        /// <summary>
        /// 字体大小自动转换比例
        /// </summary>
        internal AutoSizeFont fontSizeAutoFormate;

        private readonly TranslationDbRepository _dbRepository = new TranslationDbRepository();
        private readonly ILogger _logger = Serilog.Log.Logger;
        private readonly HashSet<string> _notExistedKey = new HashSet<string>();
        /// <summary>
        /// 本地实例
        /// </summary>
        public static MultiLangTranslator Instance
        {
            get { return getInstance(); }
        }

        /// <summary>
        /// 静态内部类实现单例模式
        /// </summary>
        private static class InternalTranslator
        {
            internal static readonly MultiLangTranslator sun = new MultiLangTranslator();
        }

        private MultiLangTranslator()
        {
            var path = Path.GetFullPath(TranslationDbRepository.dbFileName);
            var dir = Path.GetPathRoot(path);
            Directory.CreateDirectory(dir);
            new TranslationDbMigration(_logger, TranslationDbRepository.dbConnString).Migrate();
        }

        private static MultiLangTranslator getInstance()
        {
            return InternalTranslator.sun;
        }

        /// <summary>
        /// 获取或设置内置的源语种,翻译前文本的源语种
        /// </summary>
        public LanguageType SourceLang { get; set; } = LanguageType.Chinese;
        /// <summary>
        /// 获取或设置内置的源语种,目标翻译语种
        /// </summary>
        public LanguageType TargetLang { get; set; } = LanguageType.Chinese;
        

        internal string TransFormToLanguage(string content, IReadOnlyDictionary<string, string> dict)
        {
            if (dict.ContainsKey(content))
            {
                if (!string.IsNullOrEmpty(dict[content]))
                {
                    return dict[content];
                }
                _logger.Information($"翻译模块, 字符串【{content}】对应的翻译内容不存在!");
                return content;
            }
            else
            {
                _notExistedKey.Add(content);
                _logger.Information($"翻译模块, 字符串【{content}】不存在!");
                return content;
            }
        }

        /// <summary>
        /// 自动改变控件字体的大小
        /// </summary>
        /// <param name="control">控件</param>
        /// <param name="src">源语言</param>
        /// <param name="tar">目标语言</param>
        internal System.Drawing.Font AutoChangeLableSize(Control control, LanguageType src, LanguageType tar)
        {
            if (control is Label) //如果是Lable控件
            {
                System.Drawing.Font font = control.Font;
                float recover = 1; //复原倍率
                float radio = 1; //转化倍率
                switch (src)
                {
                    case LanguageType.Chinese:
                        recover = fontSizeAutoFormate.ChineseFont;
                        break;
                    case LanguageType.English:
                        recover = fontSizeAutoFormate.EnglishFont;
                        break;
                    case LanguageType.German:
                        recover = fontSizeAutoFormate.GermanFont;
                        break;
                    case LanguageType.Rassian:
                        recover = fontSizeAutoFormate.RassianFont;
                        break;
                    case LanguageType.Korean:
                        recover = fontSizeAutoFormate.KoreanFont;
                        break;
                    case LanguageType.Japanese:
                        recover = fontSizeAutoFormate.JapaneseFont;
                        break;
                    case LanguageType.French:
                        recover = fontSizeAutoFormate.FrenchFont;
                        break;
                    case LanguageType.Italian:
                        recover = fontSizeAutoFormate.ItalianFont;
                        break;
                    case LanguageType.Vietnam:
                        recover = fontSizeAutoFormate.VietnamFont;
                        break;
                    case LanguageType.Portugal:
                        recover = fontSizeAutoFormate.PortugalFont;
                        break;
                    case LanguageType.Spain:
                        recover = fontSizeAutoFormate.SpainFont;
                        break;
                    case LanguageType.TraditionalChinese:
                        recover = fontSizeAutoFormate.TraditionalChineseFont;
                        break;
                    default:
                        break;
                }
                switch (tar)
                {
                    case LanguageType.Chinese:
                        radio = fontSizeAutoFormate.ChineseFont;
                        break;
                    case LanguageType.English:
                        radio = fontSizeAutoFormate.EnglishFont;
                        break;
                    case LanguageType.German:
                        radio = fontSizeAutoFormate.GermanFont;
                        break;
                    case LanguageType.Rassian:
                        radio = fontSizeAutoFormate.RassianFont;
                        break;
                    case LanguageType.Korean:
                        radio = fontSizeAutoFormate.KoreanFont;
                        break;
                    case LanguageType.Japanese:
                        radio = fontSizeAutoFormate.JapaneseFont;
                        break;
                    case LanguageType.French:
                        radio = fontSizeAutoFormate.FrenchFont;
                        break;
                    case LanguageType.Italian:
                        radio = fontSizeAutoFormate.ItalianFont;
                        break;
                    case LanguageType.Vietnam:
                        radio = fontSizeAutoFormate.VietnamFont;
                        break;
                    case LanguageType.Portugal:
                        radio = fontSizeAutoFormate.PortugalFont;
                        break;
                    case LanguageType.Spain:
                        radio = fontSizeAutoFormate.SpainFont;
                        break;
                    case LanguageType.TraditionalChinese:
                        radio = fontSizeAutoFormate.TraditionalChineseFont;
                        break;
                    default:
                        break;
                }
                return new System.Drawing.Font(font.FontFamily, (font.Size / recover) * radio, font.Style);
            }
            return control.Font;
        }


        /// <summary>
        /// 根据中文形式的语言类型名返回对应的语言类型枚举
        /// </summary>
        /// <param name="languageName"></param>
        /// <returns></returns>
        private LanguageType ReturnKingEnum(string languageName)
        {
            LanguageType kind;
            switch (languageName)
            {
                case "中文":
                    kind = LanguageType.Chinese;
                    break;
                case "英文":
                    kind = LanguageType.English;
                    break;
                case "德文":
                    kind = LanguageType.German;
                    break;
                case "俄文":
                    kind = LanguageType.Rassian;
                    break;
                case "韩文":
                    kind = LanguageType.Korean;
                    break;
                case "日文":
                    kind = LanguageType.Japanese;
                    break;
                case "法文":
                    kind = LanguageType.French;
                    break;
                case "意大利文":
                    kind = LanguageType.Italian;
                    break;
                case "繁体中文":
                    kind = LanguageType.TraditionalChinese;
                    break;
                default:
                    kind = LanguageType.Chinese;
                    break;
            }
            return kind;
        }
    }

    /// <summary>
    /// 字体自动变换比例
    /// </summary>
    public class AutoSizeFont
    {
        /// <summary>
        /// 翻译后中文 字体自动变化为初始字体的比例,默认值1
        /// </summary>
        public float ChineseFont { get; set; } = 1;
        /// <summary>
        /// 翻译后英文 字体自动变化为初始字体的比例,默认值0.8
        /// </summary>
        public float EnglishFont { get; set; } = 0.8f;
        /// <summary>
        /// 翻译后德文 字体自动变化为初始字体的比例,默认值0.75
        /// </summary>
        public float GermanFont { get; set; } = 0.75f;
        /// <summary>
        /// 翻译后俄文 字体自动变化为初始字体的比例,默认值1
        /// </summary>
        public float RassianFont { get; set; } = 1;
        /// <summary>
        /// 翻译后韩文 字体自动变化为初始字体的比例,默认值1
        /// </summary>
        public float KoreanFont { get; set; } = 1;
        /// <summary>
        /// 翻译后日文 字体自动变化为初始字体的比例,默认值1
        /// </summary>
        public float JapaneseFont { get; set; } = 1;
        /// <summary>
        /// 翻译后法文 字体自动变化为初始字体的比例,默认值1
        /// </summary>
        public float FrenchFont { get; set; } = 1;
        /// <summary>
        /// 翻译后意大利文 字体自动变化为初始字体的比例,默认值1
        /// </summary>
        public float ItalianFont { get; set; } = 1;
        /// <summary>
        /// 翻译后越南文 字体自动变化为初始字体的比例,默认值1
        /// </summary>
        public float VietnamFont { get; set; } = 1;
        /// <summary>
        /// 翻译后葡萄牙文 字体自动变化为初始字体的比例,默认值1
        /// </summary>
        public float PortugalFont { get; set; } = 1;
        /// <summary>
        /// 翻译后西班牙文 字体自动变化为初始字体的比例,默认值1
        /// </summary>
        public float SpainFont { get; set; } = 1;
        /// <summary>
        /// 翻译后繁体中文 字体自动变化为初始字体的比例,默认值1
        /// </summary>
        public float TraditionalChineseFont { get; set; } = 1;
    }
}
