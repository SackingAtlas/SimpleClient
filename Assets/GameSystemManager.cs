using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSystemManager : MonoBehaviour
{

    GameObject inputFieldUserName, inputFieldPassword, buttonSubmit, toggleLogin, toggleCreate;
    GameObject networkedClient;
    GameObject findGameSessionButton, placeHolderGameButton;
    GameObject nameText, passwordText;
    GameObject button1, button2, button3, button4, button5, button6, button7, button8, button9, gameBoard;

    // Start is called before the first frame update
    void Start()
    {
        GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();

        foreach (GameObject go in allObjects)
        {
            if (go.name == "InputFieldUserName")
                inputFieldUserName = go;
            else if (go.name == "InputFieldPassword")
                inputFieldPassword = go;
            else if (go.name == "SubmitButton")
                buttonSubmit = go;
            else if (go.name == "ToggleLogin")
                toggleLogin = go;
            else if (go.name == "ToggleCreate")
                toggleCreate = go;

            else if (go.name == "NetworkedClient")
                networkedClient = go;

            else if (go.name == "PlaceHolderGameButton")
                placeHolderGameButton = go;
            else if (go.name == "FindGameSessionButton")
                findGameSessionButton = go;

            else if (go.name == "TextPassword")
                passwordText = go;
            else if (go.name == "TextUserName")
                nameText = go;

            else if (go.name == "Button1")
                button1 = go;
            else if (go.name == "Button2")
                button2 = go;
            else if (go.name == "Button3")
                button3 = go;
            else if (go.name == "Button4")
                button4 = go;
            else if (go.name == "Button5")
                button5 = go;
            else if (go.name == "Button6")
                button6 = go;
            else if (go.name == "Button7")
                button7 = go;
            else if (go.name == "Button8")
                button8 = go;            
            else if (go.name == "Button9")
                button9 = go;
            else if (go.name == "GameBoard")
                gameBoard = go;
        }
        buttonSubmit.GetComponent<Button>().onClick.AddListener(SubmitButtonPressed);
        toggleCreate.GetComponent<Toggle>().onValueChanged.AddListener(ToggleCreateValueChanged);
        toggleLogin.GetComponent<Toggle>().onValueChanged.AddListener(ToggleLoginValueChanged);

        findGameSessionButton.GetComponent<Button>().onClick.AddListener(FindGameSessionButtonPressed);
        placeHolderGameButton.GetComponent<Button>().onClick.AddListener(PlaceHolderGameButtonnPressed);
        //button1.GetComponent<Button>().onClick.AddListener(OXSelectionButtonnPressed(button1));
        //button2.GetComponent<Button>().onClick.AddListener(OXSelectionButtonnPressed(button2));
        //button3.GetComponent<Button>().onClick.AddListener(OXSelectionButtonnPressed(button3));
        //button4.GetComponent<Button>().onClick.AddListener(OXSelectionButtonnPressed(button4));
        //button5.GetComponent<Button>().onClick.AddListener(OXSelectionButtonnPressed(button5));
        //button6.GetComponent<Button>().onClick.AddListener(OXSelectionButtonnPressed(button6));
        //button7.GetComponent<Button>().onClick.AddListener(OXSelectionButtonnPressed(button7));
        //button8.GetComponent<Button>().onClick.AddListener(OXSelectionButtonnPressed(button8));
        //button9.GetComponent<Button>().onClick.AddListener(OXSelectionButtonnPressed(button9));

        ChangeGameState(GameStates.Login);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
            ChangeGameState(GameStates.Login);
        if (Input.GetKeyDown(KeyCode.S))
            ChangeGameState(GameStates.MainMenu);
        if (Input.GetKeyDown(KeyCode.D))
            ChangeGameState(GameStates.WaitingForMatch);
        if (Input.GetKeyDown(KeyCode.F))
            ChangeGameState(GameStates.PlayingTicTacToe);
    }

    private void SubmitButtonPressed()
    {
        string n = inputFieldUserName.GetComponent<InputField>().text;
        string p = inputFieldPassword.GetComponent<InputField>().text;
        Debug.Log(ClientToServerSignifiers.CreatAccount + "," + n + "," + p);

        if (toggleLogin.GetComponent<Toggle>().isOn)
            networkedClient.GetComponent<NetworkedClient>().SendMessageToHost(ClientToServerSignifiers.login + "," + n + "," + p);
        else
            networkedClient.GetComponent<NetworkedClient>().SendMessageToHost(ClientToServerSignifiers.CreatAccount + "," + n + "," + p);
    }

    private void ToggleCreateValueChanged(bool newValue)
    {
        toggleLogin.GetComponent<Toggle>().SetIsOnWithoutNotify(!newValue);
    }

    private void ToggleLoginValueChanged(bool newValue)
    {
        toggleCreate.GetComponent<Toggle>().SetIsOnWithoutNotify(!newValue);
    }
    private void FindGameSessionButtonPressed()
    {
        networkedClient.GetComponent<NetworkedClient>().SendMessageToHost(ClientToServerSignifiers.AddToGameSessionQueue + ",");
        ChangeGameState(GameStates.WaitingForMatch);
    }
    private void PlaceHolderGameButtonnPressed()
    {
        networkedClient.GetComponent<NetworkedClient>().SendMessageToHost(ClientToServerSignifiers.TicTacToePlay + ",");
    }

    //private void OXSelectionButtonnPressed(GameObject button)
    //{
    //    Debug.Log("gfdsg");
    //}

    public void ChangeGameState(int newState)
    {
        inputFieldUserName.SetActive(false);
        inputFieldPassword.SetActive(false);
        buttonSubmit.SetActive(false);
        toggleLogin.SetActive(false);
        toggleCreate.SetActive(false);
        findGameSessionButton.SetActive(false);
        placeHolderGameButton.SetActive(false);
        passwordText.SetActive(false);
        nameText.SetActive(false);
        button1.SetActive(false);
        button2.SetActive(false);
        button3.SetActive(false);
        button4.SetActive(false);
        button5.SetActive(false);
        button6.SetActive(false);
        button7.SetActive(false);
        button8.SetActive(false);
        button9.SetActive(false);
        gameBoard.SetActive(false);

        if (newState == GameStates.Login)
        {
            inputFieldUserName.SetActive(true);
            inputFieldPassword.SetActive(true);
            buttonSubmit.SetActive(true);
            toggleLogin.SetActive(true);
            toggleCreate.SetActive(true);
            passwordText.SetActive(true);
            nameText.SetActive(true);
        }
        else if (newState == GameStates.MainMenu)
        {
            findGameSessionButton.SetActive(true);
        }
        else if (newState == GameStates.WaitingForMatch)
        {
            //nothing showing, just waiting
        }
        else if (newState == GameStates.PlayingTicTacToe)
        {
            button1.SetActive(true);
            button2.SetActive(true);
            button3.SetActive(true);
            button4.SetActive(true);
            button5.SetActive(true);
            button6.SetActive(true);
            button7.SetActive(true);
            button8.SetActive(true);
            button9.SetActive(true);
            gameBoard.SetActive(true);
            placeHolderGameButton.SetActive(true);
        }
    }

}

public static class ClientToServerSignifiers
{
    public const int login = 1;
    public const int CreatAccount = 2;
    public const int AddToGameSessionQueue = 3;
    public const int TicTacToePlay = 4;
}

public static class ServerToClientSignifiers
{
    public const int LoginResponse = 1;
    public const int GameSessionStarted = 2;
    public const int OpponentTicTacToePlay = 3;
}

public static class LoginResponses
{
    public const int Success = 1;
    public const int FailureNameInUse = 2;
    public const int FailureNameNotFound = 3;
    public const int FailureIncorrectPassword = 4;
}

public static class GameStates
{
    public const int Login = 1;
    public const int MainMenu = 2;
    public const int WaitingForMatch = 3;
    public const int PlayingTicTacToe = 4;
}