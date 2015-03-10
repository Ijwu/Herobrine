namespace Herobrine.Abstract
{
    public interface IWorldEdit
    {
        int X { get; set; }
        int Y { get; set; }
        void Edit();
        void Revert();
    }
}