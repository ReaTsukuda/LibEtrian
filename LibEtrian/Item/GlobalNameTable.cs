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
  public const string IngredientItemNameKey = "Ingredient";

  private static readonly List<string> ExpectedEO2UKeys =
  [
    EquipItemNameKey,
    UseItemNameKey,
    IngredientItemNameKey
  ];
  
  public GlobalNameTable(Dictionary<string, string> paths, Games game)
  {
    switch (game)
    {
      case Games.EO2U:
        BuildEO2U(paths);
        break;
      default:
        Console.WriteLine($"Unsupported game for GlobalNameTable: {game}");
        break;
    }
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
      .Concat(new Table(paths[IngredientItemNameKey])
      ).ToList());
  }
}