using ApiDoc.Models;

namespace ApiDoc.DataAccess.Proxies
{
    public static class DbTypeConverter
    {
        public static Branch MapNode(Nodes_GetAllResult input)
        {
            return new Branch
                {
                    Id = input.fID,
                    Name = input.fName,
                    Description = input.fDescription,
                    Deleted = input.fDeleted
                };
        }

        public static Branch MapNode(Nodes_GetByIdResult input)
        {
            return new Branch
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

        public static Branch MapNode(Nodes_GetByNameResult input)
        {
            return new Branch
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

        internal static Branch MapNode(Nodes_GetRevisionsResult input)
        {
            return new Branch
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