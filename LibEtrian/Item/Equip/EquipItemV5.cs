using LibEtrian;

/// <summary>
/// An entry in the EO5 version of equipitemtable.tbl, which defines data for equipment.
/// </summary>
[TableComponent(0x4C)]
public class EquipItemV5(U8[] data)
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
  /// The skill this equipment gives access to.
  /// </summary>
  public S16 SkillId { get; } = BitConverter.ToInt16(data, 0x04);
  
  /// <summary>
  /// The level of the skill that is given when it is first forged.
  /// </summary>
  public S16 SkillLevel { get; } = BitConverter.ToInt16(data, 0x06);

  /// <summary>
  /// The ATK the item gives.
  /// </summary>
  public S16 Atk { get; } = BitConverter.ToInt16(data, 0x08);

  /// <summary>
  /// The DEF the item gives.
  /// </summary>
  public S16 Def { get; } = BitConverter.ToInt16(data, 0x0A);
  
  /// <summary>
  /// The MAT the item gives.
  /// </summary>
  public S16 Mat { get; } = BitConverter.ToInt16(data, 0x0C);

  /// <summary>
  /// The MDF the item gives.
  /// </summary>
  public S16 Mdf { get; } = BitConverter.ToInt16(data, 0x0E);

  /// <summary>
  /// The vulnerability modifiers on this item.
  /// </summary>
  public U8[] VulnerabilityModifiers { get; } = data.Skip(0x10).Take(22).ToArray();

  /// <summary>
  /// The stat modifiers on this item.
  /// </summary>
  public S16[] StatModifiers { get; } = data
    .Skip(0x26)
    .Take(0x10)
    .Split(2)
    .Select(e => BitConverter.ToInt16(e, 0))
    .ToArray();
  
  /// <summary>
  /// The item's speed modifier. In EO5, this is added/subtracted from a base 1.0x multiplier that's applied to the
  /// user's AGI, which determines their action speed before modifiers.
  /// </summary>
  public S16 SpeedModifier { get; } = BitConverter.ToInt16(data, 0x38);
  
  /// <summary>
  /// Unknown. This is always 0 in EO5.
  /// </summary>
  public S16 Unk1 { get; } = BitConverter.ToInt16(data, 0x3A);
  
  /// <summary>
  /// Bitfield. Determines what classes can equip this item.
  /// </summary>
  public S16 CanEquip { get; } = BitConverter.ToInt16(data, 0x3C);
  
  /// <summary>
  /// Unknown. Varies based on equipment type.
  /// </summary>
  public S16 Unk2 { get; } = BitConverter.ToInt16(data, 0x3E);
  
  /// <summary>
  /// The item's flags.
  /// </summary>
  public S16 Flags { get; } = BitConverter.ToInt16(data, 0x40);
  
  /// <summary>
  /// The item's forge flags, i.e. can it be forged, what does it give upon being recycled, etc.
  /// </summary>
  public S16 ForgeFlags { get; } = BitConverter.ToInt16(data, 0x42);
  
  /// <summary>
  /// The item's cost.
  /// </summary>
  public S32 BuyPrice { get; } = BitConverter.ToInt32(data, 0x44);
  
  /// <summary>
  /// How much the item sells for.
  /// </summary>
  public S32 SellPrice { get; } = BitConverter.ToInt32(data, 0x48);
}