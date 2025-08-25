namespace LibEtrian;

public static class TableBuilder
{
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
}