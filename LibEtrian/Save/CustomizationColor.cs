namespace LibEtrian.Save;

/// <summary>
/// A color used to customize a character in EO5 and EON.
/// </summary>
public class CustomizationColor(U8[] data)
{
  /// <summary>
  /// The color's red value.
  /// </summary>
  public U8 Red { get; set; } = data[0x0];
  
  /// <summary>
  /// The color's green value.
  /// </summary>
  public U8 Green { get; } = data[0x1];
  
  /// <summary>
  /// The color's blue value.
  /// </summary>
  public U8 Blue { get; } = data[0x2];
}