using System.Security.Cryptography;
using System.Text;

namespace LibEtrian.Save.Crypt;

/// <summary>
/// For dealing with the save encryption on EO HD saves.
/// </summary>
public static class CryptHelper
{
  // AES configuration values.
  private const string Key = "Atlus-inc-SQS3SE";
  private const string Iv = "Atlus-inc-SQS3Se";
  private const S32 BlockSize = 128;
  private const S32 KeySize = 128;

  public static U8[] Decrypt(U8[] encrypted)
  {
    var aes = InitAes();
    return aes.DecryptCbc(encrypted, aes.IV);
  }

  public static U8[] Encrypt(U8[] unencrypted)
  {
    var aes = InitAes();
    return aes.DecryptCbc(unencrypted, aes.IV);
  }

  private static Aes InitAes()
  {
    var aes = Aes.Create();
    aes.BlockSize = BlockSize;
    aes.KeySize = KeySize;
    aes.Key = Encoding.UTF8.GetBytes(Key);
    aes.IV = Encoding.UTF8.GetBytes(Iv);
    // Mode and Padding are CBC and PKCS7, the defaults for AES, so no need to set them.
    return aes;
  }
}