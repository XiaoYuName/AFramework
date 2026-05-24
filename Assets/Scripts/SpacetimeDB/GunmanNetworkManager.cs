using System;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using XFramework;
using SpacetimeDB;
using SpacetimeDB.Types;

/// <summary>
/// 枪手大作战网络管理器
/// </summary>
public class GunmanNetworkManager : MonoSingleton<GunmanNetworkManager>,IGameInitialized
{
    /// <summary>
    /// 配置表数据
    /// </summary>
    private GunmanNetworkDataManager networkDataManager;
    /// <summary>
    /// 当前连接服务器
    /// </summary>
    private GunmanNetworkData currentNetworkData;
    
    /// <summary>
    ///  订阅事件
    /// </summary>
    public event Action OnSubscriptionApplied;

    [BoxGroup("通信"),LabelText("新用户登录")]
    public bool isNewUser;

    [BoxGroup("通信"),LabelText("数据库连接器"),ReadOnly,ShowInInspector]
    public DbConnectionBuilder<DbConnection> builder { get; private set; }
    [BoxGroup("通信"),LabelText("数据库连接"),ReadOnly,ShowInInspector]
    public DbConnection Conn { get; private set; }
    [BoxGroup("通信"),LabelText("Token"),ReadOnly,ShowInInspector]
    public string Token { get; private set; }
    [BoxGroup("通信"),LabelText("身份标识"),ReadOnly,ShowInInspector]
    public Identity Identity { get; private set; }
    
    private bool isConnection;
    private bool isConnectionError;

    public async UniTask Initialized()
    {
        networkDataManager =
            await AssetsManager.Instance.LoadAssetsUniTask<GunmanNetworkDataManager>(
                "Assets/AddressableAssets/Remote/Configs/NetWork/GunmanNetworkDataManager.asset");
        currentNetworkData = networkDataManager.GetDataByID(networkDataManager.SelectedServerName);
        //构建数据库连接
        await NetworkConnect();
    }
    
    public async UniTask NetworkConnect()
    {
        builder = DbConnection.Builder()
            .OnConnect(Connect).OnConnectError(ConnectError)
            .OnDisconnect(HandleDisconnect).WithUri(currentNetworkData.Url)
            .WithDatabaseName(currentNetworkData.DatabaseName);
        
        if (!string.IsNullOrEmpty(AuthToken.Token) && !isNewUser)
        {
            Debug.Log("检测到本地Token，正在使用Token连接服务器..." + AuthToken.Token);
            builder = builder.WithToken(AuthToken.Token);
        }
        Conn = builder.Build();
        await UniTask.WaitUntil(() => isConnection || isConnectionError,cancellationToken: gameObject.GetCancellationTokenOnDestroy());
        Debug.Log("网络控制器初始化完成!");
    }
    
    private void Connect(DbConnection conn, Identity identity, string token)
    {
        isConnection = true;
        isConnectionError = false;
        Identity = identity;
        Token = token;
        AuthToken.SaveToken(token);
        Debug.Log("连接成功!" + Identity);
        Conn.SubscriptionBuilder().OnApplied(OnSubscriptionChange).SubscribeToAllTables();
        
    }

    private void ConnectError(Exception e)
    {
        isConnection = false;
        isConnectionError = true;
        Debug.LogError("连接失败!");
    }

    private void HandleDisconnect(DbConnection _conn, Exception ex)
    {
        Debug.Log("Disconnected.");
        if (ex != null)
        {
            Debug.LogException(ex);
        }
    }

    private void OnSubscriptionChange(SubscriptionEventContext ctx)
    {
        Debug.Log("收到订阅事件: ");
        OnSubscriptionApplied?.Invoke();
        UserNetworkManager.Instance.Initialized();
    }

    public async UniTask Release()
    {
        UserNetworkManager.Instance.Release();
        await UniTask.CompletedTask;
    }
}
