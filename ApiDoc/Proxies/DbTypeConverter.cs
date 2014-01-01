using ApiDoc.DataAccess;
using ApiDoc.Models;

namespace ApiDoc.Proxies
{
    public static class DbTypeConverter
    {
        public static Node MapNode(Nodes_GetAllResult input)
        {
            return new Node
                {
                    Id = input.fID,
                    Name = input.fName,
                    Description = input.fDescription,
                    Deleted = input.fDeleted
                };
        }

        public static Node MapNode(Nodes_GetByIdResult input)
        {
            return new Node
                {
                    Author = input.fAuthor,
                    ChangeDate = input.fChangeDate,
                    ChangeNote = input.fChangeNote,
                    Name = input.fName,
                    Id = input.fID,
                    Description = input.fDescription,
                    Deleted = input.fDeleted,
                    RevisionNumber = input.fRevisionNumber ?? 0
                };
        }

        public static Node MapNode(Nodes_GetByNameResult input)
        {
            return new Node
            {
                Author = input.fAuthor,
                ChangeDate = input.fChangeDate,
                ChangeNote = input.fChangeNote,
                Name = input.fName,
                Id = input.fID,
                Description = input.fDescription,
                Deleted = input.fDeleted,
                RevisionNumber = input.fRevisionNumber ?? 0
            };
        }

        internal static Node MapNode(Nodes_GetRevisionsResult input)
        {
            return new Node
            {
                Author = input.fAuthor,
                ChangeDate = input.fChangeDate,
                ChangeNote = input.fChangeNote,
                Name = input.fName,
                Id = input.fID,
                Description = input.fDescription,
                Deleted = input.fDeleted,
                RevisionNumber = input.fRevisionNumber ?? 0
            };
        }
    }
}