using System.Collections.Generic;
using ApiDoc.DataAccess;
using ApiDoc.Models;

namespace ApiDoc.Proxies
{
    public static class DbTypeConverter
    {
        public static ModuleDescription MapModuleDescription(GetModuleByNameResult input)
        {
            return new ModuleDescription
            {
                Id = input.fID,
                Name = input.fServiceName
            };
        }

        public static ModuleDescription MapModuleDescription(GetModulesForApiResult input)
        {
            return new ModuleDescription
            {
                Id = input.fID,
                Name = input.fServiceName
            };
        }

        public static ApiDescription MapApiDescription(GetApiByNameResult input)
        {
            return new ApiDescription
            {
                Id = input.fID,
                Name = input.fApiName,
                Description = input.fDescription
            };
        }


        public static ApiDescription MapApiDescription(GetAPIsResult input)
        {
            return new ApiDescription
            {
                Id = input.fID,
                Name = input.fApiName,
                Description = input.fDescription
            };
        }

        public static MethodDescription MapMethodDescription(GetMethodsForServiceResult input, IDictionary<int, string> httpMethods)
        {
            return new MethodDescription
                {
                    Id = input.fID,
                    Name = GetMethodName(httpMethods[input.frHttpVerb],input.fMethodName)
                };
        }

        public static MethodDescription MapMethodDescription(GetMethodByIdResult input, IDictionary<int, string> httpMethods)
        {
            return new MethodDescription
                {
                    Id = input.fID,
                    Desciption = input.fDescription,
                    Name = GetMethodName(httpMethods[input.frHttpVerb],input.fMethodName),
                    Request = input.fRequestSample,
                    Response = input.fResponseSample,
                    Authenticated = input.fRequiresAuthentication,
                    Authorized = input.fRequiresAuthorization,
                    ServiceId = input.frServiceId
                };
        }

        public static MethodDescription MapMethodDescription(GetMethodByNameResult input, IDictionary<int, string> httpMethods)
        {
            return new MethodDescription
            {
                Id = input.fID,
                Desciption = input.fDescription,
                Name = GetMethodName(httpMethods[input.frHttpVerb],input.fMethodName),
                Request = input.fRequestSample,
                Response = input.fResponseSample,
                Authenticated = input.fRequiresAuthentication,
                Authorized = input.fRequiresAuthorization,
                ServiceId = input.frServiceId
            };
        }

        private static string GetMethodName(string method, string name)
        {
            return string.Format("{0} {1}", method, name);
        }
    }
}