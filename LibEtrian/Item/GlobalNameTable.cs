using LibEtrian.Text.Types;

namespace LibEtrian.Item;

/// <summary>
/// Constructs the singleton name table that the games actually use at runtime. Annoyingly, the exact way this is
/// constructed varies from game to game, and includes tables specific to each game.
/// </summary>
public class GlobalNameTable : List<string>
{
  public const string EquipItemNameKey = "Equip";
  public const string UseItemNameKey = "Use";
  public const string SkyItemNameKey = "Sky";
  public const string IngredientItemNameKey = "Ingredient";
  public const string FoodItemNameKey = "Food";

  private static readonly List<string> ExpectedEO4Keys =
  [
    EquipItemNameKey,
    UseItemNameKey,
    SkyItemNameKey,
  ];

  private static readonly List<string> ExpectedEO2UKeys =
  [
    EquipItemNameKey,
    UseItemNameKey,
    IngredientItemNameKey
  ];

  private static readonly List<string> ExpectedEO5Keys =
  [
    EquipItemNameKey,
    UseItemNameKey,
    FoodItemNameKey
  ];
  
  public GlobalNameTable(Dictionary<string, string> paths, Games game)
  {
    switch (game)
    {
      case Games.EO4:
        BuildEO4(paths);
        break;
      case Games.EO2U:
        BuildEO2U(paths);
        break;
      case Games.EO5:
        BuildEO5(paths);
        break;
      default:
        Console.WriteLine($"Unsupported game for GlobalNameTable: {game}");
        break;
    }
  }

  private void BuildEO4(Dictionary<string, string> paths)
  {
    if (ExpectedEO4Keys.Any(key => !paths.ContainsKey(key)))
    {
      throw new ArgumentException($"GlobalNameTable is missing required keys: " +
                                  $"{string.Join(", ", ExpectedEO4Keys.Where(key => !paths.ContainsKey(key)))}");
    }
    AddRange(new Table(paths[EquipItemNameKey])
      .Concat(new Table(paths[UseItemNameKey]).Skip(1)
      .Concat(Enumerable.Repeat("Dummy", 502))
      .Concat(new Table(paths[SkyItemNameKey]))));
  }

  private void BuildEO2U(Dictionary<string, string> paths)
  {
    if (ExpectedEO2UKeys.Any(key => !paths.ContainsKey(key)))
    {
      throw new ArgumentException($"GlobalNameTable is missing required keys: " +
                                  $"{string.Join(", ", ExpectedEO2UKeys.Where(key => !paths.ContainsKey(key)))}");
    }
    AddRange(new Table(paths[EquipItemNameKey])
      .Concat(new Table(paths[UseItemNameKey]).Skip(1))
      .Concat(Enumerable.Repeat("Dummy", 100))
      .Concat(new Table(paths[IngredientItemNameKey])));
  }

  private void BuildEO5(Dictionary<string, string> paths)
  {
    if (ExpectedEO5Keys.Any(key => !paths.ContainsKey(key)))
    {
      throw new ArgumentException($"GlobalNameTable is missing required keys: " +
                                  $"{string.Join(", ", ExpectedEO5Keys.Where(key => !paths.ContainsKey(key)))}");
    }
    AddRange(new Table(paths[EquipItemNameKey])
      .Concat(new Table(paths[UseItemNameKey]).Skip(1))
      .Concat(Enumerable.Repeat("Dummy", 200))
      .Concat(new Table(paths[FoodItemNameKey])));
  }
}