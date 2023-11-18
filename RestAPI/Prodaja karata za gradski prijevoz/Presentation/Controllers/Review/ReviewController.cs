using Application.Config;
using Application.Services.Abstractions.Interfaces.Mapper;
using Application.Services.Abstractions.Interfaces.Repositories;
using Application.Services.Abstractions.Interfaces.Repositories.Reviews;
using Application.Services.Abstractions.Interfaces.Repositories.Tickets;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Presentation.DTO;
using Presentation.DTO.Review;

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
        _reviewRepository = reviewRepository;
        _unitOfWork = unitOfWork;
        _issuedTicket = issuedTicket;
    }

    [Authorize(Policy = AuthorizationPolicies.UserPolicyName)]
    [HttpPost("Add")]
    public async Task<IActionResult> AddReview([FromServices] IObjectMapperService objectMapperService,
         ReviewDto reviewDto, CancellationToken cancellationToken)
    {
        if (IsReviewRequestInvalid(reviewDto))
        {
            Response invalidResponse = new()
            {
                Message = "review_controller_add_review_invalid_input"
            };

            return BadRequest(invalidResponse);
        }

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
            newReview.DateOfCreation = DateTime.Now;

            await _reviewRepository.CreateAsync(newReview, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            Response response = new()
            {
                Message = "review_controller_add_review_success"
            };
            
            return Ok(response);
        }
    }

    // it's better to create a separate validator but this is quicker
    private static bool IsReviewRequestInvalid(ReviewDto reviewDto)
    {
        return string.IsNullOrEmpty(reviewDto.Title) || string.IsNullOrEmpty(reviewDto.Description) 
            || reviewDto.Score <= 0 || reviewDto.Score > 5;
    }

    [HttpGet("Get")]
    public async Task<IActionResult> GetPage(int page, int pageSize, CancellationToken cancellationToken)
    {
        var productsPerPage = await _reviewRepository
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
            .ToArrayAsync(cancellationToken);

        Response response = new()
        {
            Data = productsPerPage
        };

        return Ok(response);
    }

    [HttpGet("PagesCount")]
    public async Task<IActionResult> GetReviewCount(double pageSize, CancellationToken cancellationToken)
    {
        var total = await _reviewRepository
           .GetAll()
           .CountAsync(cancellationToken);

        Response response = new()
        {
            Data = Math.Ceiling(total / pageSize)
        };

        return Ok(response);
    }
}


