using System;

namespace Interfaces
{
    public interface IGameServerController
    {
        void Post(int currentSessions, int sessionLimit);
    }
}
