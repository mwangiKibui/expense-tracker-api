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

public class BudgetGoalCategoryController : Controller{
    // public static User user = new User();
    
    private readonly IBudgetGoalCategoryService _budgetGoalCategoryService;
    private readonly DataContext _dbContext;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public BudgetGoalCategoryController(IBudgetGoalCategoryService budgetGoalCategoryService,DataContext dbContext,IHttpContextAccessor httpContextAccessor){
        _budgetGoalCategoryService = budgetGoalCategoryService;
        _dbContext = dbContext;
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpPost]
    [Authorize]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ListResponseBudgetGoalCategoryDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]

    public async Task<ActionResult<ListResponseBudgetGoalCategoryDto>> AddBudgetGoalCategory(List<AddBudgetGoalCategoryDto> addBudgetGoalCategoryDtos){
        try{
            AuthenticatedUser? authenticatedUser = await AuthHelper.GetAuthenticatedUser(_dbContext,_httpContextAccessor?.HttpContext?.User!);
            ListResponseBudgetGoalCategoryDto result = await _budgetGoalCategoryService.AddBudgetGoalCategory(authenticatedUser,addBudgetGoalCategoryDtos);
            if(result.StatusCode == System.Net.HttpStatusCode.Created || result.StatusCode  == System.Net.HttpStatusCode.OK){
                // log that a user has created currency or restored a soft deleted record.
                return Ok(result);
            }else{
                return BadRequest(result);
            }
        }catch(Exception ex){
            String errorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            ResponseBudgetGoalCategoryDto response = new ResponseBudgetGoalCategoryDto {
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
    [ProducesResponseType(typeof(ListResponseBudgetGoalCategoryDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]

    public async Task<ActionResult<ListResponseBudgetGoalCategoryDto>> GetBudgetGoalCategories([FromQuery] CustomQueryDto customQueryDto){
        try{
            // AuthenticatedUser? authenticatedUser = await AuthHelper.GetAuthenticatedUser(_dbContext,_httpContextAccessor?.HttpContext?.User!);
            ListResponseBudgetGoalCategoryDto result = await _budgetGoalCategoryService.GetBudgetGoalCategory(customQueryDto);
            if(result.StatusCode == System.Net.HttpStatusCode.OK){
                return Ok(result);
            }else{
                return BadRequest(result);
            }
        }catch(Exception ex){
            String errorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            ResponseBudgetGoalCategoryDto response = new ResponseBudgetGoalCategoryDto {
                Success = false,
                Message = errorMessage,
                StatusCode = System.Net.HttpStatusCode.InternalServerError
            };
            return StatusCode(500,response);
        }
        
    }

    [HttpPut("{budgetGoalCategoryId}")]
    [Authorize]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ResponseBudgetGoalCategoryDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]

    public async Task<ActionResult<ResponseBudgetGoalCategoryDto>> UpdateBudgetGoalCategory(Guid budgetGoalCategoryId,[FromBody] UpdateBudgetGoalCategoryDto updateBudgetGoalCategoryDto){
        try{
            // AuthenticatedUser? authenticatedUser = await AuthHelper.GetAuthenticatedUser(_dbContext,_httpContextAccessor?.HttpContext?.User!);
            ResponseBudgetGoalCategoryDto result = await _budgetGoalCategoryService.UpdateBudgetGoalCategory(budgetGoalCategoryId,updateBudgetGoalCategoryDto);
            if(result.StatusCode == System.Net.HttpStatusCode.OK){
                // log that a user has updated a expense type
                return Ok(result);
            }else{
                return BadRequest(result);
            }
        }catch(Exception ex){
            String errorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            ResponseBudgetGoalCategoryDto response = new ResponseBudgetGoalCategoryDto {
                Success = false,
                Message = errorMessage,
                StatusCode = System.Net.HttpStatusCode.InternalServerError
            };
            return StatusCode(500,response);
        }
        
    }


    [HttpDelete("{budgetGoalCategoryId}")]
    [Authorize]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ResponseBudgetGoalCategoryDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]

    public async Task<ActionResult<ResponseBudgetGoalCategoryDto>> DeleteBudgetGoalCategory(Guid budgetGoalCategoryId){
        try{
            AuthenticatedUser? authenticatedUser = await AuthHelper.GetAuthenticatedUser(_dbContext,_httpContextAccessor?.HttpContext?.User!);
            ResponseBudgetGoalCategoryDto result = await _budgetGoalCategoryService.DeleteBudgetGoalCategory(authenticatedUser,budgetGoalCategoryId);
            if(result.StatusCode == System.Net.HttpStatusCode.OK){
                // log that a user has deleted a budget goal category
                return Ok(result);
            }else{
                return BadRequest(result);
            }
        }catch(Exception ex){
            String errorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            ResponseBudgetGoalCategoryDto response = new ResponseBudgetGoalCategoryDto {
                Success = false,
                Message = errorMessage,
                StatusCode = System.Net.HttpStatusCode.InternalServerError
            };
            return StatusCode(500,response);
        }
        
    }
}