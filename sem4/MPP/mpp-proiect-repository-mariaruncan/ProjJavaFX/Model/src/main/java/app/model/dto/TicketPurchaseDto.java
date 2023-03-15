package app.model.dto;

import java.io.Serializable;
import java.util.Objects;

public class TicketPurchaseDto implements Serializable {
    private long gameId;
    private int noOfTickets;
    private String clientName;

    public TicketPurchaseDto(long gameId, int noOfTickets, String clientName) {
        this.gameId = gameId;
        this.noOfTickets = noOfTickets;
        this.clientName = clientName;
    }

    @Override
    public boolean equals(Object o) {
        if (this == o) return true;
        if (o == null || getClass() != o.getClass()) return false;
        TicketPurchaseDto that = (TicketPurchaseDto) o;
        return gameId == that.gameId && noOfTickets == that.noOfTickets && Objects.equals(clientName, that.clientName);
    }

    @Override
    public int hashCode() {
        return Objects.hash(gameId, noOfTickets, clientName);
    }

    public long getGameId() {
        return gameId;
    }

    public void setGameId(long gameId) {
        this.gameId = gameId;
    }

    public int getNoOfTickets() {
        return noOfTickets;
    }

    public void setNoOfTickets(int noOfTickets) {
        this.noOfTickets = noOfTickets;
    }

    public String getClientName() {
        return clientName;
    }

    public void setClientName(String clientName) {
        this.clientName = clientName;
    }
}
