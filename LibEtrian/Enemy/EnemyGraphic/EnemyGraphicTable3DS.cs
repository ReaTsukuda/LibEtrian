using System.Text;

namespace LibEtrian.Enemy.EnemyGraphic;

public class EnemyGraphicTable3DS : List<EnemyGraphicTable3DS.Entry>
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
    var entries = tableData.Split(entryLength)
      .Select(e => new Entry(e));
    AddRange(entries);
  }

  public class Entry
  {
    /// <summary>
    /// The model filename of this entry.
    /// </summary>
    public string ModelFilename { get; }
  
    /// <summary>
    /// The rest of the entry. At this time, it's unknown what the rest of it contains.
    /// </summary>
    private U8[] UnknownData { get; }
  
    /// <summary>
    /// The length of the allocated space for model filenames.
    /// </summary>
    private const S32 ModelFilenameLength = 0x40;

    public Entry(U8[] data)
    {
      var encoding = Encoding.ASCII;
      ModelFilename = encoding.GetString(
        data
          .Take(ModelFilenameLength)
          .Where(u8 => u8 != 0x00)
          .ToArray());
      if (!ModelFilename.Contains(".bam"))
      {
        throw new InvalidDataException("enemygraphic entry model filename didn't contain .bam; " +
                                       "is probably malformed.");
      }
      UnknownData = data.Skip(ModelFilenameLength).ToArray();
    }
  }
}