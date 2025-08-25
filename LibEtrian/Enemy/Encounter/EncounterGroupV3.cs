namespace LibEtrian.Enemy.Encounter;

/// <summary>
/// An entry in the EO2U+ version of the encounter group table.
/// </summary>
[TableComponent(0x68)]
public class EncounterGroupV3
{
  /// <summary>
  /// The front row of enemies.
  /// </summary>
  public List<S32> FrontRow;

  /// <summary>
  /// The back row of enemies.
  /// </summary>
  public List<S32> BackRow;

  public EncounterGroupV3(U8[] data)
  {
    FrontRow = data
      .Skip(0x06)
      .Take(0x18)
      .Split(0x6)
      .Select(e => (S32)e[0])
      .ToList();
    BackRow = data
      .Skip(0x1E)
      .Take(0x18)
      .Split(0x6)
      .Select(e => (S32)e[0])
      .ToList();
  }
}