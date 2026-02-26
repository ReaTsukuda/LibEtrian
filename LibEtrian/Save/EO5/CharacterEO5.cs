using System.Text;
using HalfFullWidth;
using LibEtrian.Common;

namespace LibEtrian.Save.EO5;

/// <summary>
/// A character in the EO5 save data.
/// </summary>
public class CharacterEO5(U8[] data)
{
  /// <summary>
  /// The original binary data.
  /// </summary>
  private readonly U8[] OriginalData = data;

  /// <summary>
  /// Contains two known flags: position 0 determines whether the character exists, position 1 determines whether
  /// the character is in the party.
  /// </summary>
  public U8 Status { get; set; } = data[0x000];

  /// <summary>
  /// An ID for when this character was registered. 
  /// </summary>
  public U8 RegisteredId { get; } = data[0x001];

  /// <summary>
  /// Where in the Explorers Guild menu this character is.
  /// </summary>
  public U16 GuildListingOrder { get; } = BitConverter.ToUInt16(data, 0x002);
  
  /// <summary>
  /// The character's level.
  /// </summary>
  public U8 Level { get; set; } = data[0x004];
  
  /// <summary>
  /// The character's max level.
  /// </summary>
  public U8 MaxLevel { get; set; } = data[0x005];
  
  /// <summary>
  /// How many skill points this character currently has.
  /// </summary>
  public U8 AvailableSp { get; set; } = data[0x006];
  
  /// <summary>
  /// The highest "tier" of retirement this character has achieved.
  /// </summary>
  public U8 RetireTier { get; set; } = data[0x007];
  
  /// <summary>
  /// The character's class.
  /// </summary>
  public U8 Class { get; set; } = data[0x008];
  
  /// <summary>
  /// What disables the character is afflicted with.
  /// </summary>
  public U32 DisableStatus { get; set; } = BitConverter.ToUInt32(data, 0x00C);

  /// <summary>
  /// The character's equipment.
  /// </summary>
  public List<ItemEO5> Equipment { get; } = data
    .Skip(0x10)
    .Take(16)
    .Split(4)
    .Select(e => new ItemEO5(e))
    .ToList();

  /// <summary>
  /// The character's stat blocks.
  /// </summary>
  public List<StatBlock> StatBlocks { get; } = data
    .Skip(0x20)
    .Take(0x14 * 7)
    .Split(0x14)
    .Select(BinToStatBlock)
    .ToList();

  /// <summary>
  /// The stat bonuses the character has obtained from retirement.
  /// </summary>
  public StatBlock RetireStats => StatBlocks[0];

  /// <summary>
  /// The character's base stats. This is the result of their race stats and class stats being added together.
  /// </summary>
  public StatBlock BaseStats => StatBlocks[1];

  /// <summary>
  /// The character's stats after bonuses from skills.
  /// </summary>
  public StatBlock SkillStats => StatBlocks[2];

  /// <summary>
  /// The character's stats after bonuses from equipment.
  /// </summary>
  public StatBlock EquipmentStats => StatBlocks[3];

  /// <summary>
  /// The character's stats after their most recent battle. I'm pretty sure this is only relevant at runtime, but
  /// it's written to the save anyway.
  /// </summary>
  public StatBlock BattleStats => StatBlocks[6];

  /// <summary>
  /// The character's current HP.
  /// </summary>
  public S16 CurrentHp { get; set; } = BitConverter.ToInt16(data, 0x0AC);

  /// <summary>
  /// The character's current TP.
  /// </summary>
  public S16 CurrentTp { get; set; } = BitConverter.ToInt16(data, 0x0AE);
  
  /// <summary>
  /// The character's EXP.
  /// </summary>
  public S32 Exp { get; set; } = BitConverter.ToInt32(data, 0x0B0);
  
  /// <summary>
  /// The character's name.
  /// </summary>
  public string Name { get; set; } = Encoding.GetEncoding(932)
    .GetString(data.Skip(0x0B4).Take(0x14).Where(b => b != 0).ToArray())
    .ToHalfwidthString();

  /// <summary>
  /// The character's race.
  /// </summary>
  public U8 Race { get; set; } = data[0x0C9];

  /// <summary>
  /// The skill levels of the character's class skills.
  /// </summary>
  public U8[] ClassSkillLevels { get; } = data[0x0CA..0x0DE];

  /// <summary>
  /// The skill levels of the character's race skills.
  /// </summary>
  public U8[] RaceSkillLevels { get; } = data[0x0DE..0x0F6];

  /// <summary>
  /// The character's colors.
  /// </summary>
  public List<CustomizationColor> Colors { get; } = data
    .Skip(0x0F6)
    .Take(0x3 * 6)
    .Split(0x3)
    .Select(e => new CustomizationColor(e))
    .ToList();

  /// <summary>
  /// The highlight color of the character's left eye.
  /// </summary>
  public CustomizationColor EyeLeftHi => Colors[0];

  /// <summary>
  /// The lowlight color of the character's left eye.
  /// </summary>
  public CustomizationColor EyeLeftLo => Colors[1];

  /// <summary>
  /// The highlight color of the character's right eye.
  /// </summary>
  public CustomizationColor EyeRightHi => Colors[2];

  /// <summary>
  /// The lowlight color of the character's right eye.
  /// </summary>
  public CustomizationColor EyeRightLo => Colors[3];

  /// <summary>
  /// The highlight color of the character's hair.
  /// </summary>
  public CustomizationColor HairHi => Colors[4];

  /// <summary>
  /// The lowlight color of the character's hair.
  /// </summary>
  public CustomizationColor HairLo => Colors[5];

  /// <summary>
  /// The ID of the character's portrait.
  /// </summary>
  public U16 PortraitId { get; set; } = BitConverter.ToUInt16(data, 0x108);

  /// <summary>
  /// The ID of the character's voice.
  /// </summary>
  public U16 VoiceId { get; set; } = BitConverter.ToUInt16(data, 0x10A);

  /// <summary>
  /// The ID of the character's skin color.
  /// </summary>
  public U16 SkinColorId { get; set; } = BitConverter.ToUInt16(data, 0x10C);
  
  /// <summary>
  /// The character's class name.
  /// </summary>
  public string ClassName { get; set; } = Encoding.GetEncoding(932)
    .GetString(data.Skip(0x10E).Take(0x20).Where(b => b != 0).ToArray())
    .ToHalfwidthString();

  /// <summary>
  /// Applies the current values of the properties to the binary data.
  /// </summary>
  /// <returns>A new array of bytes, with the modifications applied.</returns>
  public U8[] GetModifiedBinaryData()
  {
    var buffer = new U8[OriginalData.Length];
    Array.Copy(OriginalData, buffer, OriginalData.Length);
    buffer[0x000] = Status;
    buffer[0x001] = RegisteredId;
    buffer.OverwriteRange(BitConverter.GetBytes(GuildListingOrder), 0x002);
    buffer[0x004] = Level;
    buffer[0x005] = MaxLevel;
    buffer[0x006] = AvailableSp;
    buffer[0x007] = RetireTier;
    buffer[0x008] = Class;
    buffer.OverwriteRange(BitConverter.GetBytes(DisableStatus), 0x00C);
    var equipmentBytes = new U8[Equipment.Count * 4];
    Equipment
      .Select((eq, i) => (eq, i))
      .ToList()
      .ForEach(eqt =>
      {
        equipmentBytes.OverwriteRange(BitConverter.GetBytes(eqt.eq.ItemId), eqt.i * 4);
        equipmentBytes.OverwriteRange(BitConverter.GetBytes(eqt.eq.Rank), 2 + (eqt.i * 4));
      });
    buffer.OverwriteRange(equipmentBytes, 0x10);
    buffer.OverwriteRange(BitConverter.GetBytes(Exp), 0x0B0);
    buffer.OverwriteRange(ClassSkillLevels, 0x0CA);
    buffer.OverwriteRange(RaceSkillLevels, 0x0DE);
    Name.WriteFullwidthToBinary(buffer, 0x0B4, 10);
    ClassName.WriteFullwidthToBinary(buffer, 0x10E, 16);
    return buffer;
  }

  /// <summary>
  /// For debugging purposes.
  /// </summary>
  /// <returns>A debug string.</returns>
  public override string ToString() => Status.IsBitSet(0) 
    ? $"{Name} (Lv{Level})"
    : "(Null)";

  /// <summary>
  /// Builds a StatBlock from binary data.
  /// </summary>
  /// <param name="data">The binary data of the stat block.</param>
  /// <returns>A StatBlock object.</returns>
  private static StatBlock BinToStatBlock(U8[] data)
  {
    return new StatBlock
    {
      MaxHp = BitConverter.ToInt32(data, 0x00),
      MaxTp = BitConverter.ToInt32(data, 0x04),
      Str = BitConverter.ToInt16(data, 0x08),
      Vit = BitConverter.ToInt16(data, 0x0A),
      Agi = BitConverter.ToInt16(data, 0x0C),
      Luc = BitConverter.ToInt16(data, 0x0E),
      Tec = BitConverter.ToInt16(data, 0x10),
      Wis = BitConverter.ToInt16(data, 0x12)
    };
  }
}