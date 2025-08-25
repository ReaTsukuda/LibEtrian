using LibEtrian;

/// <summary>
/// An entry in the EOU version of equipitemtable.tbl, which defines data for equipment.
/// </summary>
[TableComponent(0x4C)]
public class EquipItemV3(U8[] data)
{
  /// <summary>
  /// The type of equipment this item is, i.e. sword, katana, heavy armor, etc.
  /// </summary>
  public U8 Type { get; } = data[0x00];
  
  /// <summary>
  /// For weapons. Determines the base accuracy of this item.
  /// </summary>
  public U8 BaseAccuracy { get; } = data[0x01];
  
  /// <summary>
  /// The base damage type of this item. 
  /// </summary>
  public S16 DamageType { get; } = BitConverter.ToInt16(data, 0x02);
  
  /// <summary>
  /// The skill this equipment gives access to. Unused in EOU.
  /// </summary>
  public S32 SkillId { get; } = BitConverter.ToInt32(data, 0x04);

  /// <summary>
  /// The ATK the item gives.
  /// </summary>
  public S16 Atk { get; } = BitConverter.ToInt16(data, 0x08);

  /// <summary>
  /// The DEF the item gives.
  /// </summary>
  public S16 Def { get; } = BitConverter.ToInt16(data, 0x0A);

  /// <summary>
  /// The vulnerability modifiers on this item.
  /// </summary>
  public U8[] VulnerabilityModifiers { get; } = data.Skip(0x0C).Take(22).ToArray();

  /// <summary>
  /// The stat modifiers on this item.
  /// </summary>
  public S16[] StatModifiers { get; } = data
    .Skip(0x22)
    .Take(0x10)
    .Split(2)
    .Select(e => BitConverter.ToInt16(e, 0))
    .ToArray();
  
  /// <summary>
  /// The item's speed modifier. In EOU, this is added/subtracted from a base 1.0x multiplier that's applied to the
  /// user's AGI, which determines their action speed before modifiers.
  /// </summary>
  public S16 SpeedModifier { get; } = BitConverter.ToInt16(data, 0x32);
  
  /// <summary>
  /// Unknown. Seems to be 1 on weapons, 0 otherwise.
  /// </summary>
  public S16 Unk2 { get; } = BitConverter.ToInt16(data, 0x34);
  
  /// <summary>
  /// Bitfield. Determines what classes can equip this item.
  /// </summary>
  public S16 CanEquip { get; } = BitConverter.ToInt16(data, 0x36);
  
  /// <summary>
  /// The item's flags.
  /// </summary>
  public S16 Flags { get; } = BitConverter.ToInt16(data, 0x38);
  
  // The forge fields are technically still here, but they are intentionally not parsed.
  
  /// <summary>
  /// The item's cost.
  /// </summary>
  public S32 BuyPrice { get; } = BitConverter.ToInt32(data, 0x44);
  
  /// <summary>
  /// Unknown.
  /// </summary>
  public S32 Unk3 { get; } = BitConverter.ToInt32(data, 0x48);
}