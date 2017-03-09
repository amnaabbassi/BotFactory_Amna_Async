namespace BotFactory.Common.Tools

{
    public class Coordinates
    {
        #region ATTR
        public double X { get; set; }
        public double Y { get; set; }
        #endregion
        #region constructeur
        public Coordinates() { }
        public Coordinates(double x1, double y1)
        {
            X = x1;
            Y = y1;
        }
        #endregion

        #region Method

        bool Equals(double x, double y)
        {
            if (x == X && y == Y)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion
    }
}
