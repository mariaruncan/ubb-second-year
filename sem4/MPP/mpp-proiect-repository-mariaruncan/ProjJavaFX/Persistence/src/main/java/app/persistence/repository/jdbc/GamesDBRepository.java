package app.persistence.repository.jdbc;

import app.model.Game;
import app.model.Team;
import app.persistence.GamesRepository;
import org.apache.logging.log4j.LogManager;
import org.apache.logging.log4j.Logger;
import org.springframework.stereotype.Component;

import java.io.IOException;
import java.sql.*;
import java.time.LocalDate;
import java.time.format.DateTimeFormatter;
import java.util.ArrayList;
import java.util.List;
import java.util.Properties;

@Component
public class GamesDBRepository implements GamesRepository {

    private final JdbcUtils dbUtils;
    private static final Logger logger = LogManager.getLogger();

    public GamesDBRepository(Properties props){
        logger.info("Initializing GamesDBRepository with properties: {} ", props);
        dbUtils = new JdbcUtils(props);
    }

    public GamesDBRepository(){
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
    public void add(Game elem) {
        logger.traceEntry("saving task {} ", elem);
        Connection con = dbUtils.getConnection();
        DateTimeFormatter formatter = DateTimeFormatter.ofPattern("d-MMM-yy");

        try(PreparedStatement preStm = con.prepareStatement("INSERT INTO Games (team1Id, team2Id, description, " +
                "totalNoOfTickets, soldTickets, pricePerTicket, date) VALUES (?, ?, ?, ?, ?, ?, ?);")){
            preStm.setInt(1, Math.toIntExact(elem.getTeam1().getId()));
            preStm.setInt(2, Math.toIntExact(elem.getTeam2().getId()));
            preStm.setString(3, elem.getDescription());
            preStm.setInt(4, elem.getTotalNoOfTickets());
            preStm.setInt(5, elem.getSoldTickets());
            preStm.setDouble(6, elem.getPricePerTicket());
            preStm.setString(7, elem.getDate().format(formatter));
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

        try(PreparedStatement preStm = con.prepareStatement("DELETE FROM Games WHERE id = ?;")){
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
    public void update(Long aLong, Game elem) {
        logger.traceEntry("updating task {} ", elem);
        Connection con = dbUtils.getConnection();
        DateTimeFormatter formatter = DateTimeFormatter.ofPattern("d-MMM-yy");

        try(PreparedStatement preStm = con.prepareStatement("UPDATE Games SET team1Id = ?, team2Id = ?, " +
                "description = ?, totalNoOfTickets = ?, soldTickets = ?, pricePerTicket = ?, date = ?" +
                " WHERE id = ?;")){
            preStm.setInt(1, Math.toIntExact(elem.getTeam1().getId()));
            preStm.setInt(2, Math.toIntExact(elem.getTeam2().getId()));
            preStm.setString(3, elem.getDescription());
            preStm.setInt(4, elem.getTotalNoOfTickets());
            preStm.setInt(5, elem.getSoldTickets());
            preStm.setDouble(6, elem.getPricePerTicket());
            preStm.setString(7, elem.getDate().format(formatter));
            preStm.setLong(8, elem.getId());
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
    public Game findById(Long aLong) {
        logger.traceEntry();
        Connection con = dbUtils.getConnection();
        Game game = null;
        DateTimeFormatter formatter = DateTimeFormatter.ofPattern("d-MMM-yy");

        try(PreparedStatement preStm = con.prepareStatement("""
                SELECT * FROM Games G
                INNER JOIN Teams T1 on G.team1Id = T1.id
                INNER JOIN Teams T2 on G.team2Id = T2.id
                WHERE G.id = ?
                LIMIT 1;""")){
            preStm.setInt(1, Math.toIntExact(aLong));
            try(ResultSet result = preStm.executeQuery()){
                if(result.next()){
                    Long team1Id = result.getLong(9);
                    String team1Name = result.getString(10);
                    Team team1 = new Team(team1Name);
                    team1.setId(team1Id);

                    Long team2Id = result.getLong(11);
                    String team2Name = result.getString(12);
                    Team team2 = new Team(team2Name);
                    team2.setId(team2Id);

                    Long gameId = result.getLong(1);
                    String description = result.getString("description");
                    Integer totalNoOfTickets = result.getInt("totalNoOfTickets");
                    Integer soldTickets = result.getInt("soldTickets");
                    Float pricePerTicket = result.getFloat("pricePerTicket");
                    LocalDate date = LocalDate.parse(result.getString("date").split(" ")[0], formatter);

                    game = new Game(team1, team2, date, description, totalNoOfTickets, soldTickets, pricePerTicket);
                    game.setId(gameId);
                }
            }
        }
        catch(SQLException ex){
            logger.error(ex);
            System.err.println("Error DB" + ex);
        }
        logger.traceExit();
        return game;
    }

    @Override
    public Iterable<Game> findAll() {
        logger.traceEntry();
        Connection con = dbUtils.getConnection();
        List<Game> games = new ArrayList<>();
        DateTimeFormatter formatter = DateTimeFormatter.ofPattern("d-MMM-yy");

        try(PreparedStatement preStm = con.prepareStatement("""
                SELECT * FROM Games G
                INNER JOIN Teams T1 on G.team1Id = T1.id
                INNER JOIN Teams T2 on G.team2Id = T2.id;""")){
            try(ResultSet result = preStm.executeQuery()){
                while(result.next()){
                    Long team1Id = result.getLong(9);
                    String team1Name = result.getString(10);
                    Team team1 = new Team(team1Name);
                    team1.setId(team1Id);

                    Long team2Id = result.getLong(11);
                    String team2Name = result.getString(12);
                    Team team2 = new Team(team2Name);
                    team2.setId(team2Id);

                    Long gameId = result.getLong(1);
                    String description = result.getString("description");
                    Integer totalNoOfTickets = result.getInt("totalNoOfTickets");
                    Integer soldTickets = result.getInt("soldTickets");
                    Float pricePerTicket = result.getFloat("pricePerTicket");
                    String dateStr = result.getString("date").split(" ")[0];
                    LocalDate date = LocalDate.parse(dateStr, formatter);

                    Game game = new Game(team1, team2, date, description, totalNoOfTickets, soldTickets, pricePerTicket);
                    game.setId(gameId);
                    games.add(game);
                }
            }
        }
        catch(SQLException ex){
            logger.error(ex);
            System.err.println("Error DB" + ex);
        }
        logger.traceExit();
        return games;
    }
}
