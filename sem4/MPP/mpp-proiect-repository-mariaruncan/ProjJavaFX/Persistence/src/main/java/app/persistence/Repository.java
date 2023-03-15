package app.persistence;

import app.model.Entity;

public interface Repository<ID, E extends Entity<ID>> {
    void add(E elem);
    void delete(ID id);
    void update(ID id, E elem);
    E findById(ID id);
    Iterable<E> findAll();
}
