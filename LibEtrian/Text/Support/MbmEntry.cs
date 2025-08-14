using System.Text;

namespace LibEtrian.Text.Support;

public class MbmEntry(S32 index, U8[] data, List<MbmControlCode> controlCodes)
{
  public S32 Index { get; } = index;
  private U8[] Data { get; } = data;
  public List<MbmControlCode> ControlCodes { get; } = controlCodes;

  public delegate string Formatter(MbmEntry entry, MbmControlCode controlCode);
  
  public Formatter NoArgsFormatter { get; set; } = DefaultNoArgsControlCodeFormatter;
  public Formatter ShortArgsFormatter { get; set; } = DefaultShortArgsControlCodeFormatter;
  public Formatter IntArgsFormatter { get; set; } = DefaultIntArgsControlCodeFormatter;
  public Formatter StringArgFormatter { get; set; } = DefaultStringArgControlCodeFormatter;

  public string GetString()
  {
    var buffer = new StringBuilder();
    var position = 0;
    while (position < Data.Length)
    {
      // Insert control code placeholders if we're at the correct position.
      if (controlCodes.Any(cc => cc.Position == position))
      {
        var controlCode = ControlCodes.First(cc => cc.Position == position);
        if (controlCode.HasShortArguments)
        {
          buffer.Append(ShortArgsFormatter(this, controlCode));
          position += 2 + (2 * controlCode.ShortArguments.Count);
        }
        else if (controlCode.HasIntArguments)
        {
          buffer.Append(IntArgsFormatter(this, controlCode));
          position += 2 + (4 * controlCode.IntArguments.Count);
        }
        else if (controlCode.HasStringArgument)
        {
          buffer.Append(StringArgFormatter(this, controlCode));
          var lengthMultiplier = controlCode.Type == 0x12 // 0x12 is telop set (immediate)
            ? 2 // SJIS
            : 1; // ASCII
          position += 2 
                      + controlCode.StringArgument.Length * lengthMultiplier // The string
                      + 2; // Double-width null terminator
          // For ASCII arguments, remember to pad the position back to an even offset.
          if (position % 2 == 1)
          {
            position += 1;
          }
        }
        else
        {
          buffer.Append(NoArgsFormatter(this, controlCode));
          position += 2;
        }
      }
      else
      {
        buffer.Append(Encoding.GetEncoding("shift_jis")
          .GetString(Data.Skip(position).Take(2).ToArray()));
        position += 2;
      }
    }
    return buffer.ToString().Normalize(NormalizationForm.FormKC);
  }

  public static string DefaultNoArgsControlCodeFormatter(MbmEntry entry, MbmControlCode controlCode)
  {
    return $"[{controlCode.Type:X2}]";
  }

  public static string DefaultShortArgsControlCodeFormatter(MbmEntry entry, MbmControlCode controlCode)
  {
    return $"[{controlCode.Type:X2}: {string.Join(", ", controlCode.ShortArguments
      .Select(ia => $"0x{ia.Value:X4}"))}]";
  }

  public static string DefaultIntArgsControlCodeFormatter(MbmEntry entry, MbmControlCode controlCode)
  {
    return $"[{controlCode.Type:X2}: {string.Join(", ", controlCode.IntArguments
      .Select(ia => $"0x{ia.Value:X4}"))}]";
  }

  public static string DefaultStringArgControlCodeFormatter(MbmEntry entry, MbmControlCode controlCode)
  {
    return $"[{controlCode.Type:X2}: \"{controlCode.StringArgument}\"]";
  }
}