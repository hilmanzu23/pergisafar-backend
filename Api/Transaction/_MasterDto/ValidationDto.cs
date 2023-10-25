using Microsoft.IdentityModel.Tokens;

public class ValidationTransactionsDto
{
    public List<object> ValidateBannerInput(ImageUploadViewModel items)
    {
        var errors = new List<object>();

        if (items == null || string.IsNullOrEmpty(items.Name))
        {
            errors.Add(new { Name = "Name is a required field." });
        }
        return errors;
    }
}