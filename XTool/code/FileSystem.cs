using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XTool
{
    public static class FileSystem
    {


        public static bool SolicitFilepath(out string filePath)
        {
            return SolicitFilepath(String.Empty, String.Empty, String.Empty, out filePath);
        }

        public static bool SolicitFilepath(string pattern, string extension, string defaultDirectory, out string filePath)
        {
            bool b = false;
            filePath = String.Empty;
            Microsoft.Win32.SaveFileDialog dialog = new Microsoft.Win32.SaveFileDialog();
            if (!String.IsNullOrEmpty(pattern))
            {
                dialog.Filter = pattern;
            }
            if (!String.IsNullOrEmpty(extension))
            {
                dialog.DefaultExt = extension;
            }
            if (!String.IsNullOrEmpty(defaultDirectory))
            {
                dialog.InitialDirectory = defaultDirectory;
            }
            Nullable<bool> result = dialog.ShowDialog();
            if (result == true)
            {
                filePath = dialog.FileName;
                b = true;
            }

            return b;

        }
        public static bool SolicitFolderpath(out string folderPath, bool showMakeFolder)
        {
            bool b = false;
            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
            dialog.ShowNewFolderButton = showMakeFolder;
            var result = dialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                folderPath = dialog.SelectedPath;
                b = true;
            }
            else
            {
                folderPath = String.Empty;
            }
            return b;
        }

        
    }
}
