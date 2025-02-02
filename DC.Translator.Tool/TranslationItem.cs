using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DC.Translator.Tool
{
    public class StaticTranslationItem
    {
        [DisplayName("序号")]
        public int Id { get; set; }
        [DisplayName("中文")]
        public string Chinese { get; set; }
        [DisplayName("翻译")]
        public string Translation { get; set; }
        [DisplayName("插入时间")]
        public DateTime InsertTime { get; set; }
    }

    public class DynamicTranslationItem : StaticTranslationItem
    {
        [DisplayName("变量名")] public string Key { get; set; }
    }
}
