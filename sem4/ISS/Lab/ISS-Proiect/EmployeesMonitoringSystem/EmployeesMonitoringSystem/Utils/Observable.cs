using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeesMonitoringSystem.Utils
{
    public class Observable
    {
        public List<Observer> observersWorkers = new List<Observer>();
        public List<Observer> observersBosses = new List<Observer>();
    
        public void UpdateWorkers(Message data)
        {
            foreach (var worker in observersWorkers)
                worker.update(data);
        }

        public void UpdateBosses(Message data)
        {
            foreach (var boss in observersBosses)
                boss.update(data);
        }

        public void AddObserverWorker(Observer observer)
        {
            observersWorkers.Add(observer);
        }

        public void AddObserverBoss(Observer observer)
        {
            observersBosses.Add(observer);
        }

        public void RemoveObserverWorker(Observer observer)
        {
            observersWorkers.Remove(observer);
        }

        public void RemoveObserverBosses(Observer observer)
        {
            observersBosses.Remove(observer);
        }
    }
}
