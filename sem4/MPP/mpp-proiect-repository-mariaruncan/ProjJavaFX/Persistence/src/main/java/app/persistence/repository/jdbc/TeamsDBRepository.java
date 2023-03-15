package app.persistence.repository.jdbc;

import app.model.Team;
import app.persistence.TeamsRepository;
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
public class TeamsDBRepository implements TeamsRepository {

    private final JdbcUtils dbUtils;
    private static final Logger logger = LogManager.getLogger();

    public TeamsDBRepository(Properties props){
        logger.info("Initializing TeamsDBRepository with properties: {} ", props);
        dbUtils = new JdbcUtils(props);
    }

    public TeamsDBRepository(){
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
    public void add(Team elem) {
        logger.traceEntry("saving task {} ", elem);
        Connection con = dbUtils.getConnection();

        try(PreparedStatement preStm = con.prepareStatement("INSERT INTO Teams (name) VALUES (?);")){
            preStm.setString(1, elem.getName());
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

        try(PreparedStatement preStm = con.prepareStatement("DELETE FROM Teams WHERE id = ?;")){
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
    public void update(Long aLong, Team elem) {
        logger.traceEntry("updating task {} ", elem);
        Connection con = dbUtils.getConnection();

        try(PreparedStatement preStm = con.prepareStatement("UPDATE Teams SET name = ? WHERE id = ?;")){
            preStm.setString(1, elem.getName());
            preStm.setInt(2, Math.toIntExact(aLong));
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
    public Team findById(Long aLong) {
        logger.traceEntry();
        Connection con = dbUtils.getConnection();
        Team team = null;

        try(PreparedStatement preStm = con.prepareStatement("SELECT * FROM Teams WHERE id = ? LIMIT 1;")){
            preStm.setInt(1, Math.toIntExact(aLong));
            try(ResultSet result = preStm.executeQuery()){
                if(result.next()){
                    Long id = (long) result.getInt("id");
                    String name = result.getString("name");
                    team = new Team(name);
                    team.setId(id);
                }
            }
        }
        catch(SQLException ex){
            logger.error(ex);
            System.err.println("Error DB" + ex);
        }
        logger.traceExit();
        return team;
    }

    @Override
    public Iterable<Team> findAll() {
        logger.traceEntry();
        Connection con = dbUtils.getConnection();
        List<Team> teams = new ArrayList<>();

        try(PreparedStatement preStm = con.prepareStatement("SELECT * FROM Teams;")){
            try(ResultSet result = preStm.executeQuery()){
                while(result.next()){
                    Long id = (long) result.getInt("id");
                    String name = result.getString("name");
                    Team team = new Team(name);
                    team.setId(id);
                    teams.add(team);
                }
            }
        }
        catch(SQLException ex){
            logger.error(ex);
            System.err.println("Error DB" + ex);
        }
        logger.traceExit();
        return teams;
    }
}
