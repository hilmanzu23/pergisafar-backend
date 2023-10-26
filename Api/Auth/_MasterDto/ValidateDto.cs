using Microsoft.IdentityModel.Tokens;

public class ValidationAuthDto
{
    public Dictionary<string, string> ValidateLogin(LoginDto items)
    {
        var errors = new Dictionary<string, string>();

        if (items == null || string.IsNullOrEmpty(items.Email))
        {
            errors["Email"] = "Email is a required field.";
        }

        if (items == null || string.IsNullOrEmpty(items.Password))
        {
            errors["Password"] = "Image is a required field.";
        }

        return errors;
    }

}