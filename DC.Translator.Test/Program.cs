using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LanguageApplyTest
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new LanguageApply());

            //String Str = SunWeTranslator.Instance.ChianseToEnglish("专业的");
            //Console.Write(Str);
            //Console.Read();
        }
    }
}
