package app.persistence.repository.jdbc;

import app.model.Game;
import app.model.Team;
import app.model.TicketPurchase;
import app.persistence.TicketPurchasesRepository;
import org.apache.logging.log4j.LogManager;
import org.apache.logging.log4j.Logger;
import org.springframework.stereotype.Component;

import java.io.IOException;
import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.time.LocalDate;
import java.util.ArrayList;
import java.util.List;
import java.util.Properties;

@Component
public class TicketPurchasesDBRepository implements TicketPurchasesRepository {

    private final JdbcUtils dbUtils;
    private static final Logger logger = LogManager.getLogger();

    public TicketPurchasesDBRepository(Properties props){
        logger.info("Initializing TicketPurchasesDBRepository with properties: {} ", props);
        dbUtils = new JdbcUtils(props);
    }

    public TicketPurchasesDBRepository(){
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
    public void add(TicketPurchase elem) {
        logger.traceEntry("saving task {} ", elem);
        Connection con = dbUtils.getConnection();

        try(PreparedStatement preStm = con.prepareStatement("INSERT INTO TicketPurchases (gameId, noOfTickets, " +
                "totalPrice, clientName) VALUES (?, ?, ?, ?);")){
            preStm.setInt(1, Math.toIntExact(elem.getGame().getId()));
            preStm.setInt(2, elem.getNoOfTickets());
            preStm.setDouble(3, elem.getTotalPrice());
            preStm.setString(4, elem.getClientName());
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

        try(PreparedStatement preStm = con.prepareStatement("DELETE FROM TicketPurchases WHERE id = ?;")){
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
    public void update(Long aLong, TicketPurchase elem) {
        logger.traceEntry("updating task {} ", elem);
        Connection con = dbUtils.getConnection();

        try(PreparedStatement preStm = con.prepareStatement("UPDATE TicketPurchases SET gameId = ?, noOfTickets = ?," +
                "totalPrice = ?, clientName = ? WHERE id = ?;")){
            preStm.setInt(1, Math.toIntExact(elem.getGame().getId()));
            preStm.setInt(2, elem.getNoOfTickets());
            preStm.setDouble(3, elem.getTotalPrice());
            preStm.setString(4, elem.getClientName());
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
    public TicketPurchase findById(Long aLong) {
        logger.traceEntry();
        Connection con = dbUtils.getConnection();
        TicketPurchase ticketPurchase = null;

        try(PreparedStatement preStm = con.prepareStatement("SELECT * FROM TicketPurchases TP\n" +
                "WHERE id = ? LIMIT 1")){
            preStm.setInt(1, Math.toIntExact(aLong));
            try(ResultSet result = preStm.executeQuery()){
                if(result.next()){
                    Long tpId = result.getLong("TP.id");
                    long gameId = result.getLong("gameId");
                    Game game;
                    try(PreparedStatement preStm1 = con.prepareStatement("""
                            SELECT * FROM Games G
                            INNER JOIN Teams T1 on G.team1Id = T1.id
                            INNER JOIN Teams T2 on G.team2Id = T2.id
                            WHERE G.id = ?
                            LIMIT 1;""")){
                        preStm1.setInt(1, Math.toIntExact(gameId));
                        try(ResultSet result1 = preStm1.executeQuery()){

                            Long team1Id = result1.getLong("T1.id");
                            String team1Name = result1.getString("T1.name");
                            Team team1 = new Team(team1Name);
                            team1.setId(team1Id);

                            Long team2Id = result1.getLong("T2.id");
                            String team2Name = result1.getString("T2.name");
                            Team team2 = new Team(team2Name);
                            team2.setId(team2Id);

                            Long gameIdd = result1.getLong("G.id");
                            String description = result1.getString("description");
                            Integer totalNoOfTickets = result1.getInt("totalNoOfTickets");
                            Integer soldTickets = result1.getInt("soldTickets");
                            Float pricePerTicket = result1.getFloat("pricePerTicket");
                            LocalDate date =result1.getDate("date").toLocalDate();

                            game = new Game(team1, team2, date, description, totalNoOfTickets, soldTickets, pricePerTicket);
                            game.setId(gameIdd);
                        }
                    }
                    Integer noOfTickets = result.getInt("noOfTickets");
                    Float totalPrice = result.getFloat("totalPrice");
                    String clientName = result.getString("clientName");

                    ticketPurchase = new TicketPurchase(game, noOfTickets, totalPrice, clientName);
                    ticketPurchase.setId(tpId);
                }
            }
        }
        catch(SQLException ex){
            logger.error(ex);
            System.err.println("Error DB" + ex);
        }
        logger.traceExit();
        return ticketPurchase;
    }

    @Override
    public Iterable<TicketPurchase> findAll() {
        logger.traceEntry();
        Connection con = dbUtils.getConnection();
        List<TicketPurchase> ticketPurchases = new ArrayList<>();

        try(PreparedStatement preStm = con.prepareStatement("SELECT * FROM TicketPurchases TP;")){
            try(ResultSet result = preStm.executeQuery()){
                while(result.next()){
                    Long tpId = result.getLong(1);
                    long gameId = result.getLong("gameId");
                    Game game;
                    try(PreparedStatement preStm1 = con.prepareStatement("""
                            SELECT * FROM Games G
                            INNER JOIN Teams T1 on G.team1Id = T1.id
                            INNER JOIN Teams T2 on G.team2Id = T2.id
                            WHERE G.id = ?
                            LIMIT 1;""")){
                        preStm1.setInt(1, Math.toIntExact(gameId));
                        try(ResultSet result1 = preStm1.executeQuery()){

                            Long team1Id = result1.getLong("team1Id");
                            String team1Name = result1.getString(9);
                            Team team1 = new Team(team1Name);
                            team1.setId(team1Id);

                            Long team2Id = result1.getLong("team2Id");
                            String team2Name = result1.getString(11);
                            Team team2 = new Team(team2Name);
                            team2.setId(team2Id);

                            Long gameIdd = result1.getLong(1);
                            String description = result1.getString("description");
                            Integer totalNoOfTickets = result1.getInt("totalNoOfTickets");
                            Integer soldTickets = result1.getInt("soldTickets");
                            Float pricePerTicket = result1.getFloat("pricePerTicket");
                            LocalDate date = result1.getDate("date").toLocalDate();

                            game = new Game(team1, team2, date, description, totalNoOfTickets, soldTickets, pricePerTicket);
                            game.setId(gameIdd);
                        }
                    }
                    Integer noOfTickets = result.getInt("noOfTickets");
                    Float totalPrice = result.getFloat("totalPrice");
                    String clientName = result.getString("clientName");

                    TicketPurchase ticketPurchase = new TicketPurchase(game, noOfTickets, totalPrice, clientName);
                    ticketPurchase.setId(tpId);
                    ticketPurchases.add(ticketPurchase);
                }
            }
        }
        catch(SQLException ex){
            logger.error(ex);
            System.err.println("Error DB" + ex);
        }
        logger.traceExit();
        return ticketPurchases;
    }
}
