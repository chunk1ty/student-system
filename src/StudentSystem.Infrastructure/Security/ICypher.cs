namespace StudentSystem.Infrastructure.Security
{
    public interface ICypher
    {
        string Encrypt(string plainText);

        string Decrypt(string cipherText);

        bool IsPasswordMatch(string password, string hashedPassword);
    }
}