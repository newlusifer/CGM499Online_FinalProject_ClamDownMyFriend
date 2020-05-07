using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(SocketIOComponent))]
public class ConnectionManager : MonoBehaviour
{
    [System.Serializable]
    public class PlayerIDGroup
    {
        public List<string> playerIDList = new List<string>();
    }

    [System.Serializable]
    public class RoomIDGroup
    {
        public List<string> roomIDList = new List<string>();
    }

    public class PlayerData
    {
        public string uid;
        public Player playerObj;
        public Vector3 correctPos;
    }

    [System.Serializable]
    public class PlayerUpdateData
    {
        public float x, y, z;
    }

    public enum ConnectionState
    {
        Disconnected,
        Connected,
        RoleCreate,
        RoleJoin,
        InRoom,
    }

    public ConnectionState connectionState;

    public Player playerObjPref;

    public string ownerID;

    public PlayerIDGroup playerIDGroup;

    public PlayerIDGroup cachePlayerIDGroup;

    public RoomIDGroup roomIDGroup;

    private List<PlayerData> characterList = new List<PlayerData>();

    private PlayerData playerDataOwner;

    private SocketIOComponent socket;

    public string roomName;

    private bool isRoom;

    public GameObject respawnPoint;
    private Vector3 respawnPVec;

    public GameObject connect;
    public GameObject createRoom;
    public GameObject joinRoom;
    public GameObject leaveRoom;
    public GameObject addRoomNameObj;
    public GameObject back;
    public GameObject roomList;
    public GameObject joinRoomName;
    public GameObject backFromJoin;
    public GameObject makeRoom;
    public GameObject joinToRoom;
    public GameObject nameOfRoom;

    public GameObject menu;
    public int statusOfMenu = 1;

    public InputField addRoomName;
    public InputField addJoinName;

    public Text showListTotalRoom;
    public Text nameOfRoomText;

    private string[] arrayRoomName;

    public static string randomMapFromServer = "1";

    public static string statusGame = "";

    public GameObject textPlayerName;
    public GameObject PlayerNameInput;

    public InputField AddPlayerName;

    private string playerName="";

    public GameObject warningText;

    private string CurrentroomName;

    public GameObject showWinnerPanel;
    public Text namePlayerWinner;
    public Text timerOfWinner;
    public Text cdRestartGame;
    private float countDown = 10;

    private int startCD = 0;
    private string randomMapRestartGame = "1";

    public Text LDB1;
    public Text LDB2;
    public Text LDB3;

    public GameObject backToMainMenu;

    /*private void OnGUI()
    {

        switch (connectionState)
        {
            case ConnectionState.Disconnected:
                {
                    if (GUILayout.Button("Connect"))
                    {
                        socket.Connect();
                    }

                    if (socket.IsConnected)
                    {
                        connectionState = ConnectionState.Connected;
                    }

                    break;
                }

            case ConnectionState.Connected:
                {
                    if (GUILayout.Button("CreateRoom"))
                    {
                        connectionState = ConnectionState.RoleCreate;
                    }

                    if (GUILayout.Button("JoinRoom"))
                    {
                        connectionState = ConnectionState.RoleJoin;
                        socket.Emit("OnClientFetchRoomList");
                    }
                    break;
                }

            case ConnectionState.RoleCreate:
                {
                    roomName = GUILayout.TextField(roomName);
                    if (GUILayout.Button("CreateRoom"))
                    {
                        CreateRoom(roomName);
                    }
                    break;
                }

            case ConnectionState.RoleJoin:
                {
                    foreach (var _roomName in roomIDGroup.roomIDList)
                    {
                        if (GUILayout.Button(_roomName))
                        {
                            roomName = _roomName;
                            JoinRoom(_roomName);
                        }
                    }
                    break;
                }

            case ConnectionState.InRoom:
                {
                    GUILayout.TextField(ownerID);
                    if (GUILayout.Button("LeaveRoom"))
                    {
                        LeaveRoom();
                    }
                    break;
                }
        }
    }*/

    // Start is called before the first frame update
    void Start()
    {
        socket = GetComponent<SocketIOComponent>();

        socket.On("OnOwnerClientConnect", OnOwnerClientConnect);
        socket.On("OnClientConnect", OnClientConnect);
        socket.On("OnClientFetchPlayerList", OnClientFetchPlayerList);
        socket.On("OnClientDisconnect", OnClientDisconnect);

        socket.On("OnClientCreateRoomSuccess", OnClientCreateRoomSuccess);
        socket.On("OnClientCreateRoomFail", OnClientCreateRoomFail);
        socket.On("OnOwnerClientJoinRoomSuccess", OnOwnerClientJoinRoomSuccess);
        socket.On("OnClientJoinRoomSuccess", OnClientJoinRoomSuccess);
        socket.On("OnClientJoinRoomFail", OnClientJoinRoomFail);

        socket.On("OnClientLeaveRoom", OnClientLeaveRoom);

        socket.On("OnClientFetchRoomList", OnClientFetchRoomList);

        socket.On("OnClientUpdateMoveList", OnClientUpdateMoveList);

        socket.On("OnStartGame",OnGameStart);

        socket.On("OnWinnerShow",OnWinnerShow);

        socket.On("sendLeaderBoard1",OnLeaderBoard1);
        socket.On("sendLeaderBoard2", OnLeaderBoard2);
        socket.On("sendLeaderBoard3", OnLeaderBoard3);

        cachePlayerIDGroup = new PlayerIDGroup();

        respawnPoint = GameObject.Find("respawnPoint");
        respawnPVec = new Vector3(respawnPoint.transform.position.x, respawnPoint.transform.position.y, respawnPoint.transform.position.z);

        // menu = GameObject.Find("Canvas");
        // menu.SetActive(false);

        /*  connect = GameObject.Find("connect");
          addRoomNameObj = GameObject.Find("addRoomName");
          createRoom = GameObject.Find("createRoom");
          joinRoom = GameObject.Find("joinRoom");
          leaveRoom = GameObject.Find("leaveRoom");
          back = GameObject.Find("backFromCreate");
          roomList = GameObject.Find("roomList");
          joinRoomName = GameObject.Find("joinRoomName");
          backFromJoin = GameObject.Find("backFromJoin");
          makeRoom = GameObject.Find("makeRoom");
          joinToRoom = GameObject.Find("joinToRoom");*/

        warningText.SetActive(false);

        showWinnerPanel.SetActive(false);
        
    }

    // Update is called once per frame
    void Update()
    {
        socket.Emit("wantLeaderBoard1");
        socket.Emit("wantLeaderBoard2");
        socket.Emit("wantLeaderBoard3");

        connectManager();

        DetectPlayerConnect();
        UpdateAllCharacter();       
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (statusOfMenu == 0)
            {
                statusOfMenu = 1;
                menu.SetActive(true);
                backToMainMenu.SetActive(true);
            }

           else if (statusOfMenu == 1)
            {
                statusOfMenu = 0;
                menu.SetActive(false);
                backToMainMenu.SetActive(false);
            }
        }

        if (playerTrigger.statusWinner==1)
        {
            statusGame = "stop";
            playerTrigger.statusWinner = 0;

            Dictionary<string, string> data = new Dictionary<string, string>();
            
            data.Add("playername", playerName);
            data.Add("timeM", timer.minuteShare);
            data.Add("timeS", timer.secondShare);
            data.Add("currentroomname",CurrentroomName);

            JSONObject jsonObj = new JSONObject(data);

            socket.Emit("OnPlayerWinner", jsonObj);

        }

        if (startCD==1)
        {
            countDToRestartGame();
        }

        Debug.Log("Status Winner is " + playerTrigger.statusWinner); 
    }

    void UpdateAllCharacter()
    {
        for (int i = 0; i < characterList.Count; i++)
        {
            if (characterList[i].uid == ownerID)
                continue;

            Vector3 currentPos = characterList[i].playerObj.transform.position;
            currentPos = Vector3.Lerp(currentPos, characterList[i].correctPos, 5.0f * Time.deltaTime);

            characterList[i].playerObj.transform.position = currentPos;
        }
    }

    IEnumerator UpdateOwnerPlayerData()
    {
        while (connectionState == ConnectionState.InRoom)
        {
            if (playerDataOwner != null && playerDataOwner.playerObj != null)
            {
                Dictionary<string, string> data = new Dictionary<string, string>();

                Vector3 playerPos = playerDataOwner.playerObj.transform.position;
                data.Add("roomName", roomName);
                data.Add("uid", ownerID);
                data.Add("x", playerPos.x.ToString());
                data.Add("y", playerPos.y.ToString());
                data.Add("z", playerPos.z.ToString());


                JSONObject jsonObj = new JSONObject(data);

                socket.Emit("OnClientUpdateMove", jsonObj);

                yield return new WaitForSeconds(0.1f);
            }

            yield return null;
        }
    }

    private void DetectPlayerConnect()
    {
        

        if (cachePlayerIDGroup.playerIDList.Count != playerIDGroup.playerIDList.Count)
        {
            Debug.Log("Detect");

            bool checkConnect;
            List<string> firstList;
            List<string> secondList;

            if (playerIDGroup.playerIDList.Count > cachePlayerIDGroup.playerIDList.Count)
            {
                firstList = playerIDGroup.playerIDList;
                secondList = cachePlayerIDGroup.playerIDList;
                checkConnect = true;
            }
            else
            {
                firstList = cachePlayerIDGroup.playerIDList;
                secondList = playerIDGroup.playerIDList;
                checkConnect = false;
            }

            foreach (var fID in firstList)
            {
                bool isFound = false;
                foreach (var sID in secondList)
                {
                    if (fID == sID)
                    {
                        isFound = true;
                        break;
                    }
                }

                if (!isFound)
                {
                    Debug.Log("Found");
                    if (checkConnect)//Check player connect
                    {
                        Debug.Log("checkConnectisTrue");
                        //Debug.Log("Player connected : " + fID);
                        CreateCharacter(fID);
                    }
                    else//Check player disconnect
                    {
                        //Debug.Log("Player disconnected : " + fID);
                        DestroyCharacter(fID);
                    }
                }
            }
        }

        cachePlayerIDGroup.playerIDList = playerIDGroup.playerIDList;
    }

    private void CreateCharacter(string uid)
    {
        PlayerData newPlayerData = new PlayerData();

        newPlayerData.uid = uid;
        newPlayerData.playerObj = Instantiate(playerObjPref, respawnPVec, Quaternion.identity);

        newPlayerData.playerObj.name = "Player : " + uid;

        if (uid == ownerID)
        {
            newPlayerData.playerObj.canControl = true;
            playerDataOwner = newPlayerData;
        }

        characterList.Add(newPlayerData);
    }

    private void DestroyCharacter(string uid)
    {
        for (int i = 0; i < characterList.Count; i++)
        {
            if (characterList[i].uid == uid)
            {
                Destroy(characterList[i].playerObj.gameObject);
                characterList.RemoveRange(i, 1);
                break;
            }
        }
    }

    public void CreateRoom(string newRoomName)
    {
        Dictionary<string, string> data = new Dictionary<string, string>();
        data.Add("roomName", newRoomName);
        JSONObject jsonObj = new JSONObject(data);

        socket.Emit("OnClientCreateRoom", jsonObj);
    }

    public void JoinRoom(string newRoomName)
    {
        Dictionary<string, string> data = new Dictionary<string, string>();
        data.Add("roomName", newRoomName);
        JSONObject jsonObj = new JSONObject(data);

        socket.Emit("OnClientJoinRoom", jsonObj);
    }

    public void LeaveRoom()
    {
        connectionState = ConnectionState.Connected;
        roomName = "";
        socket.Emit("OnClientLeaveRoom");
    }

    private void FetchPlayerList()
    {
        Dictionary<string, string> data = new Dictionary<string, string>();
        data.Add("roomName", roomName);
        JSONObject jsonObj = new JSONObject(data);
        socket.Emit("OnClientFetchPlayerList", jsonObj);
    }

    private void connectManager()
    {
        switch (connectionState)
        {
            case ConnectionState.Disconnected:
                {
                    if (statusOfMenu == 1)
                    {
                        connect.SetActive(true);
                        addRoomNameObj.SetActive(false);
                        createRoom.SetActive(false);
                        joinRoom.SetActive(false);
                        leaveRoom.SetActive(false);
                        back.SetActive(false);
                        roomList.SetActive(false);
                        joinRoomName.SetActive(false);
                        backFromJoin.SetActive(false);
                        makeRoom.SetActive(false);
                        joinToRoom.SetActive(false);
                        nameOfRoom.SetActive(false);
                        PlayerNameInput.SetActive(false);
                        textPlayerName.SetActive(false);
                    }

                    else {
                        //still 
                        connect.SetActive(false);
                        addRoomNameObj.SetActive(false);
                        createRoom.SetActive(false);
                        joinRoom.SetActive(false);
                        leaveRoom.SetActive(false);
                        back.SetActive(false);
                        roomList.SetActive(false);
                        joinRoomName.SetActive(false);
                        backFromJoin.SetActive(false);
                        makeRoom.SetActive(false);
                        joinToRoom.SetActive(false);
                        nameOfRoom.SetActive(false);
                        PlayerNameInput.SetActive(false);
                        textPlayerName.SetActive(false);
                    }

                    if (socket.IsConnected)
                    {
                        connectionState = ConnectionState.Connected;
                    }

                    break;
                }

            case ConnectionState.Connected:
                {
                    if (statusOfMenu == 1)
                    {
                        connect.SetActive(false);
                        addRoomNameObj.SetActive(false);
                        createRoom.SetActive(true);
                        joinRoom.SetActive(true);
                        leaveRoom.SetActive(false);
                        back.SetActive(false);
                        roomList.SetActive(false);
                        joinRoomName.SetActive(false);
                        backFromJoin.SetActive(false);
                        makeRoom.SetActive(false);
                        joinToRoom.SetActive(false);
                        nameOfRoom.SetActive(false);
                        PlayerNameInput.SetActive(true);
                        textPlayerName.SetActive(true);
                        AddPlayerName.interactable=true;
                    }

                    else {
                        //still
                        connect.SetActive(false);
                        addRoomNameObj.SetActive(false);
                        createRoom.SetActive(false);
                        joinRoom.SetActive(false);
                        leaveRoom.SetActive(false);
                        back.SetActive(false);
                        roomList.SetActive(false);
                        joinRoomName.SetActive(false);
                        backFromJoin.SetActive(false);
                        makeRoom.SetActive(false);
                        joinToRoom.SetActive(false);
                        nameOfRoom.SetActive(false);
                        PlayerNameInput.SetActive(false);
                        textPlayerName.SetActive(false);
                    }

                    break;
                }

            case ConnectionState.RoleCreate:
                {
                    //Debug.Log("create");

                    if (statusOfMenu == 1)
                    {
                        connect.SetActive(false);
                        addRoomNameObj.SetActive(true);
                        createRoom.SetActive(false);
                        joinRoom.SetActive(false);
                        leaveRoom.SetActive(false);
                        back.SetActive(true);
                        roomList.SetActive(false);
                        joinRoomName.SetActive(false);
                        backFromJoin.SetActive(false);
                        makeRoom.SetActive(true);
                        joinToRoom.SetActive(false);
                        nameOfRoom.SetActive(false);

                        PlayerNameInput.SetActive(true);
                        textPlayerName.SetActive(true);
                        AddPlayerName.interactable = false;

                        AddPlayerName.text = playerName;
                    }

                    else {
                        //still
                        connect.SetActive(false);
                        addRoomNameObj.SetActive(false);
                        createRoom.SetActive(false);
                        joinRoom.SetActive(false);
                        leaveRoom.SetActive(false);
                        back.SetActive(false);
                        roomList.SetActive(false);
                        joinRoomName.SetActive(false);
                        backFromJoin.SetActive(false);
                        makeRoom.SetActive(false);
                        joinToRoom.SetActive(false);
                        nameOfRoom.SetActive(false);
                        PlayerNameInput.SetActive(false);
                        textPlayerName.SetActive(false);
                    }                   

                    break;
                }

            case ConnectionState.RoleJoin:
                {
                    if (statusOfMenu == 1)
                    {
                        connect.SetActive(false);
                        addRoomNameObj.SetActive(false);
                        createRoom.SetActive(false);
                        joinRoom.SetActive(false);
                        leaveRoom.SetActive(false);
                        back.SetActive(false);
                        roomList.SetActive(true);
                        joinRoomName.SetActive(true);
                        backFromJoin.SetActive(true);
                        makeRoom.SetActive(false);
                        joinToRoom.SetActive(true);
                        nameOfRoom.SetActive(false);

                        PlayerNameInput.SetActive(true);
                        textPlayerName.SetActive(true);
                        AddPlayerName.interactable = false;

                        AddPlayerName.text = playerName;
                    }

                    else {
                        //still
                        connect.SetActive(false);
                        addRoomNameObj.SetActive(false);
                        createRoom.SetActive(false);
                        joinRoom.SetActive(false);
                        leaveRoom.SetActive(false);
                        back.SetActive(false);
                        roomList.SetActive(false);
                        joinRoomName.SetActive(false);
                        backFromJoin.SetActive(false);
                        makeRoom.SetActive(false);
                        joinToRoom.SetActive(false);
                        nameOfRoom.SetActive(false);
                        PlayerNameInput.SetActive(false);
                        textPlayerName.SetActive(false);
                    }

                    int sizeOfList = roomIDGroup.roomIDList.Count;
                    showListTotalRoom.text = "";

                    for (int i=0;i<sizeOfList;i++)
                    {
                        showListTotalRoom.text += roomIDGroup.roomIDList[i] + "\n";
                    }

                    break;
                }

            case ConnectionState.InRoom:
                {
                    if (statusOfMenu == 1)
                    {
                        connect.SetActive(false);
                        addRoomNameObj.SetActive(false);
                        createRoom.SetActive(false);
                        joinRoom.SetActive(false);
                        leaveRoom.SetActive(true);
                        back.SetActive(false);
                        roomList.SetActive(false);
                        joinRoomName.SetActive(false);
                        backFromJoin.SetActive(false);
                        makeRoom.SetActive(false);
                        joinToRoom.SetActive(false);
                        nameOfRoom.SetActive(true);

                        PlayerNameInput.SetActive(true);
                        textPlayerName.SetActive(true);
                        AddPlayerName.interactable = false;

                        AddPlayerName.text = playerName;
                    }

                    else {
                        connect.SetActive(false);
                        addRoomNameObj.SetActive(false);
                        createRoom.SetActive(false);
                        joinRoom.SetActive(false);
                        leaveRoom.SetActive(false);
                        back.SetActive(false);
                        roomList.SetActive(false);
                        joinRoomName.SetActive(false);
                        backFromJoin.SetActive(false);
                        makeRoom.SetActive(false);
                        joinToRoom.SetActive(false);
                        nameOfRoom.SetActive(false);
                        PlayerNameInput.SetActive(false);
                        textPlayerName.SetActive(false);
                    }


                    // nameOfRoomText.text = ownerID;
                    nameOfRoomText.text ="Room Name : "+ CurrentroomName;

                    break;
                }
        }
    }//end void

    public void clickConnect()
    {
        socket.Connect();
    }

    public void clickCreateRoom()
    {
        if (AddPlayerName.text=="")
        {
            //still
            StartCoroutine(waitForHideWarning());
        }

        if (AddPlayerName.text != "")
        {
            connectionState = ConnectionState.RoleCreate;
            playerName = AddPlayerName.text;
        }
       
    }

    public void clickJoinRoom()
    {
        if (AddPlayerName.text == "")
        {
            //still
            StartCoroutine(waitForHideWarning());
        }

        if (AddPlayerName.text != "")
        {
            connectionState = ConnectionState.RoleJoin;
            socket.Emit("OnClientFetchRoomList");

            playerName = AddPlayerName.text;
        }
            
    }

    public void clickToCreateRoom()
    {
        roomName = addRoomName.text;
        CreateRoom(addRoomName.text);
        CurrentroomName = addRoomName.text;
    }

    public void clickToBackFromCreateRoom()
    {
        connectionState = ConnectionState.Connected;
    }

    public void clickToJoinToRoom()
    {
        roomName = addJoinName.text;
        JoinRoom(addJoinName.text);
        CurrentroomName = addJoinName.text;
    }

    public void clickToBackFromJoinToRoom()
    {
        connectionState = ConnectionState.Connected;
    }

    public void clickToLeaveRoom()
    {
        CurrentroomName = "";
        LeaveRoom();
       
    }

    void OnGameStart(SocketIOEvent evt)
    {
        Debug.Log("StartGame!!! : " + evt.data.ToString());
        // Debug.Log("StartGame!!! : " + evt.data["startgame"].ToString());

        statusGame = evt.data["startgame"].str;
               
       randomMapFromServer = evt.data["randomMap"].ToString();
           
       Debug.Log("updateStatus");
        
    }

    IEnumerator waitForHideWarning()
    {
        warningText.SetActive(true);
        yield return new WaitForSeconds(2f);
        warningText.SetActive(false);
    }

    void OnWinnerShow(SocketIOEvent evt)
    {
        showWinnerPanel.SetActive(true);
        namePlayerWinner.text = evt.data["playerName"].str;
        timerOfWinner.text="With time "+evt.data["timeM"].str+"."+evt.data["timeS"].str + " Minute";
        randomMapRestartGame = evt.data["randomMap"].ToString();
        statusGame = "restart";
        startCD = 1;
        Debug.Log("New map game is " + evt.data["randomMap"].ToString());
    }

    void countDToRestartGame() { 


        cdRestartGame.text = "New game will start in " +Mathf.RoundToInt(countDown) + " Second";
        countDown -= Time.deltaTime;

        if (countDown<=0)
        {
            randomMapFromServer = randomMapRestartGame;
            playerTrigger.resetPosition = 1;
            countDown = 10;
            showWinnerPanel.SetActive(false);
            startCD = 0;            
            statusGame = "start";
           // Debug.Log("New map game is "+randomMapFromServer);
        }
    }

    void OnLeaderBoard1(SocketIOEvent evt)
    {
        LDB1.text = evt.data["name"].str + " with " + evt.data["m"].str + "." + evt.data["s"].str + " minute";
    }

    void OnLeaderBoard2(SocketIOEvent evt)
    {
        LDB2.text = evt.data["name"].str + " with " + evt.data["m"].str + "." + evt.data["s"].str + " minute";
    }

    void OnLeaderBoard3(SocketIOEvent evt)
    {
        LDB3.text = evt.data["name"].str + " with " + evt.data["m"].str + "." + evt.data["s"].str + " minute";
    }

    public void backToMainMenuFunc()
    {
        SceneManager.LoadScene("title");
    }

    public void backToMainMenuFunc2()
    {
        SceneManager.LoadScene("title2");
    }

    #region Callback Group
    void OnClientConnect(SocketIOEvent evt)
    {
        Debug.Log("OnClientConnect : "+ evt.data.ToString());
        //socket.Emit("OnClientFetchPlayerList");
    }

    void OnClientDisconnect(SocketIOEvent evt)
    {
        Debug.Log("OnClientDisconnect : " + evt.data.ToString());
        //socket.Emit("OnClientFetchPlayerList");
    }

    void OnOwnerClientConnect(SocketIOEvent evt)
    {
        Debug.Log("OnOwnerClientConnect : " + evt.data.ToString());
        // Debug.Log("the random is "+ evt.data["randomMap"].ToString());
       // randomMapFromServer = evt.data["randomMap"].ToString();
    }

    void OnClientFetchPlayerList(SocketIOEvent evt)
    {
        Debug.Log("OnClientFetchPlayerList : "+ evt.data.ToString());

        playerIDGroup = JsonUtility.FromJson <PlayerIDGroup> (evt.data.ToString());
    }

    //======================== Room ===========================
    void OnClientCreateRoomSuccess(SocketIOEvent evt)
    {
        Debug.Log("OnClientCreateRoomSuccess : " + evt.data.ToString());

        connectionState = ConnectionState.InRoom;

        var dictData = evt.data.ToDictionary();

        ownerID = dictData["uid"];

        StartCoroutine(UpdateOwnerPlayerData());

        FetchPlayerList();
    }

    void OnClientCreateRoomFail(SocketIOEvent evt)
    {
        Debug.Log("OnClientCreateRoomFail : " + evt.data.ToString());
    }

    void OnOwnerClientJoinRoomSuccess(SocketIOEvent evt)
    {
        Debug.Log("OnOwnerClientJoinRoomSuccess : " + evt.data.ToString());

        connectionState = ConnectionState.InRoom;

        var dictData = evt.data.ToDictionary();
        
        ownerID = dictData["uid"];

        StartCoroutine(UpdateOwnerPlayerData());

        FetchPlayerList();
    }

    void OnClientJoinRoomSuccess(SocketIOEvent evt)
    {
        Debug.Log("OnClientJoinRoomSuccess : " + evt.data.ToString());

        FetchPlayerList();
    }

    void OnClientJoinRoomFail(SocketIOEvent evt)
    {
        Debug.Log("OnClientJoinRoomFail : " + evt.data.ToString());
    }

    void OnClientLeaveRoom(SocketIOEvent evt)
    {
        Debug.Log("OnClientLeaveRoom : " + evt.data.ToString());

        FetchPlayerList();
    }

    void OnClientFetchRoomList(SocketIOEvent evt)
    {
        Debug.Log("OnClientFetchRoomList : " + evt.data.ToString());

        roomIDGroup = JsonUtility.FromJson<RoomIDGroup>(evt.data.ToString());
    }

    void OnClientUpdateMoveList(SocketIOEvent evt)
    {
        var dataDict = evt.data.ToDictionary();

        for(int i = 0; i < characterList.Count; i++)
        {
            var newPlayerUpdateData = JsonUtility.FromJson<PlayerUpdateData>(dataDict[characterList[i].uid]);
            Vector3 newPos = new Vector3(newPlayerUpdateData.x, newPlayerUpdateData.y, newPlayerUpdateData.z);

            if(characterList[i].playerObj.transform.position == Vector3.zero)
            {
                characterList[i].playerObj.transform.position = newPos;
            }

            characterList[i].correctPos = newPos;
        }
    }
    #endregion
}
