namespace LibEtrian.Save.EO5;

/// <summary>
/// An item in EO5's save data.
/// </summary>
public class ItemEO5(U8[] data)
{
  /// <summary>
  /// The ID of the item.
  /// </summary>
  public U16 ItemId { get; set; } = BitConverter.ToUInt16(data, 0x0);
  
  /// <summary>
  /// The forge rank of the item. Should be 0 for anything that isn't a forgeable weapon.
  /// </summary>
  public U16 Rank { get; set; } = BitConverter.ToUInt16(data, 0x2);
}