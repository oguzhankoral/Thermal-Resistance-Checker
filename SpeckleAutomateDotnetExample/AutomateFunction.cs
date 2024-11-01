using Objects;
using Speckle.Automate.Sdk;
using Speckle.Automate.Sdk.Schema;
using Speckle.Core.Models;
using Speckle.Core.Models.Extensions;

public static class AutomateFunction
{
  public static async Task Run(
    AutomationContext automationContext,
    FunctionInputs functionInputs
  )
  {
    Console.WriteLine("Starting execution");
    _ = typeof(ObjectsKit).Assembly; // INFO: Force objects kit to initialize
    var threshold = 0.02;
    Console.WriteLine("Receiving version");
    var commitObject = await automationContext.ReceiveVersion();

    var objects = commitObject
      .Flatten()
      .Where(b => b.speckle_type == "Objects.BuiltElements.Wall:Objects.BuiltElements.Revit.RevitWall" &&
                  (string)b["category"]! == "Walls");

    var values = objects.Select(GetThermalResistance);

    var failedObjectIds = values.Where(val => val.value > threshold).Select(v => v.id).ToList();

    if (failedObjectIds.Count > 0)
    {
      automationContext.AttachResultToObjects(ObjectResultLevel.Error, "test", failedObjectIds);
      automationContext.MarkRunFailed("FAILED");
    }
    else
    {
      automationContext.MarkRunSuccess($"SUCCESS");
    }
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
    var thermalResistance = analyticalProperties!["Thermal Resistance (R)"] as Dictionary<string, object>;
    var value = thermalResistance!["value"] is double ? (double)thermalResistance!["value"] : 0;
    return (obj.id, value);
  }
}
