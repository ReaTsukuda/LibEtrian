using System.Text;

namespace LibEtrian.Dungeon;

/// <summary>
/// floor_system.tbl, which contains some metadata about dungeon floors (what floor to show them as, what stratum they
/// are, etc.)
/// </summary>
public class DungeonSystemTable : List<DungeonSystemTable.Floor>
{
  public DungeonSystemTable(string path)
  {
    var data = File.ReadAllBytes(path);
    if (data.Length % Floor.Length != 0)
    {
      throw new InvalidDataException($"DungeonSystemTable length is not cleanly divisible by the " +
                                     $"entry length (0x{Floor.Length:X2}).");
    }
    AddRange(data
      .Split(Floor.Length)
      .Select(e => new DungeonSystemTable.Floor(e)));
  }
  
  /// <summary>
  /// An entry in the table.
  /// </summary>
  public class Floor
  {
    /// <summary>
    /// Whether this floor actually exists. 0 for no, 1 for yes.
    /// </summary>
    public S32 Exists { get; }
    
    /// <summary>
    /// An internal ID for the floor this entry represents. Duplicated twice for some reason.
    /// </summary>
    public S32 InternalId1 { get; }
    
    /// <summary>
    /// An internal ID for the floor this entry represents. Duplicated twice for some reason.
    /// </summary>
    public S32 InternalId2 { get; }
    
    /// <summary>
    /// An internal string, formatted dxmy, where x is the stratum ID, and y is the floor. I'm not sure where this is
    /// actually used.
    /// </summary>
    public string IdentifierString { get; }
    
    /// <summary>
    /// Which stratum this floor belongs to.
    /// </summary>
    public S32 StratumId { get; }
    
    /// <summary>
    /// Which floor number this floor is shown as.
    /// </summary>
    public S32 DisplayedFloor { get; }
    
    /// <summary>
    /// The floor's width, measured in tiles.
    /// </summary>
    public S32 Width { get; }
    
    /// <summary>
    /// The floor's height, measured in tiles.
    /// </summary>
    public S32 Height { get; }
    
    /// <summary>
    /// Unknown. Always 1 on real floors.
    /// </summary>
    public S32 Unk1 { get; }
    
    /// <summary>
    /// Unknown. Ranges from 0x00 to 0x03.
    /// </summary>
    public S32 Unk2 { get; }
    
    /// <summary>
    /// Unknown. 0xFFFFFFFF on Yggdrasil Labyrinth floors, real incrementing values for Ginnungagap in EO2U.
    /// </summary>
    public S32 Unk3 { get; }
    
    /// <summary>
    /// Unknown.
    /// </summary>
    public S32 Unk4 { get; }
    
    /// <summary>
    /// Unknown.
    /// </summary>
    public S32 Unk5 { get; }
    
    /// <summary>
    /// How long each floor_system entry is.
    /// </summary>
    public const S32 Length = 0x58;

    public Floor(U8[] data)
    {
      Exists = BitConverter.ToInt32(data, 0x00);
      InternalId1 = BitConverter.ToInt32(data, 0x04);
      InternalId2 = BitConverter.ToInt32(data, 0x08);
      IdentifierString = Encoding.ASCII.GetString(data.Skip(0x0C).Take(4).ToArray());
      StratumId = BitConverter.ToInt32(data, 0x14);
      DisplayedFloor = BitConverter.ToInt32(data, 0x18);
      Width = BitConverter.ToInt32(data, 0x1C);
      Height = BitConverter.ToInt32(data, 0x20);
      Unk1 = BitConverter.ToInt32(data, 0x24);
      Unk2 = BitConverter.ToInt32(data, 0x44);
      Unk3 = BitConverter.ToInt32(data, 0x48);
      Unk4 = BitConverter.ToInt32(data, 0x4C);
      Unk5 = BitConverter.ToInt32(data, 0x50);
    }
  }
}