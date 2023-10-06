using System.IdentityModel.Tokens.Jwt;

public class ConvertJWT
{
    public async Task<object> ConvertString(string accessToken)
    {
        try
        {
            var tokenAccess = accessToken.Substring("Bearer ".Length);
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.ReadJwtToken(tokenAccess);
            if (token.ValidTo >= DateTime.UtcNow)
            {
                return true;
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