namespace LibEtrian.Dungeon.Gather;

/// <summary>
/// The variable values for a given point in EO2U, based on what set of flags are true.
/// </summary>
[TableComponent(0x28)]
public class GatherPointV4Set(U8[] data)
{
  /// <summary>
  /// This flag must be true for this set to be loaded. Unused in EO2U.
  /// </summary>
  public S16 TrueFlag { get; } = BitConverter.ToInt16(data, 0x04);
  
  /// <summary>
  /// This flag must be false for this set to be loaded. Boy do I not like that name. It's also unused in EO2U.
  /// </summary>
  public S16 FalseFlag { get; } = BitConverter.ToInt16(data, 0x06);
  
  /// <summary>
  /// The chance for this gather point to give its first item.
  /// </summary>
  public S32 Item1Chance { get; } = BitConverter.ToInt32(data, 0x08);
  
  /// <summary>
  /// The chance for this gather point to give its second item.
  /// </summary>
  public S32 Item2ChanceInternal { get; } = BitConverter.ToInt32(data, 0x0C);

  /// <summary>
  /// The actual effective chance for the second item to be distributed, factoring in the first item's chance.
  /// </summary>
  public S32 Item2Chance => (S32)((Item2ChanceInternal / 100.0) * (1 - (Item1Chance / 100.0)) * 100);
  
  /// <summary>
  /// The chance for this gather point to give its third item.
  /// </summary>
  public S32 Item3Chance => 100 - Item1Chance - Item2Chance;

  /// <summary>
  /// The items this point can give.
  /// </summary>
  public S16[] ItemIds = data
    .Skip(0x10)
    .Take(6)
    .Split(2)
    .Select(e => BitConverter.ToInt16(e, 0))
    .ToArray();

  /// <summary>
  /// The chance of the party getting ambushed when using this gather point.
  /// </summary>
  public S32 AmbushChance { get; } = BitConverter.ToInt32(data, 0x18);

  /// <summary>
  /// The ingredient this point gives.
  /// </summary>
  public S16 IngredientId { get; } = BitConverter.ToInt16(data, 0x1C);
  
  /// <summary>
  /// The chance for this point to give its ingredient. Always 100 in the retail game.
  /// </summary>
  public U8 IngredientChance { get; } = data[0x1E];

  /// <summary>
  /// Unknown. Some sort of boosted amount min?
  /// </summary>
  public U8 Unk1 { get; } = data[0x1F];

  /// <summary>
  /// Unknown. Some sort of boosted amount max?
  /// </summary>
  public U8 Unk2 { get; } = data[0x20];

  /// <summary>
  /// The minimum amount of the ingredient this point can give.
  /// </summary>
  public S32 IngredientAmountMin { get; } = data[0x21];

  /// <summary>
  /// The maximum amount of the ingredient this point can give.
  /// </summary>
  public S32 IngredientAmountMax { get; } = data[0x22];
}