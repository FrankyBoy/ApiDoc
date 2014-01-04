using ApiDoc.Models;

namespace ApiDoc.DataAccess.Proxies
{
    public static class DbTypeConverter
    {
        #region Branches
        public static Branch MapBranch(Nodes_GetAllResult input)
        {
            return new Branch
                {
                    Id = input.fID,
                    Name = input.fName,
                    Description = input.fDescription,
                    Deleted = input.fDeleted
                };
        }

        public static Branch MapBranch(Nodes_GetByNameResult input)
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

        public static Branch MapBranch(Nodes_GetRevisionsResult input)
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
        #endregion

        #region Leaves
        public static Leaf MapLeaf(Leafes_GetAllResult input, HttpVerbs httpVerbs)
        {
            return new Leaf
                {
                    Id = input.fID,
                    Name = input.fName,
                    Description = input.fDescription,
                    HttpVerb = httpVerbs[input.frHttpVerb],
                    Deleted = input.fDeleted
                };
        }



        #endregion
        public static Leaf MapLeaf(Leafes_GetByNameResult input, HttpVerbs httpVerbs)
        {
            return new Leaf
                {
                    Name = input.fName,
                    Id = input.fID,
                    Description = input.fDescription,
                    Deleted = input.fDeleted,
                    Author = input.fAuthor,
                    HttpVerb = httpVerbs[input.frHttpVerb],
                    ParentId = input.frParentId,
                    ChangeDate = input.fChangeDate,
                    ChangeNote = input.fChangeNote,
                    RevisionNumber = (int)(input.fRevisionNumber ?? 0),
                    RequiresAuthentication = input.fRequiresAuthentication,
                    RequiresAuthorization = input.fRequiresAuthorization,
                    SampleRequest = input.fRequestSample,
                    SampleResponse = input.fResponseSample
                };
        }

        public static Leaf MapLeaf(Leafes_GetRevisionsResult input, HttpVerbs httpVerbs)
        {
            return new Leaf
            {
                Name = input.fName,
                Id = input.fID,
                Description = input.fDescription,
                Deleted = input.fDeleted,
                Author = input.fAuthor,
                HttpVerb = httpVerbs[input.frHttpVerb],
                ParentId = input.frParentId,
                ChangeDate = input.fChangeDate,
                ChangeNote = input.fChangeNote,
                RevisionNumber = (int)(input.fRevisionNumber ?? 0),
                RequiresAuthentication = input.fRequiresAuthentication,
                RequiresAuthorization = input.fRequiresAuthorization,
                SampleRequest = input.fRequestSample,
                SampleResponse = input.fResponseSample
            };
        }
    }
}