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

    public string currentPlayerMarker = "O";
    string b1, b2, b3, b4, b5, b6, b7, b8, b9 = " ";
    //LinkedList<MovesMade> movesMade;

    // Start is called before the first frame update
    void Start()
    {
        //movesMade = new LinkedList<MovesMade>();

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
        button1.GetComponent<Button>().onClick.AddListener(Button1Pressed);
        button2.GetComponent<Button>().onClick.AddListener(Button2Pressed);
        button3.GetComponent<Button>().onClick.AddListener(Button3Pressed);
        button4.GetComponent<Button>().onClick.AddListener(Button4Pressed);
        button5.GetComponent<Button>().onClick.AddListener(Button5Pressed);
        button6.GetComponent<Button>().onClick.AddListener(Button6Pressed);
        button7.GetComponent<Button>().onClick.AddListener(Button7Pressed);
        button8.GetComponent<Button>().onClick.AddListener(Button8Pressed);
        button9.GetComponent<Button>().onClick.AddListener(Button9Pressed);


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

    private void Button1Pressed()
    {
        PlaceMarker(button1);
    }
    private void Button2Pressed()
    {
        PlaceMarker(button2);
    }
    private void Button3Pressed()
    {
        PlaceMarker(button3);
    }
    private void Button4Pressed()
    {
        PlaceMarker(button4);
    }
    private void Button5Pressed()
    {
        PlaceMarker(button5);
    }
    private void Button6Pressed()
    {
        PlaceMarker(button6);
    }
    private void Button7Pressed()
    {
        PlaceMarker(button7);
    }
    private void Button8Pressed()
    {
        PlaceMarker(button8);
    }
    private void Button9Pressed()
    {
        PlaceMarker(button9);
    }

    private void PlaceMarker(GameObject buttonPressed)
    {
        ChangeMarker(currentPlayerMarker);
        Text cellMarking = buttonPressed.GetComponentInChildren<Text>();
        Button button = buttonPressed.GetComponent<Button>();
        cellMarking.text = currentPlayerMarker;

        button.interactable = false;
        CheckWinCondition(buttonPressed, currentPlayerMarker);
    }
    private void ChangeMarker(string marker)
    {
       if(marker == "X")
            currentPlayerMarker = "O";
       else
            currentPlayerMarker = "X";
    }
    private void CheckWinCondition(GameObject buttonPressed, string marker)
    {



        Debug.Log(button1.GetComponentInChildren<Text>());

        if (button1.GetComponentInChildren<Text>() == button2.GetComponentInChildren<Text>() && button1.GetComponentInChildren<Text>() == button3.GetComponentInChildren<Text>())
            Debug.Log("GAME OVER DUDE");
        if (button4.GetComponentInChildren<Text>() == button5.GetComponentInChildren<Text>() && button4.GetComponentInChildren<Text>() == button6.GetComponentInChildren<Text>())
            Debug.Log("GAME OVER DUDE");
        if (button7.GetComponentInChildren<Text>() == button8.GetComponentInChildren<Text>() && button7.GetComponentInChildren<Text>() == button9.GetComponentInChildren<Text>())
            Debug.Log("GAME OVER DUDE");
        if (button1.GetComponentInChildren<Text>() == button4.GetComponentInChildren<Text>() && button1.GetComponentInChildren<Text>() == button7.GetComponentInChildren<Text>())
            Debug.Log("GAME OVER DUDE");
        if (button2.GetComponentInChildren<Text>() == button5.GetComponentInChildren<Text>() && button2.GetComponentInChildren<Text>() == button8.GetComponentInChildren<Text>())
            Debug.Log("GAME OVER DUDE");
        if (button3.GetComponentInChildren<Text>() == button6.GetComponentInChildren<Text>() && button3.GetComponentInChildren<Text>() == button9.GetComponentInChildren<Text>())
            Debug.Log("GAME OVER DUDE");
        if (button1.GetComponentInChildren<Text>() == button5.GetComponentInChildren<Text>() && button1.GetComponentInChildren<Text>() == button9.GetComponentInChildren<Text>())
            Debug.Log("GAME OVER DUDE");
        if (button3.GetComponentInChildren<Text>() == button5.GetComponentInChildren<Text>() && button3.GetComponentInChildren<Text>() == button7.GetComponentInChildren<Text>())
            Debug.Log("GAME OVER DUDE");
    }

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
    //private MovesMade PlayBackMoves(int id)
    //{
    //    foreach (MovesMade mm in movesMade)
    //    {
    //        if (mm.playerID1 == id || mm.playerID2 == id)
    //            return mm;
    //    }
    //    return null;
    //}
}

//public class MovesMade
//{
//    public int playerID1, playerID2;

//    public MovesMade(int PlayerID1, int PlayerID2)
//    {
//        playerID1 = PlayerID1;
//        playerID1 = PlayerID1;
//    }
//}

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

