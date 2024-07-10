using Microsoft.AspNetCore.Mvc;

namespace DynamicAPI;

public static class Enpoint
{
    public static WebApplication AddEndPointLogic(this WebApplication app, IConfiguration Configuration)
    {
        var apiEndpoints = Configuration.GetSection("ApiEndpoints").Get<List<ApiEndpoint>>();

        if (apiEndpoints == null) throw new Exception("endpoints not retrieved from config");
        foreach (var endpoint in apiEndpoints)
        {
            app.UseRouting(); 
            app.UseEndpoints(endpoints =>
            {
                switch (endpoint.RequestMethod.ToUpper())
                {
                    case "GET":
                        endpoints.MapGet(endpoint.Route, ([FromServices] ILogicFactory logicFactory) =>
                        {
                            var logic = logicFactory.CallMethod(endpoint.AssemblyPath, endpoint.TypeName,
                                endpoint.MethodName, "");
                            return logic;
                        });
                        break;
                    case "POST":
                        endpoints.MapPost(endpoint.Route, ([FromServices] ILogicFactory logicFactory) =>
                        {
                            var logic = logicFactory.CallMethod(endpoint.AssemblyPath, endpoint.TypeName,
                                endpoint.MethodName, "");
                            return logic;
                        });
                        break;
                    case "PUT":
                        endpoints.MapPut(endpoint.Route, ([FromServices] ILogicFactory logicFactory) =>
                        {
                            var logic = logicFactory.CallMethod(endpoint.AssemblyPath, endpoint.TypeName,
                                endpoint.MethodName);
                            return logic;
                        });
                        break;
                    case "Delete":
                        endpoints.MapDelete(endpoint.Route, ([FromServices] ILogicFactory logicFactory) =>
                        {
                            var logic = logicFactory.CallMethod(endpoint.AssemblyPath, endpoint.TypeName,
                                endpoint.MethodName);
                            return logic;
                        });
                        break;
                    default:
                        return;
                }
            });
        }

        return app;
    }
}

public class ApiEndpoint
    {
        public string Route { get; set; }
        public string AssemblyPath { get; set; }
        public string TypeName { get; set; }
        public string MethodName { get; set; }
        public string RequestMethod { get; set;}
    };
