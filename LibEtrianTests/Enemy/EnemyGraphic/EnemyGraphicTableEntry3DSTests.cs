using LibEtrian.Enemy.EnemyGraphic;

namespace LibEtrianTests.Enemy.EnemyGraphic;

public class Tests
{
  private const string EnemyGraphicEntryEO4Path = "Resources/3DS/EO4EnemyGraphicEntry.bin";
  private const string EnemyGraphicEntryEOUPath = "Resources/3DS/EOUEnemyGraphicEntry.bin";

  [Test]
  public void Ctor_EO4_LoadsCorrectly()
  {
    var entry = new EnemyGraphicTableEntry3DS(
      File.ReadAllBytes(EnemyGraphicEntryEO4Path));
    Assert.That(entry.ModelFilename, Is.EqualTo("en001a.bam"));
  }

  [Test]
  public void Ctor_NotEO4_LoadsCorrectly()
  {
    var entry = new EnemyGraphicTableEntry3DS(
      File.ReadAllBytes(EnemyGraphicEntryEOUPath));
    Assert.That(entry.ModelFilename, Is.EqualTo("en001a.bam"));
  }

  [Test]
  public void Ctor_Malformed_ThrowsInvalidDataException()
  {
    Assert.Throws<InvalidDataException>(
      () => new EnemyGraphicTableEntry3DS(Enumerable.Repeat((U8)0x00, 100).ToArray()));
  }
}