namespace LibEtrian.Enemy.Encounter;

/// <summary>
/// The EO2U+ version of the encounter group table.
/// </summary>
public class EncounterGroupTableV3 : List<EncounterGroupTableV3.EncounterGroupV3>
{
  public EncounterGroupTableV3(string path)
  {
    var tableData = File.ReadAllBytes(path);
    if (tableData.Length % EncounterGroupV3.Length != 0)
    {
      throw new InvalidDataException($"EncounterGroupTableV3 length is not cleanly divisible by the " +
                                     $"entry length (0x{EncounterGroupV3.Length:X2}).");
    }
    AddRange(tableData
      .Split(EncounterGroupV3.Length)
      .Select(e => new EncounterGroupV3(e)));
  }
  
  /// <summary>
  /// An entry in V2 of the encounter group table.
  /// </summary>
  public class EncounterGroupV3
  {
    /// <summary>
    /// How long each entry is.
    /// </summary>
    public const S32 Length = 0x68;

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
}