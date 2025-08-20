namespace LibEtrian.Enemy.Encounter;

/// <summary>
/// The EO4 and EOU version of the encounter group table.
/// </summary>
public class EncounterGroupTableV2 : List<EncounterGroupTableV2.EncounterGroupV2>
{
  public EncounterGroupTableV2(string path)
  {
    var tableData = File.ReadAllBytes(path);
    if (tableData.Length % EncounterGroupV2.Length != 0)
    {
      throw new InvalidDataException($"EncounterGroupTableV2 length is not cleanly divisible by the " +
                                     $"entry length (0x{EncounterGroupV2.Length:X2}).");
    }
    AddRange(tableData
      .Split(EncounterGroupV2.Length)
      .Select(e => new EncounterGroupV2(e)));
  }
  
  /// <summary>
  /// An entry in V2 of the encounter group table.
  /// </summary>
  public class EncounterGroupV2
  {
    /// <summary>
    /// How long each entry is.
    /// </summary>
    public const S32 Length = 0x58;

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
}