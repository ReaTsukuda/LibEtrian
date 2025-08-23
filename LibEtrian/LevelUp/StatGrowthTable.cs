using LibEtrian.Common;

namespace LibEtrian.LevelUp;

/// <summary>
/// GrowthTable.tbl and GrowthKindTable.tbl, which control the stats assigned to a class/race, respectively, upon
/// reaching a certain level.
/// </summary>
public class StatGrowthTable : List<List<StatBlock>>
{
  /// <summary>
  /// How long each level entry is.
  /// </summary>
  public const S32 LevelLength = 0x14;

  /// <summary>
  /// Each game's maximum level.
  /// </summary>
  public static readonly Dictionary<Games, int> MaxLevelByGame = new()
  {
    { Games.EO3, 99 },
    { Games.EO4, 99 },
    { Games.EOU, 99 },
    { Games.EO2U, 99 },
    { Games.EO5, 99 },
    { Games.EON, 130 },
  };

  public StatGrowthTable(string path, Games game)
  {
    var levelCount = MaxLevelByGame[game] + 1; // +1 to account for level 0.
    var tableData = File.ReadAllBytes(path);
    var subTables = tableData.Split(levelCount * LevelLength);
    foreach (var subTable in subTables)
    {
      Add(subTable
        .Split(LevelLength)
        .Select(e => new StatBlock
        {
          MaxHp = BitConverter.ToInt32(e, 0x00),
          MaxTp = BitConverter.ToInt32(e, 0x04),
          Str = BitConverter.ToInt16(e, 0x08),
          Vit = BitConverter.ToInt16(e, 0x0A),
          Agi = BitConverter.ToInt16(e, 0x0C),
          Luc = BitConverter.ToInt16(e, 0x0E),
          Tec = BitConverter.ToInt16(e, 0x10),
          Wis = BitConverter.ToInt16(e, 0x12),
        }).ToList());
    }
  }
}