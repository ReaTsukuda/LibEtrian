﻿using System.Text;

namespace LibEtrian.Enemy.EnemyGraphic;

/// <summary>
/// An entry in the EO4 version of enemygraphic.tbl.
/// </summary>
[TableComponent(0x84)]
public class EnemyGraphicEntryV2
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

  public EnemyGraphicEntryV2(U8[] data)
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