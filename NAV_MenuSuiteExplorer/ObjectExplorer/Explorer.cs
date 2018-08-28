using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NAV_MenuSuiteExplorer.ObjectExplorer
{
    class Explorer
    {
        public Explorer(string filePath)
        {
            using (StreamReader sr = new StreamReader(filePath))
            {
                int lineNo = 0;
                string line;
                string content = string.Empty;
                Base currObject = null;
                StringBuilder builder = new StringBuilder();
                StringWriter writer = null;

                do
                {
                    lineNo++;
                    line = sr.ReadLine();

                    // close previous object
                    if (currObject != null && (line.StartsWith("OBJECT ") || sr.EndOfStream))
                    {
                        currObject.SetLines(builder.ToString());
                        currObject.Explore();
                    }

                    // start new object
                    if (line.StartsWith("OBJECT "))
                    {
                        switch (GetObjectType(line))
                        {
                            // Table, Page, Codeunit, Query, Report, XMLport 
                            case "MenuSuite":
                                currObject = new MenuSuite(line);
                                break;
                            default:
                                throw new Exception("Object type " + GetObjectType(line) + " not recognized");
                        }
                        //currObject = new Base(line);
                        builder.Clear();
                        writer = new StringWriter(builder);
                    }

                    // write object content
                    writer.WriteLine(line);

                } while (!sr.EndOfStream);
            }
        }

        private string GetObjectType(string headerLine)
        {
            Regex rgxHeader = new Regex(@"^OBJECT (\w+) (\d+) (.+)$");
            Match match = rgxHeader.Match(headerLine);
            if (match.Success & match.Groups.Count == 4)
                return match.Groups[1].Value;
            throw new Exception("Object type not found in line: " + headerLine);
        }
    }
}
