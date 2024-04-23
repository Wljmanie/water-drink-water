namespace viewmodels;

public class GroupViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Code { get; set; } = null!;
    public bool OwnedByMe { get; set; }
    public IEnumerable<MemberViewModel> Members { get; set; } = null!;
}