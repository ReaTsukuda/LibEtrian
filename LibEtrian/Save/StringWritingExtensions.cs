using System.Text;
using HalfFullWidth;

namespace LibEtrian.Save;

/// <summary>
/// Extensions related to writing strings to save data.
/// </summary>
public static class StringWritingExtensions
{
  /// <summary>
  /// Converts a string, up to the provided maximum length, to fullwidth, and then writes it to the provided byte
  /// array.
  /// </summary>
  /// <param name="str">The string to convert and write.</param>
  /// <param name="data">The byte array to write the string to.</param>
  /// <param name="offset">Where in the byte array to write the string to.</param>
  /// <param name="maxLength">How many characters to limit the string to.</param>
  public static void WriteFullwidthToBinary(this string str, U8[] data, S32 offset, S32 maxLength)
  {
    var fullwidth = str.ToFullwidthString();
    var bytes = Encoding.GetEncoding(932).GetBytes(fullwidth);
    if (bytes.Length > maxLength * 2)
    {
      bytes = bytes.Take(maxLength * 2).ToArray();
    }
    else if (bytes.Length < maxLength * 2)
    {
      bytes = bytes.Concat(Enumerable.Repeat((U8)0, (maxLength * 2) - bytes.Length)).ToArray();
    }
    data.OverwriteRange(bytes, offset);
  }
}