using System;
using System.Collections.Generic;
using Model;

namespace Persistence
{
    public class RepositoryException : ApplicationException
    {
        public RepositoryException() { }
        public RepositoryException(String mess) : base(mess) { }
        public RepositoryException(String mess, Exception e) : base(mess, e) { }
    }

    public interface Repository<ID, E> where E : Entity<ID>
    {
        void add(E elem);
        void delete(ID id);
        void update(E elem, ID id);
        E findById(ID id);
        IEnumerable<E> findAll();
    }

}
