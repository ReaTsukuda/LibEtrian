namespace LibEtrian.Dungeon.Gather;

/// <summary>
/// An individual gather point defined in the EO2U version of itemPoint.tbl, which controls gather points.
/// </summary>
[TableComponent(0x70)]
public class GatherPointEO2U
{
  /// <summary>
  /// The floor this point is located on.
  /// </summary>
  public S32 Floor { get; }

  /// <summary>
  /// The ID of this point.
  /// </summary>
  public S32 InternalId { get; }

  /// <summary>
  /// The X coordinate of this point.
  /// </summary>
  public S32 X { get; }

  /// <summary>
  /// The Y coordinate of this point.
  /// </summary>
  public S32 Y { get; }

  /// <summary>
  /// The type of this gather point: take (2), mine (1), and chop (0).
  /// </summary>
  public S32 Type { get; }

  /// <summary>
  /// The chance for this point to give out its first item.
  /// </summary>
  public S32 Item1Chance { get; }

  /// <summary>
  /// The chance for this point to give out its second item. Not the actual chance, that's contextual based on the
  /// other chances.
  /// </summary>
  public S32 Item2ChanceInternal { get; }

  public S32 Item2Chance => (S32)((Item2ChanceInternal / 100.0) * (1 - (Item2ChanceInternal / 100.0)) * 100);

  /// <summary>
  /// The chance for this point to give out its third item. Not defined in the table.
  /// </summary>
  public S32 Item3Chance => 100 - Item1Chance - Item2Chance;

  /// <summary>
  /// The gather materials this gather point can give.
  /// </summary>
  public List<S32> Items { get; }

  /// <summary>
  /// The chance of the party getting ambushed when using this gather point.
  /// </summary>
  public S32 AmbushChance { get; }

  /// <summary>
  /// The ingredient this point gives.
  /// </summary>
  public S32 IngredientId { get; }

  /// <summary>
  /// Unknown. Some sort of boosted amount floor?
  /// </summary>
  public S32 Unk1 { get; }

  /// <summary>
  /// Unknown. Some sort of boosted amount ceiling?
  /// </summary>
  public S32 Unk2 { get; }

  /// <summary>
  /// The minimum amount of the ingredient this point can give.
  /// </summary>
  public S32 IngredientAmountMin { get; }

  /// <summary>
  /// The maximum amount of the ingredient this point can give.
  /// </summary>
  public S32 IngredientAmountMax { get; }

  public GatherPointEO2U(U8[] data)
  {
    Floor = BitConverter.ToInt32(data, 0x00);
    InternalId = BitConverter.ToInt32(data, 0x04);
    X = BitConverter.ToInt32(data, 0x08);
    Y = BitConverter.ToInt32(data, 0x0C);
    Type = BitConverter.ToInt32(data, 0x14);
    Item1Chance = BitConverter.ToInt32(data, 0x2C);
    Item2ChanceInternal = BitConverter.ToInt32(data, 0x30);
    Items = data.Skip(0x34).Split(2).Select(e => (S32)BitConverter.ToInt16(e)).ToList();
    AmbushChance = BitConverter.ToInt32(data, 0x3C);
    IngredientId = BitConverter.ToInt16(data, 0x40);
    Unk1 = data[0x43];
    Unk2 = data[0x44];
    IngredientAmountMin = data[0x45];
    IngredientAmountMax = data[0x46];
  }
}