using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using XFramework;

public class LoadSaveGameUI : UIBase
{
    private Tweener _tweener;
    private RectTransform PageTweener;

    private ColorButton LoadButton;
    private ColorButton CloseButton;

    [LabelText("自动保存槽位")]
    public SaveGameSlot AutoSaveGameSlot;
    [LabelText("存档槽列表")]
    public List<SaveGameSlot> SaveGameSlots;
    
    /// <summary>
    /// 初始化方法,一般不需要手动调用
    /// </summary>
    public override void Init()
    {
        PageTweener = Get<RectTransform>("UIMask/Background");
        LoadButton = Get<ColorButton>("UIMask/Background/LoadButton");
        CloseButton = Get<ColorButton>("UIMask/Background/QuitButton");
        BindColorClick(CloseButton,Close,"");
        AutoSaveGameSlot.Init();
        for (int i = 0; i < SaveGameSlots.Count; i++)
        {
            SaveGameSlots[i].Init();
        }
    }

    /// <summary>
    /// 通用UI打开方法,提供重写
    /// </summary>
    public override void Open()
    {
        base.Open();
        _tweener?.Kill();
        PageTweener.transform.localScale = Vector3.zero;
        PageTweener.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBack);
        SaveGameManager.Instance.RegionUsersChange(UpdateUsers);
    }

    /// <summary>
    /// 通用UI关闭方法,提供重写
    /// </summary>
    public override void Close()
    {
        _tweener?.Kill();
        if (SaveGameManager.IsInitialized)
        {
            SaveGameManager.Instance.RegionUsersChange(UpdateUsers);
        }
        PageTweener.transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.OutBack)
            .OnComplete(() =>
            {
                base.Close();
            });
    }

    private void UpdateUsers(List<User> users)
    {
        AutoSaveGameSlot.SetEmptyData();
        for (int i = 0; i < SaveGameSlots.Count; i++)
        {
            SaveGameSlots[i].SetEmptyData();
        }
        for (int i = 0; i < users.Count; i++)
        {
            if (users[i].UserID == 0)
            {
                AutoSaveGameSlot.SetData(users[i]);
            }
            else
            {
                SaveGameSlots[i].SetData(users[i]);
            }
        }
    }
}
