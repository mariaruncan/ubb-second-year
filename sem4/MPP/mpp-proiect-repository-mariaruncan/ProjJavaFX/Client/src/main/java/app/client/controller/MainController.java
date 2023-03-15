package app.client.controller;

import app.model.Employee;
import app.model.Game;
import app.model.TicketPurchase;
import app.services.IObserver;
import app.services.IServices;
import app.services.MyException;
import javafx.collections.FXCollections;
import javafx.collections.ObservableList;
import javafx.event.ActionEvent;
import javafx.fxml.FXML;
import javafx.fxml.Initializable;
import javafx.scene.Node;
import javafx.scene.Parent;
import javafx.scene.control.*;
import javafx.scene.control.cell.PropertyValueFactory;
import javafx.scene.input.MouseEvent;

import java.io.IOException;
import java.lang.reflect.InvocationTargetException;
import java.net.URL;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.time.Month;
import java.time.format.DateTimeFormatter;
import java.util.List;
import java.util.Objects;
import java.util.ResourceBundle;

public class MainController implements IObserver {

    private Parent parent;

    private IServices server;
    private Employee employee;

    @FXML public Label labelWelcome;
    @FXML public TableView<Game> gamesTable;
    @FXML public TableColumn<Game, Long> idColumn;
    @FXML public TableColumn<Game, String> team1Column;
    @FXML public TableColumn<Game, String> team2Column;
    @FXML public TableColumn<Game, LocalDate> dateColumn;
    @FXML public TableColumn<Game, String> availableTicketsColumn;
    ObservableList<Game> gamesList = FXCollections.observableArrayList();

    public Label labelId;
    public Label labelTeam1;
    public Label labelTeam2;
    public Label labelDate;
    public Label labelTotalTickets;
    public Label labelSoldTickets;
    public Label labelPriceTicket;
    public Label labelDescription;
    public DatePicker pickerMinDate;
    public DatePicker pickerMaxDate;
    public TextField textFieldClientName;
    public Slider sliderNoTickets;
    public Label labelSliderValue;

    public MainController(){
        System.out.println("MainController constructor without param.");
    }

    public MainController(IServices server){
        this.server = server;
        System.out.println("MainController constructor with param");
    }

    public void setServer(IServices srv){
        server = srv;
    }

    public void setEmployee(Employee employee) {
        this.employee = employee;
        this.labelWelcome.setText("Hello, " + employee.getUsername() + "!");
    }

    public void initialize1() {
        pickerMinDate.setValue(LocalDate.of(2022, 3, 1));
        pickerMaxDate.setValue(LocalDate.of(2022, 4, 1));
        sliderNoTickets.valueProperty().addListener(observable -> labelSliderValue.setText(String.valueOf((int)(sliderNoTickets.getValue()))));
        labelSliderValue.setText("0");

        idColumn.setCellValueFactory(new PropertyValueFactory<>("id"));
        team1Column.setCellValueFactory(new PropertyValueFactory<>("team1"));
        team2Column.setCellValueFactory(new PropertyValueFactory<>("team2"));
        dateColumn.setCellValueFactory(new PropertyValueFactory<>("date"));
        availableTicketsColumn.setCellValueFactory(new PropertyValueFactory<>("availableTickets"));
        showGames();

        pickerMinDate.chronologyProperty().addListener(observable -> showGames());
        pickerMaxDate.chronologyProperty().addListener(observable -> showGames());
    }

    public void showGames(){
        gamesTable.getItems().clear();

        LocalDate minDate = pickerMinDate.getValue();
        LocalDate maxDate = pickerMaxDate.getValue();

        gamesList.clear();
        try {
            List<Game> list = server.findAllGames();
            list.stream().filter(x ->
                x.getDate().isAfter(minDate) && x.getDate().isBefore(maxDate)).forEach(gamesList::add);
        } catch (MyException e) {
            System.out.println("My exception show games main controller");
        }
        gamesTable.setItems(gamesList);
    }

    public void showGameDetails(MouseEvent mouseEvent) {
        Game selectedItem = gamesTable.getSelectionModel().getSelectedItem();
        if(selectedItem != null) {
            labelId.setText(selectedItem.getId().toString());
            labelTeam1.setText(selectedItem.getTeam1().getName());
            labelTeam2.setText(selectedItem.getTeam2().getName());
            labelTotalTickets.setText(selectedItem.getTotalNoOfTickets().toString());
            labelSoldTickets.setText(selectedItem.getSoldTickets().toString());
            labelPriceTicket.setText(selectedItem.getPricePerTicket().toString());
            labelDate.setText(selectedItem.getDate().toString());
            labelDescription.setText(selectedItem.getDescription());
        }
        else{
            labelId.setText("");
            labelTeam1.setText("");
            labelTeam2.setText("");
            labelPriceTicket.setText("");
            labelDate.setText("");
            labelTotalTickets.setText("");
            labelSoldTickets.setText("");
            labelDescription.setText("");
        }
    }

    public void sellTickets(ActionEvent actionEvent) {
        Game selectedItem = gamesTable.getSelectionModel().getSelectedItem();
        if(selectedItem == null){
            Alert alert = new Alert(Alert.AlertType.INFORMATION, "Please select a game!");
            alert.show();
            return;
        }

        String clientName = textFieldClientName.getText();
        if(clientName.equals("")){
            Alert alert = new Alert(Alert.AlertType.INFORMATION, "Please enter client name!");
            alert.show();
            return;
        }

        int noOfTickets = Integer.parseInt(labelSliderValue.getText());
        if(noOfTickets == 0){
            Alert alert = new Alert(Alert.AlertType.INFORMATION, "No of tickets can not be 0!");
            alert.show();
            return;
        }

        if(noOfTickets > Integer.parseInt(labelTotalTickets.getText()) - Integer.parseInt(labelSoldTickets.getText())){
            Alert alert = new Alert(Alert.AlertType.INFORMATION, "Can not buy that much tickets!");
            alert.show();
            return;
        }

        try{
            server.purchase(selectedItem.getId(), noOfTickets, clientName);
            Alert alert = new Alert(Alert.AlertType.INFORMATION, "Purchase completed!");
            alert.show();
            showGames();
        } catch (MyException e) {
            System.out.println("Buy tickets error " + e);
        }
    }

    public void logOut(ActionEvent actionEvent) throws IOException {
        logOut2();
        System.exit(0);
    }

    @Override
    public void updateGame(Game game) {
        System.out.println("Update game in Main controller");
        int i = 0;
        for(Game g: gamesList){
            if(Objects.equals(g.getId(), game.getId())){
                gamesTable.getItems().set(i, game);
            }
            i++;
        }
    }

    void logOut2() {
        try {
            server.logOut(employee, this);
        } catch (MyException e) {
            System.out.println("Logout error " + e);
        }
    }
}
