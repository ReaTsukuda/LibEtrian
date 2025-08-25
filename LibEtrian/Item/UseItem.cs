namespace LibEtrian.Item;

/// <summary>
/// An individual useitem in useitemtable.tbl, which defines both materials (of the monster and gather variety), as
/// well as consumable items.
/// </summary>
[TableComponent(0x10)]
public class UseItem(U8[] data)
{
  /// <summary>
  /// The skill ID that is called when using the item. 0x00 for (most) useitems that do not do anything.  
  /// </summary>
  public S16 SkillId { get; } = BitConverter.ToInt16(data, 0x0);

  /// <summary>
  /// The level of the useitem's skill to call.
  /// </summary>
  public U8 SkillLevel { get; } = data[2];

  /// <summary>
  /// Unknown. Seems to be involved with defining if an item is usable or not. Can be 0, 1, or 2.
  /// </summary>
  public U8 Unk1 { get; } = data[3];

  /// <summary>
  /// The useitem's flags.
  /// </summary>
  public S32 Flags { get; } = BitConverter.ToInt32(data, 0x4);

  /// <summary>
  /// How much it costs to buy this useitem.
  /// </summary>
  public S32 BuyPrice { get; } = BitConverter.ToInt32(data, 0x8);

  /// <summary>
  /// How much this useitem sells for.
  /// </summary>
  public S32 SellPrice { get; } = BitConverter.ToInt32(data, 0xC);
}