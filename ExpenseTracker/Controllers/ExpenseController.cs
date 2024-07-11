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

public class ExpenseController : Controller{
    // public static User user = new User();
    
    private readonly IExpenseService _expenseService;
    private readonly DataContext _dbContext;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public ExpenseController(IExpenseService expenseService,DataContext dbContext,IHttpContextAccessor httpContextAccessor){
        _expenseService = expenseService;
        _dbContext = dbContext;
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpPost]
    [Authorize]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ResponseExpenseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]

    public async Task<ActionResult<ResponseExpenseDto>> AddExpense(AddExpenseDto addExpenseDto){
        try{
            AuthenticatedUser? authenticatedUser = await AuthHelper.GetAuthenticatedUser(_dbContext,_httpContextAccessor?.HttpContext?.User!);
            ResponseExpenseDto result = await _expenseService.AddExpense(authenticatedUser,addExpenseDto);
            if(result.StatusCode == System.Net.HttpStatusCode.Created || result.StatusCode  == System.Net.HttpStatusCode.OK){
                // log that a user has created an expense type or restored a soft deleted record.
                return Ok(result);
            }else{
                return BadRequest(result);
            }
        }catch(Exception ex){
            String errorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            ResponseExpenseDto response = new ResponseExpenseDto {
                Success = false,
                Message = errorMessage,
                StatusCode = System.Net.HttpStatusCode.InternalServerError
            };
            return StatusCode(500,response);
        }
        
    }

    [HttpGet("{expenseId}")]
    [Authorize]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ResponseExpenseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]

    public async Task<ActionResult<ResponseExpenseDto>> GetSingleExpense(Guid expenseId){
        try{
            // AuthenticatedUser? authenticatedUser = await AuthHelper.GetAuthenticatedUser(_dbContext,_httpContextAccessor?.HttpContext?.User!);
            ResponseExpenseDto result = await _expenseService.GetSingleExpense(expenseId);
            if(result.StatusCode == System.Net.HttpStatusCode.OK){
                // log that a user has created an expense type
                return Ok(result);
            }else{
                return BadRequest(result);
            }
        }catch(Exception ex){
            String errorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            ResponseExpenseDto response = new ResponseExpenseDto {
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
    [ProducesResponseType(typeof(ListResponseExpenseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]

    public async Task<ActionResult<ListResponseExpenseDto>> GetExpenseType([FromQuery] ExpenseQueryDto expenseQueryDto){
        try{
            // AuthenticatedUser? authenticatedUser = await AuthHelper.GetAuthenticatedUser(_dbContext,_httpContextAccessor?.HttpContext?.User!);
            ListResponseExpenseDto result = await _expenseService.GetExpenses(expenseQueryDto);
            if(result.StatusCode == System.Net.HttpStatusCode.OK){
                // log that a user has created an expense type
                return Ok(result);
            }else{
                return BadRequest(result);
            }
        }catch(Exception ex){
            String errorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            ResponseExpenseDto response = new ResponseExpenseDto {
                Success = false,
                Message = errorMessage,
                StatusCode = System.Net.HttpStatusCode.InternalServerError
            };
            return StatusCode(500,response);
        }
        
    }

    [HttpPut("{expenseId}")]
    [Authorize]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ResponseExpenseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]

    public async Task<ActionResult<ResponseExpenseDto>> UpdateExpense(Guid expenseId,[FromBody] UpdateExpenseDto updateExpenseDto){
        try{
            // AuthenticatedUser? authenticatedUser = await AuthHelper.GetAuthenticatedUser(_dbContext,_httpContextAccessor?.HttpContext?.User!);
            ResponseExpenseDto result = await _expenseService.UpdateExpense(expenseId,updateExpenseDto);
            if(result.StatusCode == System.Net.HttpStatusCode.OK){
                // log that a user has updated a expense type
                return Ok(result);
            }else{
                return BadRequest(result);
            }
        }catch(Exception ex){
            String errorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            ResponseExpenseDto response = new ResponseExpenseDto {
                Success = false,
                Message = errorMessage,
                StatusCode = System.Net.HttpStatusCode.InternalServerError
            };
            return StatusCode(500,response);
        }
        
    }


    [HttpDelete("{expenseId}")]
    [Authorize]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ResponseExpenseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]

    public async Task<ActionResult<ResponseExpenseDto>> DeleteExpense(Guid expenseId){
        try{
            AuthenticatedUser? authenticatedUser = await AuthHelper.GetAuthenticatedUser(_dbContext,_httpContextAccessor?.HttpContext?.User!);
            ResponseExpenseDto result = await _expenseService.DeleteExpense(authenticatedUser,expenseId);
            if(result.StatusCode == System.Net.HttpStatusCode.OK){
                // log that a user has deleted a expense type
                return Ok(result);
            }else{
                return BadRequest(result);
            }
        }catch(Exception ex){
            String errorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            ResponseExpenseTypeDto response = new ResponseExpenseTypeDto {
                Success = false,
                Message = errorMessage,
                StatusCode = System.Net.HttpStatusCode.InternalServerError
            };
            return StatusCode(500,response);
        }
        
    }
}