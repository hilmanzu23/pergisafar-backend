using System.IdentityModel.Tokens.Jwt;

public class ConvertJWT
{
    public object ConvertString(string accessToken)
    {
        try
        {
            var tokenAccess = accessToken.Substring("Bearer ".Length);
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.ReadJwtToken(tokenAccess);
            if (token.ValidTo >= DateTime.UtcNow)
            {
                return new { message = "Masih Berlaku" };
            }
            else
            {
                throw new UnauthorizedAccessException("Unauthorized");
            }
        }
        catch (Exception ex)
        {
            return new { ex.Message };
        }

    }
}