using BattleShips.Core.GameEntities;
using BattleShips.Core.GameEntities.Abstract;
using System;
using System.ComponentModel.DataAnnotations;

namespace BattleShips.Web.Models
{
    public class GameModel
    {
        [Required]
        public Guid? GameGuid { get; set; }
        
        public IGame Game { get; set; }

        public bool[] PlayerShipsPositions
        {
            get;
            set;
        }

        public UserCommunicationViewModel UserCommunicationVM { get; } = new UserCommunicationViewModel();
        
    }
}
