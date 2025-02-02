using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DC.Translator
{
    /// <summary>
    /// 翻译器的扩展方法
    /// </summary>
    public partial class MultiLangTranslator
    {
        /// <summary>
        /// 翻译一个控件的范围内的所有控件的Text属性(您务必指定源语种，否则默认为汉语)
        /// </summary>
        /// <param name="control">指定的控件</param>
        /// <param name="dict">源语种</param>
        private void TranslateControlDescedants(Control control, IReadOnlyDictionary<string, string> dict)
        {
            control.Text = TransFormToLanguage(control.Text, dict);
            if (control.Controls.Count == 0) { return; }
            foreach (Control item in control.Controls)
            {
                MenuStrip menuStrip = item as MenuStrip;
                if (menuStrip != null) { TranslateMenuItem(menuStrip, dict); }
                else { TranslateControlDescedants(item, dict); }
            }
        }

        /// <summary>
        /// 翻译一个菜单控件的范围内的所有菜单控件的Text属性
        /// </summary>
        /// <param name="menuStrip">指定的菜单控件</param>
        /// <param name="dict">翻译字典</param>
        private void TranslateMenuItem(MenuStrip menuStrip, IReadOnlyDictionary<string, string> dict)
        {
            foreach (ToolStripMenuItem item in menuStrip.Items)
            {
                if (string.IsNullOrEmpty(item.Text)) { continue; }
                item.Text = TransFormToLanguage(item.Text, dict);
                foreach (ToolStripItem item2 in item.DropDownItems)
                {
                    if (string.IsNullOrEmpty(item2.Text)) { continue; }
                    item2.Text = TransFormToLanguage(item2.Text, dict);
                }
            }
        }

        /// <summary>
        /// 翻译一个控件极其所有子控件
        /// </summary>
        /// <param name="control">控件</param>
        public bool TranslateControl(Control control)
        {
            try
            {
                var dictionary = _dbRepository.LoadStatic(SourceLang, TargetLang);
                TranslateControlDescedants(control, dictionary);
                _dbRepository.AddStaticKey(_notExistedKey, SourceLang);
                _notExistedKey.Clear();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "翻译时出现未知错误");
                return false;
            }
            return true;
        }

        /// <summary>
        /// 翻译一个文本
        /// </summary>
        /// <param name="text">指定的源文本</param>
        /// <param name="src">源语种</param>
        /// <param name="target">目标语种</param>
        public string TransText(string text, LanguageType src, LanguageType target)
        {
            var (exists, result) = _dbRepository.FindStatic(text, src, target);
            if (!exists)
            {
                _logger.Information($"翻译模块, 静态字符串【{text}】不存在!");
                _dbRepository.AddStaticKey(new[] { text }, SourceLang);
            }
            if (string.IsNullOrEmpty(result))
            {
                _logger.Information($"翻译模块, 静态字符串【{text}】对应的翻译内容不存在!");
                result = text;
            }
            return result;
        }

        /// <summary>
        /// 快速翻译目标文本到指定的语言，如果翻译失败则返回原文
        /// </summary>
        /// <param name="text">目标文本</param>
        /// <returns></returns>
        public string TransText(string text)
        => TransText(text, SourceLang, TargetLang);

        /// <summary>
        /// 翻译内插字符串
        /// </summary>
        /// <param name="key">变量名</param>
        /// <param name="args">动态对象集合</param>
        /// <returns></returns>
        public string InterpolateTran(string key, params object[] args)
        {
            var (exists, str) = _dbRepository.FindDynamic(key, TargetLang);
            if (!exists)
            {
                _logger.Information($"翻译模块, 动态字符串【{key}】不存在!");
                _dbRepository.AddDynamicKey(key);
            }
            return string.IsNullOrEmpty(str) ? $"{key}对应的翻译不存在，请填补完整!" : string.Format(str, args);
        }
    }
}
