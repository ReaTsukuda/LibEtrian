namespace LibEtrian.Skill.Table;

/// <summary>
/// A skill.
/// </summary>
public class Skill
{
  /// <summary>
  /// The skill's max level. Only really matters for player skills.
  /// </summary>
  public U8 MaxLevel { get; private set; }
  
  /// <summary>
  /// What type of skill this is. Determines various aspects of skill behavior.
  /// </summary>
  public U8 SkillType { get; private set; }
  
  /// <summary>
  /// Determines what body parts and equipment the user needs in order to use this skill.
  /// </summary>
  public U16 UseRequirements { get; private set; }
  
  /// <summary>
  /// Determines what conditions need to be true on the target(s) for the skill to be able to be used.
  /// </summary>
  public U16 StatusRequirements { get; private set; }

  /// <summary>
  /// Unknown S32 value that's in the EO5 and EON skill headers.
  /// </summary>
  public U32 Unk1 { get; private set; }
  
  /// <summary>
  /// The skill's target type.
  /// </summary>
  public U8 TargetType { get; private set; }
  
  /// <summary>
  /// The team the skill targets.
  /// </summary>
  public U8 TargetTeam { get; private set; }
  
  /// <summary>
  /// Where the skill can be used.
  /// </summary>
  public U8 Locations { get; private set; }
  
  /// <summary>
  /// What the skill does with modifiers.
  /// </summary>
  public U8 ModifierStatus { get; private set; }
  
  /// <summary>
  /// What type of modifier the skill applies.
  /// </summary>
  public U16 ModifierType { get; private set; }
  
  /// <summary>
  /// What elements the skill's modifier interacts with.
  /// </summary>
  public U16 ModifierElement { get; private set; }
  
  /// <summary>
  /// The skill's damage type.
  /// </summary>
  public U16 DamageElement { get; private set; }
  
  /// <summary>
  /// What the skill does with disables.
  /// </summary>
  public U16 InflictionStatus { get; private set; }
  
  /// <summary>
  /// What disables the skill interacts with.
  /// </summary>
  public U16 InflictionElement { get; private set; }
  
  /// <summary>
  /// The skill's flags.
  /// </summary>
  public U32 Flags { get; private set; }

  /// <summary>
  /// The skill's data sections.
  /// </summary>
  public List<DataSection> DataSections { get; }

  /// <summary>
  /// How many data sections are in a skill in each game.
  /// </summary>
  private static readonly Dictionary<Games, S32> GameDataSections = new()
  {
    { Games.EO3, 8 },
    { Games.EO4, 8 },
    { Games.EOU, 10 },
    { Games.EO2U, 12 },
    { Games.EO5, 12 },
    { Games.EON, 12 }
  };

  /// <summary>
  /// How many levels are in a data section in each game.
  /// </summary>
  private static readonly Dictionary<Games, S32> GameDataSectionLevels = new()
  {
    { Games.EO3, 10 },
    { Games.EO4, 10 },
    { Games.EOU, 15 }, // +5 for boost
    { Games.EO2U, 20 }, // +10 for grimoires
    { Games.EO5, 11 }, // Dummy level 0 for the custom screen
    { Games.EON, 11 } // Dummy level 0 for the custom screen
  };

  public Skill(U8[] data, Games game)
  {
    // These two values are in the same place in every game, so we can just load them here.
    MaxLevel = data[0x000];
    SkillType = data[0x001];
    // Loading the rest of the header.
    var headerEnd = game switch
    {
      Games.EO3 => LoadHeadersEO3(data),
      Games.EO4 => LoadHeadersEO4(data),
      Games.EOU => LoadHeadersEOU(data),
      Games.EO2U => LoadHeadersEO2U(data),
      Games.EO5 => LoadHeadersEO5(data),
      Games.EON => LoadHeadersEON(data),
    };
    DataSections = data
      .Split(4 + (GameDataSectionLevels[game] * 4), headerEnd)
      .Select(e => new DataSection
      {
        Type = BitConverter.ToUInt32(e, 0),
        Values =  e.Split(4, 4).Select(v => BitConverter.ToInt32(v, 0)).ToList()
      })
      .ToList();
  }

  /// <summary>
  /// Loads skill header values for EO3.
  /// </summary>
  /// <param name="data">The skill's data.</param>
  /// <returns>The offset for where the header ends.</returns>
  private S32 LoadHeadersEO3(U8[] data)
  {
    UseRequirements = BitConverter.ToUInt16(data, 0x002);
    StatusRequirements = BitConverter.ToUInt16(data, 0x004);
    TargetType = data[0x006];
    TargetTeam = data[0x007];
    Locations = data[0x008];
    ModifierStatus = data[0x009];
    ModifierType = BitConverter.ToUInt16(data, 0x00A);
    ModifierElement = BitConverter.ToUInt16(data, 0x00C);
    DamageElement = BitConverter.ToUInt16(data, 0x00E);
    InflictionStatus = BitConverter.ToUInt16(data, 0x010);
    InflictionElement = BitConverter.ToUInt16(data, 0x012);
    Flags = BitConverter.ToUInt32(data, 0x014);
    return 0x018;
  }

  /// <summary>
  /// Loads skill header values for EO4.
  /// </summary>
  /// <param name="data">The skill's data.</param>
  /// <returns>The offset for where the header ends.</returns>
  private S32 LoadHeadersEO4(U8[] data)
  {
    // Same values and positions as EO3.
    return LoadHeadersEO3(data);
  }

  /// <summary>
  /// Loads skill header values for EOU.
  /// </summary>
  /// <param name="data">The skill's data.</param>
  /// <returns>The offset for where the header ends.</returns>
  private S32 LoadHeadersEOU(U8[] data)
  {
    // Same values and positions as EO3.
    return LoadHeadersEO3(data);
  }

  /// <summary>
  /// Loads skill header values for EO2U.
  /// </summary>
  /// <param name="data">The skill's data.</param>
  /// <returns>The offset for where the header ends.</returns>
  private S32 LoadHeadersEO2U(U8[] data)
  {
    // Same values and positions as EO3.
    return LoadHeadersEO3(data);
  }

  /// <summary>
  /// Loads skill header values for EO5.
  /// </summary>
  /// <param name="data">The skill's data.</param>
  /// <returns>The offset for where the header ends.</returns>
  private S32 LoadHeadersEO5(U8[] data)
  {
    UseRequirements = BitConverter.ToUInt16(data, 0x002);
    StatusRequirements = BitConverter.ToUInt16(data, 0x004);
    // 0x006: Always 0x0000.
    Unk1 = BitConverter.ToUInt32(data, 0x008);
    TargetType = data[0x00C];
    TargetTeam = data[0x00D];
    Locations = data[0x00E];
    ModifierStatus = data[0x00F];
    ModifierType = BitConverter.ToUInt16(data, 0x010);
    ModifierElement = BitConverter.ToUInt16(data, 0x012);
    DamageElement = BitConverter.ToUInt16(data, 0x014);
    InflictionStatus = BitConverter.ToUInt16(data, 0x016);
    InflictionElement = BitConverter.ToUInt16(data, 0x018);
    // 0x01A: Always 0x0000.
    Flags = BitConverter.ToUInt32(data, 0x01C);
    return 0x020;
  }

  /// <summary>
  /// Loads skill header values for EON.
  /// </summary>
  /// <param name="data">The skill's data.</param>
  /// <returns>The offset for where the header ends.</returns>
  private S32 LoadHeadersEON(U8[] data)
  {
    UseRequirements = BitConverter.ToUInt16(data, 0x004);
    StatusRequirements = BitConverter.ToUInt16(data, 0x008);
    Unk1 = BitConverter.ToUInt32(data, 0x00C);
    TargetType = data[0x010];
    TargetTeam = data[0x011];
    Locations = data[0x012];
    ModifierStatus = data[0x013];
    ModifierType = BitConverter.ToUInt16(data, 0x014);
    ModifierElement = BitConverter.ToUInt16(data, 0x016);
    DamageElement = BitConverter.ToUInt16(data, 0x018);
    InflictionStatus = BitConverter.ToUInt16(data, 0x01A);
    InflictionElement = BitConverter.ToUInt16(data, 0x01C);
    // 0x01E: Always 0x0000.
    Flags = BitConverter.ToUInt32(data, 0x020);
    return 0x024;
  }

  /// <summary>
  /// A data section in a skill.
  /// </summary>
  public class DataSection
  {
    /// <summary>
    /// The type of the data section.
    /// </summary>
    public U32 Type { get; init; }
    
    /// <summary>
    /// The values of the data section.
    /// </summary>
    public List<S32> Values { get; init; }
  }
}