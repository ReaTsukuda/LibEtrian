using System.Text;

namespace LibEtrian.Skill.Tree;

/// <summary>
/// The skill tree format for EOU and all games after it. Contains both visual and data definitions for
/// skill trees.
/// </summary>
public class SkillTreeSktr
{
  /// <summary>
  /// The skill nodes contained in this skill tree.
  /// </summary>
  public List<SkillNode?> SkillNodes { get; }
  
  /// <summary>
  /// The prereqs contained in this skill tree.
  /// </summary>
  public List<PrerequisiteNode> PrerequisiteNodes { get; }
  
  /// <summary>
  /// The lines contained in this skill tree.
  /// </summary>
  public List<Line> Lines { get; }
  
  /// <summary>
  /// The file identifier used in sktr files. Used here as a basic form of input validation.
  /// </summary>
  private const string SktrFileIdentifier = "SKTR";

  public SkillTreeSktr(string path, Games game)
  {
    var data = File.ReadAllBytes(path);
    // EON has an unknown value before the file identifier, so we need to account for that.
    var skipAmount = game == Games.EON
      ? 4
      : 0;
    var fileIdentifier = Encoding.ASCII.GetString(data.Skip(skipAmount).Take(4).ToArray());
    if (fileIdentifier != SktrFileIdentifier)
    {
      throw new InvalidDataException($"Unknown sktr file identifier: {fileIdentifier}");
    }
    var skillsCount = BitConverter.ToInt32(data, 0x08 + skipAmount);
    var prereqsCount = BitConverter.ToInt32(data, 0x10 + skipAmount);
    var prereqsOffset = BitConverter.ToInt32(data, 0x14 + skipAmount);
    var linesCount = BitConverter.ToInt32(data, 0x18 + skipAmount);
    var linesOffset = BitConverter.ToInt32(data, 0x1C + skipAmount);
    var skillEntryLength = game == Games.EOU
      ? SkillNode.Length
      : SkillNodePost2U.Length;
    var skillNodeType = game == Games.EOU
      ? typeof(SkillNode)
      : typeof(SkillNodePost2U);
    SkillNodes = data
      .Skip(0x20 + skipAmount)
      .Take(skillEntryLength * skillsCount)
      .Split(skillEntryLength)
      .Select(e => Activator.CreateInstance(skillNodeType, e) as SkillNode)
      .ToList();
    PrerequisiteNodes = data
      .Skip(prereqsOffset)
      .Take(prereqsCount * PrerequisiteNode.Length)
      .Split(PrerequisiteNode.Length)
      .Select(e => new PrerequisiteNode(e))
      .ToList();
    PrerequisiteNodes = data
      .Skip(prereqsOffset)
      .Take(prereqsCount * PrerequisiteNode.Length)
      .Split(PrerequisiteNode.Length)
      .Select(e => new PrerequisiteNode(e))
      .ToList();
    Lines = data
      .Skip(linesOffset)
      .Take(linesCount * Line.Length)
      .Split(Line.Length)
      .Select(e => new Line(e))
      .ToList();
  }
  
  /// <summary>
  /// A skill node in a skill tree. This base version is used as-is for EOU.
  /// </summary>
  public class SkillNode
  {
    /// <summary>
    /// The ID of the skill associated with this node.
    /// </summary>
    public U16 SkillId { get; protected init; }
    
    /// <summary>
    /// The X coordinate of this node.
    /// </summary>
    public S32 X { get; protected init; }
    
    /// <summary>
    /// The Y coordinate of this node.
    /// </summary>
    public S32 Y { get; protected init; }
    
    /// <summary>
    /// S32 values of unknown purpose.
    /// </summary>
    public S32[] UnknownValues { get; protected init; }

    /// <summary>
    /// How many unknown values are present at the end of a skill node's data.
    /// </summary>
    private const S32 UnknownValuesCount = 2;

    /// <summary>
    /// The length of a skill node's binary data.
    /// </summary>
    public const S32 Length = 0x10;

    protected SkillNode()
    {
      
    }
    
    public SkillNode(U8[] data)
    {
      SkillId = BitConverter.ToUInt16(data, 0x0);
      X = BitConverter.ToInt16(data, 0x4);
      Y = BitConverter.ToInt16(data, 0x6);
      UnknownValues = new S32[UnknownValuesCount];
      for (var i = 0; i < UnknownValuesCount; i += 1)
      {
        UnknownValues[i] = BitConverter.ToInt32(data, 0x8 + (i * 4));
      }
    }
  }
  
  /// <summary>
  /// A skill node in a skill tree. This is for EO2U and later sktrs.
  /// </summary>
  public class SkillNodePost2U : SkillNode
  {
    /// <summary>
    /// The different types of skill nodes.
    /// </summary>
    public enum Types
    {
      /// <summary>
      /// A standard skill.
      /// </summary>
      Standard = 0,
      /// <summary>
      /// A mastery skill. If a prerequisite that relies on this skill is fulfilled, then the skill gated
      /// behind that prerequisite will automatically have one skill point invested in it.
      /// </summary>
      Mastery = 1,
      /// <summary>
      /// A Force Boost.
      /// </summary>
      ForceBoost = 2,
      /// <summary>
      /// A Force Break. This is automatically positioned next to the Force Boost in the skill tree.
      /// I don't know what happens if this is used without a Force Boost in the skill tree.
      /// </summary>
      ForceBreak = 3,
      /// <summary>
      /// A Fafnir skill that has another skill of type 5 that is dependent on its level.
      /// </summary>
      BaseSkill = 4,
      /// <summary>
      /// A Fafnir skill whose level is derived from an associated type 4 skill.
      /// </summary>
      TransformSkill = 5,
      /// <summary>
      /// A skill that Fafnir gains as the story progresses.
      /// </summary>
      ScenarioSkill = 6,
    }
    
    /// <summary>
    /// Where this skill is in the internal order. 
    /// </summary>
    public U8 OrderId { get; }
    
    /// <summary>
    /// The type of the node. 
    /// </summary>
    public Types Type { get; }

    /// <summary>
    /// How many unknown values are present at the end of a skill node's data.
    /// </summary>
    private const S32 UnknownValuesCount = 3;

    /// <summary>
    /// The length of a post-2U skill node's binary data.
    /// </summary>
    public new const S32 Length = 0x14;

    public SkillNodePost2U(U8[] data)
    {
      SkillId = BitConverter.ToUInt16(data, 0x0);
      OrderId = data[0x2];
      Type = (Types)data[0x3];
      X = BitConverter.ToInt16(data, 0x4);
      Y = BitConverter.ToInt16(data, 0x6);
      UnknownValues = new S32[UnknownValuesCount];
      for (var i = 0; i < UnknownValuesCount; i += 1)
      {
        UnknownValues[i] = BitConverter.ToInt32(data, 0x8 + (i * 4));
      }
    }
  }

  /// <summary>
  /// A prereq node in a skill tree, which determines what skills need to be invested in to unlock
  /// a particular skill. These contain both game logic (they determine the actual prereq for a skill)
  /// and presentation logic (they determine the X and Y coordinates of the prereq graphic). 
  /// </summary>
  /// <param name="data">The byte array containing the data for this prereq.</param>
  public class PrerequisiteNode(U8[] data)
  {
    /// <summary>
    /// The skill kept behind this prereq.
    /// </summary>
    public U16 TargetSkill { get; } = BitConverter.ToUInt16(data, 0x0);

    /// <summary>
    /// The skill that needs to be invested in to clear this prereq.
    /// </summary>
    public U16 PrerequisiteSkill { get; } = BitConverter.ToUInt16(data, 0x2);

    /// <summary>
    /// The level that the prereq skill needs to be increased to in order to clear the prereq.
    /// </summary>
    public U16 RequiredLevel { get; } = BitConverter.ToUInt16(data, 0x4);

    /// <summary>
    /// The X coordinate of the graphic.
    /// </summary>
    public U16 X { get; } = BitConverter.ToUInt16(data, 0x6);

    /// <summary>
    /// The Y coordinate of the graphic.
    /// </summary>
    public U16 Y { get; } = BitConverter.ToUInt16(data, 0x8);

    /// <summary>
    /// I'm not actually sure what this does for prereqs.
    /// </summary>
    public U16 OrderId { get; } = BitConverter.ToUInt16(data, 0xA);

    /// <summary>
    /// The length of each prereq's data.
    /// </summary>
    public const S32 Length = 0xC;
  }

  /// <summary>
  /// A line in a skill tree.
  /// </summary>
  /// <param name="data"></param>
  public class Line(U8[] data)
  {
    /// <summary>
    /// The X coordinate of the line's starting point.
    /// </summary>
    public U16 StartX { get; } = BitConverter.ToUInt16(data, 0x0);
    
    /// <summary>
    /// The Y coordinate of the line's starting point.
    /// </summary>
    public U16 StartY { get; } = BitConverter.ToUInt16(data, 0x2);
    
    /// <summary>
    /// The X coordinate of the line's endpoint.
    /// </summary>
    public U16 EndX { get; } = BitConverter.ToUInt16(data, 0x4);
    
    /// <summary>
    /// The Y coordinate of the line's endpoint.
    /// </summary>
    public U16 EndY { get; } = BitConverter.ToUInt16(data, 0x6);

    /// <summary>
    /// The length of each line's data.
    /// </summary>
    public const S32 Length = 0x8;
  }
}