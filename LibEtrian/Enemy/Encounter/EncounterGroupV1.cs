namespace LibEtrian.Enemy.Encounter;

/// <summary>
/// An entry in the EO3 version of the encounter group table.
/// </summary>
[TableComponent(0xC)]
public class EncounterGroupV1 : List<U8>
{
  public EncounterGroupV1(U8[] data)
  {
    AddRange(data.Skip(4));
  }
}