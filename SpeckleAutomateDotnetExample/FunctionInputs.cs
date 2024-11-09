using Speckle.Automate.Sdk.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


public enum ClimateZones
{
  // Tropical Climates
  Af_TropicalRainforest,
  Am_TropicalMonsoon,
  Aw_TropicalSavanna,
  As_TropicalSavanna,

  // Dry Climates
  BWh_HotDesert,
  BWk_ColdDesert,
  BSh_HotSemiArid,
  BSk_ColdSemiArid,

  // Temperate Climates
  Cfa_HumidSubtropical,
  Cfb_Oceanic,
  Cfc_SubpolarOceanic,
  Csa_MediterraneanHotSummer,
  Csb_MediterraneanWarmSummer,
  Csc_MediterraneanCoolSummer,

  // Continental Climates
  Dfa_HumidContinentalHotSummer,
  Dfb_HumidContinentalMildSummer,
  Dfc_Subarctic,
  Dfd_SubarcticExtremeWinter,
  Dsa_MediterraneanInfluenceSnowyWinter,
  Dsb_MediterraneanInfluenceSnowyWinter,
  Dsc_MediterraneanInfluenceSnowyWinter,
  Dsd_MediterraneanInfluenceSnowyWinter,

  // Polar Climates
  ET_Tundra,
  EF_IceCap
}

public static class UValues
{
  public static Dictionary<ClimateZones, double> Wall => new Dictionary<ClimateZones, double>()
  {
    { ClimateZones.Csa_MediterraneanHotSummer, 1.51 },
  };
  
  public static Dictionary<ClimateZones, double> Window => new Dictionary<ClimateZones, double>()
  {
    { ClimateZones.Csa_MediterraneanHotSummer, 0.55 },
  };
  
  public static Dictionary<ClimateZones, double> Roof => new Dictionary<ClimateZones, double>()
  {
    { ClimateZones.Csa_MediterraneanHotSummer, 2.32 },
  };
}



/// <summary>
/// This class describes the user specified variables that the function wants to work with.
/// </summary>
/// This class is used to generate a JSON Schema to ensure that the user provided values
/// are valid and match the required schema.
public struct FunctionInputs
{
  [Required]
  [EnumDataType(typeof(ClimateZones))]
  [DefaultValue(ClimateZones.Csa_MediterraneanHotSummer)]
  public string ClimateZone;

  /// <summary>
  /// The object type to count instances of in the given model version.
  /// </summary>
  [Required]
  public string SpeckleTypeToCheck;
}
