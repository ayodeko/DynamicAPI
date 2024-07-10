using System.Reflection;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace DynamicAPI;

public interface ILogicFactory
{
    object CallMethod(string dllPath, string typeName, string methodName, string? requestBody = null);
}
public class LogicFactory : ILogicFactory
{
    private object CreateInstance(string dllPath, string typeName)
    {
        // Get the base directory of the application
        // string basePath = AppContext.BaseDirectory;
        string basePath = @"C:\Users\aakindeko\RiderProjects\DynamicAPI\DynamicAPI\dlls";
        // Combine the base path with the DLL name
        dllPath = Path.Combine(basePath, dllPath);
        var assembly = Assembly.LoadFrom(dllPath);
        var type = assembly.GetType(typeName) ?? throw new DllNotFoundException($"Type {typeName} not found in assembly {dllPath}");
        return Activator.CreateInstance(type) ?? throw new DllNotFoundException($"Could not create an instance of type {typeName}");
    }
    public object CallMethod(string dllPath, string typeName, string methodName, string? requestBody = null)
    {
        // Create an instance of the type
        
        var instance = CreateInstance(dllPath, typeName);

        // Get the type of the instance
        var type = instance.GetType();

        // Get the method information
        var method = type.GetMethod(methodName) ?? throw new MissingMethodException($"Method {methodName} not found in type {typeName}");

        // Invoke the method
        var parameters = JsonConvert.DeserializeObject<object[]>(requestBody);
        var result = method.Invoke(instance, parameters);

        return result;
    }
}