using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DC.Translator
{
    /// <summary>
    /// 非动态内插字符串的翻译文件，支持间隔符的方式
    /// </summary>
    public enum IntervalMode
    {
        /// <summary>
        /// 普通的英文逗号，如果你的翻译文件的间隔符是','，请选择此枚举。
        ///  注意：如果词条中存在额外的',',将可能会引发翻译不准确的问题
        /// </summary>
        Comma,
        /// <summary>
        /// 特定的转义符，如果你的翻译文件的间隔符是#swsp，请选择此枚举。
        /// 旨在用于消除掉 Comma 枚举下的翻译不准确的问题
        /// </summary>
        EscapeCharacter,
    }
}
