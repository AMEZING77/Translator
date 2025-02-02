using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DC.Translator.Tool
{
    public interface IMyDialogService
    {
        string? OpenFileDialog(string title);
        string? OpenFolderDialog(string title);
        string? SaveFileDialog(string title);
        void Notification(string message);
        bool Confirm(string message);
    }

    public class MyDialogService : IMyDialogService
    {
        public void Notification(string message)
        {
            MessageBox.Show(message, "提示", MessageBoxButton.OK);
        }

        public bool Confirm(string message)
        {
            var res = MessageBox.Show(message, "确认", MessageBoxButton.OKCancel);
            return res == MessageBoxResult.OK;
        }

        public string? OpenFileDialog(string title)
        {
            var dialog = new OpenFileDialog();
            dialog.Multiselect = false;
            dialog.RestoreDirectory = false;
            dialog.Filter = "All Files|*.*;";
            if (dialog.ShowDialog() == true) { return dialog.FileName; }
            return null;
        }

        public string? OpenFolderDialog(string title)
        {
            var dialog = new OpenFolderDialog();
            dialog.Multiselect = false;
            if (dialog.ShowDialog() == true) { return dialog.FolderName; }
            return null;
        }

        public string? SaveFileDialog(string title)
        {
            var dialog = new SaveFileDialog();
            dialog.RestoreDirectory = false;
            dialog.Filter = "All Files|*.*;";
            if (dialog.ShowDialog() == true) { return dialog.FileName; }
            return null;
        }
    }
}
