using Application.Config;
using Application.Services.Abstractions.Interfaces.Mapper;
using Application.Services.Abstractions.Interfaces.Repositories.Requests;
using Application.Services.Abstractions.Interfaces.Requests;
using Domain.Entities.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Presentation.DTO;
using Presentation.DTO.User.Request;
using FileManager = System.IO.File;

namespace Presentation.Controllers.Admin.User;

[Authorize(Policy = AuthorizationPolicies.AdminPolicyName)]
[ApiController]
[Route("User/[controller]")]
public sealed class RequestController : ControllerBase
{
    private readonly IRequestRepository _requestRepository;
    private readonly IRequestService _requestService;

    public RequestController(IRequestRepository requestRepository, IRequestService requestService)
    {
        _requestRepository = requestRepository;
        _requestService = requestService;
    }

    [HttpGet("Get/{userId}")] 
    public async Task<IActionResult> GetRequestForUser([FromServices] IObjectMapperService mapperService, Guid userId, CancellationToken cancellationToken)
    {
        Request? request = await _requestRepository.GetAll()
            .Where(request => request.UserId == userId && request.Active)
            .Include(request => request.UserStatus)
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);

        if (request is null)
        {
            Response errorResponse = new()
            {
                Message = "Za ovog korisnika nema pronađenih zahtjeva."
            };

            return NotFound(errorResponse);
        }

        UserRequestResponseDto requestResponseData = await CreateUserRequestResponse(mapperService, request, cancellationToken);

        Response response = new()
        {
            Data = requestResponseData
        };

        return Ok(response);
    }

    private static async Task<UserRequestResponseDto> CreateUserRequestResponse(IObjectMapperService mapperService, Request request, CancellationToken cancellationToken)
    {
        UserRequestResponseDto requestResponseData = new();

        mapperService.Map(request, requestResponseData);

        requestResponseData.UserStatusName = request.UserStatus.Name;
        requestResponseData.RequestId = request.Id;

        byte[] documentFile = await FileManager.ReadAllBytesAsync(request.DocumentLink, cancellationToken);

        requestResponseData.DocumentFile = Convert.ToBase64String(documentFile);
        requestResponseData.DocumentType = request.DocumentLink.Split(".")[^1];

        return requestResponseData;
    }

    [HttpPut("Approve/{requestId}")]
    public async Task<IActionResult> ApproveRequest(Guid requestId, UserApprovalRequestDto requestRequest, CancellationToken cancellationToken)
    {
        await _requestService.ApproveRequest(requestId, requestRequest.ExpirationDate, cancellationToken);

        Response response = new()
        {
            Message = "Uspješno odobren zahtjev."
        };

        return Ok(response);
    }

    [HttpPut("Reject/{requestId}")]
    public async Task<IActionResult> RejectRequest(Guid requestId, UserRejectRequestDto requestRequest, CancellationToken cancellationToken)
    {
        bool isRejectedSuccessfully = await _requestService.RejectRequest(requestId, requestRequest.RejectionReason, cancellationToken);
        
        if (!isRejectedSuccessfully) 
        {
            Response errorResponse = new()
            {
                Message = "Zahtjev nije aktivan."
            };

            return BadRequest(errorResponse);
        }

        Response response = new()
        {
            Message = "Uspješno odbijen zahtjev."
        };

        return Ok(response);
    }
}
