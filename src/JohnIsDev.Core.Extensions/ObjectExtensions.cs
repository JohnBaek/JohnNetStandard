using System.Reflection;
using Newtonsoft.Json;

namespace JohnIsDev.Core.Extensions;

/// <summary>
/// Object 확장
/// </summary>
public static class ObjectExtensions
{
    /// <summary>
    /// T 데이터를 T로 클로닝 한다.
    /// </summary>
    /// <param name="source">원본 데이터</param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T? ToClone<T>(this T? source) where T : class
    {
        // 원본데이터가 유효하지 않을경우 
        if (source == null) 
            return null;
        
        // 질렬화 한다.
        string serialized = JsonConvert.SerializeObject(source);
        
        // 역직렬화 해서 반환한다.
        return JsonConvert.DeserializeObject<T>(serialized);
    }
    
    /// <summary>
    /// T 데이터를 T로 클로닝 한다.
    /// </summary>
    /// <param name="source">원본 데이터</param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T ToCloneToNoneNullable<T>(this T? source) where T : class, new()
    {
        T? cloned = ToClone(source);
        if (cloned == null)
            return new T();
        
        return cloned;
    }
    
    /// <summary>
    /// 대상소스로부터 데이터를 카피해서 T 형으로 반환한다.
    /// </summary>
    /// <param name="source"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T FromCopyValue<T>(this object source) where T : class
    {
        T? destination = Activator.CreateInstance<T>();

        // 소스 데이터로부터 프로퍼티를 가져온다.
        PropertyInfo[] sourceProperties = source.GetType().GetProperties();

        // 목적지 데이터로부터 프로퍼티를 가져온다.
        PropertyInfo[] destinationProperties = destination.GetType().GetProperties();
    
        // 모든 소스데이터에 대해 처리한다.
        foreach (PropertyInfo sourceProperty in sourceProperties)
        {
            // 모든 목적지 데이터에 대해 처리한다.
            foreach (PropertyInfo destinationProperty in destinationProperties)
            {
                // Is Name and Type is Not identical
                if (sourceProperty.Name != destinationProperty.Name || sourceProperty.PropertyType != destinationProperty.PropertyType) 
                    // Next 
                    continue;
                
                // Update value. sourceProperty to destination
                destinationProperty.SetValue(destination, sourceProperty.GetValue(source));
                break;
            }
        }
        
        // 수정된 destination을 반환한다.
        return destination; 
    }

    /// <summary>
    /// Checks a specified property name of property is in the class 
    /// </summary>
    /// <param name="source">A Target Object</param>
    /// <param name="propertyName">A Name of test property name</param>
    /// <returns>Returns true if a property exist in class</returns>
    public static bool HasProperty(this object source, string propertyName)
    {
        return source.GetType().GetProperty(propertyName,
            BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance
            ) != null;
    }
    
    /// <summary>
    /// Checks a specified property names of property is in the class 
    /// </summary>
    /// <param name="source">A Target Object</param>
    /// <param name="propertyNames">A Name of the test property list</param>
    /// <returns>Returns true if a properties all in class</returns>
    public static bool HasProperty(this object source, List<string> propertyNames)
    {
        // Test a class that all provided test property names 
        foreach (string propertyName in propertyNames)
        {
            if (!source.HasProperty(propertyName: propertyName))
                return false;
        }

        return true;
    }
}