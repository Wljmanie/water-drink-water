namespace viewmodels;

public class GroupViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Code { get; set; } = null!;
    public bool IsOwner { get; set; }
}