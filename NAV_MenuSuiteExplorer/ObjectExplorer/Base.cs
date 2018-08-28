using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NAV_MenuSuiteExplorer.ObjectExplorer
{
    class Base
    {
        protected int no;
        protected string name;
        protected string type;
        protected List<string> lines;

        //enum objectType { Table, Page, Codeunit, Query, Report, XMLport, MenuSuite };

        public Base(string headerLine)
        {
            SetHeaderInfo(headerLine);
        }

        private bool SetHeaderInfo(string headerLine)
        {
            Regex rgxHeader = new Regex(@"^OBJECT (\w+) (\d+) (.+)$");
            Match match = rgxHeader.Match(headerLine);
            if (match.Success & match.Groups.Count == 4)
            {
                type = match.Groups[1].Value;
                no = Int32.Parse(match.Groups[2].Value);
                name = match.Groups[3].Value;
                return true;
            }
            return false;
        }

        public void SetLines(string inputContent)
        {
            this.lines = inputContent.Split(new[] { Environment.NewLine }, StringSplitOptions.None).ToList();
        }

        public virtual void Explore()
        {
            // This function should be overrided by any child class
        }
    }
}
