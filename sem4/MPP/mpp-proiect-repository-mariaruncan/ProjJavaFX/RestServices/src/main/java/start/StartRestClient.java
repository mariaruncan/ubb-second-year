package start;

import app.model.Game;
import app.services.rest.ServiceException;
import rest.client.GamesClient;

import java.time.format.DateTimeFormatter;
import java.util.Arrays;
import java.util.HashMap;
import java.util.Map;
import java.util.stream.Stream;

public class StartRestClient {
    private final static GamesClient gamesClient = new GamesClient();

    public static void main(String[] args) {
        try{
            Game aux = gamesClient.getById(1L);

            // get by id
            show(() -> {
                System.out.println("get by id");
                System.out.println(gamesClient.getById(2L));
            });

            // create
            Game game = new Game(aux.getTeam1(), aux.getTeam2(), aux.getDate().plusDays(100),
                    "testRestClientJava", 20, 0, 10.0F);
            show(() -> {
                System.out.println("create");
                System.out.println(gamesClient.create(game));
            });

            // findAll
            show(() -> {
                System.out.println("find all");
                printAsTable(gamesClient.findAll());
            });

            Game[] temp = gamesClient.findAll();

            // update
            game.setId(temp[temp.length - 1].getId());
            game.setDescription("testRestJava");
            show(() -> {
                System.out.println("update");
                gamesClient.update(game);
            });

            // findAll
            show(() -> {
                System.out.println("find all");
                printAsTable(gamesClient.findAll());
            });

            // delete
            show(() -> gamesClient.delete(temp[temp.length - 1].getId()));

            // findAll
            show(() -> {
                System.out.println("find all");
                printAsTable(gamesClient.findAll());
            });
        } catch (Exception e) {
            System.out.println("Exception ... " + e.getMessage());
        }
    }

    private static void show(Runnable task){
        try{
            task.run();
        }
        catch(ServiceException e){
            System.out.println("Service exception " + e);
        }
    }

    public static void printAsTable(Game[] games) {
        String[][] table = new String[games.length + 1][7];
        table[0] = new String[]{"Id", "Team1", "Team2", "Description", "Total tickets", "Sold tickets", "Date"};
        for (int i = 0; i < games.length; i++) {
            Game g = games[i];
            table[i + 1] = new String[]{g.getId().toString(), g.getTeam1().getId().toString(), g.getTeam2().getId().toString(),
                    g.getDescription(), g.getTotalNoOfTickets().toString(), g.getSoldTickets().toString(),
                    g.getDate().format(DateTimeFormatter.ofPattern("d-MMM-yy"))};
        }

        Map<Integer, Integer> columnLengths = new HashMap<>();
        Arrays.stream(table).forEach(a -> Stream.iterate(0, (i -> i < a.length), (i -> ++i)).forEach(i -> {
            columnLengths.putIfAbsent(i, 0);
            if (columnLengths.get(i) < a[i].length()) {
                columnLengths.put(i, a[i].length());
            }
        }));

        final StringBuilder formatString = new StringBuilder("");
        columnLengths.forEach((key, value) -> formatString.append("| %").append(value).append("s "));
        formatString.append("|\n");

        Stream.iterate(0, (i -> i < table.length), (i -> ++i))
                .forEach(a -> System.out.printf(formatString.toString(), (Object[]) table[a]));
    }
}
