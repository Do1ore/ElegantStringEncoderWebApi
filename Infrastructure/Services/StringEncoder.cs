using System.Text;

namespace Infrastructure.Services;

public class StringEncoder : IStringEncoder
{
    public async IAsyncEnumerable<string> GetBase64StringAsync(string input)
    {
        var byteArray = Encoding.UTF8.GetBytes(input);
        var encodedString = Convert.ToBase64String(byteArray);

        foreach (var character in encodedString)
        {
            yield return character.ToString();
            await Task.Delay(Random.Shared.Next(1000, 5000));
        }
    }
}