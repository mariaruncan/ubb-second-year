package app.services.rest;

import app.model.Game;
import app.persistence.GamesRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.List;

@RestController
@RequestMapping("/app/games")
public class GameController {

    @Autowired
    private GamesRepository gamesRepository;

    @RequestMapping(method = RequestMethod.GET)
    public List<Game> findAll(){
        System.out.println("Get all games...");
        return (List<Game>) gamesRepository.findAll();
    }

    @RequestMapping(value = "/{id}", method = RequestMethod.GET)
    public ResponseEntity<?> getById(@PathVariable Long id){
        System.out.println("Get by id "+id);
        Game game = gamesRepository.findById(id);
        if (game == null)
            return new ResponseEntity<>("Game not found", HttpStatus.NOT_FOUND);
        else
            return new ResponseEntity<>(game, HttpStatus.OK);
    }

    @RequestMapping(method = RequestMethod.POST)
    public Game create(@RequestBody Game game){
        System.out.println("Creating game...");
        gamesRepository.add(game);
        return game;
    }

    @RequestMapping(value = "/{id}", method = RequestMethod.PUT)
    public Game update(@RequestBody Game game) {
        System.out.println("Updating game ...");
        gamesRepository.update(game.getId(), game);
        return game;
    }

    @RequestMapping(value="/{id}", method= RequestMethod.DELETE)
    public ResponseEntity<?> delete(@PathVariable Long id){
        System.out.println("Deleting game ... " + id);
        gamesRepository.delete(id);
        return new ResponseEntity<Game>(HttpStatus.OK);
    }

    @RequestMapping("/{id}/description")
    public String description(@PathVariable Long id){
        Game result = gamesRepository.findById(id);
        System.out.println("Result... " + result);
        return result.getDescription();
    }

    @RequestMapping("/{id}/availableTickets")
    public String availableTickets(@PathVariable Long id){
        Game result = gamesRepository.findById(id);
        System.out.println("Result... " + result);
        return result.getAvailableTickets();
    }

    @ExceptionHandler(Exception.class)
    @ResponseStatus(HttpStatus.BAD_REQUEST)
    public String userError(Exception e) {
        return e.getMessage();
    }
}
