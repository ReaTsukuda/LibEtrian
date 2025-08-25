namespace LibEtrian.Skill.Tree;

/// <summary>
/// The skill tree format for EO4. Contains skill tree info for all classes, organized on a
/// skill-by-skill basis.
/// </summary>
[TableComponent(0x34)]
public class SkillTreeEO4Entry
{
  /// <summary>
  /// Which skill this entry is for.
  /// </summary>
  public U16 SkillId { get; }

  /// <summary>
  /// Flags for this entry. Only seems to have one valid value: bit 3 being set prevents the skill
  /// from being available to subclasses.
  /// </summary>
  public U16 Flags { get; }

  /// <summary>
  /// Which page this skill resides on.
  /// </summary>
  public Pages Page { get; }

  /// <summary>
  /// Starts at 0x08, consists of 7 S32s. Don't know what these are, but my assumption is that they're
  /// related to navigation, i.e. where to go when a particular d-pad direction is pressed.
  /// </summary>
  private S32[] PossibleNavigationValues;

  /// <summary>
  /// A skill's prerequisites. If a skill doesn't have a prerequisite for a given slot, it'll show
  /// values of skill ID 0, level 0.
  /// </summary>
  public Prerequisite[] Prerequisites { get; }

  /// <summary>
  /// Starts at 0x2C, consists of 4 S16s. Don't know what these are, but my assumption is that they're
  /// related to positioning. These seem to be global, not scoped to a class's skill tree, so how
  /// they work is a mystery to me.
  /// </summary>
  public S16[] PossiblePositionValues;

  /// <summary>
  /// The bit position for whether this skill should be available to subclasses.
  /// </summary>
  private const S32 ShowOnSubclassBitPosition = 3;

  public bool ShowOnSubclass => !Flags.IsBitSet(ShowOnSubclassBitPosition);

  /// <summary>
  /// How many of those values starting at 0x08 there are.
  /// </summary>
  private const S32 PossibleNavigationValuesCount = 7;

  /// <summary>
  /// How many prerequisites are present on each entry.
  /// </summary>
  private const S32 PrerequisitesCount = 2;

  /// <summary>
  /// How many of those values starting at 0x2C there are.
  /// </summary>
  private const S32 PossiblePositionValuesCount = 4;

  public SkillTreeEO4Entry(U8[] data)
  {
    SkillId = BitConverter.ToUInt16(data, 0x00);
    Flags = BitConverter.ToUInt16(data, 0x02);
    Page = (Pages)BitConverter.ToInt32(data, 0x04);
    PossibleNavigationValues = new S32[PossibleNavigationValuesCount];
    for (var i = 0; i < PossibleNavigationValuesCount; i += 1)
    {
      PossibleNavigationValues[i] = BitConverter.ToInt32(data, 0x08 + (i * 4));
    }
    Prerequisites = new Prerequisite[PrerequisitesCount];
    for (var i = 0; i < PrerequisitesCount; i += 1)
    {
      Prerequisites[i] = new Prerequisite()
      {
        SkillId = BitConverter.ToUInt16(data, 0x24 + (i * 4)),
        Level = BitConverter.ToUInt16(data, 0x26 + (i * 4)),
      };
    }
    PossiblePositionValues = new S16[PossiblePositionValuesCount];
    for (var i = 0; i < PossiblePositionValuesCount; i += 1)
    {
      PossiblePositionValues[i] = BitConverter.ToInt16(data, 0x2C + (i * 2));
    }
  }

  /// <summary>
  /// The pages contained in each class's skill tree.
  /// </summary>
  public enum Pages
  {
    Novice = 0,
    Veteran = 1,
    Master = 2
  }

  /// <summary>
  /// A prerequisite node in an EO4 skill tree entry.
  /// </summary>
  public class Prerequisite
  {
    /// <summary>
    /// The skill ID of the prerequisite.
    /// </summary>
    public U16 SkillId { get; init; }

    /// <summary>
    /// The required level of the prerequisite.
    /// </summary>
    public U16 Level { get; init; }
  }
}