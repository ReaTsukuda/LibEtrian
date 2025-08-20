namespace LibEtrian.Enemy.Encounter;

/// <summary>
/// The EO3 version of the encounter group table.
/// </summary>
public class EncounterGroupTableV1 : List<U8[]>
{
  /// <summary>
  /// How long each entry is.
  /// </summary>
  private const S32 EntryLength = 0xC;
  
  public EncounterGroupTableV1(string path)
  {
    var tableData = File.ReadAllBytes(path);
    AddRange(tableData
      .Split(EntryLength)
      .Select(e => e.Skip(4).ToArray()));
  }
}