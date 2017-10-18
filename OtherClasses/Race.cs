namespace WpfApp1
{
    /// <summary>
    /// Describes the Races a player character can belong to. They change player stats 
    /// for better or worse.
    /// </summary>
    public class Race
    {

        public string RaceName { get; set; }
        public string RaceDescription { get; set; }

        //Shortcut to access each stat individually.
        private int[] _stats = new int[6];
        public int this[int number]
        {
            get { return _stats[number]; }
            set { _stats[number] = value; }
        }

        public int Strength
        {
            get { return _stats[0]; }
            set { _stats[0] = value; }
        }

        public int Dexterity
        {
            get { return _stats[1]; }
            set { _stats[1] = value; }
        }

        public int Constitution
        {
            get { return _stats[2]; }
            set { _stats[2] = value; }
        }

        public int Intelligence
        {
            get { return _stats[3]; }
            set { _stats[3] = value; }
        }

        public int Wisdom
        {
            get { return _stats[4]; }
            set { _stats[4] = value; }
        }

        public int Charisma
        {
            get { return _stats[5]; }
            set { _stats[5]= value; }
        }

        private int _movement;
        public int Movement
        {
            get { return _movement; }
            set { _movement = value; }
        }

        private int _speed;
        public int Speed
        {
            get { return _speed; }
            set { _speed = value; }
        }

        private int _survival;
        public int Survival
        {
            get { return _survival; }
            set { _survival = value; }
        }
    }
}