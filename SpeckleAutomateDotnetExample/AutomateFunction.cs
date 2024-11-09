using Objects;
using Speckle.Automate.Sdk;
using Speckle.Automate.Sdk.Schema;
using Speckle.Core.Models;
using Speckle.Core.Models.Extensions;

public enum SpeckleType
{
  Wall,
  Window,
  Roof
}

public static class AutomateFunction
{
  public static async Task Run(
    AutomationContext automationContext,
    FunctionInputs functionInputs
  )
  {
    Console.WriteLine("Starting execution");
    _ = typeof(ObjectsKit).Assembly; // INFO: Force objects kit to initialize

    Console.WriteLine("Receiving version");
    var commitObject = await automationContext.ReceiveVersion();

    var flatten = commitObject.Flatten().ToList();
    var failedObjectIds = new List<string>();
    var failedWallIds = CheckWalls(automationContext, functionInputs, flatten);
    failedObjectIds.AddRange(failedWallIds);
    var failedWindowIds = CheckWindows(automationContext, functionInputs, flatten);
    failedObjectIds.AddRange(failedWindowIds);
    var failedRoofIds = CheckRoofs(automationContext, functionInputs, flatten);
    failedObjectIds.AddRange(failedRoofIds);
    
    if (failedObjectIds.Count > 0)
    {
      var message = "";
      if (functionInputs.CheckWalls)
      {
        var wallCount = GetByType(flatten, SpeckleType.Wall).Count();
        message += "WALLS:\n";
        if (wallCount > 0)
        {
          message += $"{failedWallIds.Count}/{wallCount} wall(s) failed.\n";
        }
        else
        {
          message += "There are no walls\n\n";
        }
      }
      if (functionInputs.CheckWindows)
      {
        var windowCount = GetByType(flatten, SpeckleType.Window).Count();
        message += "WINDOWS:\n";
        if (windowCount > 0)
        {
          message += $"{failedWindowIds.Count}/{windowCount} window(s) failed.\n";
        }
        else
        {
          message += "There are no windows\n\n";
        }
      }
      
      if (functionInputs.CheckWindows)
      {
        var roofCount = GetByType(flatten, SpeckleType.Roof).Count();
        message += "ROOFS:\n";
        if (roofCount > 0)
        {
          message += $"{failedRoofIds.Count}/{roofCount} roof(s) failed.\n";
        }
        else
        {
          message += "There are no roofs\n\n";
        }
      }
      automationContext.MarkRunFailed(message);
    }
    else
    {
      automationContext.MarkRunSuccess($"Your building is compliant with selected climate zone!");
    }
  }

  private static List<string> CheckWalls(AutomationContext automationContext, FunctionInputs functionInputs, IEnumerable<Base> flatten)
  {
    if (!functionInputs.CheckWalls)
    {
      return new List<string>();
    }
    
    var walls = GetByType(flatten, SpeckleType.Wall);
    var uValues = walls.Select(GetThermalResistance);
    // Attempt to parse ClimateZone as a ClimateZones enum
    if (Enum.TryParse(functionInputs.ClimateZone, out ClimateZones climateZoneEnum))
    {
      var expectedValue = UValues.Wall[climateZoneEnum];
      var failedObjectIds = uValues.Where(val => val.value < expectedValue).Select(v => (v.id, v.value)).ToList();

      foreach (var (id, value) in failedObjectIds)
      {
        automationContext.AttachResultToObjects(
          ObjectResultLevel.Error, 
          "Walls", 
          new []{id}, 
          $"Wall expected to have maximum {expectedValue} U-value but it is {value}."
        );
      }
      return failedObjectIds.Select(i => i.id).ToList();
    }

    // Handle the case where the ClimateZone string is not a valid ClimateZones value
    throw new ArgumentException($"Invalid ClimateZone: {functionInputs.ClimateZone}");
  }
  
  private static List<string> CheckWindows(AutomationContext automationContext, FunctionInputs functionInputs, IEnumerable<Base> flatten)
  {
    if (!functionInputs.CheckWindows)
    {
      return new List<string>();
    }
    
    var walls = GetByType(flatten, SpeckleType.Window);
    var uValues = walls.Select(GetThermalResistance);
    if (Enum.TryParse(functionInputs.ClimateZone, out ClimateZones climateZoneEnum))
    {
      var expectedValue = UValues.Window[climateZoneEnum];
      var failedObjectIds = uValues.Where(val => val.value < expectedValue).Select(v => (v.id, v.value)).ToList();

      foreach (var (id, value) in failedObjectIds)
      {
        automationContext.AttachResultToObjects(
          ObjectResultLevel.Error, 
          "Windows",
          new []{id}, 
          $"Window expected to have maximum {expectedValue} U-value but it is {value}."
        );
      }
      return failedObjectIds.Select(i => i.id).ToList();
    }
    
    // Handle the case where the ClimateZone string is not a valid ClimateZones value
    throw new ArgumentException($"Invalid ClimateZone: {functionInputs.ClimateZone}");
  }
  
  private static List<string> CheckRoofs(AutomationContext automationContext, FunctionInputs functionInputs, IEnumerable<Base> flatten)
  {
    if (!functionInputs.CheckRoofs)
    {
      return new List<string>();
    }
    
    var walls = GetByType(flatten, SpeckleType.Roof);
    var uValues = walls.Select(GetThermalResistance);
    if (Enum.TryParse(functionInputs.ClimateZone, out ClimateZones climateZoneEnum))
    {
      var expectedValue = UValues.Roof[climateZoneEnum];
      var failedObjectIds = uValues.Where(val => val.value < expectedValue).Select(v => (v.id, v.value)).ToList();

      foreach (var (id, value) in failedObjectIds)
      {
        automationContext.AttachResultToObjects(
          ObjectResultLevel.Error, 
          "Roofs", 
          new []{id}, 
          $"Roof expected to have maximum {expectedValue} U-value but it is {value}."
        );
      }
      return failedObjectIds.Select(i => i.id).ToList();
    }
    
    // Handle the case where the ClimateZone string is not a valid ClimateZones value
    throw new ArgumentException($"Invalid ClimateZone: {functionInputs.ClimateZone}");
  }

  private static IEnumerable<Base> GetByType(IEnumerable<Base> objects, SpeckleType speckleType)
  {
    return objects.Where(b => b.speckle_type == SpeckleTypes[speckleType] &&
                       (string)b["category"]! == SpeckleCategories[speckleType]);
  }

  private static (string id, double value) GetThermalResistance(Base obj)
  {
    var properties = obj["properties"] as Dictionary<string, object>;
    if (properties is null)
    {
      return (obj.id, 0);
    }
    var typeParameters = properties!["Type Parameters"] as Dictionary<string, object>;
    var analyticalProperties = typeParameters!["Analytical Properties"] as Dictionary<string, object>;
    var u = analyticalProperties!["Heat Transfer Coefficient (U)"] as Dictionary<string, object>;
    var value = u!["value"] is double ? (double)u!["value"] : 0;
    return (obj.id, value);
  }
  
  private static readonly Dictionary<SpeckleType, string> SpeckleTypes = new Dictionary<SpeckleType, string>()
  {
    { SpeckleType.Wall, "Objects.BuiltElements.Wall:Objects.BuiltElements.Revit.RevitWall"},
    { SpeckleType.Window, "Objects.BuiltElements.Revit.RevitElement"},
    { SpeckleType.Roof, "Objects.BuiltElements.Roof:Objects.BuiltElements.Revit.RevitRoof.RevitRoof:Objects.BuiltElements.Revit.RevitRoof.RevitExtrusionRoof"},
  };
  
  private static readonly Dictionary<SpeckleType, string> SpeckleCategories = new Dictionary<SpeckleType, string>()
  {
    { SpeckleType.Wall, "Walls"},
    { SpeckleType.Window, "Windows"},
    { SpeckleType.Roof, "Roofs"},
  };
}
