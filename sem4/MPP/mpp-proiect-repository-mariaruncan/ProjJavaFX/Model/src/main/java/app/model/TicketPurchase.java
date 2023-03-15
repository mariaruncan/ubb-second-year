package app.model;

import java.io.Serializable;
import java.util.Objects;

public class TicketPurchase extends Entity<Long> implements Serializable {
    private Game game;
    private Integer noOfTickets;
    private Float totalPrice;
    private String clientName;

    public TicketPurchase() {
        this.game = null;
        this.noOfTickets = 0;
        this.totalPrice = 0F;
        this.clientName = "";
    }

    public TicketPurchase(Game game, Integer noOfTickets, Float totalPrice, String clientName) {
        this.game = game;
        this.noOfTickets = noOfTickets;
        this.totalPrice = totalPrice;
        this.clientName = clientName;
    }

    public Game getGame() {
        return game;
    }

    public void setGame(Game game) {
        this.game = game;
    }

    public Integer getNoOfTickets() {
        return noOfTickets;
    }

    public void setNoOfTickets(Integer noOfTickets) {
        this.noOfTickets = noOfTickets;
    }

    public Float getTotalPrice() {
        return totalPrice;
    }

    public void setTotalPrice(Float totalPrice) {
        this.totalPrice = totalPrice;
    }

    public String getClientName() {
        return clientName;
    }

    public void setClientName(String clientName) {
        this.clientName = clientName;
    }

    @Override
    public String toString() {
        return "TicketPurchase: " + super.getId() + " , game " + game.getId() + ", " +
                noOfTickets + " tickets bought by " + clientName + " for " + totalPrice + "$";
    }

    @Override
    public boolean equals(Object o) {
        if (this == o) return true;
        if (o == null || getClass() != o.getClass()) return false;
        TicketPurchase that = (TicketPurchase) o;
        return Objects.equals(game, that.game) && Objects.equals(noOfTickets, that.noOfTickets) &&
                Objects.equals(totalPrice, that.totalPrice) && Objects.equals(clientName, that.clientName);
    }

    @Override
    public int hashCode() {
        return Objects.hash(game, noOfTickets, totalPrice, clientName);
    }
}

