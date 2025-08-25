namespace LibEtrian.Item;

/// <summary>
/// An entry in the EO4, EOU, EO2U, EO5, and EON version of the itemcompound tables.
/// </summary>
[TableComponent(0xC)]
public class UnlockEntryV2(U8[] data)
{
  /// <summary>
  /// The global flag required to show the unlock requirements, even if none of the components have anything yet. This
  /// is only used in EO4, but all later games still keep it.
  /// </summary>
  public U16 Flag { get; } = BitConverter.ToUInt16(data, 0x0);
  
  /// <summary>
  /// The item IDs that comprise this unlock requirement.
  /// </summary>
  public U16[] ItemIds { get; } = data
    .Skip(0x2)
    .Take(0x6)
    .Split(2)
    .Select(e => BitConverter.ToUInt16(e, 0))
    .ToArray();

  /// <summary>
  /// The amounts of each required item to fulfill this unlock requirement.
  /// </summary>
  public U8[] ItemAmounts { get; } = data.Skip(0x8).Take(3).ToArray();
}