using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;

namespace XFramework
{
    /// <summary>
    /// Addressable 资源管理器
    /// </summary>
    public class AddressableManager : MonoOdinSingleton<AddressableManager>,IGameInitialized
    {
        private List<IResourceLocator> AssetsKeys;

        /// <summary>
        /// 所有要更新的资源Keys
        /// </summary>
        private List<string> _update_keys;

        /// <summary>
        /// 所有要更新切有资源大小的Keys
        /// </summary>
        private List<IResourceLocator> UpdateKeys;

        private CancellationToken _token;
        
        [BoxGroup("热更新"),LabelText("热更新UI")]
        public XFrameworkHotUpdateUI HotUpdateUI;
        
        public async UniTask Initialized()
        {
            HotUpdateUI.Init();
            _token = gameObject.GetCancellationTokenOnDestroy();
            UpdateKeys = new List<IResourceLocator>();
            try
            {
                Debug.Log("开始执行热更新流程!");
                HotUpdateUI.OptionPage(HotUpdateState.Download);
                HotUpdateUI.InitProcess(0);
                await HotUpdateUI.UpdateProcess(0.1f,1f);
                await Addressables.InitializeAsync();
                await HotUpdateUI.UpdateProcess(0.25f,1f);
                if (await CheckUpdateAssets())
                {
                    if (await CheckUpdateAssetsSize())
                    {
                        await HotUpdateUI.CompleteProcess(1f);
                        HotUpdateUI.OptionPage(HotUpdateState.UseButton);
                        HotUpdateUI.InitUpdateButton(OnGameFailure, () =>
                        {
                            DownloadDependencies().Forget();
                        });
                    }
                }
                OnGameSuccess().Forget();

            }
            catch (Exception e)
            {
                Debug.LogError("异常处理: " + e.Message);
                OnGameFailure();
            }
        }

        public async UniTask Release()
        {
            HotUpdateUI.Close();
            await UniTask.CompletedTask;
        }
        
        
        //2.检查是否有更新
        private async UniTask<bool> CheckUpdateAssets()
        { 
            _update_keys = await Addressables.CheckForCatalogUpdates().
                ToUniTask(cancellationToken:_token);
            if (_update_keys.Count <= 0)
            {
                return false;
            }
            return true;
        }
        
        //3.检查更新资源大小
        private async UniTask<bool>  CheckUpdateAssetsSize()
        {
            AssetsKeys = await Addressables.UpdateCatalogs(
                true, _update_keys).ToUniTask(cancellationToken: _token);
            long downloadAssetSize = 0;
            foreach (var item in AssetsKeys)
            {
                downloadAssetSize += await Addressables.GetDownloadSizeAsync(item.Keys).
                    ToUniTask(cancellationToken: _token);
                if (downloadAssetSize / 1048579f > 0)
                {
                    UpdateKeys.Add(item);
                }
            }
            //仅删除资源没有新增资源时的情况
            if (downloadAssetSize <= 0)
            {
                return false;
            }
            float UpdateSize = downloadAssetSize / 1048579f;
            Debug.Log($"检测到更新大小{UpdateSize}M,是否进行下载!");
            return true;
        }
        
        private async UniTask DownloadDependencies()
        {
            HotUpdateUI.OptionPage(HotUpdateState.Download);
            HotUpdateUI.InitProcess(0);
            var progress = Progress.Create<float>(x => HotUpdateUI.SetProcess(x));
            await Addressables.DownloadDependenciesAsync(UpdateKeys)
                .ToUniTask(cancellationToken: _token,progress: progress);
            await HotUpdateUI.CompleteProcess(1f);
            HotUpdateUI.OptionPage(HotUpdateState.WaitGame);
        }
        
        /// <summary>
        /// 关闭游戏,重新启动
        /// </summary>
        private void RestartAndroid()
        {
            if (Application.isEditor) return;
            using var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            const int kIntent_FLAG_ACTIVITY_CLEAR_TASK = 0x00008000;
            const int kIntent_FLAG_ACTIVITY_NEW_TASK = 0x10000000;
            var currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            var pm = currentActivity.Call<AndroidJavaObject>("getPackageManager");
            var intent = pm.Call<AndroidJavaObject>("getLaunchIntentForPackage", Application.identifier);

            intent.Call<AndroidJavaObject>("setFlags", kIntent_FLAG_ACTIVITY_NEW_TASK | kIntent_FLAG_ACTIVITY_CLEAR_TASK);
            currentActivity.Call("startActivity", intent);
            currentActivity.Call("finish");
            var process = new AndroidJavaClass("android.os.Process");
            int pid = process.CallStatic<int>("myPid");
            process.CallStatic("killProcess", pid);
        }

        /// <summary>
        /// 当用户的资源下载失败时,清理缓存
        /// </summary>
        private void DeleteCheckFile()
        {
            string path = Application.persistentDataPath+"/com.unity.addressables";
            Directory.Delete(path,true);
            string checkpath = Application.persistentDataPath + "/CheckAssets";
            Directory.Delete(checkpath,true);
        }
        
        private async UniTask OnGameSuccess()
        {
            await GameManager.Instance.Initialized();
            await HotUpdateUI.CompleteProcess(3f);
            HotUpdateUI.OptionPage(HotUpdateState.WaitGame);
        }

        private void OnGameFailure()
        {
            HotUpdateUI.CompleteProcess(3f).Forget();
        }


    }
    
}

