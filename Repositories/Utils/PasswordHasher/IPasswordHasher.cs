namespace Repositories.Utils.PasswordHasher
{
    public interface IPasswordHasher
    {
        string Hash(string password);
        bool Verify(string storedHashedPassword, string inputPassword);
    }
}