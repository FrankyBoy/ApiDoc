using System;
using System.Collections.Generic;
using System.Linq;
using ApiDoc.Models;

namespace ApiDoc.Utility
{
    public static class NodeListExtensions
    {
        private static readonly DiffMatchPatch Dmp = new DiffMatchPatch();

        public static Node Compare(this IList<Node> items, int rev1, int rev2)
        {
            var r1 = items.First(x => x.RevisionNumber == rev1);
            var r2 = items.First(x => x.RevisionNumber == rev2);

            if (r1.GetType() != r2.GetType())
                throw new Exception("Cannot compare two items of different type");

            if (r1 is Branch)
                return new Branch
                    {
                        Name = GetPrettyHtmlDiff(r1.Name, r2.Name),
                        Description = GetPrettyHtmlDiff(r1.Description, r2.Description),
                    };
            if(r1 is Leaf)
                throw new NotImplementedException();

            throw new Exception("Not any known type");
        }

        private static string GetPrettyHtmlDiff(string text1, string text2)
        {
            var diffs = Dmp.diff_main(text1, text2);
            Dmp.diff_cleanupSemantic(diffs);
            return Dmp.diff_prettyHtml(diffs);
        }
    }
}