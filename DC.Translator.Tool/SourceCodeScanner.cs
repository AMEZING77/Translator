using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace DC.Translator.Tool
{
    internal partial class SourceCodeScanner
    {
        public SourceCodeScanner()
        {

        }


        public async Task<HashSet<(string, bool)>> ScanDir(string directory)
        {
            var designerFiles = Directory.GetFiles(directory, "*.cs", SearchOption.AllDirectories);
            var result = new HashSet<(string, bool)>(512);
            foreach (var srcFile in designerFiles)
            {
                if (srcFile.EndsWith("Designer.cs"))
                { await ExtractLiteralFromDesignerFile(srcFile, result); }
                else { }
            }
            return result;
        }


        private async Task ExtractLiteralFromDesignerFile(string scrFile, HashSet<(string, bool)> result)
        {
            var fileContent = await File.ReadAllTextAsync(scrFile);

            Regex stringRegex = DesignerTextRegex();

            // 提取所有字符串字面量
            var matches = stringRegex.Matches(fileContent);
            Regex chineseRegex = ChineseRegex();
            foreach (Match match in matches)
            {
                string str = match.Value;
                // 去掉字符串的引号
                str = str[8..].TrimEnd('"');
                if (!string.IsNullOrEmpty(str) && !string.IsNullOrWhiteSpace(str)
                    && chineseRegex.IsMatch(str))
                { result.Add((str, true)); }
            }
        }

        private async Task ExtractDynamicLiteral(string scrFile, HashSet<(string, bool)> result)
        {
            var fileContent = await File.ReadAllTextAsync(scrFile);
            // 正则表达式：匹配注释
            Regex commentRegex = CommentRegex();
            // 去掉注释
            string codeWithoutComments = commentRegex.Replace(fileContent, string.Empty);
            var literalRegex = TextRegex();
            var matches = literalRegex.Matches(fileContent);
            Regex chineseRegex = ChineseRegex();

            foreach (Match match in matches)
            {
                string str = match.Value;
                // 去掉字符串的引号
                str = str.Trim('"').Replace("\"\"", "\"");

                // 查找中文字符
                if (chineseRegex.IsMatch(str))
                {
                    result.Add((str, false));
                }

            }
        }


        [GeneratedRegex(@"(?<!@)""[^""]*""|@\(""[^""]*""\)", RegexOptions.Singleline)]
        private static partial Regex TextRegex();
        [GeneratedRegex(@"Text = (?<!@)""[^""]*""|@\(""[^""]*""\)", RegexOptions.Singleline)]
        private static partial Regex DesignerTextRegex();
        [GeneratedRegex(@"//.*?$|/\*[\s\S]*?\*/", RegexOptions.Multiline | RegexOptions.Singleline)]
        private static partial Regex CommentRegex();
        [GeneratedRegex(@"[\u4e00-\u9fff]+")]
        private static partial Regex ChineseRegex();
    }
}
