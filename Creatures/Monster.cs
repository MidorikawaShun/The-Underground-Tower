using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using WpfApp1;
using WpfApp1.Creatures;

namespace TheUndergroundTower.Creatures
{
    public class Monster : Creature
    {

        private bool _awareOfPlayer;
        private string _damageRange;
        private bool _rangedAttacker;
        
        public bool AwareOfPlayer { get => _awareOfPlayer; set => _awareOfPlayer = value; }
        public string DamageRange { get => _damageRange; set => _damageRange = value; }
        public bool RangedAttacker { get => _rangedAttacker; set => _rangedAttacker = value; }

        public Monster(XmlNode monster)
        {
            _awareOfPlayer = false;
            Name = monster.Attributes["Name"].Value;
            Description = monster.ChildNodes[0].FirstChild.Value;
            HP = Convert.ToInt32(monster.ChildNodes[1].FirstChild.Value);
            RangedAttacker = Convert.ToBoolean(monster.ChildNodes[2].FirstChild.Value);
            MeleeSkill = Convert.ToDouble(monster.ChildNodes[3].FirstChild.Value);
            DamageRange = monster.ChildNodes[4].FirstChild.Value;
            Index = Convert.ToInt32(monster.ChildNodes[5].FirstChild.Value);
            GameData.POSSIBLE_MONSTERS.Add(this);
        }

        public Monster (Monster monster)
        {
            _awareOfPlayer = false;
            Name = monster.Name;
            Description = monster.Description;
            HP = monster.HP;
            RangedAttacker = monster.RangedAttacker;
            MeleeSkill = monster.MeleeSkill;
            DamageRange = monster.DamageRange;
            Image = CreateTile.GetImageFromTileset(monster.Index);
            GameStatus.CREATURES.Add(this);
        }
    }
}
