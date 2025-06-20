namespace AnimeListWpf.Models.Content;

internal class UnclassifiedContent : AContent
{
    public UnclassifiedContent() { }

    [Obsolete("Is not implemented, will throw exception.")]
    public override List<DescriptionSegment> Description()
    {
        throw new NotImplementedException();
    }
}
