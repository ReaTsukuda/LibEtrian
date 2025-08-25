﻿using LibEtrian.Common;

namespace LibEtrian.Enemy.EnemyData;

/// <summary>
/// An entry in the EOU version of the enemy data table.
/// </summary>
[TableComponent(0x64)]
public class EnemyDataV2(U8[] data)
{
  /// <summary>
  /// This enemy's level.
  /// </summary>
  public U16 Level = BitConverter.ToUInt16(data, 0x00);

  /// <summary>
  /// The internal ID of this enemy.
  /// </summary>
  public U16 InternalOrder = BitConverter.ToUInt16(data, 0x02);

  /// <summary>
  /// How much EXP this enemy gives when defeated.
  /// </summary>
  public S32 Exp = BitConverter.ToInt32(data, 0x04);

  /// <summary>
  /// Flags for this enemy.
  /// </summary>
  public S32 Flags = BitConverter.ToInt32(data, 0x08);

  /// <summary>
  /// Offset 0x0C. Unknown.
  /// </summary>
  public S16 Unk2 = BitConverter.ToInt16(data, 0x0C);

  /// <summary>
  /// Offset 0x0E. Unknown.
  /// </summary>
  public S16 Unk3 = BitConverter.ToInt16(data, 0x0E);

  /// <summary>
  /// The enemy's stats.
  /// </summary>
  public StatBlock Stats = new()
  {
    MaxHp = BitConverter.ToInt32(data, 0x10),
    MaxTp = BitConverter.ToInt32(data, 0x14),
    Str = BitConverter.ToInt16(data, 0x18),
    Vit = BitConverter.ToInt16(data, 0x1A),
    Agi = BitConverter.ToInt16(data, 0x1C),
    Luc = BitConverter.ToInt16(data, 0x1E),
    Tec = BitConverter.ToInt16(data, 0x20),
    Wis = BitConverter.ToInt16(data, 0x22),
  };

  /// <summary>
  /// The type of damage the enemy's normal attacks deal.
  /// </summary>
  public U16 DamageType = BitConverter.ToUInt16(data, 0x24);

  /// <summary>
  /// The enemy's base accuracy.
  /// </summary>
  public U16 BaseAccuracy = BitConverter.ToUInt16(data, 0x26);

  /// <summary>
  /// The enemy's vulnerabilities.
  /// </summary>
  public VulnerabilityBlock Vulnerabilities = new()
  {
    Cut = BitConverter.ToInt16(data, 0x28),
    Bash = BitConverter.ToInt16(data, 0x2A),
    Stab = BitConverter.ToInt16(data, 0x2C),
    Fire = BitConverter.ToInt16(data, 0x2E),
    Ice = BitConverter.ToInt16(data, 0x30),
    Volt = BitConverter.ToInt16(data, 0x32),
    InstantDeath = BitConverter.ToInt16(data, 0x34),
    Petrification = BitConverter.ToInt16(data, 0x36),
    Sleep = BitConverter.ToInt16(data, 0x38),
    Panic = BitConverter.ToInt16(data, 0x3A),
    Plague = BitConverter.ToInt16(data, 0x3C),
    Poison = BitConverter.ToInt16(data, 0x3E),
    Blind = BitConverter.ToInt16(data, 0x40),
    Curse = BitConverter.ToInt16(data, 0x42),
    Paralysis = BitConverter.ToInt16(data, 0x44),
    Stun = BitConverter.ToInt16(data, 0x46),
    Head = BitConverter.ToInt16(data, 0x48),
    Arm = BitConverter.ToInt16(data, 0x4A),
    Leg = BitConverter.ToInt16(data, 0x4C),
    Almighty = BitConverter.ToInt16(data, 0x4E),
    Fear = BitConverter.ToInt16(data, 0x50)
  };

  /// <summary>
  /// The enemy's first drop.
  /// </summary>
  public Drop Drop1 = new()
  {
    ItemId = BitConverter.ToUInt16(data, 0x52),
    Chance = BitConverter.ToUInt16(data, 0x54),
    RequirementArg = data[0x56],
    RequirementType = data[0x57]
  };

  /// <summary>
  /// The enemy's second drop.
  /// </summary>
  public Drop Drop2 = new()
  {
    ItemId = BitConverter.ToUInt16(data, 0x58),
    Chance = BitConverter.ToUInt16(data, 0x5A),
    RequirementArg = data[0x5C],
    RequirementType = data[0x5D]
  };

  /// <summary>
  /// The enemy's third drop.
  /// </summary>
  public Drop Drop3 = new()
  {
    ItemId = BitConverter.ToUInt16(data, 0x5E),
    Chance = BitConverter.ToUInt16(data, 0x60),
    RequirementArg = data[0x62],
    RequirementType = data[0x63]
  };
}