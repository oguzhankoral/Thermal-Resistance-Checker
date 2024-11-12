using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TestAutomateFunction;

/// <summary>
/// This class describes the user specified variables that the function wants to work with.
/// </summary>
/// This class is used to generate a JSON Schema to ensure that the user provided values
/// are valid and match the required schema.
public struct FunctionInputs
{
  [Required]
  [EnumDataType(typeof(ClimateZone))]
  [DefaultValue(TestAutomateFunction.ClimateZone.Csa_MediterraneanHotSummer)]
  public string ClimateZone;
  
  [Required]
  [DefaultValue(true)]
  public bool CheckWalls;
  
  [Required]
  [DefaultValue(true)]
  public bool CheckWindows;
  
  [Required]
  [DefaultValue(true)]
  public bool CheckRoofs;
}
