using Application.Config;
using Application.Services.Abstractions.Interfaces.Repositories.News;
using Microsoft.AspNetCore.Mvc;
using Presentation.DTO;
using Microsoft.EntityFrameworkCore;
using Application.Services.Abstractions.Interfaces.Mapper;
using Presentation.DTO.News;
using NewsEntity = Domain.Entities.News.News;
using Application.Services.Abstractions.Interfaces.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace Presentation.Controllers.News;

[ApiController]
[Route("[controller]")]
public sealed class NewsController : ControllerBase
{
    private readonly INewsRepository _newsRepository;
    private readonly IUnitOfWork _unitOfWork;

    public NewsController(INewsRepository newsRepository, IUnitOfWork unitOfWork)
    {
        _newsRepository = newsRepository;
        _unitOfWork = unitOfWork;
    }

    [HttpGet("GetNewsById")]
    public async Task<IActionResult> GetNewsById([FromServices] IObjectMapperService objectMapperService, Guid id,
        CancellationToken cancellationToken)
    {
        var news = await _newsRepository.GetByIdAsync(id, cancellationToken: cancellationToken);

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

    [HttpGet("Pagination")]
    public async Task<IActionResult> GetPage([FromServices] INewsRepository newsRepository, int page, int pageSize)
    {
        var newsPerPage = await newsRepository
            .GetAll()
            .OrderByDescending(news => news.Date)
            .Select(news => new NewsDto
            {
                Id = news.Id,
                Title = news.Title,
                Content = news.Content,
                Date = news.Date
            })
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToArrayAsync();

        Response response = new()
        {
            Data = newsPerPage
        };


        return Ok(response);
    }

    [HttpGet("NewsPagesCount")]
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
    public async Task<IActionResult> GetAllNews(CancellationToken cancellationToken)
    {
        IEnumerable<NewsDto> newsList = await _newsRepository.GetAll()
            .Select(news => new NewsDto
            {
                Id = news.Id,
                Content = news.Content,
                Date = news.Date,
                Title = news.Title,
                CreatedBy = $"{news.User.FirstName} {news.User.LastName}"
            })
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        Response response = new()
        {
            Data = newsList
        };

        return Ok(response);
    }

    [Authorize(Policy = AuthorizationPolicies.AdminPolicyName)]
    [HttpPut("/Admin/[controller]/Edit/{newsId}")]
    public async Task<IActionResult> EditNews(Guid newsId, NewsDto newsDto, CancellationToken cancellationToken)
    {
        Response response = new();
        NewsEntity? news = await _newsRepository.GetByIdAsync(newsId, cancellationToken: cancellationToken);

        if (news is null)
        {
            response.Message = "Nepostojeća obavijest";
            return NotFound(response);
        }

        news.Content = newsDto.Content;
        news.Title = newsDto.Title;

        await _newsRepository.UpdateAsync(news, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        response.Message = "Uspješno editovana obavijest";

        return Ok(response);
    }

    [Authorize(Policy = AuthorizationPolicies.AdminPolicyName)]
    [HttpDelete("/Admin/[controller]/Delete/{newsId}")]
    public async Task<IActionResult> DeleteNews(Guid newsId, CancellationToken cancellationToken)
    {
        Response response = new();
        NewsEntity? news = await _newsRepository.GetByIdAsync(newsId, cancellationToken: cancellationToken);

        if (news is null)
        {
            response.Message = "Nepostojeća obavijest";
            return NotFound(response);
        }

        await _newsRepository.DeleteAsync(news, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        response.Message = "Uspješno obrisana obavijest.";

        return Ok(response);
    }
}