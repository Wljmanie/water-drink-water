namespace TbdFriends.WaterDrinkWater.Data.Models;

public class Membership
{
    public int Id { get; set; }
    public int GroupId { get; set; }
    public int AccountId { get; set; }

    public virtual Group Group { get; set; } = null!;
    public virtual Account Account { get; set; } = null!;
}