using AutoMapper;
using JobVacancy.API.models.dtos.Category;
using JobVacancy.API.models.dtos.Users;
using JobVacancy.API.models.entities;
using JobVacancy.API.Services.Interfaces;
using JobVacancy.API.Utils.Filters.Category;
using JobVacancy.API.Utils.Page;
using JobVacancy.API.Utils.Res;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace JobVacancy.API.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class CategoryController(
    ITokenService tokenService,
    ICategoryService categoryService,
    IRolesService rolesService,
    IConfiguration configuration,
    IMapper mapper
    ) : Controller
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseHttp<CategoryDto>))]
    [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ResponseHttp<object>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResponseHttp<object>))]
    [EnableRateLimiting("CreateItemPolicy")]
    [Authorize(Roles = "MASTER_ROLE, SUPER_ADM_ROLE")]
    public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryDto dto)
    {
        try
        {
            bool checkName = await categoryService.ExistsByName(dto.Name);
            if (checkName)
            {
                return StatusCode(StatusCodes.Status409Conflict, new ResponseHttp<object>
                {
                    Code = StatusCodes.Status409Conflict,
                    Message = "Name is taken",
                    Data = null,
                    Status = false,
                    Timestamp = DateTimeOffset.UtcNow,
                    TraceId = HttpContext.TraceIdentifier,
                    Version = 1
                });
            }

            CategoryEntity category = mapper.Map<CategoryEntity>(dto);

            category = await categoryService.CreateAsync(category);

            return StatusCode(StatusCodes.Status201Created, new ResponseHttp<CategoryDto>
            {
                Data = mapper.Map<CategoryDto>(category),
                Message = "Category created with success.",
                Status = true,
                Timestamp = DateTimeOffset.UtcNow,
                TraceId = HttpContext.TraceIdentifier,
                Version = 1
            });
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseHttp<object>
            {
                Data = e.StackTrace,
                Code = StatusCodes.Status500InternalServerError,
                Message = e.Message,
                TraceId = HttpContext.TraceIdentifier,
                Version = 1,
                Status = false,
                Timestamp = DateTimeOffset.UtcNow,
            });
        }
    }

    [HttpGet("{id:required}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseHttp<CategoryDto>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResponseHttp<object>))]
    public async Task<IActionResult> GetCategory(string id)
    {
        try
        {
            CategoryEntity? category = await categoryService.GetByIdAsync(id);
            if (category is null)
            {
                return StatusCode(StatusCodes.Status404NotFound, new ResponseHttp<object>
                {
                    Code = StatusCodes.Status404NotFound,
                    Message = "Category not found",
                    Data = null,
                    Status = false,
                    Timestamp = DateTimeOffset.UtcNow,
                    TraceId = HttpContext.TraceIdentifier,
                    Version = 1
                });
            }
            
            CategoryDto dto = mapper.Map<CategoryDto>(category);
            
            return StatusCode(StatusCodes.Status200OK, new ResponseHttp<CategoryDto>
            {
                Code = 200,
                Data = mapper.Map<CategoryDto>(category),
                Message = "Category found with success.",
                Status = true,
                Timestamp = DateTimeOffset.UtcNow,
                TraceId = HttpContext.TraceIdentifier,
                Version = 1
            });
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseHttp<object>
            {
                Data = e.StackTrace,
                Code = StatusCodes.Status500InternalServerError,
                Message = e.Message,
                TraceId = HttpContext.TraceIdentifier,
                Version = 1,
                Status = false,
                Timestamp = DateTimeOffset.UtcNow,
            });
        }
    }
    
    [HttpDelete("{id:required}")]
    [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(ResponseHttp<object>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResponseHttp<object>))]
    [Authorize(Roles = "MASTER_ROLE, SUPER_ADM_ROLE")]
    public async Task<IActionResult> DeleteCategory(string id)
    {
        try
        {
            CategoryEntity? category = await categoryService.GetByIdAsync(id);
            if (category is null)
            {
                return StatusCode(StatusCodes.Status404NotFound, new ResponseHttp<object>
                {
                    Code = StatusCodes.Status404NotFound,
                    Message = "Category not found",
                    Data = null,
                    Status = false,
                    Timestamp = DateTimeOffset.UtcNow,
                    TraceId = HttpContext.TraceIdentifier,
                    Version = 1
                });
            }
            
            await categoryService.DeleteAsync(category);
            
            return StatusCode(StatusCodes.Status204NoContent, new ResponseHttp<object>
            {
                Code = StatusCodes.Status204NoContent,
                Data = null,
                Message = "Category deleted with success.",
                Status = true,
                Timestamp = DateTimeOffset.UtcNow,
                TraceId = HttpContext.TraceIdentifier,
                Version = 1
            });
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseHttp<object>
            {
                Data = e.StackTrace,
                Code = StatusCodes.Status500InternalServerError,
                Message = e.Message,
                TraceId = HttpContext.TraceIdentifier,
                Version = 1,
                Status = false,
                Timestamp = DateTimeOffset.UtcNow,
            });
        }
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Page<CategoryDto>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResponseHttp<object>))]
    public async Task<IActionResult> GetCategories([FromQuery] CategoryFilterParams filter)
    {
        IQueryable<CategoryEntity> iQueryable = categoryService.GetIQueryable();
        IQueryable<CategoryEntity> appliedFilter = CategoryFilterQuery.ApplyFilter(iQueryable, filter);
        PaginatedList<CategoryEntity> paginatedList = await PaginatedList<CategoryEntity>.CreateAsync(
            source: appliedFilter,
            pageSize: filter.PageSize,
            pageIndex: filter.PageNumber
        );
        
        Page<CategoryDto> dtos = mapper.Map<Page<CategoryDto>>(paginatedList);
        
        return Ok(dtos);
    }
    
    
}