using Application.Services.Abstractions.Interfaces.Mapper;
using Application.Services.Abstractions.Interfaces.Repositories.News;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Presentation.DTO;
using Presentation.DTO.News;

namespace Presentation.Controllers.News;

[ApiController]
[Route("[controller]")]
public sealed class NewsController : ControllerBase
{
    private readonly INewsRepository _newsRepository;

    public NewsController(INewsRepository newsRepository)
    {
        _newsRepository = newsRepository;
    }

    [HttpGet("Get/{newsId}")]
    public async Task<IActionResult> GetNewsById([FromServices] IObjectMapperService objectMapperService, Guid newsId,
        CancellationToken cancellationToken)
    {
        var news = await _newsRepository.GetByIdAsync(newsId, cancellationToken: cancellationToken);

        if (news == null)
        {
            Response errorResponse = new()
            {
                Message = "news_controller_get_news_by_id_no_news_found_error"
            };

            return NotFound(errorResponse);
        }

        var newsDto = new NewsDto();
        objectMapperService.Map(news, newsDto);

        Response response = new()
        {
            Data = newsDto
        };

        return Ok(response);
    }

    [HttpGet("PagesCount")]
    public async Task<IActionResult> GetNewsCount([FromServices] INewsRepository newsRepository, double pageSize)
    {
        var total = await newsRepository
            .GetAll()
            .CountAsync();

        Response response = new()
        {
            Data = Math.Ceiling(total / pageSize)
        };

        return Ok(response);
    }

    [HttpGet("All")]
    public async Task<IActionResult> GetAllNews(int? page, int? pageSize, CancellationToken cancellationToken)
    {
        IQueryable<NewsDto> query = _newsRepository.GetAll()
            .Select(news => new NewsDto
            {
                Id = news.Id,
                Content = news.Content,
                Date = news.Date,
                Title = news.Title,
                CreatedBy = $"{news.User.FirstName} {news.User.LastName}"
            })
            .OrderByDescending(news => news.Date);

        if (page is not null && pageSize is not null) 
        {
            query = query.Skip(((int)page - 1) * (int)pageSize)
                .Take((int)pageSize);
        }

        IEnumerable<NewsDto> newsList = await query.ToListAsync(cancellationToken);

        Response response = new()
        {
            Data = newsList
        };

        return Ok(response);
    }
}