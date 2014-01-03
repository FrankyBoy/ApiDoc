using ApiDoc.Models;

namespace ApiDoc.DataAccess.Proxies
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
                    RevisionNumber = (int) (input.fRevisionNumber ?? 0),
                    ParentId = input.frParentId
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
                RevisionNumber = (int) (input.fRevisionNumber ?? 0),
                ParentId = input.frParentId
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
                RevisionNumber = (int) (input.fRevisionNumber ?? 0),
                ParentId = input.frParentId
            };
        }
    }
}