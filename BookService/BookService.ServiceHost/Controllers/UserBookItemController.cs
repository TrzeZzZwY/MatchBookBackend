using BookService.Application.Extensions;
using BookService.Application.Handlers.CreateImage;
using BookService.Application.Handlers.GetImage;
using BookService.Application.Handlers.GetUserLikes;
using BookService.Application.Handlers.ToggleLike;
using BookService.Application.Handlers.CreateUserBookItem;
using BookService.Application.Handlers.DeleteUserBookItem;
using BookService.Application.Handlers.EditUserBookItem;
using BookService.Application.Handlers.EditUserBookItem.AdminActions;
using BookService.Application.Handlers.EditUserBookItem.BatchActions;
using BookService.Application.Handlers.GetUserBookItems;
using BookService.Domain.Common;
using BookService.ServiceHost.Controllers.Dto;
using BookService.ServiceHost.Controllers.Dto.BookPoint;
using BookService.ServiceHost.Controllers.Dto.UserBookItem;
using BookService.ServiceHost.Controllers.Dto.UserItemBook;
using BookService.ServiceHost.Controllers.Dto.UserLikes;
using BookService.ServiceHost.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookService.ServiceHost.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserBookItemController : ControllerBase
{
    private readonly IMediator _mediator;

    public UserBookItemController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("image")]
    [Authorize(Roles = "User")]
    public async Task<ActionResult<ImageResponse>> AddItemImage(IFormFile file, CancellationToken cancellation)
    {
        var imageType = file.ContentType.ToImageType();
        if (imageType == ImageType.UNKNOWN) return BadRequest(new GenericError { Description = "Invalid ContentType" });

        using var ms = new MemoryStream();
        file.CopyTo(ms);

        var command = new CreateImageCommand
        {
            FileName = file.FileName,
            ByteArray = ms.ToArray(),
            ImageType = imageType
        };
        var result = await _mediator.Send(command, cancellation);

        if (result.IsSuccess)
        {
            return StatusCode(StatusCodes.Status201Created, new ImageResponse { ImageId = result.Value.ImageId });
        }
        return result.Error.ToErrorResult();
    }

    [HttpGet("image/{imageId:int}")]
    [Authorize]
    public async Task<ActionResult> GetImage([FromRoute] int imageId, CancellationToken cancellation)
    {
        var command = new GetImageCommand
        {
            ImageId = imageId
        };
        var result = await _mediator.Send(command, cancellation);

        if (result.IsSuccess)
        {
            return result.Value is null ?
                NotFound() :
                File(result.Value.ImageBytes, result.Value.ImageType.ToContentType());
        }

        return result.Error.ToErrorResult();
    }

    [HttpPost]
    [Authorize(Roles = "User")]
    public async Task<ActionResult<CreateEntityResponse>> AddUserItemBook([FromBody] UserBookItemRequest request, CancellationToken cancellation)
    {
        var region = User.GetRegion();
        var userId = User.GetId();
        if (region is null || userId is null) return StatusCode(StatusCodes.Status400BadRequest);

        var command = new CreateUserBookItemCommand
        {
            UserId = (int)userId,
            BookReferenceId = request.BookReferenceId,
            BookPointId = request.BookPointId,
            Description = request.Description,
            Status = request.Status,
            ImageId = request.ImageId,
            Region = (Region)region
        };

        var result = await _mediator.Send(command, cancellation);

        if (result.IsSuccess) return StatusCode(StatusCodes.Status201Created, new CreateEntityResponse { Id = result.Value.UserBookItemId });

        return result.Error.ToErrorResult();
    }

    [HttpGet]
    [Authorize("Admin")]
    public async Task<ActionResult<PaginationWrapper<UserBookItemResponse>>> GetManyUserBookItem(CancellationToken cancellation,
                [FromQuery] int pageSize = 50,
                [FromQuery] int pageNumber = 1,
                [FromQuery] bool includeBookAuthors = false,
                [FromQuery] UserBookItemStatus? itemStatus = null,
                [FromQuery] Region? region = null,
                [FromQuery] int? userId = null,
                [FromQuery] string? title = null)
    {;
        var command = new GetManyUserBookItemsCommand
        {
            PaginationOptions = new PaginationOptions
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            },
            InludeAuthorDetails = includeBookAuthors,
            ItemStatus = itemStatus,
            Region = region,
            UserId = userId,
            Title = title
        };

        var result = await _mediator.Send(command, cancellation);

        if (result.IsSuccess)
        {
            var bookPoints = result.Value.GetPaginationResult(DtoExtensions.ToDto);

            return StatusCode(StatusCodes.Status200OK, bookPoints);
        };

        return result.Error.ToErrorResult();
    }

    [HttpGet("user-books/{userId:int}")]
    [Authorize("User")]
    public async Task<ActionResult<PaginationWrapper<UserBookItemResponse>>> GetUserBook(CancellationToken cancellation,
                [FromRoute] int userId,
                [FromQuery] int pageSize = 50,
                [FromQuery] int pageNumber = 1,
                [FromQuery] bool includeBookAuthors = false,
                [FromQuery] string? title = null)
    {
        var requestUserId = User.GetId();
        var command = new GetAllUserItemsCommand
        {
            PaginationOptions = new PaginationOptions
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            },
            Title = title,
            UserId = userId,
            RequestUserId = (int)requestUserId
        };

        var result = await _mediator.Send(command, cancellation);

        if (result.IsSuccess)
        {
            var bookPoints = result.Value.GetPaginationResult(DtoExtensions.ToDto);

            return StatusCode(StatusCodes.Status200OK, bookPoints);
        };

        return result.Error.ToErrorResult();
    }

    [HttpGet("feed")]
    [Authorize("User")]
    public async Task<ActionResult<PaginationWrapper<UserBookItemResponse>>> GetFeed(CancellationToken cancellation,
            [FromQuery] int pageSize = 50,
            [FromQuery] int pageNumber = 1)
    {
        var requestUserId = User.GetId();
        var region = User.GetRegion();
        var command = new GetFeedCommand
        {
            PaginationOptions = new PaginationOptions
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            },
            Region = (Region)region,
            RequestUserId = (int)requestUserId
        };

        var result = await _mediator.Send(command, cancellation);

        if (result.IsSuccess)
        {
            var bookPoints = result.Value.GetPaginationResult(DtoExtensions.ToDto);

            return StatusCode(StatusCodes.Status200OK, bookPoints);
        };

        return result.Error.ToErrorResult();
    }

    [HttpGet("{userBookItemId:int}")]
    [Authorize]
    public async Task<ActionResult<BookPointResponse>> GetById([FromRoute] int userBookItemId, CancellationToken cancellation)
    {
        var command = new GetUserBookItemCommand
        {
            UserBookItemId = userBookItemId
        };

        var result = await _mediator.Send(command, cancellation);

        if (result.IsSuccess)
        {
            return result.Value is null ?
                NotFound() :
                Ok(result.Value.ToDto());
        };

        return result.Error.ToErrorResult();
    }

    [HttpDelete("{userBookItemId:int}")]
    [Authorize]
    public async Task<ActionResult> Delete([FromRoute] int userBookItemId, CancellationToken cancellation)
    {
        //Only for admin or 
        var userId = User.GetId();
        var isAdmin = User.IsAdmin();
        var command = new DeleteUserBookItemCommand
        {
            UserBookItemId = userBookItemId,
            IsAdmin = isAdmin,
            UserId = (int)userId
        };

        var result = await _mediator.Send(command, cancellation);

        if (result.IsSuccess)
        {
            return NoContent();
        }

        return result.Error.ToErrorResult();
    }

    [HttpPut("{userBookItemId:int}")]
    [Authorize(Roles = "User")]
    public async Task<ActionResult> Edit([FromRoute] int userBookItemId, [FromBody] UserBookItemRequest request, CancellationToken cancellation)
    {
        var region = User.GetRegion();
        var userId = User.GetId();
        if (region is null || userId is null) return StatusCode(StatusCodes.Status400BadRequest);
        var command = new EditUserBookItemCommand
        {
            UserBookItemId = userBookItemId,
            Description = request.Description,
            Status = request.Status,
            BookReferenceId = request.BookReferenceId,
            UserId = (int)userId,
            BookPointId = request.BookPointId,
            ImageId = request.ImageId,
            Region = (Region)region
        };
        var result = await _mediator.Send(command, cancellation);

        if (result.IsSuccess)
        {
            return Ok();
        };

        return result.Error.ToErrorResult();
    }

    [HttpPut("batch-change-region")]
    [Authorize(Roles = "User")]
    public async Task<ActionResult> BatchChangeRegion(CancellationToken cancellation)
    {
        var region = User.GetRegion();
        var userId = User.GetId();
        if (region is null || userId is null) return StatusCode(StatusCodes.Status400BadRequest);
        var command = new EditAllUserBookItemsRegionCommand
        {
            UserId = (int)userId,
            Region = (Region)region
        };
        var result = await _mediator.Send(command, cancellation);

        if (result.IsSuccess)
        {
            return Ok();
        };

        return result.Error.ToErrorResult();
    }

    [HttpPut("{userBookItemId:int}/change-status")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> EditStatus([FromRoute] int userBookItemId, [FromBody] ChangeStatusRequest request, CancellationToken cancellation)
    {
        // Only for admin
        var command = new EditUserBookItemStatusCommand
        {
            UserBookItemId = userBookItemId,
            Status = request.Status,
        };
        var result = await _mediator.Send(command, cancellation);

        if (result.IsSuccess)
        {
            return Ok();
        };

        return result.Error.ToErrorResult();
    }

    [HttpPost("toggle-like")]
    [Authorize(Roles = "User")]
    public async Task<ActionResult> ToggleLikeUserItem([FromBody] UserLikeBookRequest request, CancellationToken cancellation)
    {
        var userId = User.GetId();
        if (userId is null) return StatusCode(StatusCodes.Status400BadRequest);
        var command = new ToggleLikeCommand { UserBookItemId = request.UserBookItemId, UserId = (int)userId };

        var result = await _mediator.Send(command, cancellation);

        if (result.IsSuccess)
            return StatusCode(StatusCodes.Status200OK);

        return result.Error.ToErrorResult();
    }

    [HttpGet("get-like")]
    [Authorize]
    public async Task<ActionResult<UserLikesResponse>> GetUserLikes(CancellationToken cancellation,
                [FromQuery] int pageSize = 50,
                [FromQuery] int pageNumber = 1,
                [FromQuery] int userId = 1)
    {
        var command = new GetUserLikesCommand
        {
            PaginationOptions = new PaginationOptions
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            },
            UserId = userId
        };

        var result = await _mediator.Send(command, cancellation);

        if (result.IsSuccess)
        {
            var userLikes = new UserLikesResponse
            {
                UserId = result.Value.UserId,
                UserLikes = result.Value.Items.GetPaginationResult(DtoExtensions.ToDto)
            };

            return StatusCode(StatusCodes.Status200OK, userLikes);
        };

        return result.Error.ToErrorResult();
    }

    [HttpGet("test-token-any")]
    [Authorize]
    public async Task<ActionResult> TestAny()
    {
        var a = User.GetRegion();
        return Ok();
    }

    [HttpGet("test-token-user")]
    [Authorize(Roles = "User")]
    public async Task<ActionResult> TestUser()
    {
        return Ok();
    }

    [HttpGet("test-token-admin")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> TestAdmin()
    {
        return Ok();
    }

}
