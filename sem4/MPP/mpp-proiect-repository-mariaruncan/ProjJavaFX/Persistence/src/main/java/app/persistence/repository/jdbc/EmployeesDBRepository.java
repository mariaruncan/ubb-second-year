package app.persistence.repository.jdbc;

import app.model.Employee;
import app.persistence.EmployeesRepository;
import org.apache.logging.log4j.LogManager;
import org.apache.logging.log4j.Logger;
import org.springframework.stereotype.Component;

import java.io.IOException;
import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.util.ArrayList;
import java.util.List;
import java.util.Properties;

@Component
public class EmployeesDBRepository implements EmployeesRepository {

    private final JdbcUtils dbUtils;
    private static final Logger logger = LogManager.getLogger();

    public EmployeesDBRepository(Properties props){
        logger.info("Initializing EmployeesDBRepository with properties: {} ", props);
        dbUtils = new JdbcUtils(props);
    }

    public EmployeesDBRepository(){
        Properties props = new Properties();
        try {
            props.load(EmployeesDBRepository.class.getResourceAsStream("/bd.properties"));
            System.out.println("Server properties set. ");
            props.list(System.out);
        } catch (IOException e) {
            System.err.println("Cannot find server.properties " + e);
            dbUtils = null;
            return;
        }
        dbUtils = new JdbcUtils(props);
    }

    @Override
    public void add(Employee elem) {
        logger.traceEntry("saving task {} ", elem);
        Connection con = dbUtils.getConnection();

        try(PreparedStatement preStm = con.prepareStatement("INSERT INTO Employees (firstname, lastname, username," +
                " hashedPassword) VALUES (?, ?, ?, ?);")){
            preStm.setString(1, elem.getFirstname());
            preStm.setString(2, elem.getLastname());
            preStm.setString(3, elem.getUsername());
            preStm.setString(4, elem.getHashedPassword());
            int result = preStm.executeUpdate();
            logger.trace("Saved {} instances", result);
        }
        catch(SQLException ex){
            logger.error(ex);
            System.err.println("Error DB" + ex);
        }
        logger.traceExit();
    }

    @Override
    public void delete(Long id) {
        logger.traceEntry("deleting task {} ", id);
        Connection con = dbUtils.getConnection();

        try(PreparedStatement preStm = con.prepareStatement("DELETE FROM Employees WHERE id = ?;")){
            preStm.setInt(1, Math.toIntExact(id));
            int result = preStm.executeUpdate();
            logger.trace("Deleted {} instances", result);
        }
        catch(SQLException ex){
            logger.error(ex);
            System.err.println("Error DB" + ex);
        }
        logger.traceExit();
    }

    @Override
    public void update(Long aLong, Employee elem) {
        logger.traceEntry("updating task {} ", elem);
        Connection con = dbUtils.getConnection();

        try(PreparedStatement preStm = con.prepareStatement("UPDATE Employees SET firstname = ?, lastname = ?, " +
                "username = ?, hashedPassword = ? WHERE id = ?;")){
            preStm.setString(1, elem.getFirstname());
            preStm.setString(2, elem.getLastname());
            preStm.setString(3, elem.getUsername());
            preStm.setString(4, elem.getHashedPassword());
            preStm.setInt(5, Math.toIntExact(aLong));
            int result = preStm.executeUpdate();
            logger.trace("Updated {} instances", result);
        }
        catch(SQLException ex){
            logger.error(ex);
            System.err.println("Error DB" + ex);
        }
        logger.traceExit();
    }

    @Override
    public Employee findById(Long aLong) {
        logger.traceEntry();
        Connection con = dbUtils.getConnection();
        Employee employee = null;

        try(PreparedStatement preStm = con.prepareStatement("SELECT * FROM Employees WHERE id = ? LIMIT 1;")){
            preStm.setInt(1, Math.toIntExact(aLong));
            try(ResultSet result = preStm.executeQuery()){
                if(result.next()){
                    Long id = (long) result.getInt("id");
                    String firstname = result.getString("firstname");
                    String lastname = result.getString("lastname");
                    String username = result.getString("username");
                    String hashedPassword = result.getString("hashedPassword");
                    employee = new Employee(firstname, lastname, username, hashedPassword);
                    employee.setId(id);
                }
            }
        }
        catch(SQLException ex){
            logger.error(ex);
            System.err.println("Error DB" + ex);
        }
        logger.traceExit();
        return employee;
    }

    @Override
    public Iterable<Employee> findAll() {
        logger.traceEntry();
        Connection con = dbUtils.getConnection();
        List<Employee> employees = new ArrayList<>();

        try(PreparedStatement preStm = con.prepareStatement("SELECT * FROM Employees;")){
            try(ResultSet result = preStm.executeQuery()){
                while(result.next()){
                    Long id = (long) result.getInt("id");
                    String firstname = result.getString("firstname");
                    String lastname = result.getString("lastname");
                    String username = result.getString("username");
                    String hashedPassword = result.getString("hashedPassword");
                    Employee employee = new Employee(firstname, lastname, username, hashedPassword);
                    employee.setId(id);
                    employees.add(employee);
                }
            }
        }
        catch(SQLException ex){
            logger.error(ex);
            System.err.println("Error DB" + ex);
        }
        logger.traceExit();
        return employees;
    }

    @Override
    public Employee findByUsername(String username) {
        logger.traceEntry();
        Connection con = dbUtils.getConnection();
        Employee employee = null;

        try(PreparedStatement preStm = con.prepareStatement("SELECT * FROM Employees WHERE username = ? LIMIT 1;")){
            preStm.setString(1, username);
            try(ResultSet result = preStm.executeQuery()){
                if(result.next()){
                    Long id = (long) result.getInt("id");
                    String firstname = result.getString("firstname");
                    String lastname = result.getString("lastname");
                    String hashedPassword = result.getString("hashedPassword");
                    employee = new Employee(firstname, lastname, username, hashedPassword);
                    employee.setId(id);
                }
            }
        }
        catch(SQLException ex){
            logger.error(ex);
            System.err.println("Error DB" + ex);
        }
        logger.traceExit();
        return employee;
    }
}
