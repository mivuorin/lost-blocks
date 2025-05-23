﻿namespace LostBlocks.Api.Color;

public record UpdateColorDto
{
    public required string Name { get; init; }
    public required string Rgb { get; init; }
    public required bool IsTransparent { get; init; }
}
