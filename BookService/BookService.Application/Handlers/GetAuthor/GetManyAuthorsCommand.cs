﻿using BookService.Domain.Common;
using CSharpFunctionalExtensions;
using MediatR;

namespace BookService.Application.Handlers.GetAuthor;
public class GetManyAuthorsCommand : IRequest<Result<List<GetAuthorResult>,Error>>
{
    public required PaginationOptions PaginationOptions { get; init; }
}
