using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkedClient : MonoBehaviour
{

    int connectionID;
    int maxConnections = 1000;
    int reliableChannelID;
    int unreliableChannelID;
    int hostID;
    int socketPort = 5491;
    byte error;
    bool isConnected = false;
    int ourClientID;

    GameObject gameSystemManager;

    // Start is called before the first frame update
    void Start()
    {
        GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();

        foreach (GameObject go in allObjects)
        {
            if (go.name == "GameManager")
                gameSystemManager = go;
        }
        Connect();
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.S))
        //    SendMessageToHost("Hello from client");

        UpdateNetworkConnection();
    }

    private void UpdateNetworkConnection()
    {
        if (isConnected)
        {
            int recHostID;
            int recConnectionID;
            int recChannelID;
            byte[] recBuffer = new byte[1024];
            int bufferSize = 1024;
            int dataSize;
            NetworkEventType recNetworkEvent = NetworkTransport.Receive(out recHostID, out recConnectionID, out recChannelID, recBuffer, bufferSize, out dataSize, out error);

            switch (recNetworkEvent)
            {
                case NetworkEventType.ConnectEvent:
                    Debug.Log("connected.  " + recConnectionID);
                    ourClientID = recConnectionID;
                    break;
                case NetworkEventType.DataEvent:
                    string msg = Encoding.Unicode.GetString(recBuffer, 0, dataSize);
                    ProcessRecievedMsg(msg, recConnectionID);
                    //Debug.Log("got msg = " + msg);
                    break;
                case NetworkEventType.DisconnectEvent:
                    isConnected = false;
                    Debug.Log("disconnected.  " + recConnectionID);
                    break;
            }
        }
    }

    private void Connect()
    {

        if (!isConnected)
        {
            Debug.Log("Attempting to create connection");

            NetworkTransport.Init();

            ConnectionConfig config = new ConnectionConfig();
            reliableChannelID = config.AddChannel(QosType.Reliable);
            unreliableChannelID = config.AddChannel(QosType.Unreliable);
            HostTopology topology = new HostTopology(config, maxConnections);
            hostID = NetworkTransport.AddHost(topology, 0);
            Debug.Log("Socket open.  Host ID = " + hostID);

            connectionID = NetworkTransport.Connect(hostID, "192.168.1.2", socketPort, 0, out error); // server is local on network

            if (error == 0)
            {
                isConnected = true;

                Debug.Log("Connected, id = " + connectionID);

            }
        }
    }

    public void Disconnect()
    {
        NetworkTransport.Disconnect(hostID, connectionID, out error);
    }

    public void SendMessageToHost(string msg)
    {
        byte[] buffer = Encoding.Unicode.GetBytes(msg);
        NetworkTransport.Send(hostID, connectionID, reliableChannelID, buffer, msg.Length * sizeof(char), out error);
    }

    private void ProcessRecievedMsg(string msg, int id)
    {
        Debug.Log("msg recieved = " + msg + ".  connection id = " + id);

        string[] csv = msg.Split(',');
        int signifier = int.Parse(csv[0]);
        if(signifier == ServerToClientSignifiers.LoginResponse)
        {
            int loginResultSignifier = int.Parse(csv[1]);

            if(loginResultSignifier == LoginResponses.Success)
                gameSystemManager.GetComponent<GameSystemManager>().ChangeGameState(GameStates.MainMenu);
        }
        else if (signifier == ServerToClientSignifiers.GameSessionStarted)
        {
            int PlayerTurnSignifier = int.Parse(csv[1]);
            gameSystemManager.GetComponent<GameSystemManager>().turnInOrder = PlayerTurnSignifier;
            gameSystemManager.GetComponent<GameSystemManager>().ChangeGameState(GameStates.PlayingTicTacToe);
        }
        else if (signifier == ServerToClientSignifiers.OpponentTicTacToePlay)
        {
            int OpponantMoveSignifier = int.Parse(csv[1]);
                gameSystemManager.GetComponent<GameSystemManager>().GetOpponentsPlay(OpponantMoveSignifier); 
        }
        else if (signifier == ServerToClientSignifiers.ObserverEntered)
        {
            int playCount = int.Parse(csv[1]);
            //Debug.Log(playCount);
           // gameSystemManager.GetComponent<GameSystemManager>().ObserverCatchUp(playCount);
        }
        else if (signifier == ServerToClientSignifiers.ObserverCatchUp)
        {
            int cellPlayed = int.Parse(csv[1]);
            gameSystemManager.GetComponent<GameSystemManager>().ObserverCatchUp(cellPlayed);
        }
        else if (signifier == ServerToClientSignifiers.Replay)
        {
            int cellPlayed = int.Parse(csv[1]);
            gameSystemManager.GetComponent<GameSystemManager>().ReplayGameMoves(cellPlayed);
        }
        else if (signifier == ServerToClientSignifiers.PassedCommunication)
        {
            int MessagePassed = int.Parse(csv[1]);
            gameSystemManager.GetComponent<GameSystemManager>().DisplayMessage(MessagePassed);
        }
    }

    public bool IsConnected()
    {
        return isConnected;
    }


}