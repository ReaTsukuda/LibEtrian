using System.Text;
using System.Text.RegularExpressions;
using LibEtrian.Text.Support;

namespace LibEtrian.Text.Types;

public class Mbm : List<MbmEntry?>
{
  private readonly List<S32> EntryIds = [];
  private Encoding SjisEncoding;

  /// <summary>
  /// Whether null entries cause the entry index to increment. False for EO3 to EO2U, true for EO5 and EON.
  /// </summary>
  private bool NullEntriesWriteIndex { get; set; }

  private S64 EntryTableEndAddress { get; set; } = S64.MaxValue;
  private bool ParsedNonNullEntry { get; set; }

  /// <summary>
  /// For loading and modifying EO text archives (MBMs).
  /// </summary>
  /// <param name="location">The location of the MBM file to load.</param>
  public Mbm(string location)
  {
    var encodingProviderInstance = CodePagesEncodingProvider.Instance;
    Encoding.RegisterProvider(encodingProviderInstance);
    SjisEncoding = Encoding.GetEncoding("shift_jis");
    using var reader = new BinaryReader(new FileStream(location, FileMode.Open));
    reader.ReadInt32(); // Always 0x00000000.
    reader.ReadInt32(); // MSG2 file identifier.
    reader.ReadInt32(); // Unknown; always 0x00010000.
    reader.ReadInt32(); // File size, excluding null entries.
    // This is supposed to be the number of entries, but it cannot be relied on for most EO games.
    reader.ReadUInt32();
    var entryTablePointer = reader.ReadUInt32();
    reader.ReadInt32(); // Unused.
    reader.ReadInt32(); // Unused.
    reader.BaseStream.Seek(entryTablePointer, SeekOrigin.Begin);
    // Since we can't rely on the number of entries, the way we check for end of the entry table
    // is a bit convoluted. We have to parse entries until we find the first non-null one,
    // and then mark its location as the end of the entry table. Until we find that non-null
    // entry, we have to set the end of the entry table to a very high value to keep the loop
    // from breaking.
    try
    {
      while (reader.BaseStream.Position < EntryTableEndAddress)
      {
        ReadEntry(reader);
      }
    }
    catch (EndOfStreamException)
    {
      Console.WriteLine($"{location} seems to be an empty MBM");
    }
  }

  private void ReadEntry(BinaryReader reader)
  {
    var entryIndex = reader.ReadInt32();
    var entryLength = reader.ReadUInt32();
    var stringPointer = reader.ReadUInt32();
    reader.ReadInt32(); // Always 0x00000000.
    // If this entry is not null, and we have not parsed a non-null entry yet,
    // mark that we have, and set the entry table end address accordingly.
    if (entryLength > 0 && stringPointer > 0 && !ParsedNonNullEntry)
    {
      ParsedNonNullEntry = true;
      EntryTableEndAddress = stringPointer;
    }
    // If this entry is not null, add its string.
    if (entryLength > 0 && stringPointer > 0)
    {
      var storedPosition = reader.BaseStream.Position;
      reader.BaseStream.Seek(stringPointer, SeekOrigin.Begin);
      var entryData = reader.ReadBytes((S32)entryLength);
      // Skip the 0xFFFF terminator.
      entryData = entryData.Take(entryData.Length - 2).ToArray();
      var controlCodes = GetControlCodes(entryData);
      Add(new MbmEntry(entryIndex, entryData, controlCodes));
      EntryIds.Add(entryIndex);
      reader.BaseStream.Seek(storedPosition, SeekOrigin.Begin);
    }
    // If the entry is null, add a null.
    else
    {
      Add(null!);
      // If a null entry has a non-zero index, then we're dealing with an EO5/EON MBM.
      if (entryIndex != 0)
      {
        NullEntriesWriteIndex = true;
      }
    }
  }

  /// <summary>
  /// Reads the binary representation of a string, and returns a list of all control codes found inside
  /// it. This, among other things, allows the text parser to know where and how long each control
  /// code is, so that they can be cleanly left out of text parsing.
  /// </summary>
  private List<MbmControlCode> GetControlCodes(U8[] entryData)
  {
    var result = new List<MbmControlCode>();
    var position = 0;
    while (position < entryData.Length)
    {
      var chunk = entryData.Skip(position).Take(2).ToArray();
      switch (chunk[0])
      {
        // Control code. Get what we need, update the position based on what's returned. 
        case 0x80:
        case 0xF8:
          var (controlCode, positionOffset) = GetControlCode(entryData, position);
          result.Add(controlCode);
          position += positionOffset;
          break;
        // Not a control code. Continue to the next chunk.
        default:
          position += 2;
          break;
      }
    }
    return result;
  }

  /// <summary>
  /// Constructs a MbmControlCode from the chunk and, if applicable, 
  /// </summary>
  /// <param name="entryData"></param>
  /// <param name="position"></param>
  /// <returns></returns>
  private (MbmControlCode controlCode, S32 positionOffset) GetControlCode(U8[] entryData, S32 position)
  {
    var chunk = entryData.Skip(position).Take(2).ToArray();
    var controlCode = new MbmControlCode
    {
      Type = chunk[1],
      Position = position
    };
    var positionOffset = 0;
    var stringArgument = new StringBuilder();
    var shortArguments = 0;
    switch (chunk[1])
    {
      // Int arguments
      case 0x13: // VO call (EOU, EO2U)
        controlCode.IntArguments.Add(new MbmControlCode.NumericArgument
        {
          Value = BitConverter.ToInt16(entryData.Skip(position + 2).Take(4).ToArray()),
          Position = position + 2
        });
        controlCode.IntArguments.Add(new MbmControlCode.NumericArgument
        {
          Value = BitConverter.ToInt16(entryData.Skip(position + 6).Take(4).ToArray()),
          Position = position + 6
        });
        positionOffset += 6;
        break;
      // SJIS string argument
      case 0x12: // Set telop immediate (EO2U, EO5, EON)
        var sjisStringChunk = entryData.Skip(position + 2).Take(2).ToArray();
        positionOffset += 2;
        var sjisStringChunkValue = BitConverter.ToInt16(sjisStringChunk);
        while (sjisStringChunkValue != 0x0000)
        {
          stringArgument.Append(SjisEncoding.GetString(sjisStringChunk));
          positionOffset += 2;
          sjisStringChunk = entryData.Skip(position + positionOffset).Take(2).ToArray();
          sjisStringChunkValue = BitConverter.ToInt16(sjisStringChunk);
        }
        break;
      // ASCII string argument
      case 0x1B: // VO call (EO5, EON)
        var currentByte = entryData.Skip(position + 2).Take(1);
        positionOffset += 3;
        while (currentByte.First() > 0x00)
        {
          stringArgument.Append(Encoding.ASCII.GetString([currentByte.First()]));
          currentByte = entryData.Skip(position + positionOffset).Take(1);
          positionOffset += 1;
        }
        // Adjust the position offset to account for the VO call opcode always ending on an even offset.
        if ((position + positionOffset) % 2 == 1)
        {
          positionOffset += 1;
        }
        break;
      // Short arguments
      default:
        if (MbmControlCode.ShortArgumentsByType.ContainsKey(chunk[1]))
        {
          shortArguments = MbmControlCode.ShortArgumentsByType[chunk[1]];
        }
        else
        {
          Console.WriteLine($"  Unknown control code type 0x{chunk[1]:X2}");
          positionOffset += 2;
        }
        break;
    }
    // Read however many int arguments this control code has.
    for (var i = 0; i < shortArguments; i += 1)
    {
      controlCode.ShortArguments.Add(new MbmControlCode.NumericArgument
      {
        Value = BitConverter.ToInt16(entryData.Skip(position + (2 * (i + 1))).Take(2).ToArray()),
        Position = position + 2
      });
    }
    positionOffset += (2 * (shortArguments + 1));
    controlCode.StringArgument = stringArgument.ToString().Normalize(NormalizationForm.FormKC);
    return new ValueTuple<MbmControlCode, S32>(controlCode, positionOffset);
  }
}