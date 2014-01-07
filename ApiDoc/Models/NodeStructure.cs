using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ApiDoc.Models.Exceptions;

namespace ApiDoc.Models
{
    public class NodeStructure
    {
        private Exception _pathError;

        public IList<Node> Nodes { get; set; }
        public string OriginalPath { get; set; }
        public Node Last() { return Nodes.Last(); }
        
        public void CheckPath()
        {
            // case 1: we already have an error set -> just throw
            if (_pathError != null)
                throw _pathError;

            // case 2: structure is wrong, eg. because parentId was changed
            for (int i = 1; i < Nodes.Count ; i++)
            {
                var parent = Nodes[i - 1];
                var node = Nodes[i];
                if (parent.Id != node.ParentId)
                {
                    _pathError = new InvalidPathWarning("The node structure has changed.");
                    throw _pathError;
                }
            }

            // case 3: some node was renamed
            if (!((OriginalPath == null && Nodes.Count == 1) || // only root
                (OriginalPath != null // deeper nodes
                    && Path.ToLower().StartsWith(OriginalPath.ToLower())))) // just ommitting http verb
                  
            {
                _pathError = new InvalidPathWarning("A node in the path was renamed.");
                throw _pathError;
            }

            // case 4: some node was deleted
            if (Nodes.Any(x => x.Deleted))
            {
                _pathError = new InvalidPathWarning("A node in the path has been deleted.");
                throw _pathError;
            }
        }
        

        public string Path
        {
            get
            {
                var output = new StringBuilder();
                foreach (var item in Nodes.Where(item => item.Id != 0))
                {
                    output.Append(item.GetWikiUrlString());
                }

                return output.ToString();
            }
        }

        public void SetPathError(Exception ex)
        {
            _pathError = ex;
        }
    }
}