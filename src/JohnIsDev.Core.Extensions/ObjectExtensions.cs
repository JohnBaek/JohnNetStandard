using System.Collections;
using System.Reflection;
using Newtonsoft.Json;

namespace JohnIsDev.Core.Extensions;

/// <summary>
/// Object 확장
/// </summary>
public static class ObjectExtensions 
{
    /// <summary>
    /// 
    /// </summary>
    private static readonly Lazy<UltraMapper.Mapper> Mapper = new(() => new UltraMapper.Mapper());

    
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
    /// Copies property values from the source object to a new instance of type T.
    /// </summary>
    /// <param name="source">The source object containing the data to copy.</param>
    /// <typeparam name="T">The type of the object to create and populate.</typeparam>
    /// <returns>A new instance of type T with properties populated from the source object.</returns>
    /// <exception cref="Exception">Thrown if an error occurs during property copying or object creation.</exception>
    public static T FromCopyValue<T>(this object source) where T : class
    {
        T? destination = Activator.CreateInstance<T>();
        try
        {
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
        catch (Exception) 
        {
            throw;
        }
    }

    /// <summary>
    /// Creates a deep copy of the source object to a new instance of type T using safe mapping.
    /// </summary>
    /// <param name="source">The source object containing the data to copy.</param>
    /// <typeparam name="T">The target type to create and map data to.</typeparam>
    /// <returns>A new instance of type T with properties deeply copied from the source object.</returns>
    public static T FromCopyValueDeepSafe<T>(this object source) where T : class, new()
        => Mapper.Value.Map<T>(source);
}