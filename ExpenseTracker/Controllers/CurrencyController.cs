using System.Net.Mime;
using ExpenseTracker.Data;
using ExpenseTracker.DTO;
using ExpenseTracker.Helpers;
using ExpenseTracker.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Controllers;

[ApiController]
[Route("api/[controller]")]

public class CurrencyController : Controller{
    // public static User user = new User();
    
    private readonly ICurrencyService _currencyService;
    private readonly DataContext _dbContext;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public CurrencyController(ICurrencyService currencyService,DataContext dbContext,IHttpContextAccessor httpContextAccessor){
        _currencyService = currencyService;
        _dbContext = dbContext;
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpPost]
    [Authorize]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ListResponseCurrencyDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]

    public async Task<ActionResult<ListResponseCurrencyDto>> AddCurrency(List<AddCurrencyDto> addCurrencyDto){
        try{
            AuthenticatedUser? authenticatedUser = await AuthHelper.GetAuthenticatedUser(_dbContext,_httpContextAccessor?.HttpContext?.User!);
            ListResponseCurrencyDto result = await _currencyService.AddCurrency(authenticatedUser,addCurrencyDto);
            if(result.StatusCode == System.Net.HttpStatusCode.Created || result.StatusCode  == System.Net.HttpStatusCode.OK){
                // log that a user has created currency or restored a soft deleted record.
                return Ok(result);
            }else{
                return BadRequest(result);
            }
        }catch(Exception ex){
            String errorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            ResponseCurrencyDto response = new ResponseCurrencyDto {
                Success = false,
                Message = errorMessage,
                StatusCode = System.Net.HttpStatusCode.InternalServerError
            };
            return StatusCode(500,response);
        }
        
    }

    [HttpGet]
    [Authorize]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ListResponseCurrencyDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]

    public async Task<ActionResult<ListResponseCurrencyDto>> GetCurrencies(){
        try{
            // AuthenticatedUser? authenticatedUser = await AuthHelper.GetAuthenticatedUser(_dbContext,_httpContextAccessor?.HttpContext?.User!);
            ListResponseCurrencyDto result = await _currencyService.GetCurrency();
            if(result.StatusCode == System.Net.HttpStatusCode.OK){
                // log that a user has created an expense type
                return Ok(result);
            }else{
                return BadRequest(result);
            }
        }catch(Exception ex){
            String errorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            ResponseCurrencyDto response = new ResponseCurrencyDto {
                Success = false,
                Message = errorMessage,
                StatusCode = System.Net.HttpStatusCode.InternalServerError
            };
            return StatusCode(500,response);
        }
        
    }

    [HttpPut("{currencyId}")]
    [Authorize]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ResponseCurrencyDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]

    public async Task<ActionResult<ResponseCurrencyDto>> UpdateCurrency(Guid currencyId,[FromBody] UpdateCurrencyDto updateCurrencyDto){
        try{
            // AuthenticatedUser? authenticatedUser = await AuthHelper.GetAuthenticatedUser(_dbContext,_httpContextAccessor?.HttpContext?.User!);
            ResponseCurrencyDto result = await _currencyService.UpdateCurrency(currencyId,updateCurrencyDto);
            if(result.StatusCode == System.Net.HttpStatusCode.OK){
                // log that a user has updated a expense type
                return Ok(result);
            }else{
                return BadRequest(result);
            }
        }catch(Exception ex){
            String errorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            ResponseCurrencyDto response = new ResponseCurrencyDto {
                Success = false,
                Message = errorMessage,
                StatusCode = System.Net.HttpStatusCode.InternalServerError
            };
            return StatusCode(500,response);
        }
        
    }


    [HttpDelete("{currencyId}")]
    [Authorize]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ResponseCurrencyDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]

    public async Task<ActionResult<ResponseCurrencyDto>> DeleteCurrency(Guid currencyId){
        try{
            AuthenticatedUser? authenticatedUser = await AuthHelper.GetAuthenticatedUser(_dbContext,_httpContextAccessor?.HttpContext?.User!);
            ResponseCurrencyDto result = await _currencyService.DeleteCurrency(authenticatedUser,currencyId);
            if(result.StatusCode == System.Net.HttpStatusCode.OK){
                // log that a user has deleted a expense type
                return Ok(result);
            }else{
                return BadRequest(result);
            }
        }catch(Exception ex){
            String errorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            ResponseCurrencyDto response = new ResponseCurrencyDto {
                Success = false,
                Message = errorMessage,
                StatusCode = System.Net.HttpStatusCode.InternalServerError
            };
            return StatusCode(500,response);
        }
        
    }
}