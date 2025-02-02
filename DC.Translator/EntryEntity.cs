using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace DC.Translator
{
    /// <summary>
    /// 表示翻译文件的一些关键信息
    /// </summary>
    internal class EntryEntity
    {
        /// <summary>
        /// 上一次文件最后写入的时间
        /// </summary>
        internal string lastWriteTime;
        /// <summary>
        /// 当前文件的间隔符方式
        /// </summary>
        internal IntervalMode intervalMode;
        /// <summary>
        /// 语言类型表头
        /// </summary>
        internal LanguageType[] languageHeads;
        /// <summary>
        /// 每一行的翻译内容
        /// </summary>
        internal List<string[]> contextList;
        /// <summary>
        /// (源词条, 源词条坐标) / (ID, ID坐标)
        /// </summary>
        internal Dictionary<string, EntryAdress> entryToAdressDic;        
    }    
}
