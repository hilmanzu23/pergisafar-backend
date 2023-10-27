using System.Text.RegularExpressions;

public class GlobalValidator
{
    public static bool PhoneValidator(string phone)
    {
        string pattern = "^[0-9]+$";
        bool isNumeric = Regex.IsMatch(phone, pattern);
        if (!isNumeric)
        {
            throw new CustomException(400, "Phone", "Harus Angka Numerik");
        }
        if (phone.Length < 11)
        {
            throw new CustomException(400, "Phone", "Nomor harus lebih 11 karakter");
        }
        return true;
    }
}