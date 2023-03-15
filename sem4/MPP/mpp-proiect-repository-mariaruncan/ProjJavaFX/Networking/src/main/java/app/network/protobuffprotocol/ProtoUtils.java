package app.network.protobuffprotocol;

import app.model.Employee;
import app.model.Game;
import app.model.Team;
import app.model.dto.EmployeeDto;
import app.model.dto.TicketPurchaseDto;

import java.time.LocalDate;
import java.time.chrono.ChronoLocalDate;
import java.time.format.DateTimeFormatter;
import java.util.ArrayList;
import java.util.List;
import java.util.Locale;

public class ProtoUtils {
    public static AppProtobuffs.Request createLoginRequest (String username, String password) {
        AppProtobuffs.EmployeeDto empDto = AppProtobuffs.EmployeeDto.newBuilder().setUsername(username).setPassword(password).build();
        AppProtobuffs.Request request = AppProtobuffs.Request.newBuilder().setType(AppProtobuffs.Request.Type.LOGIN)
                .setEmpDto(empDto).build();
        return request;
    }

    public static AppProtobuffs.Request createLogoutRequest(Employee employee){
        AppProtobuffs.EmployeeDto empDto = AppProtobuffs.EmployeeDto.newBuilder().setUsername(employee.getUsername()).build();
        AppProtobuffs.Request request = AppProtobuffs.Request.newBuilder().setType(AppProtobuffs.Request.Type.LOGOUT)
                .setEmpDto(empDto).build();
        return request;
    }

    public static AppProtobuffs.Response createOkResponse(){
        AppProtobuffs.Response response = AppProtobuffs.Response.newBuilder()
                .setType(AppProtobuffs.Response.Type.OK).build();
        return response;
    }

    public static AppProtobuffs.Response createErrorResponse(String text){
        AppProtobuffs.Response response = AppProtobuffs.Response.newBuilder()
                .setType(AppProtobuffs.Response.Type.ERROR)
                .setError(text).build();
        return response;
    }

    public static String getError(AppProtobuffs.Response response){
        String errorMessage = response.getError();
        return errorMessage;
    }

    public static Game getGame(AppProtobuffs.Response response) {
        Game g = new Game();
        Team team1 = new Team();
        Team team2 = new Team();
        team1.setId(response.getGame().getTeam1().getId());
        team1.setName(response.getGame().getTeam1().getName());
        team2.setId(response.getGame().getTeam2().getId());
        team2.setName(response.getGame().getTeam2().getName());
        g.setId(response.getGame().getId());
        g.setTeam1(team1);
        g.setTeam2(team2);
        g.setDate(parseStringToLocalDate(response.getGame().getDate()));
        g.setDescription(response.getGame().getDescription());
        g.setTotalNoOfTickets(response.getGame().getTotalNoTickets());
        g.setSoldTickets(response.getGame().getSoldTickets());
        g.setPricePerTicket(response.getGame().getPricePerTicket());
        return g;
    }

    public static AppProtobuffs.Request createPurchaseRequest(long gameId, int noOfTickets, String clientName) {
        AppProtobuffs.TicketPurchaseDto tpDto = AppProtobuffs.TicketPurchaseDto.newBuilder().setGameId(gameId)
                .setNoOfTickets(noOfTickets).setClientName(clientName).build();
        AppProtobuffs.Request request = AppProtobuffs.Request.newBuilder().setType(AppProtobuffs.Request.Type.PURCHASE)
                .setTpDto(tpDto).build();
        return request;
    }

    public static AppProtobuffs.Request createFindAllGamesRequest() {
        AppProtobuffs.Request request = AppProtobuffs.Request.newBuilder().setType(AppProtobuffs.Request.Type.GET_GAMES)
                .build();
        return request;
    }

    public static List<Game> getAllGames(AppProtobuffs.Response response) {
        List<Game> games = new ArrayList<>();
        for(int i = 0; i < response.getGamesCount(); i++){
            AppProtobuffs.Game gpb = response.getGames(i);
            Game game = new Game();
            Team team1 = new Team();
            Team team2 = new Team();
            team1.setId(gpb.getTeam1().getId());
            team1.setName(gpb.getTeam1().getName());
            team2.setId(gpb.getTeam2().getId());
            team2.setName(gpb.getTeam2().getName());
            game.setId(gpb.getId());
            game.setTeam1(team1);
            game.setTeam2(team2);
            String dateStr = gpb.getDate();
            game.setDate(parseStringToLocalDate(dateStr));
            game.setDescription(gpb.getDescription());
            game.setTotalNoOfTickets(gpb.getTotalNoTickets());
            game.setSoldTickets(gpb.getSoldTickets());
            game.setPricePerTicket(gpb.getPricePerTicket());
            games.add(game);
        }
        return games;
    }

    public static AppProtobuffs.Response createGameResponse(Game game) {
        AppProtobuffs.Team team1 = AppProtobuffs.Team.newBuilder().setId(game.getTeam1().getId())
                .setName(game.getTeam1().getName()).build();
        AppProtobuffs.Team team2 = AppProtobuffs.Team.newBuilder().setId(game.getTeam2().getId())
                .setName(game.getTeam2().getName()).build();
        AppProtobuffs.Game gameR = AppProtobuffs.Game.newBuilder().setId(game.getId()).setTeam1(team1).setTeam2(team2)
                .setDate(game.getDate().toString()).setDescription(game.getDescription())
                .setTotalNoTickets(game.getTotalNoOfTickets()).setSoldTickets(game.getSoldTickets())
                .setPricePerTicket(game.getPricePerTicket()).build();
        AppProtobuffs.Response response = AppProtobuffs.Response.newBuilder().setType(AppProtobuffs.Response.Type.GAME)
                .setGame(gameR).build();
        return response;
    }

    public static EmployeeDto getEmpDto(AppProtobuffs.Request request) {
        EmployeeDto employeeDto = new EmployeeDto(request.getEmpDto().getUsername(), request.getEmpDto().getPassword());
        return employeeDto;
    }

    public static TicketPurchaseDto getTicketPurchaseDto(AppProtobuffs.Request request) {
        TicketPurchaseDto tpDto = new TicketPurchaseDto(request.getTpDto().getGameId(), request.getTpDto().getNoOfTickets(),
                request.getTpDto().getClientName());
        return tpDto;
    }

    public static AppProtobuffs.Response createFindAllGamesResponse(List<Game> gamesList) {
        AppProtobuffs.Response.Builder response = AppProtobuffs.Response.newBuilder()
                .setType(AppProtobuffs.Response.Type.GET_GAME);
        for(Game g : gamesList){
            AppProtobuffs.Team team1 = AppProtobuffs.Team.newBuilder().setId(g.getTeam1().getId()).setName(g.getTeam1().getName()).build();
            AppProtobuffs.Team team2 = AppProtobuffs.Team.newBuilder().setId(g.getTeam2().getId()).setName(g.getTeam2().getName()).build();
            AppProtobuffs.Game game = AppProtobuffs.Game.newBuilder().setId(g.getId()).setTeam1(team1).setTeam2(team2)
                            .setDate(g.getDate().toString()).setDescription(g.getDescription()).setTotalNoTickets(g.getTotalNoOfTickets())
                            .setSoldTickets(g.getSoldTickets()).setPricePerTicket(g.getPricePerTicket()).build();
            response.addGames(game);
        }
        return response.build();
    }

    private static LocalDate parseStringToLocalDate(String str){
        String[] parts = str.split("-");
        return LocalDate.of(Integer.parseInt(parts[2]), Integer.parseInt(parts[1]), Integer.parseInt(parts[0]));
    }
}
