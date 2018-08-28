using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NAV_MenuSuiteExplorer.ObjectExplorer;

namespace NAV_MenuSuiteExplorer
{
    public partial class Form1 : Form
    {
        Explorer explorer;
        string recentFilePathsFile = System.IO.Path.GetTempPath() + @"/NAV_ObjectExplorer_Recent.txt";
        List<string> recentFilePathLines;

        public Form1()
        {
            InitializeComponent();
            LoadRecentFilePath(recentFilePathsFile);

        }

        private void ToolStripImport_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string filePath = openFileDialog1.FileName;
                SaveRecentFilePath(filePath);
                explorer = new Explorer(filePath);
            }
        }

        private void SaveRecentFilePath(string filePath)
        {
            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(recentFilePathsFile, false))
            {
                sw.WriteLine(filePath);
                int lineNo = 1;
                foreach (string line in recentFilePathLines)
                {
                    if (line != filePath)
                    {
                        sw.WriteLine(line);
                        lineNo++;
                    }
                    if (recentFilePathLines.Count > 5)
                        break;
                }
            }
        }

        private void LoadRecentFilePath(string FilePath)
        {
            recentFilePathLines = System.IO.File.ReadAllLines(recentFilePathsFile).ToList();
            foreach (string line in recentFilePathLines)
            {
                ToolStripMenuItem item = new ToolStripMenuItem(line);
                item.Click += new EventHandler(MenuItemRecentClickHandler);
                toolStripRecent.DropDownItems.Add(item);
            }
        }

        private void MenuItemRecentClickHandler(object sender, EventArgs e)
        {
            ToolStripMenuItem clickedItem = (ToolStripMenuItem)sender;
            SaveRecentFilePath(clickedItem.Text);
            explorer = new Explorer(clickedItem.Text);
        }
    }
}
