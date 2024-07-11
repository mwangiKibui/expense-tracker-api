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

public class ExpenseTypeController : Controller{
    // public static User user = new User();
    
    private readonly IExpenseTypeService _expenseTypeService;
    private readonly DataContext _dbContext;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public ExpenseTypeController(IExpenseTypeService expenseTypeService,DataContext dbContext,IHttpContextAccessor httpContextAccessor){
        _expenseTypeService = expenseTypeService;
        _dbContext = dbContext;
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpPost]
    [Authorize]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ResponseExpenseTypeDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]

    public async Task<ActionResult<ResponseExpenseTypeDto>> AddExpenseType(AddExpenseTypeDto addExpenseTypeDto){
        try{
            AuthenticatedUser? authenticatedUser = await AuthHelper.GetAuthenticatedUser(_dbContext,_httpContextAccessor?.HttpContext?.User!);
            ResponseExpenseTypeDto result = await _expenseTypeService.AddExpenseType(authenticatedUser,addExpenseTypeDto);
            if(result.StatusCode == System.Net.HttpStatusCode.Created || result.StatusCode  == System.Net.HttpStatusCode.OK){
                // log that a user has created an expense type or restored a soft deleted record.
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

    [HttpGet]
    [Authorize]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ListResponseExpenseTypeDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]

    public async Task<ActionResult<ListResponseExpenseTypeDto>> GetExpenseType([FromQuery] ExpenseTypeQueryDto expenseTypeQueryDto){
        try{
            // AuthenticatedUser? authenticatedUser = await AuthHelper.GetAuthenticatedUser(_dbContext,_httpContextAccessor?.HttpContext?.User!);
            ListResponseExpenseTypeDto result = await _expenseTypeService.GetExpenseType(expenseTypeQueryDto);
            if(result.StatusCode == System.Net.HttpStatusCode.OK){
                // log that a user has created an expense type
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

    [HttpPut("{id}")]
    [Authorize]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ResponseExpenseTypeDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]

    public async Task<ActionResult<ResponseExpenseTypeDto>> UpdateExpenseType(Guid id,UpdateExpenseTypeDto updateExpenseTypeDto){
        try{
            // AuthenticatedUser? authenticatedUser = await AuthHelper.GetAuthenticatedUser(_dbContext,_httpContextAccessor?.HttpContext?.User!);
            ResponseExpenseTypeDto result = await _expenseTypeService.UpdateExpenseType(id,updateExpenseTypeDto);
            if(result.StatusCode == System.Net.HttpStatusCode.OK){
                // log that a user has updated a expense type
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


    [HttpDelete("{id}")]
    [Authorize]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ResponseExpenseTypeDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]

    public async Task<ActionResult<ResponseExpenseTypeDto>> DeleteExpenseType(Guid id){
        try{
            AuthenticatedUser? authenticatedUser = await AuthHelper.GetAuthenticatedUser(_dbContext,_httpContextAccessor?.HttpContext?.User!);
            ResponseExpenseTypeDto result = await _expenseTypeService.DeleteExpenseType(authenticatedUser,id);
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