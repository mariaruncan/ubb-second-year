package app.client.controller;

import app.model.Employee;
import app.services.IServices;
import app.services.MyException;
import javafx.event.ActionEvent;
import javafx.event.EventHandler;
import javafx.fxml.FXML;
import javafx.scene.Node;
import javafx.scene.Parent;
import javafx.scene.Scene;
import javafx.scene.control.Alert;
import javafx.scene.control.PasswordField;
import javafx.scene.control.TextField;
import javafx.stage.Stage;
import javafx.stage.WindowEvent;

import java.io.IOException;

public class LogInController {
    private IServices server;
    private MainController mainController;
    private Employee crtEmployee;

    @FXML public TextField textFieldUsername;
    @FXML public PasswordField textFieldPassword;


    private Parent parent;

    public void setServer(IServices srv){
        server = srv;
    }

    public void setParent(Parent parent){
        this.parent = parent;
    }

    public void logIn(ActionEvent actionEvent) throws IOException {
        String username = textFieldUsername.getText();
        String password = textFieldPassword.getText();
        crtEmployee = new Employee(username, password);

        try{
            server.logIn(username, password, mainController);
            Stage stage = new Stage();
            stage.setTitle("Main window for " + crtEmployee.getUsername());
            stage.setScene(new Scene(parent));

            stage.setOnCloseRequest(new EventHandler<WindowEvent>() {
                @Override
                public void handle(WindowEvent event) {
                    mainController.logOut2();
                    System.exit(0);
                }
            });

            stage.show();
            mainController.setEmployee(crtEmployee);
            ((Node)(actionEvent.getSource())).getScene().getWindow().hide();
            mainController.initialize1();
        }
        catch(MyException e){
            Alert alert = new Alert(Alert.AlertType.INFORMATION);
            alert.setTitle("Error");
            alert.setContentText("Wrong username or password!");
            alert.showAndWait();
        }
    }

    public void pressCancel(ActionEvent actionEvent) {
        System.exit(0);
    }

    public void setUser(Employee employee) {
        this.crtEmployee = employee;
    }

    public void setMainController(MainController mainController) {
        this.mainController = mainController;
    }
}