using BattleShips.Core.GameEntities;
using BattleShips.Core.GameEntities.Abstract;
using BattleShips.Core.GameEntities.Structs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BattleShips.Web.Models
{
    public class GameModel
    {
        [Required]
        public Guid? GameGuid { get; set; }
        
        public IGame Game { get; set; }

        public UserShipsLocationViewModel PlayerShipsPositions { get; set; }

        public UserCommunicationViewModel UserCommunicationVM { get; } = new UserCommunicationViewModel();
        
    }
}
