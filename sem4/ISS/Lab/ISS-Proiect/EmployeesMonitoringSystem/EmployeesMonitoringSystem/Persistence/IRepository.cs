using System;
using System.Collections.Generic;

namespace EmployeesMonitoringSystem.Persistence
{
    public interface IRepository<E> where E : class
    {
        void Add(E elem);
        void Remove(int id);
        void Update(E elem, int id);
        E FindById(int id);
        List<E> FindAll();
    }

    public class RepoException : Exception
    {
        public RepoException(string message) : base(message) { }
    }
}
