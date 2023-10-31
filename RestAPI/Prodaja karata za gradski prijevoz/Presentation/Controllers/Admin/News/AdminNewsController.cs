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

namespace Presentation.Controllers.Admin.News;

[Authorize(Policy = AuthorizationPolicies.AdminPolicyName)]
[ApiController]
[Route("[controller]")]
public sealed class AdminNewsController : ControllerBase
{
    private readonly INewsRepository _newsRepository;
    private readonly IObjectMapperService _mapperService;
    private readonly IUnitOfWork _unitOfWork;

    public AdminNewsController(INewsRepository newsRepository, IObjectMapperService mapperService, IUnitOfWork unitOfWork)
    {
        _newsRepository = newsRepository;
        _mapperService = mapperService;
        _unitOfWork = unitOfWork;
    }

    [HttpPost("/News")]
    public async Task<IActionResult> PublishNews(NewsDto newsDto, CancellationToken cancellationToken)
    {
        string authorizeHeader = Request.Headers["Authorization"];
        string tokenString = authorizeHeader.Split(" ")[1];

        JwtSecurityToken token = new JwtSecurityTokenHandler().ReadJwtToken(tokenString);
        Claim idClaim = token.Claims.Single(claim => claim.Type.Equals("id", StringComparison.OrdinalIgnoreCase));
        Guid userId = new(idClaim.Value);

        Domain.Entities.News.News newNews = new();
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

}
