namespace LibEtrian;

/// <summary>
/// For assembling TableComponents together into a List.
/// </summary>
public static class TableBuilder
{
  /// <summary>
  /// Constructs a table from a file.
  /// </summary>
  /// <param name="path">The path to the table.</param>
  /// <typeparam name="T">The type the table is built out of.</typeparam>
  /// <returns>A List containing the constructed table entries.</returns>
  /// <exception cref="InvalidDataException">Thrown when there are issues with the T or the args.</exception>
  public static List<T> BuildTable<T>(string path)
  {
    var data = File.ReadAllBytes(path);
    if (Attribute.GetCustomAttribute(typeof(T), typeof(TableComponentAttribute)) 
        is not TableComponentAttribute attr)
    {
      throw new InvalidDataException($"BuildTable called on {typeof(T)}, " +
                                     $"which is not marked with TableComponentAttribute.");
    }
    if (data.Length % attr.Length != 0)
    {
      throw new InvalidDataException($"Table length is not cleanly divisible by the " +
                                     $"entry length (0x{attr.Length:X2}).");
    }
    var ctor = typeof(T).GetConstructor([typeof(U8[])]);
    if (ctor is null)
    {
      throw new InvalidDataException($"BuildTable called on {typeof(T)}, " +
                                     $"which does not have a U8[] constructor.");
    }
    return data
      .Split(attr.Length)
      .Select(e => (T)ctor.Invoke([e]))
      .ToList();
  }
  
  /// <summary>
  /// Constructs a table from an in-memory byte array.
  /// </summary>
  /// <param name="data">The table's binary data.</param>
  /// <typeparam name="T">The type the table is built out of.</typeparam>
  /// <returns>A List containing the constructed table entries.</returns>
  /// <exception cref="InvalidDataException">Thrown when there are issues with the T or the args.</exception>
  public static List<T> BuildTable<T>(U8[] data)
  {
    if (Attribute.GetCustomAttribute(typeof(T), typeof(TableComponentAttribute)) 
        is not TableComponentAttribute attr)
    {
      throw new InvalidDataException($"BuildTable called on {typeof(T)}, " +
                                     $"which is not marked with TableComponentAttribute.");
    }
    if (data.Length % attr.Length != 0)
    {
      throw new InvalidDataException($"Table length is not cleanly divisible by the " +
                                     $"entry length (0x{attr.Length:X2}).");
    }
    var ctor = typeof(T).GetConstructor([typeof(U8[])]);
    if (ctor is null)
    {
      throw new InvalidDataException($"BuildTable called on {typeof(T)}, " +
                                     $"which does not have a U8[] constructor.");
    }
    return data
      .Split(attr.Length)
      .Select(e => (T)ctor.Invoke([e]))
      .ToList();
  }

  /// <summary>
  /// Constructs a table from an in-memory byte array, with a specified count.
  /// </summary>
  /// <param name="rawData">The table's binary data.</param>
  /// <param name="count">How many elements to construct objects for.</param>
  /// <param name="offset">How far into the table data to begin constructing the table object.</param>
  /// <typeparam name="T">The type the table is built out of.</typeparam>
  /// <returns>A List containing the constructed table entries.</returns>
  /// <exception cref="InvalidDataException">Thrown when there are issues with the T or the args.</exception>
  public static List<T> BuildTable<T>(U8[] rawData, S32 count, S32 offset = 0)
  {
    if (Attribute.GetCustomAttribute(typeof(T), typeof(TableComponentAttribute)) 
        is not TableComponentAttribute attr)
    {
      throw new InvalidDataException($"BuildTable called on {typeof(T)}, " +
                                     $"which is not marked with TableComponentAttribute.");
    }
    var data = rawData.Skip(offset).Take(attr.Length * count).ToArray();
    if (data.Length % attr.Length != 0)
    {
      throw new InvalidDataException($"Table length is not cleanly divisible by the " +
                                     $"entry length (0x{attr.Length:X2}).");
    }
    var ctor = typeof(T).GetConstructor([typeof(U8[])]);
    if (ctor is null)
    {
      throw new InvalidDataException($"BuildTable called on {typeof(T)}, " +
                                     $"which does not have a U8[] constructor.");
    }
    return data
      .Split(attr.Length)
      .Select(e => (T)ctor.Invoke([e]))
      .ToList();
  }
}