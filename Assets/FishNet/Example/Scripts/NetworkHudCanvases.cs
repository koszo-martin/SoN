using FishNet.Managing;
using FishNet.Transporting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NetworkHudCanvases : MonoBehaviour
{
    #region Types.
    /// <summary>
    /// Ways the HUD will automatically start a connection.
    /// </summary>
    private enum AutoStartType
    {
        Disabled,
        Host,
        Server,
        Client
    }
    #endregion

    #region Serialized.
    /// <summary>
    /// What connections to automatically start on play.
    /// </summary>
    [Tooltip("What connections to automatically start on play.")]
    [SerializeField]
    private AutoStartType _autoStartType = AutoStartType.Disabled;
    #endregion

    #region Private.
    /// <summary>
    /// Found NetworkManager.
    /// </summary>
    private NetworkManager _networkManager;
    /// <summary>
    /// Current state of client socket.
    /// </summary>
    private LocalConnectionState _clientState = LocalConnectionState.Stopped;
    /// <summary>
    /// Current state of server socket.
    /// </summary>
    private LocalConnectionState _serverState = LocalConnectionState.Stopped;
#if !ENABLE_INPUT_SYSTEM
    /// <summary>
    /// EventSystem for the project.
    /// </summary>
    private EventSystem _eventSystem;
#endif
    #endregion

    void OnGUI()
    {
#if ENABLE_INPUT_SYSTEM        
        string GetNextStateText(LocalConnectionState state)
        {
            if (state == LocalConnectionState.Stopped)
                return "Start";
            else if (state == LocalConnectionState.Starting)
                return "Starting";
            else if (state == LocalConnectionState.Stopping)
                return "Stopping";
            else if (state == LocalConnectionState.Started)
                return "Stop";
            else
                return "Invalid";
        }

        GUILayout.BeginArea(new Rect(16, 16, 256, 9000));
        Vector2 defaultResolution = new Vector2(1920f, 1080f);
        GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(Screen.width / defaultResolution.x, Screen.height / defaultResolution.y, 1));

        GUIStyle style = GUI.skin.GetStyle("button");
        int originalFontSize = style.fontSize;

        Vector2 buttonSize = new Vector2(256f, 64f);
        style.fontSize = 28;
        //Server button.
        if (Application.platform != RuntimePlatform.WebGLPlayer)
        {
            if (GUILayout.Button($"{GetNextStateText(_serverState)} Server", GUILayout.Width(buttonSize.x), GUILayout.Height(buttonSize.y)))
                OnClick_Server();
            GUILayout.Space(10f);
        }

        //Client button.
        if (GUILayout.Button($"{GetNextStateText(_clientState)} Client", GUILayout.Width(buttonSize.x), GUILayout.Height(buttonSize.y)))
            OnClick_Client();

        style.fontSize = originalFontSize;

        GUILayout.EndArea();
#endif
    }

    private void Start()
    {
#if !ENABLE_INPUT_SYSTEM
        SetEventSystem();
        BaseInputModule inputModule = FindObjectOfType<BaseInputModule>();
        if (inputModule == null)
            gameObject.AddComponent<StandaloneInputModule>();
#endif

        _networkManager = FindObjectOfType<NetworkManager>();
        if (_networkManager == null)
        {
            Debug.LogError("NetworkManager not found, HUD will not function.");
            return;
        }
        else
        {
            _networkManager.ServerManager.OnServerConnectionState += ServerManager_OnServerConnectionState;
            _networkManager.ClientManager.OnClientConnectionState += ClientManager_OnClientConnectionState;
        }

        if (_autoStartType == AutoStartType.Host || _autoStartType == AutoStartType.Server)
            OnClick_Server();
        if (!Application.isBatchMode && (_autoStartType == AutoStartType.Host || _autoStartType == AutoStartType.Client))
            OnClick_Client();
    }


    private void OnDestroy()
    {
        if (_networkManager == null)
            return;

        _networkManager.ServerManager.OnServerConnectionState -= ServerManager_OnServerConnectionState;
        _networkManager.ClientManager.OnClientConnectionState -= ClientManager_OnClientConnectionState;
    }

    private void ClientManager_OnClientConnectionState(ClientConnectionStateArgs obj)
    {
        _clientState = obj.ConnectionState;
    }


    private void ServerManager_OnServerConnectionState(ServerConnectionStateArgs obj)
    {
        _serverState = obj.ConnectionState;
    }


    public void OnClick_Server()
    {
        if (_networkManager == null)
            return;

        if (_serverState != LocalConnectionState.Stopped)
            _networkManager.ServerManager.StopConnection(true);
        else
            _networkManager.ServerManager.StartConnection();

        DeselectButtons();
    }


    public void OnClick_Client()
    {
        if (_networkManager == null)
            return;

        if (_clientState != LocalConnectionState.Stopped)
            _networkManager.ClientManager.StopConnection();
        else
            _networkManager.ClientManager.StartConnection();

        DeselectButtons();
    }


    private void SetEventSystem()
    {
#if !ENABLE_INPUT_SYSTEM
        if (_eventSystem != null)
            return;
        _eventSystem = FindObjectOfType<EventSystem>();
        if (_eventSystem == null)
            _eventSystem = gameObject.AddComponent<EventSystem>();
#endif
    }

    private void DeselectButtons()
    {
#if !ENABLE_INPUT_SYSTEM
        SetEventSystem();
        _eventSystem?.SetSelectedGameObject(null);
#endif
    }
}