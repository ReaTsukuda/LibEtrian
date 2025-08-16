using System.Globalization;
using System.Numerics;

namespace LibEtrian;

/// <summary>
/// Extension methods meant to assist in dealing with bitfields.
/// </summary>
public static class BitfieldExtensions
{
  public static bool IsBitSet<T>(this T t, S32 pos) where T : struct, IConvertible
  {
    var value = t.ToInt64(CultureInfo.CurrentCulture);
    return (value & (1 << pos)) != 0;
  }
}