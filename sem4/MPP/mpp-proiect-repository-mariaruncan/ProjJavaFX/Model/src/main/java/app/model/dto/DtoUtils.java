package app.model.dto;

import app.model.Employee;

public class DtoUtils {
    public static Employee getFromDTO(EmployeeDto empdto){
        String username = empdto.getUsername();
        String password = empdto.getPassword();
        return new Employee(username, password);
    }
    public static EmployeeDto getDTO(Employee emp){
        String username = emp.getUsername();
        String pass = emp.getHashedPassword();
        return new EmployeeDto(username, pass);
    }
}
