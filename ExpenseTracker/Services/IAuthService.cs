using ExpenseTracker.DTO;

namespace ExpenseTracker.Services
{
    public interface IAuthService
    {
         Task<AuthDto> registerUser(RegisterUserDto registerUserDto);
         Task<AuthDto> loginUser(LoginUserDto loginUserDto);

         Task<AuthDto> forgotPassword(ForgotPasswordDto forgotPasswordDto);

         Task<AuthDto> resetPassword(ResetPasswordDto resetPasswordDto);
    }
}