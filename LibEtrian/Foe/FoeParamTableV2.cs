using System.Text;

namespace LibEtrian.Foe;

/// <summary>
/// Defines individual FOE types in EO2U. Sets what field model they use, what enemy they turn into when you
/// initiate a battle, etc.
/// </summary>
public class FoeParamTableV2 : List<FoeParamTableV2.FoeParamV2>
{
  public FoeParamTableV2(string path)
  {
    var data = File.ReadAllBytes(path);
    if (data.Length % FoeParamV2.Length != 0)
    {
      throw new InvalidDataException($"FoeParamTableV2 length is not cleanly divisible by the " +
                                     $"entry length (0x{FoeParamV2.Length:X2}).");
    }
    AddRange(data
      .Split(FoeParamV2.Length)
      .Select(e => new FoeParamV2(e)));
  }
  
  /// <summary>
  /// An individual entry for an FOE.
  /// </summary>
  public class FoeParamV2
  {
    /// <summary>
    /// How long each FoeParam entry is.
    /// </summary>
    public const S32 Length = 0x38;
    
    /// <summary>
    /// The name of the FOE's field model.
    /// </summary>
    public string ModelName { get; }
    
    /// <summary>
    /// What enemy this FOE resolves to when a battle is initiated.
    /// </summary>
    public S32 EnemyId { get; }

    public FoeParamV2(U8[] data)
    {
      ModelName = Encoding.ASCII.GetString(data.Take(0x10).ToArray());
      EnemyId = BitConverter.ToInt32(data, 0x14);
    }
  }
}