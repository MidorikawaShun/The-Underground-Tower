using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using WpfApp1;

namespace TheUndergroundTower.OtherClasses
{
    /// <summary>
    /// A class for describing difficulty levels.
    /// </summary>
    public class Difficulty : GameObject
    {
        /// <summary>
        /// The multiplier for enemy HP for this difficulty.
        /// </summary>
        private double _playerDamage;
        public double PlayerDamage { get => _playerDamage; set => _playerDamage = value; }

        /// <summary>
        /// The multiplier for enemy damage for this difficulty.
        /// </summary>
        private double _enemyDamage;
        public double EnemyDamage { get => _enemyDamage; set => _enemyDamage = value; }

        /// <summary>
        /// The multiplier for leaderboard score for this difficulty.
        /// </summary>
        private double _scoreMultiplier;
        public double ScoreMultiplier { get => _scoreMultiplier; set => _scoreMultiplier = value; }

        /// <summary>
        /// Creates a difficulty object from XML.
        /// </summary>
        /// <param name="difficulty">The difficulty XmlNode.</param>
        public Difficulty(XmlNode difficulty)
        {
            Name = difficulty.Attributes["Name"].Value;
            Description = difficulty.ChildNodes[0].FirstChild.Value;
            PlayerDamage = Convert.ToDouble(difficulty.ChildNodes[1].FirstChild.Value);
            EnemyDamage = Convert.ToDouble(difficulty.ChildNodes[2].FirstChild.Value);
            ScoreMultiplier = Convert.ToDouble(difficulty.ChildNodes[3].FirstChild.Value);
            GameData.POSSIBLE_DIFFICULTIES.Add(this);
        }

    }
}
