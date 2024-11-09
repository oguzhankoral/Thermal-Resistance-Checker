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
  public static Dictionary<ClimateZones, double> Wall => new ()
  {
    // Tropical Climates
    { ClimateZones.Af_TropicalRainforest, 0.9 },
    { ClimateZones.Am_TropicalMonsoon, 1.0 },
    { ClimateZones.Aw_TropicalSavanna, 1.1 },
    { ClimateZones.As_TropicalSavanna, 1.1 },

    // Dry Climates
    { ClimateZones.BWh_HotDesert, 1.0 },
    { ClimateZones.BWk_ColdDesert, 1.2 },
    { ClimateZones.BSh_HotSemiArid, 1.2 },
    { ClimateZones.BSk_ColdSemiArid, 1.5 },

    // Temperate Climates
    { ClimateZones.Cfa_HumidSubtropical, 1.4 },
    { ClimateZones.Cfb_Oceanic, 1.3 },
    { ClimateZones.Cfc_SubpolarOceanic, 1.2 },
    { ClimateZones.Csa_MediterraneanHotSummer, 1.51 },
    { ClimateZones.Csb_MediterraneanWarmSummer, 1.4 },
    { ClimateZones.Csc_MediterraneanCoolSummer, 1.3 },

    // Continental Climates
    { ClimateZones.Dfa_HumidContinentalHotSummer, 1.3 },
    { ClimateZones.Dfb_HumidContinentalMildSummer, 1.2 },
    { ClimateZones.Dfc_Subarctic, 0.7 },
    { ClimateZones.Dfd_SubarcticExtremeWinter, 0.6 },
    { ClimateZones.Dsa_MediterraneanInfluenceSnowyWinter, 1.2 },
    { ClimateZones.Dsb_MediterraneanInfluenceSnowyWinter, 1.1 },
    { ClimateZones.Dsc_MediterraneanInfluenceSnowyWinter, 0.9 },
    { ClimateZones.Dsd_MediterraneanInfluenceSnowyWinter, 0.8 },

    // Polar Climates
    { ClimateZones.ET_Tundra, 0.5 },
    { ClimateZones.EF_IceCap, 0.4 }
  };

  public static Dictionary<ClimateZones, double> Window => new ()
  {
    // Tropical Climates
    { ClimateZones.Af_TropicalRainforest, 0.8 },
    { ClimateZones.Am_TropicalMonsoon, 0.8 },
    { ClimateZones.Aw_TropicalSavanna, 0.9 },
    { ClimateZones.As_TropicalSavanna, 0.9 },

    // Dry Climates
    { ClimateZones.BWh_HotDesert, 0.7 },
    { ClimateZones.BWk_ColdDesert, 0.9 },
    { ClimateZones.BSh_HotSemiArid, 0.8 },
    { ClimateZones.BSk_ColdSemiArid, 0.85 },

    // Temperate Climates
    { ClimateZones.Cfa_HumidSubtropical, 0.6 },
    { ClimateZones.Cfb_Oceanic, 0.7 },
    { ClimateZones.Cfc_SubpolarOceanic, 0.75 },
    { ClimateZones.Csa_MediterraneanHotSummer, 0.55 },
    { ClimateZones.Csb_MediterraneanWarmSummer, 0.65 },
    { ClimateZones.Csc_MediterraneanCoolSummer, 0.7 },

    // Continental Climates
    { ClimateZones.Dfa_HumidContinentalHotSummer, 0.75 },
    { ClimateZones.Dfb_HumidContinentalMildSummer, 0.8 },
    { ClimateZones.Dfc_Subarctic, 0.5 },
    { ClimateZones.Dfd_SubarcticExtremeWinter, 0.45 },
    { ClimateZones.Dsa_MediterraneanInfluenceSnowyWinter, 0.7 },
    { ClimateZones.Dsb_MediterraneanInfluenceSnowyWinter, 0.65 },
    { ClimateZones.Dsc_MediterraneanInfluenceSnowyWinter, 0.55 },
    { ClimateZones.Dsd_MediterraneanInfluenceSnowyWinter, 0.5 },

    // Polar Climates
    { ClimateZones.ET_Tundra, 0.3 },
    { ClimateZones.EF_IceCap, 0.25 }
  };

  public static Dictionary<ClimateZones, double> Roof => new ()
  {
    // Tropical Climates
    { ClimateZones.Af_TropicalRainforest, 1.2 },
    { ClimateZones.Am_TropicalMonsoon, 1.3 },
    { ClimateZones.Aw_TropicalSavanna, 1.4 },
    { ClimateZones.As_TropicalSavanna, 1.4 },

    // Dry Climates
    { ClimateZones.BWh_HotDesert, 1.1 },
    { ClimateZones.BWk_ColdDesert, 1.3 },
    { ClimateZones.BSh_HotSemiArid, 1.2 },
    { ClimateZones.BSk_ColdSemiArid, 1.3 },

    // Temperate Climates
    { ClimateZones.Cfa_HumidSubtropical, 1.1 },
    { ClimateZones.Cfb_Oceanic, 1.0 },
    { ClimateZones.Cfc_SubpolarOceanic, 0.9 },
    { ClimateZones.Csa_MediterraneanHotSummer, 1.2 },
    { ClimateZones.Csb_MediterraneanWarmSummer, 1.1 },
    { ClimateZones.Csc_MediterraneanCoolSummer, 1.0 },

    // Continental Climates
    { ClimateZones.Dfa_HumidContinentalHotSummer, 1.0 },
    { ClimateZones.Dfb_HumidContinentalMildSummer, 0.9 },
    { ClimateZones.Dfc_Subarctic, 0.6 },
    { ClimateZones.Dfd_SubarcticExtremeWinter, 0.5 },
    { ClimateZones.Dsa_MediterraneanInfluenceSnowyWinter, 0.9 },
    { ClimateZones.Dsb_MediterraneanInfluenceSnowyWinter, 0.8 },
    { ClimateZones.Dsc_MediterraneanInfluenceSnowyWinter, 0.7 },
    { ClimateZones.Dsd_MediterraneanInfluenceSnowyWinter, 0.6 },

    // Polar Climates
    { ClimateZones.ET_Tundra, 0.4 },
    { ClimateZones.EF_IceCap, 0.35 }
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
  public ClimateZones ClimateZone;
  
  [Required]
  [DefaultValue(true)]
  [Description("Include Walls")]
  public bool CheckWalls;
  
  [Required]
  [DefaultValue(true)]
  [Description("Include Windows")]
  public bool CheckWindows;
  
  [Required]
  [DefaultValue(true)]
  [Description("Include Roofs")]
  public bool CheckRoofs;
}
