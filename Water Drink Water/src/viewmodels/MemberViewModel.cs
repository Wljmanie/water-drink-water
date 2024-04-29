namespace viewmodels;

public class MemberViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int Progress { get; set; } = 0;
    public bool IsOwner { get; set; }
}