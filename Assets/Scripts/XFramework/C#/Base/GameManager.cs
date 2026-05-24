using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace XFramework
{
    /// <summary>
    /// 游戏总管理器
    /// </summary>
    public class GameManager : MonoOdinSingleton<GameManager>
    {
        [BoxGroup("热更新"),LabelText("本地Canvas 面板")]
        public Canvas LocalCanvas;
        
        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            HotUpdate();
        }

        private void HotUpdate()
        {
            AddressableManager.Instance.Initialized().Forget();
        }
        
        public async UniTask Initialized()
        {
            await AudioManager.Instance.Initialized();
            await UISystem.Instance.Initialized();
            var task1 = XLuaManager.Instance.Initialized();
            await UniTask.WhenAll(task1);
        }

        public void StarGame()
        {
            AddressableManager.Instance.Release().Forget();
            LocalCanvas.gameObject.SetActive(false);
            LuaUISystem.Instance.OpenUI("CommonUI");
            AudioManager.Instance.PlayAudio("HomeBGM");
        }

        #region 进入游戏主体

        public void EnterGunmanGame()
        {
           
        }
        public void QuitGunmanGame()
        {
            
        }

        #endregion
    }
}

