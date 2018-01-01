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

        public const int SIGHTRANGE = 10;

        private bool _awareOfPlayer;
        private string _damageRange;
        private bool _rangedAttacker,_followingPlayer;
        private int _turnsWithoutPlayerInSight;
        private int _lastKnownPlayerLocationX, _lastKnownPlayerLocationY;
        private int _defense;

        public bool AwareOfPlayer { get => _awareOfPlayer; set => _awareOfPlayer = value; }
        public string DamageRange { get => _damageRange; set => _damageRange = value; }
        public bool RangedAttacker { get => _rangedAttacker; set => _rangedAttacker = value; }
        public int TurnsWithoutPlayerInSight { get => _turnsWithoutPlayerInSight; set => _turnsWithoutPlayerInSight = value; }
        public int LastKnownPlayerLocationX { get => _lastKnownPlayerLocationX; set => _lastKnownPlayerLocationX = value; }
        public int LastKnownPlayerLocationY { get => _lastKnownPlayerLocationY; set => _lastKnownPlayerLocationY = value; }
        public int Defense { get => _defense; set => _defense = value; }
        public bool FollowingPlayer { get => _followingPlayer; set => _followingPlayer = value; }

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
            _defense = Convert.ToInt32(monster.ChildNodes[6].FirstChild.Value);

            GameData.POSSIBLE_MONSTERS.Add(this);
        }

        public Monster (Monster monster)
        {
            _awareOfPlayer = false;
            _turnsWithoutPlayerInSight = 0;
            Name = monster.Name;
            Description = monster.Description;
            HP = monster.HP;
            RangedAttacker = monster.RangedAttacker;
            MeleeSkill = monster.MeleeSkill;
            DamageRange = monster.DamageRange;
            Image = CreateTile.GetImageFromTileset(monster.Index);
            Defense = monster.Defense;
            _followingPlayer = false;
        }

        public override string ToString()
        {
            return $"Monster: {Name} at {X},{Y}";
        }

        public override void Attack(Creature player) 
        {
            int targetDefense = (player as Player).DefenseSkill + GameLogic.Roll20(1);
            double monsterAttack = MeleeSkill + GameLogic.Roll20(1);
            if (monsterAttack > targetDefense)
                player.TakeDamage(GameLogic.DiceRoll(_damageRange));
        }
    }
}
