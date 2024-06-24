using ExpenseTracker.DTO;
using ExpenseTracker.Models;
using ExpenseTracker.Services;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Controllers;

[ApiController]
[Route("api/[controller]")]

public class AuthController : Controller{
    // public static User user = new User();
    
    private readonly IAuthService _authService;
    public AuthController(IAuthService authService){
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthDto>> Register(RegisterUserDto request){
       try{
          var result = await _authService.registerUser(request);
          return Ok(result);
       }catch(Exception err){
          return BadRequest(new AuthDto {
                Success = false,
                Message = err.Message,
          });
       }
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthDto>> Login(LoginUserDto request){
        try{
            var result = await _authService.loginUser(request);
            return Ok(result);
        }catch(Exception err){
            return BadRequest(new AuthDto {
                Success = false,
                Message = err.Message,
          });
        }
    }

    [HttpPost("forgotPassword")]
    public async Task<ActionResult<AuthDto>> ForgotPassword(ForgotPasswordDto request){
        try{
            var result = await _authService.forgotPassword(request);
            return Ok(result);
        }catch(Exception err){
            return BadRequest(new AuthDto{
                Success = false,
                Message = err.Message
            });
        }
    }

    [HttpPost("resetPassword")]
    public async Task<ActionResult<AuthDto>> ResetPassword(ResetPasswordDto request){
        try{
            var result = await _authService.resetPassword(request);
            return Ok(result);
        }catch(Exception err){
            return BadRequest(new AuthDto{
                Success = false,
                Message = err.Message
            });
        }
    }




}