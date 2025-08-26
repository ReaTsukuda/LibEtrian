namespace LibEtrian.Dungeon.Gather;

/// <summary>
/// An individual gather point defined in the EO5/EON version of itemPoint.tbl, which controls gather points. Also
/// controls fishing/forage points.
/// </summary>
[TableComponent(0x78)]
public class GatherPointV5(U8[] data)
{
  /// <summary>
  /// The floor this point is located on.
  /// </summary>
  public S32 Floor { get; } = BitConverter.ToInt32(data, 0x00);

  /// <summary>
  /// The ID of this point.
  /// </summary>
  public S32 InternalId { get; } = BitConverter.ToInt32(data, 0x04);

  /// <summary>
  /// The X coordinate of this point.
  /// </summary>
  public S32 X { get; } = BitConverter.ToInt32(data, 0x08);

  /// <summary>
  /// The Y coordinate of this point.
  /// </summary>
  public S32 Y { get; } = BitConverter.ToInt32(data, 0x0C);

  /// <summary>
  /// The type of this gather point: take (2), mine (1), chop (0), fish (3), and forage (4).
  /// </summary>
  public S32 Type { get; } = BitConverter.ToInt32(data, 0x14);
  
  /// <summary>
  /// How many times this gather point can be used per day. Always 1 in the retail game.
  /// </summary>
  public S32 AttemptsPerDay { get; } = BitConverter.ToInt32(data, 0x18);
  
  /// <summary>
  /// Unknown. Always 1 in the retail game.
  /// </summary>
  public S32 Unk1 { get; } = BitConverter.ToInt32(data, 0x1C);

  /// <summary>
  /// The sets for this gather point. The last one doesn't have a trailing 0x00000000, so we have to get a bit ugly
  /// with parsing this.
  /// </summary>
  public GatherPointV5Set[] Sets { get; } = data
    .Skip(0x24)
    .Take(0x54)
    .Concat(Enumerable.Repeat((U8)0x0, 4)) // Fake a trailing 0x00000000 on the last set.
    .Split(0x2C)
    .Select(e => new GatherPointV5Set(e))
    .ToArray();
}