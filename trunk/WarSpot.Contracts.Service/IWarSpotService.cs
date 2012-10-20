using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSpot.Contracts.Service
{
    public interface IWarSpotService
    {

        bool register(string username, string pass);

        bool login(string inputUsername, string inputPass);

    }
}
