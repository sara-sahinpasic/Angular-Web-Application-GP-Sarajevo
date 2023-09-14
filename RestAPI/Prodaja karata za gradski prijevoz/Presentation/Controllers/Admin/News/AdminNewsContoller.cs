using Application.Services.Abstractions.Interfaces.Mapper;
using Application.Services.Abstractions.Interfaces.Repositories;
using Application.Services.Abstractions.Interfaces.Repositories.News;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.DTO;
using Presentation.DTO.Admin.News;

namespace Presentation.Controllers.Admin.News
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public sealed class AdminNewsContoller : ControllerBase
    {
        private readonly INewsRepository newsRepository;
        private readonly IObjectMapperService mapperService;
        private readonly IUnitOfWork unitOfWork;

        public AdminNewsContoller(INewsRepository newsRepository, IObjectMapperService mapperService, IUnitOfWork unitOfWork)
        {
            this.newsRepository = newsRepository;
            this.mapperService = mapperService;
            this.unitOfWork = unitOfWork;
        }

        [HttpPost("/News")]
        public async Task<IActionResult> PublishNews(NewsDto newsDto, CancellationToken cancellationToken)
        {
            Domain.Entities.News.News newNews = new();
            mapperService.Map(newsDto, newNews);

            newsRepository.Create(newNews);
            await unitOfWork.CommitAsync(cancellationToken);

            Response response = new()
            {
                Message = "Uspješno objavljena nova obavijest.",
                Data = newNews
            };
            return Ok(response);
        }

    }
}
