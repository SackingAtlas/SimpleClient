using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;

public class GameSystemManager : MonoBehaviour
{

    GameObject inputFieldUserName, inputFieldPassword, buttonSubmit, toggleLogin, toggleCreate;
    GameObject networkedClient;
    GameObject findGameSessionButton, replayButton, watchGameButton;
    GameObject nameText, passwordText;
    GameObject inputFieldCommunication, messageDisplayText, wellPlayedButton, goodGameButton, insultButton, messageDisplay;
    GameObject button1, button2, button3, button4, button5, button6, button7, button8, button9, gameBoard, buttonBlocker;

    public string currentPlayerMarker = "O";
    public int lastPlay;
    public int turnInOrder;
    private float timer = 0;
    int[] CellButtonsPosition = new int[9];
    GameObject[] CellButtons = new GameObject[9];
    string[] CellMarkers = new string[9];
    int indexCounter = 0;
    int indexer = 0;
    bool playBack = false;



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

            else if (go.name == "ReplayButton")
                replayButton = go;
            else if (go.name == "FindGameSessionButton")
                findGameSessionButton = go;           
            else if (go.name == "WatchGameButton")
                watchGameButton = go;

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
            else if (go.name == "ButtonBlocker")
                buttonBlocker = go;

            else if (go.name == "InputFieldCommunication")
                inputFieldCommunication = go;
            else if (go.name == "MessageDisplayText")
                messageDisplayText = go;
            else if (go.name == "WellPlayedButton")
                wellPlayedButton = go;
            else if (go.name == "GoodGameButton")
                goodGameButton = go;
            else if (go.name == "InsultButton")
                insultButton = go;          
            else if (go.name == "MessageDisplay")
                messageDisplay = go;
        }
        buttonSubmit.GetComponent<Button>().onClick.AddListener(SubmitButtonPressed);
        toggleCreate.GetComponent<Toggle>().onValueChanged.AddListener(ToggleCreateValueChanged);
        toggleLogin.GetComponent<Toggle>().onValueChanged.AddListener(ToggleLoginValueChanged);

        findGameSessionButton.GetComponent<Button>().onClick.AddListener(FindGameSessionButtonPressed);
        replayButton.GetComponent<Button>().onClick.AddListener(ReplayButtonnPressed);
        watchGameButton.GetComponent<Button>().onClick.AddListener(watchGameButtonPressed);

        button1.GetComponent<Button>().onClick.AddListener(Button1Pressed);
        button2.GetComponent<Button>().onClick.AddListener(Button2Pressed);
        button3.GetComponent<Button>().onClick.AddListener(Button3Pressed);
        button4.GetComponent<Button>().onClick.AddListener(Button4Pressed);
        button5.GetComponent<Button>().onClick.AddListener(Button5Pressed);
        button6.GetComponent<Button>().onClick.AddListener(Button6Pressed);
        button7.GetComponent<Button>().onClick.AddListener(Button7Pressed);
        button8.GetComponent<Button>().onClick.AddListener(Button8Pressed);
        button9.GetComponent<Button>().onClick.AddListener(Button9Pressed);

        wellPlayedButton.GetComponent<Button>().onClick.AddListener(WellPlayedButttonPressed);
        goodGameButton.GetComponent<Button>().onClick.AddListener(GoodGameButttonPressed);
        insultButton.GetComponent<Button>().onClick.AddListener(InsultButttonPressed);


        ChangeGameState(GameStates.Login);
    }

    void Update()
    {
        if(timer >= 0)
        timer -= Time.deltaTime;
        else
        {
            if(playBack == true)
            {
                if (indexCounter != 0)
                {
                    //PlaceMarker(CellButtons[CellButtonsPosition[indexer]]);
                    Debug.Log("counter2");
                    GetOpponentsPlay(CellButtonsPosition[indexer]);
                   ++indexer;
                    --indexCounter;
                    timer = 1;
                }
                else
                {
                    playBack = false;
                }
            }
            messageDisplayText.GetComponentInChildren<Text>().text = " ";
        }
    }
    private void InsultButttonPressed()
    {
        int insult = 1;
        networkedClient.GetComponent<NetworkedClient>().SendMessageToHost(ClientToServerSignifiers.CommunicationPassing + "," + insult);
    }
    private void GoodGameButttonPressed()
    {
        int gg = 2;
        networkedClient.GetComponent<NetworkedClient>().SendMessageToHost(ClientToServerSignifiers.CommunicationPassing + "," + gg);
    }
    private void WellPlayedButttonPressed()
    {
        int wp = 3;
        networkedClient.GetComponent<NetworkedClient>().SendMessageToHost(ClientToServerSignifiers.CommunicationPassing + "," + wp);
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

    private void watchGameButtonPressed()
    {
        networkedClient.GetComponent<NetworkedClient>().SendMessageToHost(ClientToServerSignifiers.AddOberverToSession + ",");
        ChangeGameState(GameStates.WatchingTicTacToe);
    }
    private void ReplayButtonnPressed()
    {
        networkedClient.GetComponent<NetworkedClient>().SendMessageToHost(ClientToServerSignifiers.ReplayRequest + ",");
        currentPlayerMarker = "O"; 
        GameObject[] CellButtons = { button1, button2, button3, button4, button5, button6, button7, button8, button9 };
        foreach (GameObject marker in CellButtons)
        {
            marker.GetComponentInChildren<Text>().text = "";
        }
    }
    public void ObserverCatchUp(int Cell)
    {
        GameObject[] CellButtons = { button1, button2, button3, button4, button5, button6, button7, button8, button9 };
        PlaceMarker(CellButtons[Cell - 1]);
    }
    public void ReplayGameMoves(int Cell) // the number of location
    {
        CellButtonsPosition[indexCounter] = Cell;
        ++indexCounter;
        timer = 1;
        playBack = true;
    }

    public void DisplayMessage(int MessagePassed)
    {
        switch (MessagePassed)
        {
            case 1:
                Debug.Log("afdsssssssssssssssssssssffDSAFDSAFDSAFDSAFDSAF");
                messageDisplayText.GetComponentInChildren<Text>().text = "Your mother was a hamster and your father smelt of elderberries";
                break;
            case 2:
                //good game
                messageDisplayText.GetComponentInChildren<Text>().text = "Good Game!";
                break;
            case 3:
                // well played
                messageDisplayText.GetComponentInChildren<Text>().text = "Well Played";
                break;
        }
        timer = 5;
    }
    //repetitive, condense
    private void Button1Pressed()
    {
        lastPlay = 1;
        networkedClient.GetComponent<NetworkedClient>().SendMessageToHost(ClientToServerSignifiers.TicTacToePlay + "," + lastPlay);
        PlaceMarker(button1);
    }
    private void Button2Pressed()
    {
        lastPlay = 2;
        networkedClient.GetComponent<NetworkedClient>().SendMessageToHost(ClientToServerSignifiers.TicTacToePlay + "," + lastPlay);
        PlaceMarker(button2);
    }
    private void Button3Pressed()
    {
        lastPlay = 3;
        networkedClient.GetComponent<NetworkedClient>().SendMessageToHost(ClientToServerSignifiers.TicTacToePlay + "," + lastPlay);
        PlaceMarker(button3);
    }
    private void Button4Pressed()
    {
        lastPlay = 4;
        networkedClient.GetComponent<NetworkedClient>().SendMessageToHost(ClientToServerSignifiers.TicTacToePlay + "," + lastPlay);
        PlaceMarker(button4);
    }
    private void Button5Pressed()
    {
        lastPlay = 5;
        networkedClient.GetComponent<NetworkedClient>().SendMessageToHost(ClientToServerSignifiers.TicTacToePlay + "," + lastPlay);
        PlaceMarker(button5);
    }
    private void Button6Pressed()
    {
        lastPlay = 6;
        networkedClient.GetComponent<NetworkedClient>().SendMessageToHost(ClientToServerSignifiers.TicTacToePlay + "," + lastPlay);
        PlaceMarker(button6);
    }
    private void Button7Pressed()
    {
        lastPlay = 7;
        networkedClient.GetComponent<NetworkedClient>().SendMessageToHost(ClientToServerSignifiers.TicTacToePlay + "," + lastPlay);
        PlaceMarker(button7);
    }
    private void Button8Pressed()
    {
        lastPlay = 8;
        networkedClient.GetComponent<NetworkedClient>().SendMessageToHost(ClientToServerSignifiers.TicTacToePlay + "," + lastPlay);
        PlaceMarker(button8);
    }
    private void Button9Pressed()
    {
        lastPlay = 9;
        networkedClient.GetComponent<NetworkedClient>().SendMessageToHost(ClientToServerSignifiers.TicTacToePlay + "," + lastPlay);
        PlaceMarker(button9);
    }

    public void GetOpponentsPlay(int playedCell)
    {
        lastPlay = playedCell;
        switch (playedCell)
        {
            case 1:
                PlaceMarker(button1);
                break;
            case 2:
                PlaceMarker(button2);
                break;
            case 3:
                PlaceMarker(button3);
                break;
            case 4:
                PlaceMarker(button4);
                break;
            case 5:
                PlaceMarker(button5);
                break;
            case 6:
                PlaceMarker(button6);
                break;
            case 7:
                PlaceMarker(button7);
                break;
            case 8:
                PlaceMarker(button8);
                break;
            case 9:
                PlaceMarker(button9);
                break;
        }
    }
    private void PlaceMarker(GameObject buttonPressed)
    {
        if (currentPlayerMarker == "X")
            currentPlayerMarker = "O";
        else
            currentPlayerMarker = "X";


        Text cellMarking = buttonPressed.GetComponentInChildren<Text>(); 
        Button button = buttonPressed.GetComponent<Button>();
        cellMarking.text = currentPlayerMarker;
        //CellButtons[indexCounter] = buttonPressed;
        //CellMarkers[indexCounter] = currentPlayerMarker;
        //++indexCounter;
        button.interactable = false;
        CheckWinCondition();
    }

    private void CheckWinCondition()
    {
        if (button1.GetComponentInChildren<Text>().text != "" && button1.GetComponentInChildren<Text>().text == button2.GetComponentInChildren<Text>().text && button1.GetComponentInChildren<Text>().text == button3.GetComponentInChildren<Text>().text)
            GameOver(true);
        else if(button4.GetComponentInChildren<Text>().text != "" && button4.GetComponentInChildren<Text>().text == button5.GetComponentInChildren<Text>().text && button4.GetComponentInChildren<Text>().text == button6.GetComponentInChildren<Text>().text)
            GameOver(true);
        else if(button7.GetComponentInChildren<Text>().text != "" && button7.GetComponentInChildren<Text>().text == button8.GetComponentInChildren<Text>().text && button7.GetComponentInChildren<Text>().text == button9.GetComponentInChildren<Text>().text)
            GameOver(true);
        else if(button1.GetComponentInChildren<Text>().text != "" && button1.GetComponentInChildren<Text>().text == button4.GetComponentInChildren<Text>().text && button1.GetComponentInChildren<Text>().text == button7.GetComponentInChildren<Text>().text)
            GameOver(true);
        else if(button2.GetComponentInChildren<Text>().text != "" && button2.GetComponentInChildren<Text>().text == button5.GetComponentInChildren<Text>().text && button2.GetComponentInChildren<Text>().text == button8.GetComponentInChildren<Text>().text)
            GameOver(true);
        else if(button3.GetComponentInChildren<Text>().text != "" && button3.GetComponentInChildren<Text>().text == button6.GetComponentInChildren<Text>().text && button3.GetComponentInChildren<Text>().text == button9.GetComponentInChildren<Text>().text)
            GameOver(true);
        else if(button1.GetComponentInChildren<Text>().text != "" && button1.GetComponentInChildren<Text>().text == button5.GetComponentInChildren<Text>().text && button1.GetComponentInChildren<Text>().text == button9.GetComponentInChildren<Text>().text)
            GameOver(true);
        else if(button3.GetComponentInChildren<Text>().text != "" && button3.GetComponentInChildren<Text>().text == button5.GetComponentInChildren<Text>().text && button3.GetComponentInChildren<Text>().text == button7.GetComponentInChildren<Text>().text)
            GameOver(true);
        else
            GameOver(false);
    }
    private void GameOver(bool gg)
    {
        if(gg == true)
        {
            buttonBlocker.SetActive(true);
            if(turnInOrder == 1 || turnInOrder == 2)
            replayButton.SetActive(true);
        }
        else
            SwitchTurns();
    }

    public void ChangeGameState(int newState)
    {
        inputFieldUserName.SetActive(false);
        inputFieldPassword.SetActive(false);
        buttonSubmit.SetActive(false);
        toggleLogin.SetActive(false);
        toggleCreate.SetActive(false);
        findGameSessionButton.SetActive(false);
        replayButton.SetActive(false);
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
        buttonBlocker.SetActive(false);
        watchGameButton.SetActive(false);

        inputFieldCommunication.SetActive(false);
        messageDisplayText.SetActive(false);
        wellPlayedButton.SetActive(false);
        goodGameButton.SetActive(false);
        insultButton.SetActive(false);
        messageDisplay.SetActive(false);

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
            watchGameButton.SetActive(true);
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
            inputFieldCommunication.SetActive(true);
            messageDisplayText.SetActive(true);
            wellPlayedButton.SetActive(true);
            goodGameButton.SetActive(true);
            insultButton.SetActive(true);
            messageDisplay.SetActive(true);

            SwitchTurns();
        }
        else if (newState == GameStates.WatchingTicTacToe)
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
            buttonBlocker.SetActive(true);
        }
    }

    private void SwitchTurns()
    {
        if (turnInOrder == 2)
        {
            buttonBlocker.SetActive(true);
            turnInOrder = 1;
        }
        else if (turnInOrder == 1)
        {
            buttonBlocker.SetActive(false);
            turnInOrder = 2;
        }
    }
}


public static class ClientToServerSignifiers
{
    public const int login = 1;
    public const int CreatAccount = 2;
    public const int AddToGameSessionQueue = 3;
    public const int TicTacToePlay = 4;
    public const int AddOberverToSession = 5;
    public const int ReplayRequest = 6;
    public const int CommunicationPassing = 7;
}

public static class ServerToClientSignifiers
{
    public const int LoginResponse = 1;
    public const int GameSessionStarted = 2;
    public const int OpponentTicTacToePlay = 3;
    public const int ObserverEntered = 4;
    public const int ObserverCatchUp = 5;
    public const int Replay = 6;
    public const int PassedCommunication = 7;
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
    public const int WatchingTicTacToe = 5;
}

public static class SquarePlayedIn
{
    public const int TopLeft = 1;
    public const int TopCenter = 2;
    public const int TopRight = 3;
    public const int MiddleLeft = 4;
    public const int MiddleCenter = 5;
    public const int MiddleRight = 6;
    public const int BottomLeft = 7;
    public const int BottomCenter = 8;
    public const int BottomRight = 9;
}