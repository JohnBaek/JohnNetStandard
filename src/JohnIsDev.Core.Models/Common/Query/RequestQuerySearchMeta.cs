using JohnIsDev.Core.Models.Common.Enums;

namespace JohnIsDev.Core.Models.Common.Query;

/// <summary>
/// Query Search Meta Informations 
/// </summary>
public class RequestQuerySearchMeta
{
    /// <summary>
    /// Type of Query search
    /// </summary>
    public EnumQuerySearchType SearchType { get; init; } = EnumQuerySearchType.Equals;

    /// <summary>
    /// Field Name
    /// </summary>
    public string Field { get; init; } = "";
    
    /// <summary>
    /// Excel Header Name
    /// </summary>
    public string ExcelHeaderName { get; init; } = "";

    /// <summary>
    /// True: Include Excel Columns
    /// </summary>
    public bool IsIncludeExcelHeader { get; init; } = false;

    /// <summary>
    /// Value of Sum
    /// </summary>
    public double Sum { get; set; } = 0;

    /// <summary>
    /// Is Have to Sum?
    /// </summary>
    public bool IsSum { get; set; } = false;

    /// <summary>
    /// If value has EnumType this will be not null
    /// </summary>
    public Type? EnumType { get; init; } = null;

    /// <summary>
    /// If is not null , Used as bool parser boolKeywords[0] if true keyword , boolKeywords[2] if false kewword
    /// </summary>
    public List<String>? BoolKeywords = null;
    
    /// <summary>
    /// Cell Drop down list
    /// </summary>
    public List<String> CellDropDowns { get; set; } = new List<string>();
    public bool IsUseCellDropDown
    {
        get
        {
            return this.CellDropDowns.Count > 0;
        }
    }
}