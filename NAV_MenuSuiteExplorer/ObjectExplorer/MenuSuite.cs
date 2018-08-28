using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NAV_MenuSuiteExplorer.ObjectExplorer
{
    class MenuSuite : Base
    {
        List<Node> nodes = new List<Node>();

        public MenuSuite(string headerLine) : base(headerLine)
        {

        }

        public override void Explore()
        {
            List<string> elementRange;

            // OBJECT-PROPERTIES
            elementRange = GetSectionLinesRange(lines.FindIndex(x => x == "  OBJECT-PROPERTIES"), lines);
            // ExploreObjectProperties(elementRange);

            // OBJECT-PROPERTIES
            elementRange = GetSectionLinesRange(lines.FindIndex(x => x == "  PROPERTIES"), lines);
            // ExploreProperties(elementRange);

            // OBJECT-PROPERTIES
            elementRange = GetSectionLinesRange(lines.FindIndex(x => x == "  MENUNODES"), lines);
            ExploreMenuNodes(elementRange);

        }

        private int[] GetSectionBracketsIndexes(int ofElementLineNo, List<string> inList)
        {
            int[] bracketLineNo = new int[2];
            // begin bracket
            bracketLineNo[0] = ofElementLineNo;
            while (!inList[bracketLineNo[0]].Contains('{'))
                bracketLineNo[0]++;
            //end bracket
            string beginBracketLine = inList[bracketLineNo[0]];
            bracketLineNo[1] = ofElementLineNo + inList.Skip(ofElementLineNo).ToList().FindIndex(x => x == beginBracketLine.Replace('{', '}'));

            return bracketLineNo;
        }

        private List<string> GetSectionLinesRange(int ofElementLineNo, List<string> inList)
        {
            int[] bracketLineNo = GetSectionBracketsIndexes(ofElementLineNo, inList);
            return inList.GetRange(bracketLineNo[0] + 1, bracketLineNo[1] - bracketLineNo[0] - 1);
        }

        private void ExploreObjectProperties(List<string> linesRange)
        {
            throw new NotImplementedException();
        }

        private void ExploreProperties(List<string> linesRange)
        {
            throw new NotImplementedException();
        }

        private void ExploreMenuNodes(List<string> linesRange)
        {
            for (int i = 0; i < linesRange.Count; i++)
            {
                if (linesRange[i].StartsWith("    { "))
                {
                    Node node = new Node(linesRange[i]);
                    nodes.Add(node);
                }
            }
        }
    }

    class Node
    {
        // HUGE REGEX LOL
        /* ^    { (?<TYPE>\w+)\s*;\[\{(?<GUID>[\w-]+)\}\] ;
         * (Name=(?<Name>\w+);)?[\r\n]?\s*
         * (CaptionML=(?<CaptionML>.+);)?[\r\n]?\s*
         * (ParentNodeID=\[\{(?<ParentNodeID>.+)\}\];)?[\r\n]?\s*
         * (Image=(?<Image>\d+);)?[\r\n]?\s*
         * (IsShortcut=(?<IsShortcut>(Yes)|(No));)?[\r\n]?\s*
         * (Visible=(?<Visible>(Yes)|(No));)?[\r\n]?\s*
         * (Enabled=(?<Enabled>(Yes)|(No));)?[\r\n]?\s*
         * (NextNodeID=\[\{(?<NextNodeID>.+)\}\];)?[\r\n]?\s*
         * (FirstChild=\[\{(?<FirstChild>.+)\}\];?)?[\r\n]?\s*
        */
        static Regex rgxNodeMain = new Regex(@"^    { (\w+)\s*;\[\{([\w-]+)\}\] ;");
        string type;
        string guid;

        static Node()
        {
            // initialize REGEX
            string rgxSeparator = @";?[\s\S]*"; // ;?[\s\S]*(?=NextPart) - look ahead / behind
            string rgxEnd = @" \}$";
            string rgxPartType = @"^    { (?<TYPE>\w+)\s*;";
            string rgxPartGuid = @"\[\{(?<GUID>[\w-]+)\}\] ;";
            string rgxPartName = @"(Name=(?<Name>.+);)?";
            string rgxPartCaptionML = @"(CaptionML=(?<CaptionML>.+);)?";
            string rgxPartMemberOfMenu = @"(MemberOfMenu=\[\{(?<MemberOfMenu>.+)\}\];)?";
            string rgxPartParentNodeID = @"(ParentNodeID=\[\{(?<ParentNodeID>.+)\}\];)?";
            string rgxPartImage = @"(Image=(?<Image>\d+);)?";
            string rgxPartIsShortcut = @"(IsShortcut=(?<IsShortcut>(Yes)|(No));)?";
            string rgxPartVisible = @"(Visible=(?<Visible>(Yes)|(No));)?";
            string rgxPartEnabled = @"(Enabled=(?<Enabled>(Yes)|(No));)?";
            string rgxPartNextNodeID = @"(NextNodeID=\[\{(?<NextNodeID>.+)\}\];)?";
            string rgxPartFirstChild = @"(FirstChild=\[\{(?<FirstChild>.+)\}\];)?";


        }

        public Node(string firstLine)
        {
            Match match = rgxNodeMain.Match(firstLine);
            if (match.Success & match.Groups.Count == 3)
            {
                type = match.Groups[1].Value;
                guid = match.Groups[2].Value;
            }
            else
              throw new Exception("Node data not match");
        }
    }
}
