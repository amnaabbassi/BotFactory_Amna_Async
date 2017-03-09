using BotFactory.Common.Tools;
using BotFactory.Interface;
using BotFactory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BotFactory.Factories
{
    public class UnitFactory : IUnitFactory
    {
        #region ATTR
        public int QueueCapacity { get; set; }
        public int StorageCapacity { get; set; }
        public int QueueFreeSlots { get; set; }
        public int StorageFreeSlots { get; set; }
        public Coordinates WorkingPos { get; set; }
        public Coordinates ParkingPos { get; set; }

        private List<IFactoryQueueElement> _queue;
        private List<ITestingUnit> _storage;

        private TimeSpan _queueTime = TimeSpan.FromSeconds(0);
        public event EventHandler FactoryStatus;
        public List<IFactoryQueueElement> Queue
        {
            get
            {
                return _queue.ToList();
            }
            set
            {
                _queue = value;
            }


        }
        public List<ITestingUnit> Storage
        {
            get
            {
                return _storage.ToList();
            }

            set
            {
                _storage = value;
            }
        }
        public TimeSpan QueueTime
        {
            get
            {
                return _queueTime;
            }

            set
            {
                _queueTime = value;
            }
        }
        #endregion

        #region Method 
        public async Task<bool> AddWorkableUnitToQueue(Type model, string name, Coordinates parkingpos, Coordinates workingpos)
        {
            FactoryQueueElement fqe = null;
            QueueFreeSlots = QueueCapacity - _queue.Count;
            StorageFreeSlots = StorageCapacity - _storage.Count;

            if ((QueueFreeSlots > 0) && (StorageFreeSlots > 0) && (StorageFreeSlots - _queue.Count > 0))
            {
                fqe = new FactoryQueueElement() { Name = name, Model = model, ParkingPos = parkingpos, WorkingPos = workingpos };
                _queue.Add(fqe);
            }
            bool result = await Task.Run(() => { return CreaInstance(fqe); });
                       
            if (_queue.Count() > 0 && result)
            {
                _queue.Remove(fqe);
            }

            return result;
        }
        public bool CreaInstance(FactoryQueueElement felement)
        {
            lock (this)
            {
                ITestingUnit elementinstan = Construire(felement);
                _storage.Add(elementinstan);
                return true;
            }
        }

        public ITestingUnit Construire(FactoryQueueElement factoryelement)
        {
            // Create an instance of the ITestingUnit type using 
            // Activator.CreateInstance.
            ITestingUnit testUnit = (ITestingUnit)Activator.CreateInstance(factoryelement.Model);
            Thread.Sleep(TimeSpan.FromSeconds(testUnit.BuildTime));

            testUnit.ParkingPos = factoryelement.ParkingPos;
            testUnit.WorkingPos = factoryelement.WorkingPos;
            testUnit.CurrentPos = factoryelement.ParkingPos;
            testUnit.Name = factoryelement.Name;
            testUnit.Model = factoryelement.Model.Name;

            OnStatusChanged(testUnit, new StatusChangedEventArgs() { NewStatus = "I'm building my bot.." });
            QueueTime += TimeSpan.FromSeconds(testUnit.BuildTime);

            return testUnit;
        }

        public UnitFactory(int q, int s)
        {
            QueueCapacity = q;
            StorageCapacity = s;
            Queue = new List<IFactoryQueueElement>();
            Storage = new List<ITestingUnit>();
        }

        private void OnStatusChanged(object sender, StatusChangedEventArgs statusChangedEventArgs)
        {
            FactoryStatus?.Invoke(sender, statusChangedEventArgs);
        }
        #endregion

    }
}
