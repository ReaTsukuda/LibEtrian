using System.Text;

namespace LibEtrian.Dungeon;

/// <summary>
/// An entry in floor_system.tbl, which contains some metadata about dungeon floors (what floor to show them as, what
/// stratum they are, etc.)
/// </summary>
[TableComponent(0x58)]
public class FloorSystemMetadata(U8[] data)
{
  /// <summary>
  /// Whether this floor actually exists. 0 for no, 1 for yes.
  /// </summary>
  public S32 Exists { get; } = BitConverter.ToInt32(data, 0x00);

  /// <summary>
  /// An internal ID for the floor this entry represents. Duplicated twice for some reason.
  /// </summary>
  public S32 InternalId1 { get; } = BitConverter.ToInt32(data, 0x04);

  /// <summary>
  /// An internal ID for the floor this entry represents. Duplicated twice for some reason.
  /// </summary>
  public S32 InternalId2 { get; } = BitConverter.ToInt32(data, 0x08);

  /// <summary>
  /// An internal string, formatted dxmy, where x is the stratum ID, and y is the floor. I'm not sure where this is
  /// actually used.
  /// </summary>
  public string IdentifierString { get; } = Encoding.ASCII.GetString(data.Skip(0x0C).Take(4).ToArray());

  /// <summary>
  /// Which stratum this floor belongs to.
  /// </summary>
  public S32 StratumId { get; } = BitConverter.ToInt32(data, 0x14);

  /// <summary>
  /// Which floor number this floor is shown as.
  /// </summary>
  public S32 DisplayedFloor { get; } = BitConverter.ToInt32(data, 0x18);

  /// <summary>
  /// The floor's width, measured in tiles.
  /// </summary>
  public S32 Width { get; } = BitConverter.ToInt32(data, 0x1C);

  /// <summary>
  /// The floor's height, measured in tiles.
  /// </summary>
  public S32 Height { get; } = BitConverter.ToInt32(data, 0x20);

  /// <summary>
  /// Unknown. Always 1 on real floors.
  /// </summary>
  public S32 Unk1 { get; } = BitConverter.ToInt32(data, 0x24);

  /// <summary>
  /// Unknown. Ranges from 0x00 to 0x03.
  /// </summary>
  public S32 Unk2 { get; } = BitConverter.ToInt32(data, 0x44);

  /// <summary>
  /// Unknown. 0xFFFFFFFF on Yggdrasil Labyrinth floors, real incrementing values for Ginnungagap in EO2U.
  /// </summary>
  public S32 Unk3 { get; } = BitConverter.ToInt32(data, 0x48);

  /// <summary>
  /// Unknown.
  /// </summary>
  public S32 Unk4 { get; } = BitConverter.ToInt32(data, 0x4C);

  /// <summary>
  /// Unknown.
  /// </summary>
  public S32 Unk5 { get; } = BitConverter.ToInt32(data, 0x50);
}