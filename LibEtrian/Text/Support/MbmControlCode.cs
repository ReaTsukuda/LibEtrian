namespace LibEtrian.Text.Support;

public class MbmControlCode
{
  public static readonly Dictionary<S32, string> TypeDescriptions = new()
  {
    { 0x01, "Linebreak" },
    { 0x02, "New Page" },
    { 0x04, "Text Color" },
    { 0x06, "Wait for Input" },
    { 0x10, "FlowScript String" },
    { 0x11, "Type 1 Non-FlowScript String" },
    { 0x12, "Set Telop (Immediate)" },
    { 0x13, "VO Call (ID)" },
    { 0x14, "Type 2 Non-FlowScript String" },
    { 0x15, "Type 3 Non-FlowScript String" },
    { 0x17, "Unknown (SSQ2 + SQ5 Only)" },
    { 0x19, "Food Effect Value" },
    { 0x1B, "VO Call (Path)" },
    { 0x40, "Guild Name" },
    { 0x41, "Item Name" },
    { 0x42, "Enemy Name" },
    { 0x43, "PC Name" },
    { 0x44, "Ship Name" },
    { 0x45, "Ingredient Icon + Name" },
    { 0x46, "Conditional Linebreak" },
    { 0x47, "Level Recommendation (QR) "},
    { 0x48, "Enemy Name (QR/Req)" },
    { 0x49, "Item Name (QR/Req)" },
    { 0x4A, "Quantity (QR/Req)" },
    { 0x50, "Quest Name (QR/Req)" },
    { 0x51, "Reward (QR/Req)" },
    { 0x52, "Floor (Req)" },
    { 0x53, "Protag Name  " },
    { 0x54, "Conditional Text Color" },
    { 0x55, "Bustup Expression" },
    { 0x56, "Frederica Name" },
    { 0x57, "Guild House Name" },
    { 0x58, "Unk Bustup Change" },
    { 0x59, "Set Telop" },
    { 0x5A, "Type 1 Data Section Value"},
    { 0x5B, "Type 2 Data Section Value"},
    { 0x5C, "Telop Off" },
    { 0x5E, "Protag Chloe Name" },
    { 0x5F, "Arianna Chloe Name" },
    { 0x60, "Flavio Chloe Name" },
    { 0x7A, "Unk Debug" },
  };

  public static readonly Dictionary<S32, S32> ShortArgumentsByType = new()
  {
    { 0x01, 0 },
    { 0x02, 0 },
    { 0x06, 0 },
    { 0x17, 0 },
    { 0x40, 0 },
    { 0x44, 0 },
    { 0x46, 0 },
    { 0x47, 0 },
    { 0x50, 0 },
    { 0x51, 0 },
    { 0x52, 0 },
    { 0x53, 0 },
    { 0x56, 0 },
    { 0x57, 0 },
    { 0x5C, 0 },
    { 0x5E, 0 },
    { 0x5F, 0 },
    { 0x60, 0 },
    { 0x04, 1 },
    { 0x10, 1 },
    { 0x11, 1 },
    { 0x14, 1 },
    { 0x15, 1 },
    { 0x19, 1 },
    { 0x41, 1 },
    { 0x42, 1 },
    { 0x43, 1 },
    { 0x45, 1 },
    { 0x48, 1 },
    { 0x49, 1 },
    { 0x4A, 1 },
    { 0x54, 1 },
    { 0x59, 1 },
    { 0x5A, 1 },
    { 0x5B, 1 },
    { 0x7A, 1 },
    { 0x55, 2 },
    { 0x58, 3 },
  };
  
  public S32 Position { get; init; }
  public S32 Type { get; init; }
  public List<NumericArgument> ShortArguments { get; } = [];
  public bool HasShortArguments => ShortArguments.Count > 0;
  public List<NumericArgument> IntArguments { get; } = [];
  public bool HasIntArguments => IntArguments.Count > 0;
  public bool HasStringArgument => !string.IsNullOrWhiteSpace(StringArgument);
  public string StringArgument { get; set; }
  public override string ToString()
  {
    var typeString = TypeDescriptions.ContainsKey(Type)
      ? TypeDescriptions[Type]
      : $"0x{Type:X2}";
    if (HasShortArguments)
    {
      return $"{typeString} @ 0x{Position:X4} ({string.Join(", ",  ShortArguments)})";
    }
    if (HasStringArgument)
    {
      return $"{typeString} @ 0x{Position:X4} (\"{StringArgument}\")";
    }
    return $"{typeString} @ 0x{Position:X4}";
  }

  public class NumericArgument
  {
    public S32 Position { get; init; }
    public S32 Value { get; init; }
    public override string ToString() => $"0x{Value:X2}";
  }
}