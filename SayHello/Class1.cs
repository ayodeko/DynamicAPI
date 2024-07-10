namespace SayHello;

public class SayingHello
{
    public string Hello() => "Hello";

    public string G()
    {
        var httpCLinet = new HttpClient();
        var url = new Uri("https://example.com");
        return httpCLinet.GetAsync(url).Result.Content.ReadAsStringAsync().Result;
    }
}