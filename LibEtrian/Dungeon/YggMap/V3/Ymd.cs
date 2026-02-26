namespace LibEtrian.Dungeon.YggMap.V3;

/// <summary>
/// A V3 YMD file, used by EO3.
/// </summary>
public class Ymd
{
  /// <summary>
  /// The version string that EO3 YMDs should start with.
  /// </summary>
  private const string VersionString = "YGMD3003";
  
  /// <summary>
  /// How many tiles comprise a row.
  /// </summary>
  private const S32 TilesPerRow = 35;

  /// <summary>
  /// How many rows comprise a floor.
  /// </summary>
  private const S32 RowsPerFloor = 30;

  /// <summary>
  /// The raw linear list of tiles on this floor.
  /// </summary>
  public List<Tile> Tiles;

  /// <summary>
  /// The stairs on this floor.
  /// </summary>
  public List<StairsStruct> Stairs;

  public Ymd(string path)
  {
    var data = File.ReadAllBytes(path);
    // TODO: version string check
    var header = new Header(data);
    Tiles = TableBuilder.BuildTable<Tile>(data, TilesPerRow * RowsPerFloor, header.TileDataOffset);
    Stairs = TableBuilder.BuildTable<StairsStruct>(data, header.StairsCount, header.StairsOffset);
  }

  /// <summary>
  /// Retrieves the tile for this floor at the given coordinates. This is 0-indexed.
  /// </summary>
  /// <param name="x">The X coordinate of the tile.</param>
  /// <param name="y">The Y coordinate of the tile.</param>
  /// <returns>The tile at the provided coordinates.</returns>
  public Tile GetTile(S32 x, S32 y)
  {
    // Very basic bounds checking.
    if (x >= TilesPerRow || y >= RowsPerFloor)
    {
      throw new ArgumentException($"Invalid coordinate: ({x}, {y})");
    }
    return Tiles[(y * RowsPerFloor) + x];
  }

  /// <summary>
  /// The YMD data for each tile in the physical layout of a floor.
  /// </summary>
  [TableComponent(0x8)]
  public class Tile(U8[] data)
  {
    /// <summary>
    /// The tile's type, which determines how the player interacts with it.
    /// </summary>
    public Types Type => (Types)data[0];

    /// <summary>
    /// The tile's ID, which is how other tiles on the floor interact with it.
    /// </summary>
    public S8 Id => (S8)data[1];

    /// <summary>
    /// The set of encounters the player can encounter if they start a battle on this tile.
    /// </summary>
    public U8 EncounterGroup => data[2];
    
    /// <summary>
    /// How much danger this tile adds.
    /// </summary>
    public S16 Danger => BitConverter.ToInt16(data, 4);
    
    public S16 Unk1 => BitConverter.ToInt16(data, 6);

    /// <summary>
    /// The different types of tiles.
    /// </summary>
    public enum Types
    {  
      Wall = 0x00,
      Walkable = 0x01,
      Damage = 0x02,
      Ice = 0x03,
      Hole = 0x04,
      Push1 = 0x05,
      Push2 = 0x06,
      Push3 = 0x07,
      Push4 = 0x08,
      Mud = 0x0A,
      NoMap = 0x0B,
      Spinner = 0x0C,
      Campsite = 0x0D,
      InvisibleOob = 0x0E,
      VisibleOob = 0x0F,
      Door = 0x10,
      GeomagneticPole = 0x11,
      Stairs = 0x12,
      Chest = 0x13,
      OneWay = 0x14,
      TwoWay = 0x15,
      Shutter = 0x16,
      LockedDoor = 0x17,
      Teleporter = 0x18,
      Button = 0x19,
      SeaWhirlpool = 0x1A,
      SeaShallows = 0x1B,
      SeaReef = 0x1C,
      MudNoMap = 0x1D,
      Eye = 0x1E,
    }
  }

  /// <summary>
  /// The YMD data for stairs. It's called this because I can't just call it Stairs or Stair.
  /// </summary>
  /// <param name="data">The byte data for these stairs.</param>
  [TableComponent(0x8)]
  public class StairsStruct(U8[] data)
  {
    /// <summary>
    /// The floor this set of stairs leads to. A value of 30 will send the player back to town.
    /// </summary>
    public U8 DestinationFloor => data[0];

    /// <summary>
    /// The X coordinate the player will be sent to on the destination floor.
    /// </summary>
    public U8 DestinationX => data[1];

    /// <summary>
    /// The Y coordinate the player will be sent to on the destination floor.
    /// </summary>
    public U8 DestinationY => data[2];

    /// <summary>
    /// Bitfield; angle the player will be facing on the destination floor, as well as what angle the player needs to be
    /// facing in order for the stairs to be interactable. Be careful with this, improper values  can result in the 3D
    /// viewport and the map disagreeing about what angle the player is facing, resulting in very strange behavior.
    /// </summary>
    public U8 AngleInfo => data[3];

    /// <summary>
    /// What sound will be played upon using the stairs.
    /// 1: Standard
    /// 2: Falling down a hole
    /// 3: Teleporter
    /// 4: Unknown, some kind of click
    /// 5: Entering a shortcut
    /// </summary>
    public U8 Sfx => data[4];

    /// <summary>
    /// The prompt to show in the top-right corner of the screen when facing the stairs. The following values reference
    /// uses the English text.
    /// 1: Up
    /// 2: Talk
    /// 3: Open
    /// 4: Down
    /// 5: Check
    /// 6: Camp
    /// </summary>
    public U8 Prompt => data[5];

    public U8 Unk1 => data[6];

    public U8 Unk2 => data[7];

    public override string ToString() => DestinationFloor == 30 
        ? "Town" 
        : $"{DestinationFloor}F @ ({DestinationX + 1}, {DestinationY + 1})";
  }

  private class Header(U8[] data)
  {    
    public S32 TileDataOffset => BitConverter.ToInt32(data, 0x8);
    public S32 StairsCount => BitConverter.ToInt32(data, 0x10);
    public S32 StairsOffset => BitConverter.ToInt32(data, 0x14);
    public S32 ChestsCount => BitConverter.ToInt32(data, 0x18);
    public S32 ChestsOffset => BitConverter.ToInt32(data, 0x1C);
    public S32 OneWayCount => BitConverter.ToInt32(data, 0x20);
    public S32 OneWayOffset => BitConverter.ToInt32(data, 0x24);
    public S32 TwoWayCount => BitConverter.ToInt32(data, 0x28);
    public S32 TwoWayOffset => BitConverter.ToInt32(data, 0x2C);
    public S32 ShutterCount => BitConverter.ToInt32(data, 0x30);
    public S32 ShutterOffset => BitConverter.ToInt32(data, 0x34);
    public S32 LockedDoorCount => BitConverter.ToInt32(data, 0x38);
    public S32 LockedDoorOffset => BitConverter.ToInt32(data, 0x3C);
    public S32 TeleporterCount => BitConverter.ToInt32(data, 0x40);
    public S32 TeleporterOffset => BitConverter.ToInt32(data, 0x44);
    public S32 RaftCount => BitConverter.ToInt32(data, 0x48);
    public S32 RaftOffset => BitConverter.ToInt32(data, 0x4C);
    public S32 CampCount => BitConverter.ToInt32(data, 0x50);
    public S32 CampOffset => BitConverter.ToInt32(data, 0x54);
    public S32 ButtonCount => BitConverter.ToInt32(data, 0x58);
    public S32 ButtonOffset => BitConverter.ToInt32(data, 0x5C);
    public S32 SpinnerCount => BitConverter.ToInt32(data, 0x60);
    public S32 SpinnerOffset => BitConverter.ToInt32(data, 0x64);
  }
}