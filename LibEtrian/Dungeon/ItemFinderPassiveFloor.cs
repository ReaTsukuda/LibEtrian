namespace LibEtrian.Dungeon;

/// <summary>
/// An individual entry in the table Used for the Patrol skills in EO5, and Excavation + Godsend in EON. Defines what
/// items these skills can obtain from  each floor of the labyrinth, and the weight of each item when the skill
/// activates.
/// </summary>
[TableComponent(0x50)]
public class ItemFinderPassiveFloor
{
  /// <summary>
  /// The floor this entry applies to.
  /// </summary>
  public U32 FloorId { get; }

  /// <summary>
  /// The weights of each item.
  /// </summary>
  public List<U32> Weights { get; }

  /// <summary>
  /// The items assigned to this entry.
  /// </summary>
  public List<U32> Items { get; }

  public ItemFinderPassiveFloor(U8[] data)
  {
    FloorId = BitConverter.ToUInt32(data, 0x00);
    Weights = data
      .Skip(0x08)
      .Take(0x24)
      .Split(0x4)
      .Select(e => BitConverter.ToUInt32(e, 0x00))
      .ToList();
    Items = data
      .Skip(0x2C)
      .Take(0x24)
      .Split(0x4)
      .Select(e => BitConverter.ToUInt32(e, 0x00))
      .ToList();
  }
}