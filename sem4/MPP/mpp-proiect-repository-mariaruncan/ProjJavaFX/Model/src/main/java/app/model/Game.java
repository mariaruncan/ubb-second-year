package app.model;

import com.fasterxml.jackson.annotation.JsonIgnore;

import java.io.Serializable;
import java.time.LocalDate;
import java.util.Objects;

public class Game extends Entity<Long> implements Serializable {
    private Team team1;
    private Team team2;
    private LocalDate date;
    private String description;
    private Integer totalNoOfTickets;
    private Integer soldTickets;
    private Float pricePerTicket;

    public Game(){
        this.team1 = null;
        this.team2 = null;
        this.date = null;
        this.description = "";
        this.totalNoOfTickets = 0;
        this.soldTickets = 0;
        this.pricePerTicket = 0F;
    }

    public Game(Team team1, Team team2, LocalDate date, String description, Integer totalNoOfTickets, Integer soldTickets, Float pricePerTicket) {
        this.team1 = team1;
        this.team2 = team2;
        this.date = date;
        this.description = description;
        this.totalNoOfTickets = totalNoOfTickets;
        this.soldTickets = soldTickets;
        this.pricePerTicket = pricePerTicket;
    }

    public LocalDate getDate() {
        return date;
    }

    public void setDate(LocalDate date) {
        this.date = date;
    }

    public Team getTeam1() {
        return team1;
    }

    public void setTeam1(Team team1) {
        this.team1 = team1;
    }

    public Team getTeam2() {
        return team2;
    }

    public void setTeam2(Team team2) {
        this.team2 = team2;
    }

    public String getDescription() {
        return description;
    }

    public void setDescription(String description) {
        this.description = description;
    }

    public Integer getTotalNoOfTickets() {
        return totalNoOfTickets;
    }

    public void setTotalNoOfTickets(Integer totalNoOfTickets) {
        this.totalNoOfTickets = totalNoOfTickets;
    }

    public Integer getSoldTickets() {
        return soldTickets;
    }

    public void setSoldTickets(Integer soldTickets) {
        this.soldTickets = soldTickets;
    }

    public Float getPricePerTicket() {
        return pricePerTicket;
    }

    public void setPricePerTicket(Float pricePerTicket) {
        this.pricePerTicket = pricePerTicket;
    }

    @JsonIgnore
    public String getAvailableTickets(){
        if(Objects.equals(totalNoOfTickets, soldTickets))
            return "SOLD OUT!";
        return String.valueOf(totalNoOfTickets - soldTickets);
    }

    @Override
    public String toString() {
        return "Game: " + super.getId() + " " + date + " " + team1.getName() + " vs. " + team2.getName() +
                "\n" + description + "\nTickets available: " + (totalNoOfTickets - soldTickets) +
                " for only " + pricePerTicket + "$ per ticket";
    }

    @Override
    public boolean equals(Object o) {
        if (this == o) return true;
        if (o == null || getClass() != o.getClass()) return false;
        Game game = (Game) o;
        return Objects.equals(team1, game.team1) && Objects.equals(team2, game.team2) && Objects.equals(date, game.date)
                && Objects.equals(description, game.description) && Objects.equals(totalNoOfTickets, game.totalNoOfTickets)
                && Objects.equals(soldTickets, game.soldTickets) && Objects.equals(pricePerTicket, game.pricePerTicket);
    }

    @Override
    public int hashCode() {
        return Objects.hash(team1, team2, date, description, totalNoOfTickets, soldTickets, pricePerTicket);
    }
}
