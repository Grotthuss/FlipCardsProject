namespace FlipCardProject.Records;

using Enums;

public record FlipcardState
{
    public FlipCardStateEnum _state { get; set; } = FlipCardStateEnum.UNANSWERED;
}

