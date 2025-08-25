namespace LibEtrian.Item;

/// <summary>
/// An entry in the EO3 version of the itemcompound tables.
/// </summary>
[TableComponent(0xA)]
public class UnlockEntryV1(U8[] data)
{
  /// <summary>
  /// The item IDs that comprise this unlock requirement.
  /// </summary>
  public U16[] ItemIds { get; } = data
    .Take(0x6)
    .Split(2)
    .Select(e => BitConverter.ToUInt16(e, 0))
    .ToArray();

  /// <summary>
  /// The amounts of each required item to fulfill this unlock requirement.
  /// </summary>
  public U8[] ItemAmounts { get; } = data.Skip(0x6).Take(3).ToArray();
}