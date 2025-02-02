using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DC.Translator
{
    /// <summary>
    /// 当前模块支持语种系列
    /// </summary>
    public enum LanguageType
    {
        /// <summary>
        /// 中间值
        /// </summary>
        None = 99,
        /// <summary>
        /// 简体中文
        /// </summary>
        Chinese = 0,
        /// <summary>
        /// 英文
        /// </summary>
        English,
        /// <summary>
        /// 德文
        /// </summary>
        German,
        /// <summary>
        /// 俄文
        /// </summary>
        Rassian,
        /// <summary>
        /// 韩文
        /// </summary>
        Korean,
        /// <summary>
        /// 日本文
        /// </summary>
        Japanese,
        /// <summary>
        /// 法文
        /// </summary>
        French,
        /// <summary>
        /// 意大利文
        /// </summary>
        Italian,
        /// <summary>
        /// 越南文
        /// </summary>
        Vietnam,
        /// <summary>
        /// 葡萄牙文
        /// </summary>
        Portugal,
        /// <summary>
        /// 西班牙文
        /// </summary>
        Spain,
        /// <summary>
        /// 繁体中文
        /// </summary>
        TraditionalChinese,
    }
}
