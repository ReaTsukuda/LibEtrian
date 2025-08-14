using System.Text;
using LibEtrian.Text.Support;

namespace LibEtrian.Text.Types;

public class Bmd : Dictionary<string, string>
{
  private static readonly S32 MaximumMessageIdLength = 0x18;

  public Bmd(string path)
  {
    var encodingProviderInstance = CodePagesEncodingProvider.Instance;
    Encoding.RegisterProvider(encodingProviderInstance);
    using var reader = new BinaryReader(new FileStream(path, FileMode.Open), Encoding.GetEncoding("shift_jis"));
    reader.ReadInt32(); // Chunk ID.
    reader.ReadUInt32(); // File size.
    reader.ReadInt32(); // MSG1
    reader.ReadInt32(); // Null.
    reader.ReadUInt32(); // "End of the text table."
    reader.ReadUInt32(); // "Number of offsets," whatever that means.
    var numberOfEntries = reader.ReadUInt32(); // Number of entries.
    reader.ReadUInt32(); // Always 20000d.
    var offsets = new List<S32>();
    for (var offsetIndex = 0; offsetIndex < numberOfEntries; offsetIndex += 1)
    {
      reader.ReadInt32(); // Always 0.
      offsets.Add(reader.ReadInt32() + 0x20); // Add 0x20, since offsets are relative to 0x20.
    }
    var stringBuilder = new StringBuilder();
    foreach (var offset in offsets)
    {
      stringBuilder.Clear();
      reader.BaseStream.Seek(offset, SeekOrigin.Begin);
      // BMD message IDs have a hard limit of 0x18 characters.
      for (var messageIdIndex = 0; messageIdIndex < MaximumMessageIdLength; messageIdIndex += 1)
      {
        var charBuffer = reader.ReadChar();
        if (charBuffer != 0) { stringBuilder.Append(charBuffer); }
      }
      var messageId = stringBuilder.ToString();
      stringBuilder.Clear();
      reader.ReadBytes(0x8); // Don't know what these things are, but they don't appear to be relevant here.
      var messageLength = reader.ReadInt32();
      var isJapaneseText = false;
      for (var messageIndex = 0; messageIndex < messageLength; messageIndex += 1)
      {
        var byteBuffer = reader.ReadByte();
        if (!isJapaneseText && byteBuffer >= 0x80 && byteBuffer <= 0x90)
        {
          isJapaneseText = true;
        }
        if (byteBuffer == 0xF2)
        {
          var remainingControlCodeBytes = reader.ReadBytes(3);
          messageIndex += 3;
          if (remainingControlCodeBytes[0] != 0x05)
          {
            stringBuilder.Append($"[{byteBuffer.ToString("X2")} {remainingControlCodeBytes[0].ToString("X2")} {remainingControlCodeBytes[1].ToString("X2")} {remainingControlCodeBytes[2].ToString("X2")}]");
          }
        }
        else if (byteBuffer == 0xF1 || byteBuffer == 0xF5)
        {
          var secondControlCodeByte = reader.ReadByte();
          messageIndex += 1;
          if (secondControlCodeByte != 0x41)
          {
            stringBuilder.Append($"[{byteBuffer.ToString("X2")} {secondControlCodeByte.ToString("X2")}]");
          }
        }
        else if (byteBuffer == 0xA)
        {
          stringBuilder.Append(' ');
        }
        else if (byteBuffer == 0)
        {
          continue;
        }
        else if (!isJapaneseText)
        {
          if (byteBuffer > 0x10)
          {
            stringBuilder.Append((char)byteBuffer);
          }
        }
        else
        {
          reader.BaseStream.Seek(-1, SeekOrigin.Current);
          var nextChar = reader.ReadChar();
          messageIndex += 1;
          if (SJISTables.FromFullwidth.ContainsKey(nextChar))
          {
            stringBuilder.Append(SJISTables.FromFullwidth[nextChar]);
          }
          else { stringBuilder.Append(nextChar); }
        }
      }
      var message = stringBuilder.ToString();
      // Remove trailing whitespace.
      if (message.Last() == ' ')
      {
        message = message.Remove(message.Length - 1);
      }
      if (ContainsKey(messageId))
      {
        this[messageId] = message;
      }
      else
      {
        Add(messageId, message);
      }
    }
  }
}