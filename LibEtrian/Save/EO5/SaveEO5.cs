using System.Text;
using HalfFullWidth;

namespace LibEtrian.Save.EO5;

/// <summary>
/// A save file for EO5.
/// </summary>
public class SaveEO5(U8[] data)
{
  /// <summary>
  /// The current hour in the save.
  /// </summary>
  public U8 Hour { get; set; } = data[0x0116];

  /// <summary>
  /// The current minute in the save. One minute passes for every step taken in the labyrinth.
  /// </summary>
  public U8 Minute { get; set; } = data[0x0117];

  /// <summary>
  /// The current day in the save.
  /// </summary>
  public S32 Day { get; set; } = BitConverter.ToInt32(data, 0x0118);

  /// <summary>
  /// How much money the player has.
  /// </summary>
  public S32 Ental { get; set; } = BitConverter.ToInt32(data, 0x0140);
  
  /// <summary>
  /// The characters stored in the save.
  /// </summary>
  public CharacterEO5[] Characters = data
    .Skip(0x0144)
    .Take(0x150 * 30)
    .Split(0x150)
    .Select(e => new CharacterEO5(e))
    .ToArray();
  
  /// <summary>
  /// The guild name in the save.
  /// </summary>
  public string GuildName => Encoding.GetEncoding(932)
    .GetString(data.Skip(0x2AB4).Take(0x12).Where(b => b != 0).ToArray())
    .ToHalfwidthString();

  /// <summary>
  /// The save's inventory.
  /// </summary>
  public ItemEO5[] Inventory = data
    .Skip(0x2ADC)
    .Take(4 * 85)
    .Split(4)
    .Select(e => new ItemEO5(e))
    .ToArray();

  /// <summary>
  /// The save's key item inventory.
  /// </summary>
  public ItemEO5[] KeyItems = data
    .Skip(0x2C30)
    .Take(4 * 60)
    .Split(4)
    .Select(e => new ItemEO5(e))
    .ToArray();

  /// <summary>
  /// The save's inn storage. Only contains item IDs, quantities come later.
  /// </summary>
  public ItemEO5[] Storage = data
    .Skip(0x2D20)
    .Take(4 * 99)
    .Split(4)
    .Select(e => new ItemEO5(e))
    .ToArray();

  /// <summary>
  /// The save's food inventory.
  /// </summary>
  public U16[] Food = data
    .Skip(0x2EAC)
    .Take(4 * 60)
    .Split(60)
    .Select(e => BitConverter.ToUInt16(e))
    .ToArray();

  /// <summary>
  /// The quantities of each item stored at the inn.
  /// </summary>
  public U8[] StorageQuantities = data
    .Skip(0x2F58)
    .Take(99)
    .ToArray();
  
  /// <summary>
  /// The hound name in the save. Unlike guild/character names, this is null-terminated. Bad things will happen
  /// if you don't null terminate it.
  /// </summary>
  public string HoundName => Encoding.GetEncoding(932)
    .GetString(data.Skip(0x304C).Take(0x14).Where(b => b != 0).ToArray())
    .ToHalfwidthString();
  
  /// <summary>
  /// The hawk name in the save. Unlike guild/character names, this is null-terminated. Bad things will happen
  /// if you don't null terminate it.
  /// </summary>
  public string HawkName => Encoding.GetEncoding(932)
    .GetString(data.Skip(0x3060).Take(0x14).Where(b => b != 0).ToArray())
    .ToHalfwidthString();

  /// <summary>
  /// The stock of individual materials at the shop.
  /// </summary>
  public U8[] ShopStock = data
    .Skip(0x4FD8)
    .Take(400)
    .ToArray();

  /// <summary>
  /// The global flags array, which is a huge amount of bytes. Each flag is one bit in a byte, so if you know what
  /// flag you want to check, divide its ID by 8 and drop the remainder to figure out what byte it is in.
  /// </summary>
  public U8[] GlobalFlags = data
    .Skip(0x5168)
    .Take(3140)
    .ToArray();

  /// <summary>
  /// The Item Compendium's completion status. Each byte corresponds to an item. Bit 0 determines if it's been
  /// logged, bit 1 determines if it's been reported, bit 2 determines if it's been viewed in the menu.
  /// </summary>
  public U8[] ItemCompendium = data
    .Skip(0x5DAC)
    .Take(400)
    .ToArray();

  /// <summary>
  /// The Monstrous Codex's completion status. Each byte corresponds to a monster. Bit 0 determines if it's been
  /// logged, bit 1 determines if it's been reported, bit 2 determines if it's been viewed in the menu.
  /// </summary>
  public U8[] MonstrousCodex = data
    .Skip(0x5DAC)
    .Take(400)
    .ToArray();
}