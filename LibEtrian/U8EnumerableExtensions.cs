﻿namespace LibEtrian;

/// <summary>
/// Extensions for U8[].
/// </summary>
public static class U8EnumerableExtensions
{
  public static IEnumerable<U8[]> Split(this IEnumerable<U8> data, S32 entryLength, S32 offset = 0)
  {
    return data
      .Skip(offset)
      .Select((v, i) => new { Index = i, Value = v })
      .GroupBy(v => v.Index / entryLength)
      .Select(v => v.Select(a => a.Value).ToArray());
  }

  public static void OverwriteRange(this U8[] originalData, U8[] newData, S32 offset)
  {
    for (var i = 0; i < newData.Length; i += 1)
    {
      originalData[offset + i] =  newData[i];
    }
  }
}