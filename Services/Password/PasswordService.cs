
namespace back_dotnet.Services.Password;

public class PasswordService : IPasswordService
{
    private readonly Random _random;

    public PasswordService()
    {
        _random = new Random();
    }
    public string GeneratePassword()
    {
        string specialCharacters = "!?|_-+=@#%&*";
        string digits = "0123456789";
        string letters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        string password = "";

        int nombreSpecialChar = _random.Next(0, 3);

        for( int i = 0; i < nombreSpecialChar; ++i )
            password += GetRandomCharacter(specialCharacters);

        int nombreDigits = nombreSpecialChar == 0 ? _random.Next(1, 3) : 1;

        for( int i = 0; i < nombreDigits; ++i )
            password += GetRandomCharacter(digits);

        int nombreLetters = 8 - password.Length;
        for( int i = 0; i < nombreLetters; ++i )
            password += GetRandomCharacter(letters);

        return Shuffle(password);
    }

    private char GetRandomCharacter(string allChar)
    {
        return allChar[_random.Next(0, allChar.Length)];
    }

    private string Shuffle(string password)
    {
        char[] passChar = password.ToCharArray();

        for(int i = passChar.Length - 1; i > 0; --i)
        {
            int j = _random.Next(0, i + 1);
            char temp = passChar[i];
            passChar[i] = passChar[j];
            passChar[j] = temp;
        }
        return new string(passChar);
    }
}