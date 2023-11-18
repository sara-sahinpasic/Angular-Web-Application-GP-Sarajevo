using Application.Config;
using Application.Services.Abstractions.Interfaces.Mapper;
using Application.Services.Abstractions.Interfaces.Repositories;
using Application.Services.Abstractions.Interfaces.Repositories.News;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.DTO;
using Presentation.DTO.News;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using NewsEntity = Domain.Entities.News.News;

namespace Presentation.Controllers.Admin.News;

[Authorize(Policy = AuthorizationPolicies.AdminPolicyName)]
[ApiController]
[Route("[controller]")]
public sealed class NewsController : ControllerBase
{
    private readonly INewsRepository _newsRepository;
    private readonly IObjectMapperService _mapperService;
    private readonly IUnitOfWork _unitOfWork;

    public NewsController(INewsRepository newsRepository, IObjectMapperService mapperService, IUnitOfWork unitOfWork)
    {
        _newsRepository = newsRepository;
        _mapperService = mapperService;
        _unitOfWork = unitOfWork;
    }

    [HttpPost("/Admin/[controller]/Publish")]
    public async Task<IActionResult> PublishNews(NewsDto newsDto, CancellationToken cancellationToken)
    {
        string authorizeHeader = Request.Headers["Authorization"];
        string tokenString = authorizeHeader.Split(" ")[1];

        JwtSecurityToken token = new JwtSecurityTokenHandler().ReadJwtToken(tokenString);
        Claim idClaim = token.Claims.Single(claim => claim.Type.Equals("id", StringComparison.OrdinalIgnoreCase));
        Guid userId = new(idClaim.Value);

        NewsEntity newNews = new();
        _mapperService.Map(newsDto, newNews);

        newNews.UserId = userId;
        newNews.Date = DateTime.Now;

        await _newsRepository.CreateAsync(newNews, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        Response response = new()
        {
            Message = "Uspješno objavljena nova obavijest.",
            Data = newNews
        };

        return Ok(response);
    }

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
