namespace LibEtrian.Enemy.EnemyGraphic;

public class EnemyGraphicTable3DS : List<EnemyGraphicTableEntry3DS>
{
  /// <summary>
  /// Length of an enemygraphic entry in EO4.
  /// </summary>
  private const S32 EntryLengthEO4 = 0x84;
  
  /// <summary>
  /// Length of an enemygraphic entry in all other 3DS games.
  /// </summary>
  private const S32 EntryLengthOthers = 0x88;

  public EnemyGraphicTable3DS(string path, Games game)
  {
    if (game == Games.EO3)
    {
      throw new ArgumentException("EO3 is not a valid game for EnemyGraphicTable3DS.");
    }
    var entryLength = game switch
    {
      Games.EO4 => EntryLengthEO4,
      _ => EntryLengthOthers
    };
    var tableData = File.ReadAllBytes(path);
    if (tableData.Length % entryLength != 0)
    {
      throw new InvalidDataException($"enemygraphic table length is not cleanly divisible by the " +
                                     $"entry length for {game} (0x{entryLength:X2}).");
    }
    var entries = tableData
      .Select((v, i) => new { Index = i, Value = v })
      .GroupBy(v => v.Index / entryLength)
      .Select(v => 
        new EnemyGraphicTableEntry3DS(v.Select(a => a.Value).ToArray()))
      .ToList();
    AddRange(entries);
  }
}