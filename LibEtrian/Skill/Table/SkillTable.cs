namespace LibEtrian.Skill.Table;

/// <summary>
/// An EO game's table of skills.
/// </summary>
public class SkillTable : List<Skill>
{
  /// <summary>
  /// How long a skill is in each game.
  /// </summary>
  private static readonly Dictionary<Games, S32> GameSkillLengths = new()
  {
    { Games.EO3, 0x178 },
    { Games.EO4, 0x178 },
    { Games.EOU, 0x298 },
    { Games.EO2U, 0x408 },
    { Games.EO5, 0x260 },
    { Games.EON, 0x264 }
  };
  
  public SkillTable(string path, Games game)
  {
    var tableData = File.ReadAllBytes(path);
    if (tableData.Length % GameSkillLengths[game] != 0)
    {
      throw new InvalidDataException($"Table length of {tableData.Length} is not " +
                                     $"cleanly divisible by {GameSkillLengths[game]}. " +
                                     $"The wrong game was likely selected.");
    }
    var dataSplit = tableData.Split(GameSkillLengths[game]);
    foreach (var data in dataSplit)
    {
      Add(new Skill(data, game));
    }
  }
}