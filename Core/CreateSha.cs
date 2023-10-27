using System.Security.Cryptography;
using System.Text;

public class CreateSha
{
    private readonly string apidev;
    private readonly string username;
    private readonly string apiprod;
    private readonly string endpointDev;

    public CreateSha(IConfiguration configuration)
    {
        this.username = configuration.GetSection("IAKSettings")["Username"];
        this.apidev = configuration.GetSection("IAKSettings")["Dev"];
        this.apiprod = configuration.GetSection("IAKSettings")["Prod"];
        this.endpointDev = configuration.GetSection("IAKSettings")["EndPointDev"];
    }
    public string md5Conv(string req){

        string combinedString = username + apidev + req;

        // Create an instance of the MD5 hashing algorithm
        using (MD5 md5 = MD5.Create())
        {
            // Convert the combined string to bytes
            byte[] inputBytes = Encoding.UTF8.GetBytes(combinedString);

            // Calculate the MD5 hash
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            // Convert the hash bytes to a hexadecimal string
            string md5Hash = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

            return md5Hash;
        }
    }
}