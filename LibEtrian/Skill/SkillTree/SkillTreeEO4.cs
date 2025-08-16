namespace LibEtrian.Skill.SkillTree;

/// <summary>
/// The skill tree format for EO4. Contains skill tree info for all classes, organized on a
/// skill-by-skill basis.
/// </summary>
public class SkillTreeEO4 : List<SkillTreeEntryEO4>
{
  /// <summary>
  /// Length of each entry.
  /// </summary>
  private const S32 EntryLength = 0x34;
  
  public SkillTreeEO4(string path)
  {
    var data = File.ReadAllBytes(path);
    if (data.Length % EntryLength != 0)
    {
      throw new InvalidDataException($"SkillTreeTable length is not cleanly divisible by the " +
                                     $"entry length (0x{EntryLength:X2}).");
    }
    var entries = data.Split(EntryLength)
      .Select(e => new SkillTreeEntryEO4(e));
    AddRange(entries);
  }
}