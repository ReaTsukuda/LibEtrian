namespace LibEtrian.Save;

/// <summary>
/// mo2r##_game.sav. Contains the bulk of what people who want to edit their save will care about: party data, money,
/// inventory/shop state, completion state.
/// </summary>
public class EO2UGameSave
{
  /// <summary>
  /// The save's file identifier, located at the very top of the file.
  /// </summary>
  private const string SaveIdentifier = "MO2RGAME";
}