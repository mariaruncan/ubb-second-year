package app.model;

import java.util.Objects;

public class Employee extends Entity<Long> {
    private String firstname;
    private String lastname;
    private String username;
    private String hashedPassword;

    public Employee(){
        this.firstname = "";
        this.lastname = "";
        this.username = "";
        this.hashedPassword = "";
    }

    public Employee(String username, String pass){
        this.username = username;
        this.hashedPassword = pass;
    }

    public Employee(String firstname, String lastname, String username, String hashedPassword) {
        this.firstname = firstname;
        this.lastname = lastname;
        this.username = username;
        this.hashedPassword = hashedPassword;
    }

    public String getFirstname() {
        return firstname;
    }

    public void setFirstname(String firstname) {
        this.firstname = firstname;
    }

    public String getLastname() {
        return lastname;
    }

    public void setLastname(String lastname) {
        this.lastname = lastname;
    }

    public String getUsername() {
        return username;
    }

    public void setUsername(String username) {
        this.username = username;
    }

    public String getHashedPassword() {
        return hashedPassword;
    }

    public void setHashedPassword(String hashedPassword) {
        this.hashedPassword = hashedPassword;
    }

    @Override
    public String toString() {
        return "Employee: " + super.getId() + " " + firstname + " " + lastname + " " + username;
    }

    @Override
    public boolean equals(Object o) {
        if (this == o) return true;
        if (o == null || getClass() != o.getClass()) return false;
        Employee employee = (Employee) o;
        return Objects.equals(firstname, employee.firstname) && Objects.equals(lastname, employee.lastname) &&
                Objects.equals(username, employee.username) && Objects.equals(hashedPassword, employee.hashedPassword);
    }

    @Override
    public int hashCode() {
        return Objects.hash(firstname, lastname, username, hashedPassword);
    }
}

