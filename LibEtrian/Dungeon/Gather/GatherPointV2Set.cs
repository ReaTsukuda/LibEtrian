namespace LibEtrian.Dungeon.Gather;

/// <summary>
/// The variable values for a given point in EO4, based on what set of flags are true.
/// </summary>
[TableComponent(0x18)]
public class GatherPointV2Set(U8[] data)
{
  /// <summary>
  /// This flag must be true for this set to be loaded.
  /// </summary>
  public S16 TrueFlag { get; } = BitConverter.ToInt16(data, 0x04);
  
  /// <summary>
  /// This flag must be false for this set to be loaded. Boy do I not like that name.
  /// </summary>
  public S16 FalseFlag { get; } = BitConverter.ToInt16(data, 0x06);
  
  /// <summary>
  /// The chance for this gather point to give its first item.
  /// </summary>
  public S32 Item1Chance { get; } = BitConverter.ToInt16(data, 0x08);
  
  /// <summary>
  /// The chance for this gather point to give its second item.
  /// </summary>
  public S32 Item2Chance => 100 - Item1Chance;

  /// <summary>
  /// The items this point can give.
  /// </summary>
  public S32[] ItemIds = data
    .Skip(0xC)
    .Take(8)
    .Split(4)
    .Select(e => BitConverter.ToInt32(e, 0))
    .ToArray();
}