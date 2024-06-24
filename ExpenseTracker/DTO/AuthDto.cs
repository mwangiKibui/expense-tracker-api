namespace ExpenseTracker.DTO
{
    public class AuthData {
       public required String AuthToken { get; set; }
       public String? RefreshToken { get; set; }
    }

    public class AuthDto
    {
        public required Boolean Success { get; set; }
        public required String Message { get; set; }

        public AuthData? Data { get; set; }
    }

    public class RegisterUserDto {
        public required String FirstName { get; set; }
        public String? MiddleName { get; set; }
        public required String LastName { get; set; }
        public required String Email { get; set; }
        public required String Password { get; set; }
    }

    public class LoginUserDto {
        public required String Email {get;set;}
        public required String Password {get;set;}
    }

    public class ForgotPasswordDto {
        public required String Email {get;set;}
    }

    public class ResetPasswordDto {
        public required String Code {get;set;}
        public required String Password {get;set;}
    }
}