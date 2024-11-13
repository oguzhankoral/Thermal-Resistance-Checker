using Objects;
using Speckle.Automate.Sdk;
using Speckle.Automate.Sdk.Schema;
using Speckle.Core.Models;
using Speckle.Core.Models.Extensions;
using Speckle.Core.Models.GraphTraversal;

namespace TestAutomateFunction;

public enum SpeckleType
{
  Wall,
  Window,
  Roof,
}

public static class AutomateFunction
{
  public static async Task Run(
    AutomationContext automationContext,
    FunctionInputs functionInputs
  )
  {
    var climateZone = GetClimateZone(functionInputs.ClimateZone);

    Console.WriteLine("Starting execution");
    _ = typeof(ObjectsKit).Assembly; // INFO: Force objects kit to initialize

    Console.WriteLine("Receiving version");
    var rootObject = await automationContext.ReceiveVersion();

    Console.WriteLine("Flatten the root object");
    var flatten = rootObject.Flatten().ToList();

    Console.WriteLine("Traverse the objects by type");
    var walls = GetByType(flatten, SpeckleType.Wall);
    var windows = GetByType(flatten, SpeckleType.Window);
    var roofs = GetByType(flatten, SpeckleType.Roof);

    Console.WriteLine("Checking for compliance");
    var failedObjects = new List<ObjectToCheck>();
    var failedWalls = CheckCompliance(walls, climateZone, SpeckleType.Wall);
    var failedWindows = CheckCompliance(windows, climateZone, SpeckleType.Window);
    var failedRoofs = CheckCompliance(roofs, climateZone, SpeckleType.Roof);

    failedObjects.AddRange(failedWalls);
    failedObjects.AddRange(failedWindows);
    failedObjects.AddRange(failedRoofs);

    Console.WriteLine("Reporting compliance for failed objects");
    AttachReportToFailedObjects(automationContext, failedObjects);

    Console.WriteLine("Reporting the status of all automation");
    ReportStatus(
      automationContext,
      functionInputs,
      walls.Count(),
      failedWalls.Count(),
      windows.Count(),
      failedWindows.Count(),
      roofs.Count(),
      failedRoofs.Count()
    );
  }

  private static void AttachReportToFailedObjects(
    AutomationContext automationContext,
    IEnumerable<ObjectToCheck> failedObjects
  )
  {
    foreach (var failedObject in failedObjects)
    {
      var speckleTypeString = failedObject.SpeckleType.ToString();
      string message = "";
      if (failedObject.UValue == 0)
      {
        message =
          $"{speckleTypeString[..^1]} has no any material that have thermal properties.";
      }
      else
      {
        message =
          $"{speckleTypeString[..^1]} expected to have maximum {failedObject.ExpectedUValue} U-value but it is {failedObject.UValue}.";
      }

      automationContext.AttachResultToObjects(
        ObjectResultLevel.Error,
        speckleTypeString,
        new[] { failedObject.Id },
        message
      );
    }
  }

  private static void ReportStatus(
    AutomationContext automationContext,
    FunctionInputs functionInputs,
    int numberOfWalls,
    int numberOfFailedWalls,
    int numberOfWindows,
    int numberOfFailedWindows,
    int numberOfRoofs,
    int numberOfFailedRoofs
  )
  {
    if (numberOfFailedWalls + numberOfFailedWindows + numberOfFailedRoofs > 0)
    {
      var message = "";
      if (functionInputs.CheckWalls)
      {
        message += "WALLS:\n";
        if (numberOfWalls > 0)
        {
          message += $"{numberOfFailedWalls}/{numberOfWalls} wall(s) failed.\n";
        }
        else
        {
          message += "There are no walls\n\n";
        }
      }
      if (functionInputs.CheckWindows)
      {
        message += "WINDOWS:\n";
        if (numberOfWindows > 0)
        {
          message += $"{numberOfFailedWindows}/{numberOfWindows} window(s) failed.\n";
        }
        else
        {
          message += "There are no windows\n\n";
        }
      }

      if (functionInputs.CheckWindows)
      {
        message += "ROOFS:\n";
        if (numberOfRoofs > 0)
        {
          message += $"{numberOfFailedRoofs}/{numberOfRoofs} roof(s) failed.\n";
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
      automationContext.MarkRunSuccess(
        $"Your building is compliant with selected climate zone!"
      );
    }
  }

  private static double GetExpectedValue(
    ClimateZone climateZone,
    SpeckleType speckleType
  )
  {
    switch (speckleType)
    {
      case SpeckleType.Wall:
        return UValues.Wall[climateZone];
      case SpeckleType.Window:
        return UValues.Window[climateZone];
      case SpeckleType.Roof:
        return UValues.Roof[climateZone];
      default:
        return 0;
    }
  }

  private static IEnumerable<ObjectToCheck> CheckCompliance(
    IEnumerable<Base> objects,
    ClimateZone climateZone,
    SpeckleType speckleType
  )
  {
    var expectedValue = GetExpectedValue(climateZone, speckleType);
    var objectsToCheck = objects.Select(o => new ObjectToCheck(
      o,
      expectedValue,
      speckleType
    ));
    return objectsToCheck.Where(obj => obj.UValue > expectedValue || obj.UValue == 0);
  }

  private static ClimateZone GetClimateZone(string climateZoneString)
  {
    if (Enum.TryParse(climateZoneString, out ClimateZone climateZoneEnum))
    {
      return climateZoneEnum;
    }

    // Handle the case where the ClimateZone string is not a valid ClimateZones value
    throw new ArgumentException($"Invalid ClimateZone: {climateZoneString}");
  }

  private static IEnumerable<Base> GetByType(
    IEnumerable<Base> objects,
    SpeckleType speckleType
  )
  {
    return objects.Where(b =>
      b.speckle_type == SpeckleTypes[speckleType]
      && (string)b["category"]! == SpeckleCategories[speckleType]
    );
  }

  private static IEnumerable<Base> GetByType<T>(Base root)
    where T : Base
  {
    var traversal = new GraphTraversal();
    return traversal
      .Traverse(root)
      .Where(obj => obj.Current is T)
      .Select(obj => obj.Current);
  }

  private static readonly Dictionary<SpeckleType, string> SpeckleTypes =
    new()
    {
      {
        SpeckleType.Wall,
        "Objects.BuiltElements.Wall:Objects.BuiltElements.Revit.RevitWall"
      },
      { SpeckleType.Window, "Objects.BuiltElements.Revit.RevitElement" },
      {
        SpeckleType.Roof,
        "Objects.BuiltElements.Roof:Objects.BuiltElements.Revit.RevitRoof.RevitRoof:Objects.BuiltElements.Revit.RevitRoof.RevitExtrusionRoof"
      },
    };

  private static readonly Dictionary<SpeckleType, string> SpeckleCategories =
    new()
    {
      { SpeckleType.Wall, "Walls" },
      { SpeckleType.Window, "Windows" },
      { SpeckleType.Roof, "Roofs" },
    };
}
