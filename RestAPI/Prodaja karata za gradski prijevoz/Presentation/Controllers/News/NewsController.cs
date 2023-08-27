using Application.Services.Abstractions.Interfaces.Repositories.News;
using Microsoft.AspNetCore.Mvc;
using Presentation.DTO;
using Microsoft.EntityFrameworkCore;
using Application.Services.Abstractions.Interfaces.Mapper;
using Presentation.DTO.News;


namespace Presentation.Controllers.News
{
    [ApiController]
    [Route("[controller]")]
    public sealed class NewsController : ControllerBase
    {
        private readonly INewsRepository newsRepository;

        public NewsController(INewsRepository newsRepository)
        {
            this.newsRepository = newsRepository;
        }

        [HttpGet("GetNewsById")]
        public async Task<IActionResult> GetNewsById(Guid id, CancellationToken cancellationToken, [FromServices] IObjectMapperService objectMapperService)
        {
            var data = await newsRepository.GetByIdAsync(id, cancellationToken);

            if (data == null)
            {
                Response errorResponse = new()
                {
                    Message = "news_controller_get_news_by_id_no_news_found_error"
                };

                return NotFound(errorResponse);
            }

            var news = new NewsDto();
            objectMapperService.Map(data, news);

            Response response = new()
            {
                Data = news
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
                   Date = (DateTime)news.Date
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
    }
}
