using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BattleShips.Web.Models
{
    public class UserCommunicationViewModel
    {
        public IList<string> MessageToUser { get; set; } = new List<string>();
        public IList<string> CurrentGameStatus { get; set; } = new List<string>();
    }
}
