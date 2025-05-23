﻿using FluentValidation;

namespace LostBlocks.Api.Part;

public class UpdatePartDtoValidator : AbstractValidator<UpdatePartDto>
{
    public UpdatePartDtoValidator()
    {
        RuleFor(p => p.Name).NotEmpty();
    }
}
