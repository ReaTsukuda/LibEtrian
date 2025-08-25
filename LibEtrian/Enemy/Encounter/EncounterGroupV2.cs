namespace LibEtrian.Enemy.Encounter;

/// <summary>
/// An entry in the EO4 and EOU version of the encounter group table.
/// </summary>
[TableComponent(0x58)]
public class EncounterGroupV2
{
  /// <summary>
  /// The front row of enemies.
  /// </summary>
  public List<S32> FrontRow;

  /// <summary>
  /// The back row of enemies.
  /// </summary>
  public List<S32> BackRow;

  public EncounterGroupV2(U8[] data)
  {
    FrontRow = data
      .Skip(0x06)
      .Take(0x10)
      .Split(0x4)
      .Select(e => (S32)e[0])
      .ToList();
    BackRow = data
      .Skip(0x16)
      .Take(0x10)
      .Split(0x4)
      .Select(e => (S32)e[0])
      .ToList();
  }
}