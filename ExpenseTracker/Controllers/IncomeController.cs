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

public class IncomeController : Controller{
    // public static User user = new User();
    
    private readonly IIncomeService _incomeService;
    private readonly DataContext _dbContext;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public IncomeController(IIncomeService incomeService,DataContext dbContext,IHttpContextAccessor httpContextAccessor){
        _incomeService = incomeService;
        _dbContext = dbContext;
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpPost]
    [Authorize]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ResponseIncomeDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]

    public async Task<ActionResult<ResponseIncomeDto>> AddIncome(AddIncomeDto addIncomeDto){
        try{
            AuthenticatedUser? authenticatedUser = await AuthHelper.GetAuthenticatedUser(_dbContext,_httpContextAccessor?.HttpContext?.User!);
            ResponseIncomeDto result = await _incomeService.AddIncome(authenticatedUser,addIncomeDto);
            if(result.StatusCode == System.Net.HttpStatusCode.Created || result.StatusCode  == System.Net.HttpStatusCode.OK){
                // log that a user has created an expense type or restored a soft deleted record.
                return Ok(result);
            }else{
                return BadRequest(result);
            }
        }catch(Exception ex){
            String errorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            ResponseIncomeDto response = new ResponseIncomeDto {
                Success = false,
                Message = errorMessage,
                StatusCode = System.Net.HttpStatusCode.InternalServerError
            };
            return StatusCode(500,response);
        }
        
    }

    [HttpGet("{incomeId}")]
    [Authorize]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ResponseIncomeDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]

    public async Task<ActionResult<ResponseIncomeDto>> GetSingleIncome(Guid incomeId){
        try{
            // AuthenticatedUser? authenticatedUser = await AuthHelper.GetAuthenticatedUser(_dbContext,_httpContextAccessor?.HttpContext?.User!);
            ResponseIncomeDto result = await _incomeService.GetSingleIncome(incomeId);
            if(result.StatusCode == System.Net.HttpStatusCode.OK){
                // log that a user has created an expense type
                return Ok(result);
            }else{
                return BadRequest(result);
            }
        }catch(Exception ex){
            String errorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            ResponseIncomeDto response = new ResponseIncomeDto {
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
    [ProducesResponseType(typeof(ListResponseIncomeDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]

    public async Task<ActionResult<ListResponseIncomeDto>> GetIncomes([FromQuery] IncomeQueryDto incomeQueryDto){
        try{
            // AuthenticatedUser? authenticatedUser = await AuthHelper.GetAuthenticatedUser(_dbContext,_httpContextAccessor?.HttpContext?.User!);
            ListResponseIncomeDto result = await _incomeService.GetIncomes(incomeQueryDto);
            if(result.StatusCode == System.Net.HttpStatusCode.OK){
                // log that a user has created an expense type
                return Ok(result);
            }else{
                return BadRequest(result);
            }
        }catch(Exception ex){
            String errorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            ResponseIncomeDto response = new ResponseIncomeDto {
                Success = false,
                Message = errorMessage,
                StatusCode = System.Net.HttpStatusCode.InternalServerError
            };
            return StatusCode(500,response);
        }
        
    }

    [HttpPut("{incomeId}")]
    [Authorize]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ResponseIncomeDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]

    public async Task<ActionResult<ResponseIncomeDto>> UpdateIncome(Guid incomeId,[FromBody] UpdateIncomeDto updateIncomeDto){
        try{
            // AuthenticatedUser? authenticatedUser = await AuthHelper.GetAuthenticatedUser(_dbContext,_httpContextAccessor?.HttpContext?.User!);
            ResponseIncomeDto result = await _incomeService.UpdateIncome(incomeId,updateIncomeDto);
            if(result.StatusCode == System.Net.HttpStatusCode.OK){
                // log that a user has updated a expense type
                return Ok(result);
            }else{
                return BadRequest(result);
            }
        }catch(Exception ex){
            String errorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            ResponseIncomeDto response = new ResponseIncomeDto {
                Success = false,
                Message = errorMessage,
                StatusCode = System.Net.HttpStatusCode.InternalServerError
            };
            return StatusCode(500,response);
        }
        
    }


    [HttpDelete("{incomeId}")]
    [Authorize]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ResponseIncomeDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]

    public async Task<ActionResult<ResponseIncomeDto>> DeleteIncome(Guid incomeId){
        try{
            AuthenticatedUser? authenticatedUser = await AuthHelper.GetAuthenticatedUser(_dbContext,_httpContextAccessor?.HttpContext?.User!);
            ResponseIncomeDto result = await _incomeService.DeleteIncome(authenticatedUser,incomeId);
            if(result.StatusCode == System.Net.HttpStatusCode.OK){
                // log that a user has deleted a expense type
                return Ok(result);
            }else{
                return BadRequest(result);
            }
        }catch(Exception ex){
            String errorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            ResponseIncomeDto response = new ResponseIncomeDto {
                Success = false,
                Message = errorMessage,
                StatusCode = System.Net.HttpStatusCode.InternalServerError
            };
            return StatusCode(500,response);
        }
        
    }
}