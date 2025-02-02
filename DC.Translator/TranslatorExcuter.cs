using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DC.Translator
{
    /// <summary>
    /// 对于MultiLangTranslator的扩展方法
    /// </summary>
    public static class TranslatorExcuter
    {
        /// <summary>
        /// 所有的多语言翻译通过这个方法关联，方便统一管理
        /// </summary>
        /// <remarks>由于子界面初始化时都默认是中文，所以封装此方法</remarks>
        /// <param name="action"></param>
        /// <param name="source"></param>
        /// <param name="dst"></param>
        public static void TranslateRegion(Action action, LanguageType source = LanguageType.None, LanguageType dst = LanguageType.None)
        {
            if (source != LanguageType.None)
            {
                MultiLangTranslator.Instance.SourceLang = source;
            }
            if (dst != LanguageType.None)
            {
                MultiLangTranslator.Instance.TargetLang = dst;
            }
            //使用Invoke，避免异步修改SourceLang和TargetLang，导致前面未生效的翻译异常
            action.Invoke();
        }
        /// <summary>
        /// 拓展翻译文本
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string TranslateText(this string text)
        {
            return MultiLangTranslator.Instance.TransText(text);
        }
        /// <summary>
        /// 拓展翻译winform控件
        /// </summary>
        /// <param name="control"></param>
        /// <returns></returns>
        public static bool TranslateControl(this System.Windows.Forms.Control control)
        {
            return MultiLangTranslator.Instance.TranslateControl(control);
        }

    }
}
