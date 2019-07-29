using BattleShips.Core.GameEntities.Abstract;
using System;
using System.ComponentModel.DataAnnotations;

namespace BattleShips.Web.Models
{
    public class GameModel
    {
        public IGame CurrentGame { get; set; }

        [Required]
        public Guid CurrentGameGuid { get; set; }

        public bool[] PlayerShipsPositions
        {
            get;
            set;
        }
        public ShootResultDTO LastShootResult { get; }

        public UserCommunicationViewModel UserCommunicationVM { get; } = new UserCommunicationViewModel();
        
    }
}
