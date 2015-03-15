using System.Collections.Generic;

namespace Herobrine.Abstract
{
    public interface IHauntingRepository
    {
        List<IHaunting> GetHauntingsForPlayer(int userId);
        void SavePlayerHauntings(int userId, List<IHaunting> hauntings);
    }
}