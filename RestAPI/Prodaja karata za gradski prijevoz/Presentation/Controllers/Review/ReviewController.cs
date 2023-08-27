using Application.Services.Abstractions.Interfaces.Mapper;
using Application.Services.Abstractions.Interfaces.Repositories;
using Application.Services.Abstractions.Interfaces.Repositories.Reviews;
using Microsoft.AspNetCore.Mvc;
using Presentation.DTO;
using Presentation.DTO.Review;
using Application.Services.Abstractions.Interfaces.Repositories.Tickets;
using Microsoft.EntityFrameworkCore;

namespace Presentation.Controllers.Review;

[ApiController]
[Route("[controller]")]
public sealed class ReviewController : ControllerBase
{
    private readonly IReviewRepository _reviewRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IIssuedTicketRepository _issuedTicket;

    public ReviewController(IReviewRepository reviewRepository, IUnitOfWork unitOfWork,
        IIssuedTicketRepository issuedTicket)
    {
        this._reviewRepository = reviewRepository;
        this._unitOfWork = unitOfWork;
        this._issuedTicket = issuedTicket;
    }
    [HttpPost("AddReview")]
    public async Task<IActionResult> AddReview([FromServices] IObjectMapperService objectMapperService,
         ReviewDto reviewDto, CancellationToken cancellationToken)
    {
        bool hasUserPurchasedAnyTicket = await _issuedTicket.HasUserPurchasedAnyTicketAsync(reviewDto.UserId, cancellationToken);

        if (!hasUserPurchasedAnyTicket)
        {
            Response errorResponse = new()
            {
                Message = "review_controller_add_review_no_purchased_tickets_error"
            };
            return BadRequest(errorResponse);
        }
        else
        {
            Domain.Entities.Reviews.Review newReview = new();

            objectMapperService.Map(reviewDto, newReview);

            _reviewRepository.Create(newReview);
            await _unitOfWork.CommitAsync(cancellationToken);

            Response response = new()
            {
                Message = "review_controller_add_review_success"
            };
            return Ok(response);
        }
    }
    [HttpGet("Pagination")]
    public async Task<IActionResult> GetPage([FromServices] IReviewRepository reviewRepository, int page, int pageSize)
    {
        var productsPerPage = await reviewRepository
           .GetAll()
           .OrderByDescending(review => review.DateOfCreation)
           .Select(review => new ReviewDto
           {
               Title = review.Title,
               Description = review.Description,
               Score = review.Score
           })
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToArrayAsync();

        int totalCount = productsPerPage.Count();

        return Ok(productsPerPage);
    }

    [HttpGet("ReviewPagesCount")]
    public async Task<IActionResult> GetReviewCount([FromServices] IReviewRepository reviewRepository, double pageSize)
    {
        var total = await reviewRepository
           .GetAll()
           .CountAsync();

        Response response = new()
        {
            Data = Math.Ceiling(total / pageSize)
        };

        return Ok(response);
    }
}


