namespace LibEtrian.Common;

/// <summary>
/// An item an enemy can drop.
/// </summary>
public class Drop
{
  /// <summary>
  /// The ID of the item for this drop.
  /// </summary>
  public U16 ItemId { get; init; }
  
  /// <summary>
  /// The chance for this item to drop, if its condition is fulfilled.
  /// </summary>
  public U16 Chance { get; init; }
  
  /// <summary>
  /// An argument value for the conditional drop requirement.
  /// </summary>
  public U8 RequirementArg { get; init; }
  
  /// <summary>
  /// The conditional drop requirement type.
  /// </summary>
  public U8 RequirementType { get; init; }
}