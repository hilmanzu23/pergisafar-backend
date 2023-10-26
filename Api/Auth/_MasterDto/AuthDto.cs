public class LoginDto
{
    public string? Email { get; set; }
    public string? Password { get; set; }
}

public class RegisterDto
{
    public string? Email { get; set; }
    public string? FullName { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Password { get; set; }
}

public class UpdatePasswordDto
{
    public string? Password { get; set; }
    public string? ConfirmPassword { get; set; }

}

public class OtpDto
{
    public string? Otp { get; set; }

}
