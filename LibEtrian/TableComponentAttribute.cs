namespace LibEtrian;

/// <summary>
/// An interface that defines the class as being able to be built by the TableBuilder. 
/// </summary>
public class TableComponentAttribute(S32 length) : Attribute
{
  /// <summary>
  /// How long this component is.
  /// </summary>
  public S32 Length { get; }= length;
}