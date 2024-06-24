
using ExpenseTracker.Data;
using ExpenseTracker.DTO;
using ExpenseTracker.Models;
using ExpenseTracker.Helpers;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Services
{
    public class AuthService:IAuthService
    {
        private readonly DataContext _dbContext;
        private readonly IConfiguration _configuration;
        public AuthService(DataContext dbContext,IConfiguration configuration){
            _dbContext = dbContext;
            _configuration = configuration;
        }

        public async Task<AuthDto> registerUser(RegisterUserDto registerUserDto){
            try{
                new System.Net.Mail.MailAddress(registerUserDto.Email);
                var existingUser = await _dbContext.Users.Where(usr => usr.Email.ToLower() == registerUserDto.Email.ToLower()).FirstOrDefaultAsync();
                if (existingUser is not null){
                    throw new Exception("Invalid email address");
                }
                if(registerUserDto.Password.Length < 6){
                    throw new Exception("Password must be at least six characters long");
                }
                var passwordHash = BCrypt.Net.BCrypt.HashPassword(registerUserDto.Password);
                // else create the user.
                var user = new User {
                    UserID = new Guid(),
                    FirstName = registerUserDto.FirstName,
                    MiddleName = registerUserDto.MiddleName!,
                    LastName = registerUserDto.LastName,
                    Email = registerUserDto.Email,
                    Password = passwordHash
                };
                _dbContext.Add(user);
                await _dbContext.SaveChangesAsync();
                // generate the JWT token.
                var token = new AuthHelper().GenerateToken(
                    user,
                    _configuration.GetSection("AppSettings:Token").Value!,
                    int.Parse(_configuration.GetSection("AppSettings:TokenLifetimeInMinutes").Value!)
                );
                return new AuthDto {
                    Success = true,
                    Message = "Account registered successfully",
                    Data = new AuthData {
                        AuthToken = token,
                        RefreshToken = token
                    }
                };
            }catch(Exception err){
                throw new Exception(err.Message);
            }
        }

        public async Task<AuthDto> loginUser(LoginUserDto loginUserDto){
            try{
                new System.Net.Mail.MailAddress(loginUserDto.Email);
                var existingUser = await _dbContext.Users.Where(usr => usr.Email.ToLower() == loginUserDto.Email.ToLower()).FirstOrDefaultAsync();
                if (existingUser is null){
                    throw new Exception("Invalid credentials");
                }
                var validPassword = BCrypt.Net.BCrypt.Verify(loginUserDto.Password,existingUser.Password);
                if(!validPassword){
                    throw new Exception("Invalid credentials");
                }
                // generate the JWT token.
                var token = new AuthHelper().GenerateToken(
                    existingUser,
                    _configuration.GetSection("AppSettings:Token").Value!,
                    int.Parse(_configuration.GetSection("AppSettings:TokenLifetimeInMinutes").Value!)
                );
                return new AuthDto {
                    Success = true,
                    Message = "Logged in successfully",
                    Data = new AuthData {
                        AuthToken = token,
                        RefreshToken = token
                    }
                };
            }catch(Exception err){
                throw new Exception(err.Message);
            }
        }

        public async Task<AuthDto> forgotPassword(ForgotPasswordDto forgotPasswordDto){
            try{    
                new System.Net.Mail.MailAddress(forgotPasswordDto.Email);
                var existingUser = await _dbContext.Users.Where(usr => usr.Email.ToLower() == forgotPasswordDto.Email.ToLower()).FirstOrDefaultAsync();
                if (existingUser is null){
                    throw new Exception("Invalid Email Address");
                }
                var forgotPasswordRequest = new ForgotPasswordRequest {
                    UserID = existingUser.UserID,
                    expiresOn = DateTime.UtcNow.AddMinutes(int.Parse(_configuration.GetSection("AppSettings:ForgotPasswordTokenLifeTime").Value!)),
                    code = GenerateRandomNumber(1000,9999).ToString()
                };
                // Todo::: send the email
                _dbContext.Add(forgotPasswordRequest);
                await _dbContext.SaveChangesAsync();
                return new AuthDto{
                    Success = true,
                    Message = "Recovery code sent to your email address"
                };
            }catch(Exception err){
                throw new Exception(err.Message);
            }
        }

        public async Task<AuthDto> resetPassword(ResetPasswordDto resetPasswordDto){
            try{
                var existingCode = await _dbContext.ForgotPasswordRequests.Where(fpr => fpr.code == resetPasswordDto.Code).FirstOrDefaultAsync();
                if(existingCode is null){
                    throw new Exception("Invalid reset password code");
                }
                var validResetPasswordCode = existingCode.expiresOn > DateTime.UtcNow && existingCode.isActive;
                if(!validResetPasswordCode){
                    throw new Exception("Invalid reset password code");
                }
                var existingUser = await _dbContext.Users.Where(usr => usr.UserID == existingCode.UserID).FirstOrDefaultAsync();
                if(existingCode is null){
                    throw new Exception("Invalid reset password code");
                }

                // password requirememts.
                if(resetPasswordDto.Password.Length < 6){
                    throw new Exception("Password length must be greater or equal to six characters");
                }
                var newPasswordHash = BCrypt.Net.BCrypt.HashPassword(resetPasswordDto.Password);
                existingUser!.Password = newPasswordHash;
                existingCode.isActive = false;
                await _dbContext.SaveChangesAsync();

                // TODO: notify the user that the password has changed.
                return new AuthDto{
                    Success = true,
                    Message = "Password reset successfully"
                };
            }catch(Exception err){
                throw new Exception(err.Message);
            }
        }

        private static int GenerateRandomNumber(int min,int max){
            Random _rdm = new Random();
            return _rdm.Next(min,max);
        }
    }
}