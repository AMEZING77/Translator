# Translator (DC.Translator)

多语言翻译器库，支持多种语言的翻译和字体自动调整功能。

## 📚 项目简介

DC.Translator是一个多语言翻译器库，提供了统一的多语言翻译接口，支持中文、英文、德文、俄文、韩文、日文、法文、意大利文、越南文、葡萄牙文、西班牙文、繁体中文等多种语言。同时提供了字体自动调整功能，可以根据不同语言自动调整控件字体大小。

## 🎯 主要功能

- **多语言支持**: 支持13种语言的翻译
- **数据库存储**: 使用SQLite数据库存储翻译数据
- **字体自动调整**: 根据语言类型自动调整控件字体大小
- **WPF工具界面**: 提供可视化的翻译管理工具
- **单例模式**: 使用单例模式确保全局唯一实例
- **扩展方法**: 提供便捷的扩展方法简化翻译调用

## 🛠️ 技术栈

- **语言**: C#
- **框架**: 
  - .NET Framework（主库）
  - WPF（工具界面）
- **数据库**: SQLite
- **ORM**: Entity Framework
- **日志**: Serilog
- **序列化**: JSON

## 📁 项目结构

```
Translator/
├── DC.Translator/                    # 核心翻译库
│   ├── MultiLangTranslator.cs       # 多语言翻译器主类
│   ├── TranslationDbRepository.cs   # 翻译数据仓储
│   ├── TranslationDbMigration.cs    # 数据库迁移
│   ├── TranslatorExcuter.cs        # 翻译执行器
│   ├── TranslatorExtension.cs       # 翻译扩展方法
│   ├── LanguageType.cs              # 语言类型枚举
│   ├── EntryEntity.cs               # 翻译条目实体
│   └── ...
├── DC.Translator.Test/              # 测试项目
│   ├── LanguageApply.cs             # 语言应用测试界面
│   └── ...
├── DC.Translator.Tool/              # WPF管理工具
│   ├── MainWindow.xaml              # 主窗口
│   ├── MainViewModel.cs             # 主视图模型
│   ├── BaiduTranslationClient.cs    # 百度翻译客户端
│   ├── SourceCodeScanner.cs         # 源代码扫描器
│   └── ...
├── DC.Translator.sln                # 解决方案文件
└── packages/                        # NuGet包目录
```

## 🚀 快速开始

### 环境要求

- Visual Studio 2019或更高版本
- .NET Framework 4.x
- SQLite数据库

### 安装步骤

1. **克隆或下载项目**

2. **还原NuGet包**

   右键解决方案 -> 还原NuGet包

3. **编译项目**

   使用Visual Studio打开 `DC.Translator.sln`，编译解决方案。

### 基本使用

```csharp
// 获取翻译器实例
var translator = MultiLangTranslator.Instance;

// 设置源语言和目标语言
translator.SourceLang = LanguageType.Chinese;
translator.TargetLang = LanguageType.English;

// 翻译文本
string translated = translator.Translate("你好");

// 使用扩展方法翻译控件
label.Translate(LanguageType.Chinese, LanguageType.English);

// 自动调整字体大小
translator.fontSizeAutoFormate = new AutoSizeFont
{
    ChineseFont = 1.0f,
    EnglishFont = 0.8f,
    GermanFont = 0.75f
    // ... 其他语言字体比例
};
```

## 📖 核心类说明

### MultiLangTranslator

多语言翻译器主类，采用单例模式：

- `Instance`: 获取单例实例
- `SourceLang`: 源语言类型
- `TargetLang`: 目标语言类型
- `fontSizeAutoFormate`: 字体自动调整配置

### LanguageType

支持的语言类型：

- `Chinese` - 中文
- `English` - 英文
- `German` - 德文
- `Rassian` - 俄文
- `Korean` - 韩文
- `Japanese` - 日文
- `French` - 法文
- `Italian` - 意大利文
- `Vietnam` - 越南文
- `Portugal` - 葡萄牙文
- `Spain` - 西班牙文
- `TraditionalChinese` - 繁体中文

### AutoSizeFont

字体自动调整配置类，可以为每种语言设置字体大小比例。

## 🛠️ 工具使用

### DC.Translator.Tool

WPF管理工具提供了以下功能：

- **翻译条目管理**: 添加、编辑、删除翻译条目
- **源代码扫描**: 扫描源代码中的文本，自动提取需要翻译的内容
- **批量翻译**: 使用百度翻译API进行批量翻译
- **数据库管理**: 管理翻译数据库

### 使用工具

1. 运行 `DC.Translator.Tool` 项目
2. 在主界面中管理翻译条目
3. 使用源代码扫描功能提取文本
4. 使用批量翻译功能自动翻译

## 📝 数据库结构

翻译数据存储在SQLite数据库中，包含以下表：

- **TranslationEntries**: 翻译条目表
  - Key: 翻译键
  - SourceLanguage: 源语言
  - TargetLanguage: 目标语言
  - Translation: 翻译内容

## 🔧 配置说明

### 数据库配置

数据库文件路径在 `TranslationDbRepository` 中配置，默认使用SQLite数据库。

### 字体配置

通过 `AutoSizeFont` 类配置各语言的字体大小比例：

```csharp
var fontSizeConfig = new AutoSizeFont
{
    ChineseFont = 1.0f,      // 中文字体比例
    EnglishFont = 0.8f,      // 英文字体比例
    GermanFont = 0.75f,      // 德文字体比例
    // ... 其他语言
};
```

## 🧪 测试

项目包含测试项目 `DC.Translator.Test`，提供了语言应用测试界面，可以测试翻译功能和字体调整功能。

## 📦 NuGet包

主要依赖的NuGet包：

- `DC.Common2` - 通用库
- `Serilog` - 日志库
- `System.Data.SQLite` - SQLite数据库支持
- `EntityFramework` - ORM框架

## 📄 许可证

详见 [LICENSE](LICENSE) 文件

## 👤 作者

AMEZING77

## 📅 创建时间

2024年

## 🔗 相关资源

- [Entity Framework文档](https://docs.microsoft.com/ef/)
- [Serilog文档](https://serilog.net/)
