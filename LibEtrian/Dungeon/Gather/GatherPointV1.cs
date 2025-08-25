namespace LibEtrian.Dungeon.Gather;

/// <summary>
/// An individual gather point defined in the EO3 version of itemPoint.tbl, which controls gather points.
/// </summary>
[TableComponent(0x18)]
public class GatherPointV1(U8[] data)
{
  /// <summary>
  /// The floor this point is located on.
  /// </summary>
  public U8 Floor { get; } = data[0x00];

  /// <summary>
  /// The X coordinate of this point.
  /// </summary>
  public U8 X { get; } = data[0x01];

  /// <summary>
  /// The Y coordinate of this point.
  /// </summary>
  public U8 Y { get; } = data[0x02];

  /// <summary>
  /// The type of this gather point: take (2), mine (1), and chop (0).
  /// </summary>
  public U8 Type { get; } = data[0x03];

  /// <summary>
  /// The minimum duration of the initial gather period. Always 1 in the retail game.
  /// </summary>
  public U8 InitialDurationMin { get; } = data[0x04];

  /// <summary>
  /// The maximum duration of the initial gather period. Always 1 in the retail game.
  /// </summary>
  public U8 InitialDurationMax { get; } = data[0x05];

  /// <summary>
  /// The minimum duration of the common gather period.
  /// </summary>
  public U8 CommonDurationMin { get; } = data[0x06];

  /// <summary>
  /// The maximum duration of the common gather period.
  /// </summary>
  public U8 CommonDurationMax { get; } = data[0x07];

  /// <summary>
  /// The minimum duration of the rare gather period.
  /// </summary>
  public U8 RareDurationMin { get; } = data[0x08];

  /// <summary>
  /// The maximum duration of the rare gather period.
  /// </summary>
  public U8 RareDurationMax { get; } = data[0x09];

  /// <summary>
  /// The chance for this point to give out its second item.
  /// </summary>
  public U8 Item2Chance { get; } = data[0x0B];

  /// <summary>
  /// The chance for this point to give out its third item. Not defined in the table.
  /// </summary>
  public S32 Item3Chance => 100 - Item2Chance;
  
  /// <summary>
  /// Unknown.
  /// </summary>
  public U16 Unk1 { get; } = BitConverter.ToUInt16(data, 0x0E);

  /// <summary>
  /// The gather materials this gather point can give.
  /// </summary>
  public U16[] Items { get; } = data
    .Skip(0x10)
    .Take(6)
    .Split(2)
    .Select(e => BitConverter.ToUInt16(e, 0))
    .ToArray();
}