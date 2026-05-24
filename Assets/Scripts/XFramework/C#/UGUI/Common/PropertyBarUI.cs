using System;
using SpacetimeDB.Types;
using TMPro;
using UnityEngine;
using XFramework;

public class PropertyBarUI : UIBase
{
    private TextMeshProUGUI GoldNumberTex;
    private TextMeshProUGUI DiamondNumberTex;
    private bool isInit;
    
    /// <summary>
    /// 初始化方法,一般不需要手动调用
    /// </summary>
    public override void Init()
    {
        isInit = true;
        GoldNumberTex = Get<TextMeshProUGUI>("GoldFarme/GoldNumberTex");
        DiamondNumberTex = Get<TextMeshProUGUI>("DiamondFarme/DiamondNumberTex");
    }

    private void OnEnable()
    {
        UserNetworkManager.Instance.BindUserChange(UpdateGunmanUser);
    }
    
    private void OnDisable()
    {
        UserNetworkManager.Instance.UnBindUserChange(UpdateGunmanUser);
    }

    private void UpdateGunmanUser(GunmanUser user)
    {
        if (!isInit)
        {
            Init();
        }

        GoldNumberTex.text = user.Gold.ToString();
        DiamondNumberTex.text = user.Diamond.ToString();
    }

    
}
