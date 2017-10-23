using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheUndergroundTower.OtherClasses;

namespace WpfApp1.Creatures
{
    public class Player : Creature
    {
        #region Properties
        private Race _playerRace;
        public Race PlayerRace { get => _playerRace; set => _playerRace = value; }

        private Class _playerClass;
        public Class PlayerClass { get => _playerClass; set => _playerClass = value; }



        #endregion



    }
}
