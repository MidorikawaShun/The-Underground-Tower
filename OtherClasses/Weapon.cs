using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Windows.MetaMenus;
using static WpfApp1.Utilities;

namespace TheUndergroundTower.OtherClasses
{
    class Weapon : Item
    {
        private double _speed;
        public double Speed
        {
            get { return _speed; }
            set
            {
                if (value > 0)
                    _speed = value;
            }
        }

        private double _hitChance;
        public double HitChance
        {
            get { return _hitChance; }
            set
            {
                if (value > 0)
                    _hitChance = value;
            }
        }

        private string _damageRange;
        public string DamageRange
        {
            get { return _damageRange; }
            set
            {
                try
                {
                    string[] splitValue = value.Split('-');
                    if (splitValue.Length > 2)
                        throw new Exception($"DamageRange of weapon {Name} is incorrectly formatted! Must contain only two numbers, divided by a single '-' and no spaces!");
                    int minDamage = Convert.ToInt32(splitValue[0]);
                    int maxDamage = Convert.ToInt32(splitValue[1]);
                    if (minDamage > maxDamage)
                        throw new Exception("DamageRange of weapon {Name} has a minimum value higher than it's max value!");
                    _damageRange = value;
                }
                catch (Exception ex)
                {
                    ErrorLog.Log(ex, $"DamangeRange of weapon {Name} not in correct format! Correct format is \"[smaller number]-[higher number]\"");
                    ErrorPrompt prompt = new ErrorPrompt();
                    prompt.ShowDialog();
                }
            }
        }
    }
}
