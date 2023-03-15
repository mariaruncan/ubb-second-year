package app.persistence;

import app.model.Employee;

public interface EmployeesRepository extends Repository<Long, Employee>{
    public Employee findByUsername(String username);
}
