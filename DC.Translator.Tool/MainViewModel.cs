using Serilog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DC.Translator.Tool
{
    public class MainViewModel : BindableBase
    {
        private readonly IMyDialogService _myDialogService;
        private readonly IDialogService _dialogService;
        private string? _dbFilePath;
        private string? _srcCodeFolder;
        private readonly ILogger _logger;
        private readonly BaiduTranslationClient _translationClient;
        private readonly SynchronizationContext _syncContext = SynchronizationContext.Current!;
        public MainViewModel(IMyDialogService dialogService, IDialogService dialogService1,
            BaiduTranslationClient translationClient, ILogger logger)
        {
            _myDialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));
            _dialogService = dialogService1 ?? throw new ArgumentNullException(nameof(dialogService1));
            _logger = logger;
            _translationClient = translationClient;
            Languages = Common.Languages.Keys.ToList();
            SelectDbCmd = new AsyncDelegateCommand(SelectTranslationDb, CanExecuteBase);
            ExtractLiteralCmd = new AsyncDelegateCommand(ExtractLiteralFromSrcFolder, CanExtractExecute);
            SelectSrcFolderCmd = new DelegateCommand(SelectSrcFolder);
            DeleteItemCmd = new AsyncDelegateCommand(Delete, CanChange);
            CreateItemCmd = new DelegateCommand(CreateTranslationItem, CanChange);
            EditItemCmd = new DelegateCommand(EditTranslationtem, CanChange);
            CreateDbCmd = new AsyncDelegateCommand(CreateDb);
            this.PropertyChanged += MainViewModel_PropertyChanged;
            TranByBaiduCmd = new AsyncDelegateCommand(TranByBaidu, CanChange);
        }


        private List<string> _shouldRaiseChangeProperties = [nameof(DbFilePath), nameof(SrcCodeFolder), nameof(IsTranslating), nameof(IsExtracting)];
        private async void MainViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(SelectedLang))
            {
                await OnSelectedLangChanged(SelectedLang);
            }
            if (_shouldRaiseChangeProperties.Contains(e.PropertyName))
            {
                RaiseCanExecuteChanged();
            }
        }

        public AsyncDelegateCommand CreateDbCmd { get; }
        public AsyncDelegateCommand SelectDbCmd { get; }
        public DelegateCommand SelectSrcFolderCmd { get; set; }
        public AsyncDelegateCommand ExtractLiteralCmd { get; }
        public AsyncDelegateCommand DeleteItemCmd { get; }
        public DelegateCommand CreateItemCmd { get; }
        public DelegateCommand EditItemCmd { get; }
        public AsyncDelegateCommand SelectedLangChangedCmd { get; }
        public AsyncDelegateCommand TranByBaiduCmd { get; }

        public string? DbFilePath { get => _dbFilePath; set => SetProperty(ref _dbFilePath, value); }
        public string? SrcCodeFolder { get => _srcCodeFolder; set => SetProperty(ref _srcCodeFolder, value); }
        public ObservableCollection<StaticTranslationItem> StaticItems { get; set; } = new ObservableCollection<StaticTranslationItem>();
        public ObservableCollection<DynamicTranslationItem> DynamicItems { get; set; } = new ObservableCollection<DynamicTranslationItem>();
        private string _selectedLang = "英语";
        public string SelectedLang { get => _selectedLang; set => SetProperty(ref _selectedLang, value); }
        public List<string> Languages { get; set; }
        public DynamicTranslationItem SelectedDynamicItem { get; set; }
        public StaticTranslationItem SelectedStaticItem { get; set; }
        public int SelectedIndex { get; set; } = 0;

        public async Task CreateDb()
        {
            DbFilePath = _myDialogService.SaveFileDialog("请选择翻译数据库目录...");
            if (string.IsNullOrEmpty(DbFilePath)) { return; }
            var repo = new TranslationRepository(DbFilePath);
            await repo.Initialize();
            _myDialogService.Notification("新建数据库成功!");
        }

        public async Task SelectTranslationDb()
        {
            DbFilePath = _myDialogService.OpenFileDialog("请选择翻译数据库...");
            if (string.IsNullOrEmpty(DbFilePath)) { return; }
            var repo = new TranslationRepository(DbFilePath);
            var (valid, message) = await repo.VerifyDb();
            if (!valid)
            {
                _myDialogService.Notification($"请检查所选择的数据是否为翻译数据库: {message}");
            }
            else
            {
                await OnSelectedLangChanged(SelectedLang);
            }
        }

        public async Task OnSelectedLangChanged(string lang)
        {
            if (string.IsNullOrEmpty(DbFilePath)) { return; }
            var repo = new TranslationRepository(DbFilePath);
            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                StaticItems.Clear();
                DynamicItems.Clear();
            });
            var staticItems = await repo.LoadStatic(Common.Languages[lang]);
            foreach (var item in staticItems)
            {
                await Application.Current.Dispatcher.InvokeAsync(() =>
                {
                    StaticItems.Add(item);
                });
            }
            var itemsFromDb = await repo.LoadDynamic(Common.Languages[SelectedLang]);
            foreach (var item in itemsFromDb)
            {
                await Application.Current.Dispatcher.InvokeAsync(() =>
                {
                    DynamicItems.Add(item);
                });
            }
        }

        public void SelectSrcFolder()
        {
            SrcCodeFolder = _myDialogService.OpenFolderDialog("请选择源码数据...");
        }

        private bool _isExtracting = false;
        public bool IsExtracting { get => _isTranslating; set => SetProperty(ref _isTranslating, value); }
        public async Task ExtractLiteralFromSrcFolder()
        {
            if (string.IsNullOrWhiteSpace(DbFilePath))
            {
                _myDialogService.Notification("请首先选择翻译数据!");
                return;
            }
            if (string.IsNullOrEmpty(SrcCodeFolder))
            {
                _myDialogService.Notification("请选择源码所在目录!");
                return;
            }
            _isExtracting = true;
            try
            {
                var scanner = new SourceCodeScanner();
                var textList = await scanner.ScanDir(SrcCodeFolder).ConfigureAwait(false);
                var repo = new TranslationRepository(DbFilePath);
                await repo.Update(textList).ConfigureAwait(false);
            }
            finally
            {
                _isExtracting = false;
            }
            await OnSelectedLangChanged(SelectedLang);
        }

        public void CreateTranslationItem()
        {
            if (string.IsNullOrWhiteSpace(DbFilePath))
            {
                _myDialogService.Notification("请首先选择翻译数据!");
                return;
            }
            if (SelectedIndex == 0)
            {
                CreateStaticItem();
            }
            else
            {
                CreateDynamicItem();
            }

            void CreateStaticItem()
            {
                var item = new StaticTranslationItem();
                var parameters = new DialogParameters
                {
                    { "lang", SelectedLang },
                    { "dbFilePath", DbFilePath },
                    { "item", item },
                    { "edit", false },
                };
                _dialogService.ShowDialog(nameof(CreateEditStaticItemDialog), parameters, (res) =>
                {
                    if (res.Result == ButtonResult.OK)
                    {
                        _myDialogService.Notification("新建翻译条目成功!");
                        StaticItems.Add(item);
                    }
                });
            }

            void CreateDynamicItem()
            {
                var item = new DynamicTranslationItem();
                var parameters = new DialogParameters
            {
                { "lang", SelectedLang },
                { "dbFilePath", DbFilePath },
                { "item", item },
                { "edit", false },
            };
                _dialogService.ShowDialog(nameof(CreateEditDynamicItemDialog), parameters, (res) =>
                {
                    if (res.Result == ButtonResult.OK)
                    {
                        _myDialogService.Notification("新建翻译条目成功!");
                        DynamicItems.Add(item);
                    }
                });
            }

        }
        public void EditTranslationtem()
        {
            if (string.IsNullOrWhiteSpace(DbFilePath))
            {
                _myDialogService.Notification("请首先选择翻译数据!");
                return;
            }

            if (SelectedIndex == 0)
            {
                if (SelectedStaticItem == null)
                {
                    _myDialogService.Notification("请选择要编辑的行！");
                    return;
                }
                EditStaticItem();
            }
            else
            {
                if (SelectedDynamicItem == null)
                {
                    _myDialogService.Notification("请选择要编辑的行！");
                    return;
                }
                EditDynamicItem();
            }

            void EditDynamicItem()
            {
                var item = SelectedDynamicItem;
                var parameters = new DialogParameters
                {
                    { "lang", SelectedLang },
                    { "dbFilePath", DbFilePath },
                    { "item", item },
                    { "edit", true },
                };
                _dialogService.ShowDialog(nameof(CreateEditDynamicItemDialog), parameters, (res) =>
                {
                    if (res.Result == ButtonResult.OK)
                    {
                        DynamicItems.Remove(item);
                        DynamicItems.Insert(0, item);
                        _myDialogService.Notification("编辑条目成功!");
                    }
                });
            }

            void EditStaticItem()
            {
                var item = SelectedStaticItem;
                var parameters = new DialogParameters
                {
                    { "lang", SelectedLang },
                    { "dbFilePath", DbFilePath },
                    { "item", item },
                    { "edit", true },
                };
                _dialogService.ShowDialog(nameof(CreateEditStaticItemDialog), parameters, (res) =>
                {
                    if (res.Result == ButtonResult.OK)
                    {
                        StaticItems.Remove(item);
                        StaticItems.Insert(0, item);
                        _myDialogService.Notification("编辑条目成功!");
                    }
                });
            }
        }

        public async Task Delete()
        {
            if (string.IsNullOrWhiteSpace(DbFilePath))
            {
                _myDialogService.Notification("请首先选择翻译数据!");
                return;
            }
            if ((SelectedIndex == 0 && SelectedStaticItem == null)
                && (SelectedIndex == 1 && SelectedDynamicItem == null))
            {
                _myDialogService.Notification("请首先选中要删除的条目!");
                return;
            }
            if (_myDialogService.Confirm("确定要删除吗?"))
            {
                var repo = new TranslationRepository(DbFilePath);
                await repo.DeleteKey(SelectedIndex == 0 ? SelectedStaticItem.Id : SelectedDynamicItem.Id
                    , SelectedIndex == 0);
                if (SelectedIndex == 0) { StaticItems.Remove(SelectedStaticItem); }
                else { DynamicItems.Remove(SelectedDynamicItem); }
            }
            //_myDialogService.Notification("成功删除!");
        }

        private bool _isTranslating = false;
        public bool IsTranslating { get => _isTranslating; set => SetProperty(ref _isTranslating, value); }
        public async Task TranByBaidu()
        {
            if (string.IsNullOrWhiteSpace(DbFilePath))
            {
                _myDialogService.Notification("请首先选择翻译数据!");
                return;
            }
            IsTranslating = true;
            var db = new TranslationRepository(DbFilePath);
            var lang = Common.Languages[SelectedLang];//将SelectedLang替换为对应column
            var items = await db.LoadStatic(lang, true).ConfigureAwait(false);
            foreach (var item in items)
            {
                var result = await _translationClient.Translate(item.Chinese, lang).ConfigureAwait(false);
                item.Translation = result;
                await db.UpdateStaticItem(item, lang).ConfigureAwait(false);
            }

            var dynItems = await db.LoadDynamic(lang, true).ConfigureAwait(false);
            foreach (var item in dynItems)
            {
                var result = await _translationClient.Translate(item.Chinese, SelectedLang).ConfigureAwait(false);
                item.Translation = result;
                await db.UpdateDynamicItem(item, lang).ConfigureAwait(false);
            }
            if (items.Count + dynItems.Count > 0)
            {
                _myDialogService.Notification("自动翻译完成!");
                await OnSelectedLangChanged(SelectedLang);
            }
            else
            {
                _myDialogService.Notification("没有未翻译的条目!");
            }
            IsTranslating = false;
        }

        private void RaiseCanExecuteChanged()
        {
            _syncContext.Post((_) =>
            {
                DeleteItemCmd.RaiseCanExecuteChanged();
                CreateItemCmd.RaiseCanExecuteChanged();
                EditItemCmd.RaiseCanExecuteChanged();
                TranByBaiduCmd.RaiseCanExecuteChanged();
                ExtractLiteralCmd.RaiseCanExecuteChanged();
            }, null);
        }

        public bool CanExecuteBase()
            => !IsTranslating && !IsExtracting;

        public bool CanChange()
            => !string.IsNullOrEmpty(DbFilePath) && CanExecuteBase();
        public bool CanExtractExecute()
        => !string.IsNullOrEmpty(SrcCodeFolder) && CanExecuteBase();

    }
}
