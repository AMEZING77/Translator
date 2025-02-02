using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DC.Translator.Tool
{
    internal class CreateEditStaticItemVM : BindableBase, IDialogAware
    {
        private StaticTranslationItem _item;
        private string _dbFilePath;

        private string _chinese;
        public string Chinese
        {
            get { return _chinese; }
            set { SetProperty(ref _chinese, value); }
        }

        private string _translation;
        public string Translation
        {
            get { return _translation; }
            set { SetProperty(ref _translation, value); }
        }

        private string _title = "新建翻译项";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private string _lang = "英文";
        public string Language
        {
            get { return _lang; }
            set { SetProperty(ref _lang, value); }
        }

        private DialogCloseListener _dialogCloseListener = new();
        public DialogCloseListener RequestClose { get => _dialogCloseListener; }

        public virtual void RaiseRequestClose(IDialogResult dialogResult)
        {
            RequestClose.Invoke(dialogResult);
        }

        public virtual bool CanCloseDialog()
        {
            return true;
        }

        public virtual void OnDialogClosed()
        {
            _item.Chinese = _chinese;
            _item.Translation = _translation;
            if (!_edit) { _item.InsertTime = DateTime.Now; }
        }

        public AsyncDelegateCommand OkCmd { get; private set; }

        public async Task OK()
        {
            if (string.IsNullOrEmpty(_chinese?.Trim()))
            {
                _dialogService.Notification("中文不能为空!");
                return;
            }
            var repo = new TranslationRepository(_dbFilePath);
            var exists = await repo.CheckKeyExists(_chinese, true, _edit, _item.Id);
            if (exists) { _dialogService.Notification("具体相同中文的翻译条目已存在!"); }
            else
            {
                _item.Translation = _translation;
                _item.Chinese = _chinese;
                if (!_edit)
                {
                    var id = await repo.AddStaticItem(
                        new StaticTranslationItem { Chinese = _chinese, Translation = _translation }
                        , Common.Languages[_lang]);
                    _item.Id = id;
                }
                else
                {
                    await repo.UpdateStaticItem(_item, Common.Languages[_lang]);
                }
                RaiseRequestClose(new DialogResult(ButtonResult.OK));
            }
        }

        public DelegateCommand CancelCmd { get; private set; }

        public void Cancel()
        {
            RaiseRequestClose(new DialogResult(ButtonResult.Cancel));
        }

        private readonly IMyDialogService _dialogService;
        private bool _edit = false;
        public CreateEditStaticItemVM(IMyDialogService myDialogService)
        {
            _dialogService = myDialogService;
            OkCmd = new AsyncDelegateCommand(OK);
            CancelCmd = new DelegateCommand(Cancel);
        }

        public virtual void OnDialogOpened(IDialogParameters parameters)
        {
            Language = parameters.GetValue<string>("lang");
            _item = parameters.GetValue<StaticTranslationItem>("item");
            _dbFilePath = parameters.GetValue<string>("dbFilePath");
            _edit = parameters.GetValue<bool>("edit");
            if (_edit)
            {
                Title = "编辑翻译项";
            }
            Translation = _item.Translation;
            Chinese = _item.Chinese;
        }
    }
}
