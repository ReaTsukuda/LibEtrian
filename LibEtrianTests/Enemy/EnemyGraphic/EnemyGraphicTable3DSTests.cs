using LibEtrian;
using LibEtrian.Enemy.EnemyGraphic;

namespace LibEtrianTests.Enemy.EnemyGraphic;

public class EnemyGraphicTable3DSTests
{
  private const string EnemyGraphicEO4Path = "Resources/3DS/EO4EnemyGraphic.tbl";
  private const string EnemyGraphicEOUPath = "Resources/3DS/EOUEnemyGraphic.tbl";

  [Test]
  public void Ctor_EO4_LoadsCorrectly()
  {
    var table = new EnemyGraphicTable3DS(EnemyGraphicEO4Path, Games.EO4);
    using (Assert.EnterMultipleScope())
    {
      Assert.That(table[0].ModelFilename, Is.EqualTo("en001a.bam"));
      Assert.That(table[10].ModelFilename, Is.EqualTo("en051a.bam"));
    }
  }
  
  [Test]
  public void Ctor_EOU_LoadsCorrectly()
  {
    var table = new EnemyGraphicTable3DS(EnemyGraphicEOUPath, Games.EOU);
    using (Assert.EnterMultipleScope())
    {
      Assert.That(table[0].ModelFilename, Is.EqualTo("en001a.bam"));
      Assert.That(table[10].ModelFilename, Is.EqualTo("en054b.bam"));
    }
  }
  
  [Test]
  public void Ctor_EO3_ThrowsArgumentException()
  {
    Assert.Throws<ArgumentException>(
      () => new EnemyGraphicTable3DS(EnemyGraphicEO4Path, Games.EO3));
  }
  
  [Test]
  public void Ctor_Any_IncorrectLengthThrowsInvalidDataException()
  {
    using (Assert.EnterMultipleScope())
    {
      Assert.Throws<InvalidDataException>(
        () => new EnemyGraphicTable3DS(EnemyGraphicEO4Path, Games.EOU));
      Assert.Throws<InvalidDataException>(
        () => new EnemyGraphicTable3DS(EnemyGraphicEOUPath, Games.EO4));
    }
  }
}