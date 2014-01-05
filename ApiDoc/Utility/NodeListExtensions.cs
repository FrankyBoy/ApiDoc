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
            var node1 = items.First(x => x.RevisionNumber == rev1);
            var node2 = items.First(x => x.RevisionNumber == rev2);

            if (node1.GetType() != node2.GetType())
                throw new Exception("Cannot compare two items of different type");

            if (node1 is Branch)
                return new Branch
                    {
                        Name = GetPrettyHtmlDiff(node1.Name, node2.Name),
                        Description = GetPrettyHtmlDiff(node1.Description, node2.Description),
                    };

            var leaf1 = node1 as Leaf;
            
            if (leaf1 != null){
                var leaf2 = (Leaf)node2;
                return new Leaf
                    {
                        Name = GetPrettyHtmlDiff(leaf1.Name, node2.Name),
                        Description = GetPrettyHtmlDiff(leaf1.Description, node2.Description),
                        HttpVerb = GetPrettyHtmlDiff(leaf1.HttpVerb, leaf2.HttpVerb),
                        SampleRequest = GetPrettyHtmlDiff(leaf1.SampleRequest, leaf2.SampleRequest),
                        SampleResponse = GetPrettyHtmlDiff(leaf1.SampleResponse, leaf2.SampleResponse)
                    };
            }

            throw new Exception("Not any known type");
        }

        private static string GetPrettyHtmlDiff(string text1, string text2)
        {
            if (text1 == null)
                text1 = "";

            if (text2 == null)
                text2 = "";

            var diffs = Dmp.diff_main(text1, text2);
            Dmp.diff_cleanupSemantic(diffs);
            return Dmp.diff_prettyHtml(diffs);
        }
    }
}