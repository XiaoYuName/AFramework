using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using XFramework;

[CreateAssetMenu(fileName = "GunmanNetworkDataManager", menuName = "XFramework/GunmanNetworkDataManager")]
public class GunmanNetworkDataManager : OdinScriptableObject<GunmanNetworkData>
{
    [BoxGroup("服务器"),LabelText("当前选中服务器"),ValueDropdown("GetServerName")]
    public string SelectedServerName;

    public IEnumerable GetServerName()
    {
        if (DataList == null)
        {
            return new List<string>();
        }
        return DataList.Where(x => !string.IsNullOrEmpty(x.ServerName) 
                                   &&  !string.IsNullOrEmpty(x.DatabaseName) 
                                   && !string.IsNullOrEmpty(x.Url))
            .Select(t=> t.ServerName);
    }
}

[System.Serializable]
public class GunmanNetworkData : OdinDataItem<GunmanNetworkData>
{
    [BoxGroup("服务器"),LabelText("服务器名称")]
    public string ServerName;
    [BoxGroup("服务器"),LabelText("数据库名称")]
    public string DatabaseName;
    [BoxGroup("服务器"),LabelText("数据库地址")]
    public string Url;
    
    public override string GetID()
    {
        return ServerName;
    }
}
