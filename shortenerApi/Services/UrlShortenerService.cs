namespace shortenerApi.Services;

public class UrlShortenerService
{
    public const int NumberOfChartsShortlink = 7;
    private const string Alphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

    private readonly Random _random = new();
    
    public string GenerateUniqueCode()
    {
        var codeChars = new char[NumberOfChartsShortlink];

        for (var i = 0; i < NumberOfChartsShortlink; i++)
        {
            var randomIndex = _random.Next(Alphabet.Length);

            codeChars[i] = Alphabet[randomIndex];
        }

        var code = new string(codeChars);

        return code;

    }

}
