using System;
using Cysharp.Threading.Tasks;
using SpacetimeDB.Types;
using UnityEngine;
using XFramework;

/// <summary>
/// 用户网络数据管理器
/// </summary>
public class UserNetworkManager : MonoSingleton<UserNetworkManager>
{
    private GunmanUser user;
    private Action<GunmanUser> OnUserChange;
    
    
    public void Initialized()
    {
        user = GunmanNetworkManager.Instance.Conn.Db.GunmanUser.Identity.Find(GunmanNetworkManager.Instance.Identity); 
        GunmanNetworkManager.Instance.Conn.Db.GunmanUser.OnUpdate += OnUserUpdate;
    }
    
    public void Release()
    {
        GunmanNetworkManager.Instance.Conn.Db.GunmanUser.OnUpdate -= OnUserUpdate;
    }
    
    private void OnUserUpdate(EventContext context, GunmanUser oldRow, GunmanUser newRow)
    {
        if (newRow.Identity == GunmanNetworkManager.Instance.Identity)
        {
            OnUserChange?.Invoke(newRow);
        }
    }


    #region User Evnet

    public void BindUserChange(Action<GunmanUser> callback)
    {
        if (OnUserChange == null)
        {
            OnUserChange = callback;
            callback?.Invoke(user);
            return;
        }

        OnUserChange += callback;
        callback?.Invoke(user);
    }
    
    public void UnBindUserChange(Action<GunmanUser> callback)
    {
        OnUserChange -= callback;
    }

    #endregion
    
}
