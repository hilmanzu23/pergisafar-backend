using System;

public class OperatorChecker
{
    public static string GetOperatorName(string phoneNumber)
    {
        if (phoneNumber != null && phoneNumber.Length >= 4)
        {
            string operatorCode = phoneNumber.Substring(0, 4); // Get the first 4 digits
            switch (operatorCode)
            {
                case "0814":
                case "0815":
                case "0816":
                case "0855":
                case "0856":
                case "0857":
                case "0858":
                    return "hindosat";
                case "0817":
                case "0818":
                case "0819":
                case "0859":
                case "0877":
                case "0878":
                    return "XL";
                case "0838":
                case "0837":
                case "0831":
                case "0832":
                    return "AXIS";
                case "0812":
                case "0813":
                case "0852":
                case "0853":
                case "0821":
                case "0823":
                case "0822":
                case "0851":
                    return "TELKOMSEL";
                case "0881":
                case "0882":
                case "0883":
                case "0884":
                case "0885":
                case "0886":
                case "0887":
                case "0888":
                    return "SMARTFREN";
                case "0896":
                case "0897":
                case "0898":
                case "0899":
                case "0895":
                    return "THREE";
                case "085154":
                case "085155":
                case "085156":
                case "085157":
                case "085158":
                    return "by.U";
                default:
                    return "Other Operator"; // You can change this message to fit your requirements.
            }
        }
        
        return "Invalid Phone Number"; // Handle cases where the phone number is too short to check.
    }

    public static void Main(string[] args)
    {
        string phoneNumber = "081412345678"; // Replace this with the phone number you want to check.
        string operatorName = GetOperatorName(phoneNumber);
        Console.WriteLine($"Phone number {phoneNumber} belongs to {operatorName}");
    }
}