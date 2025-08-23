namespace LibEtrian.Common;

/// <summary>
/// A block of stats, shared between players and enemies.
/// </summary>
public class StatBlock
{
  /// <summary>
  /// An entity's max HP.
  /// </summary>
  public S32 MaxHp { get; init; }
  
  /// <summary>
  /// An entity's max TP.
  /// </summary>
  public S32 MaxTp { get; init; }
  
  /// <summary>
  /// An entity's STR.
  /// </summary>
  public S16 Str { get; init; }
  
  /// <summary>
  /// An entity's VIT.
  /// </summary>
  public S16 Vit { get; init; }
  
  /// <summary>
  /// An entity's AGI.
  /// </summary>
  public S16 Agi { get; init; }
  
  /// <summary>
  /// An entity's LUC.
  /// </summary>
  public S16 Luc { get; init; }
  
  /// <summary>
  /// An entity's TEC.
  /// </summary>
  public S16 Tec { get; init; }
  
  /// <summary>
  /// An entity's WIS. Exists as padding in EO3 through EO2U, actually used in EO5 and EON.
  /// </summary>
  public S16 Wis { get; init; }

  /// <summary>
  /// An alias for when working with EO5 and EON.
  /// </summary>
  public S16 Int => Tec;

  /// <summary>
  /// This is primarily for debugging purposes.
  /// </summary>
  public override string ToString() => $"{MaxHp}, {MaxTp}, {Str}, {Tec}, {Vit}, {Wis}, {Agi}, {Luc}";
}