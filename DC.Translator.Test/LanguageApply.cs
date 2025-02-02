using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DC.Translator;

namespace LanguageApplyTest
{
    /// <summary>
    /// 作测试用
    /// </summary>
    internal partial class LanguageApply : Form
    {
        int BeforeSelected = 0;
        System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
        string directory = "";
        public LanguageApply()
        {
            InitializeComponent();
            if (AppDomain.CurrentDomain.BaseDirectory.IndexOf(@"Debug\") != -1)
            {
                directory = AppDomain.CurrentDomain.BaseDirectory.Remove(AppDomain.CurrentDomain.BaseDirectory.IndexOf(@"Debug\")) + @"Settings\";
            }
            else
            {
                directory = AppDomain.CurrentDomain.BaseDirectory + @"Settings\";
            }
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            MultiLangTranslator.Instance.SourceLang = LanguageType.Chinese;
            MultiLangTranslator.Instance.ChangeLanguage(LanguageType.English);
            var str = MultiLangTranslator.Instance.TransText("测试");
            var str1 = MultiLangTranslator.Instance.TransText("哈哈");
            var str2 = MultiLangTranslator.Instance.TransText("嘻嘻");
            var str3 = "测试".TranslateText();
            var bIsOk = (this as Control).TranslateControl();


            //MultiLangTranslator.Instance. = LanguageType.English;
            // -普通测试1
            //string transPathFBCS = directory + "1006版闭环文本翻译标准件.txt";
            //string source1 = "辊压机PLC连接状态：";
            //source1 = source1.FastTransText(transPathFBCS);

            // -普通测试2
            //string source2 = "操作";
            //string transPathFBCS2 = directory + "1006版闭环控件翻译标准件.ini";
            //source2 = source2.FastTransText(transPathFBCS2, IntervalMode.Comma);

            //转义字符测试
            //string source3 = "辊压机PLC连接状态：";
            //string transPathTransfer = directory + "词条转义符测试.txt";
            //source3 = source3.FastTransText(transPathTransfer, IntervalMode.EscapeCharacter);

            //新的模块 - 性能测试
            //long minis = TestUnit(source, transPathFBCS, 10000);

            //字体大小测试
            //(this as Control).TransRangeControlWithIni(LanguageKind.Chinese, LanguageKind.English, transPathFBCS,false,true);

            //内插字符串测试
            //string name = "周子杰、朱杰、孙伟";
            //string food = " balana";
            //string dynamicStr = $"我的名字是{name}";
            //string dynamicStr2 = $"我喜欢吃{food}";
            //string transPathSunWe = directory + "内插字符串测试.sunwe";
            //string result = dynamicStr2.InterpolationTrans("sunsun", LanguageKind.English, transPathSunWe, food);
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            string csvPath = directory + "翻译测试2.csv";

        }

        private void comboLangList_SelectedIndexChanged(object sender, EventArgs e)
        {
            BeforeSelected = comboLangList.SelectedIndex;
            richTextTranslation.Text = null;
        }

        ///// <summary>
        ///// 单元测试
        ///// </summary>
        ///// <param name="input">输入文本</param>
        ///// <param name="path">翻译文件路径</param>
        ///// <param name="maxCount">最大翻译次数后停止</param>
        ///// <returns>翻译完成后消耗的时间,单位ms</returns>
        //private long TestUnit(string input, string path, long maxCount)
        //{
        //    //string transPathApp = directory + "Translate.ini";
        //    //string transPathFBCS = directory + "1006版闭环文本翻译标准件.txt";
        //    //System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
        //    //stopwatch.Restart();
        //    //long count = 0;
        //    //while (count <= maxCount)
        //    //{
        //    //    count++;
        //    //    string result = input.FastTransText(transPathFBCS);
        //    //}
        //    //stopwatch.Stop();
        //    //return stopwatch.ElapsedMilliseconds;
        //}

    }
}
