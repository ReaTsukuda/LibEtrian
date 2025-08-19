namespace LibEtrian.Dungeon;

/// <summary>
/// Used for the Patrol skills in EO5, and Excavation + Godsend in EON. Defines what items these skills can obtain from
/// each floor of the labyrinth, and the weight of each item when the skill activates.
/// </summary>
public class ItemFinderPassiveTable : List<ItemFinderPassiveTable.Floor>
{
  public ItemFinderPassiveTable(string path)
  {
    var data = File.ReadAllBytes(path);
    if (data.Length % Floor.Length != 0)
    {
      throw new InvalidDataException($"ItemFinderPassiveTable length is not cleanly divisible by the " +
                                     $"entry length (0x{Floor.Length:X2}).");
    }
    AddRange(data
      .Split(Floor.Length)
      .Select(e => new ItemFinderPassiveTable.Floor(e)));
  }
  
  /// <summary>
  /// An individual entry in this table. Corresponds to one floor of the labyrinth.
  /// </summary>
  public class Floor
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

    public Floor(U8[] data)
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

    /// <summary>
    /// How long a flor entry is.
    /// </summary>
    public const S32 Length = 0x50;
  }
}