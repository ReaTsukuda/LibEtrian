using System.Text;

namespace LibEtrian.Foe;

/// <summary>
/// An individual entry for an FOE in EO2U.
/// </summary>
[TableComponent(0x38)]
public class FoeParamV2(U8[] data)
{
  /// <summary>
  /// The name of the FOE's field model.
  /// </summary>
  public string ModelName { get; } = Encoding.ASCII.GetString(data.Take(0x10).ToArray());

  /// <summary>
  /// What enemy this FOE resolves to when a battle is initiated.
  /// </summary>
  public S32 EnemyId { get; } = BitConverter.ToInt32(data, 0x14);
}