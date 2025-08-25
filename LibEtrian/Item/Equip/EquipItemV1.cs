namespace LibEtrian.Item.Equip;

/// <summary>
/// An entry in the EO3 version of equipitemtable.tbl, which defines data for equipment.
/// </summary>
[TableComponent(0x3C)]
public class EquipItemV1(U8[] data)
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
  /// The ATK the item gives.
  /// </summary>
  public S16 Atk { get; } = BitConverter.ToInt16(data, 0x04);

  /// <summary>
  /// The DEF the item gives.
  /// </summary>
  public S16 Def { get; } = BitConverter.ToInt16(data, 0x06);

  /// <summary>
  /// The disable vulnerability modifiers on this item. Functionally unused in EO3.
  /// </summary>
  public U8[] DisableVulnerabilityModifiers { get; } = data.Skip(0x08).Take(18).ToArray();

  /// <summary>
  /// The stat modifiers on this item. In EO3, this is only used on the game clear equipment: the Armoroad Emblem,
  /// the Deep City Medal, and the Armoroad Shield.
  /// </summary>
  public U8[] StatModifiers { get; } = data.Skip(0x1A).Take(8).ToArray();
  
  /// <summary>
  /// The item's speed modifier. In EO3, this is an amount added to the user's action speed, where 1 speed = 1 AGI.
  /// </summary>
  public S16 SpeedModifier { get; } = BitConverter.ToInt16(data, 0x22);
  
  /// <summary>
  /// Unknown. Seems to be 1 on weapons, 0 otherwise.
  /// </summary>
  public S16 Unk1 { get; } = BitConverter.ToInt16(data, 0x24);
  
  /// <summary>
  /// Bitfield. Determines what classes can equip this item.
  /// </summary>
  public S16 CanEquip { get; } = BitConverter.ToInt16(data, 0x26);
  
  /// <summary>
  /// The item's flags.
  /// </summary>
  public S16 Flags { get; } = BitConverter.ToInt16(data, 0x28);

  /// <summary>
  /// How many forge slots this item has. Subtract the amount of preset forges from this to get the amount of open forge
  /// slots on this item. This can be less than the amount of preset forges, but that leads to rendering oddities.
  /// </summary>
  public U8 ForgeCount { get; } = data[0x2A];

  /// <summary>
  /// The forges this item comes with.
  /// </summary>
  public U8[] PresetForges { get; } = data.Skip(0x2B).Take(9).ToArray();
  
  /// <summary>
  /// The item's cost.
  /// </summary>
  public S32 BuyPrice { get; } = BitConverter.ToInt32(data, 0x34);
  
  /// <summary>
  /// Unknown.
  /// </summary>
  public S32 Unk2 { get; } = BitConverter.ToInt32(data, 0x34);
}