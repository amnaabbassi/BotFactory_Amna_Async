using BotFactory.Common.Tools;
using BotFactory.Interface;
using System;
using System.Threading.Tasks;

namespace BotFactory.Models
{
    public abstract class BaseUnit : ReportingUnit , IBaseUnit
    {
        
        #region ATTR
        public double vitesse { get; set; }
        private string model;
        public string Name { get; set; }
        public Coordinates CurrentPos { get; set; }
        #endregion

        #region constructeur
        public BaseUnit()
        {
            
        }
        public BaseUnit(string nom, double vit = 1) 
        {
            CurrentPos = new Coordinates();
            Name = nom;
            vitesse = vit;
           
        }
        public BaseUnit(double vit, double buildtime)
              : base(buildtime)
        {
            CurrentPos = new Coordinates();
            vitesse = vit;
        }
        #endregion

        #region Move Asynchrone
         public async Task<double>  Move( Coordinates begin , Coordinates end)
        {
            DateTime  start;

            start = DateTime.Now;

            Vector v = Vector.FromCoordinates(begin,end);
            double distance = v.Length(v);
                      
            await Task.Delay(1000);
 
            TimeSpan delaiexecution = DateTime.Now - start;

            return distance;

        }
        #endregion

    }
}
