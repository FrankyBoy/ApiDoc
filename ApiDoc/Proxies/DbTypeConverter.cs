using System.Collections.Generic;
using ApiDoc.DataAccess;
using ApiDoc.Models;

namespace ApiDoc.Proxies
{
    public static class DbTypeConverter
    {
        public static ModuleDescription MapModuleDescription(Modules_GetAllResult input)
        {
            return new ModuleDescription
            {
                Id = input.fID,
                Name = input.fModuleName,
                Deleted = input.fDeleted
            };
        }

        public static ModuleDescription MapModuleDescription(Modules_GetByNameResult input)
        {
            return new ModuleDescription
            {
                Id = input.fID,
                Name = input.fModuleName,
                Deleted = input.fDeleted,
                Author = input.fAuthor,
                ChangeDate = input.fChangeDate,
                RevisionNumber = input.fRevisionNumber ?? 0

            };
        }
        
        public static ModuleDescription MapModuleDescription(Modules_GetByIdResult input)
        {
            return new ModuleDescription
            {
                Id = input.fID,
                Name = input.fModuleName,
                Deleted = input.fDeleted,
                Author = input.fAuthor,
                ChangeDate = input.fChangeDate,
                RevisionNumber = input.fRevisionNumber ?? 0

            };
        }

        public static ModuleDescription MapModuleDescription(Modules_GetRevisionsResult input)
        {
            return new ModuleDescription
            {
                Id = input.fID,
                Name = input.fModuleName,
                Deleted = input.fDeleted,
                Author = input.fAuthor,
                ChangeDate = input.fChangeDate,
                RevisionNumber = input.fRevisionNumber ?? 0

            };
        }

        public static ApiDescription MapApiDescription(Apis_GetAllResult input)
        {
            return new ApiDescription
            {
                Id = input.fID,
                Name = input.fApiName,
                Description = input.fDescription,
                Deleted = input.fDeleted
            };
        }

        public static ApiDescription MapApiDescription(Apis_GetByNameResult input)
        {
            return new ApiDescription
            {
                Id = input.fID,
                Name = input.fApiName,
                Description = input.fDescription,
                ChangeDate = input.fChangeDate,
                Author = input.fAuthor,
                Deleted = input.fDeleted,
                RevisionNumber = input.fRevisionNumber ?? 0
            };
        }

        public static ApiDescription MapApiDescription(Apis_GetByIdResult input)
        {
            return new ApiDescription
            {
                Id = input.fID,
                Name = input.fApiName,
                Description = input.fDescription,
                ChangeDate = input.fChangeDate,
                Author = input.fAuthor,
                Deleted = input.fDeleted,
                RevisionNumber = input.fRevisionNumber ?? 0
            };
        }

        public static ApiDescription MapApiDescription(Apis_GetRevisionsResult input)
        {
            return new ApiDescription
            {
                Id = input.fID,
                Name = input.fApiName,
                Description = input.fDescription,
                ChangeDate = input.fChangeDate,
                Author = input.fAuthor,
                Deleted = input.fDeleted,
                RevisionNumber = input.fRevisionNumber ?? 0
            };
        }
    }
}