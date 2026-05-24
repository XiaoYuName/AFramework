using System;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using XLua;


namespace XFramework
{
   /// <summary>
   /// xLua C# 端主文件
   /// </summary>
   public class XLuaManager : MonoOdinSingleton<XLuaManager>
   {
      private const float GCInterval = 1; //1 second
      private float lastGCTime = 0;
      [ReadOnly,BoxGroup("XLua"),LabelText("Lua虚拟机")]
      public LuaEnv luaEnv;
      [BoxGroup("XLua"), LabelText("Lua文件路径"),FolderPath]
      public string luaAssetPath = "Assets/Scripts/XFramework/Lua/";
      [BoxGroup("XLua"), LabelText("远程Lua文件路径"),FolderPath,ShowIf("isEditorLoadAddressable")]
      public string remoteAssetPath = "Assets/AddressableAssets/Remote/xLua";
      [BoxGroup("XLua"), LabelText("是否在Editor下加载Lua的AB包文件")]
      public bool isEditorLoadAddressable; //Editor 环境下是否加载lua的AB包文件
      [BoxGroup("XLua"),ShowInInspector] 
      private Injection[] injections;

      private Action _luaMainStart;
      private Action _luaUpdate;
      private Action _luaFixedUpdate;
      private Action _luaLateUpdate;

      private void Start()
      {
         if (_luaMainStart != null) _luaMainStart();
      }

      public async UniTask Initialized()
      {
         luaEnv = new LuaEnv();
         if (injections != null)
         {
            foreach (var injection in injections)
            {
               luaEnv.Global.Set(injection.name, injection.value);
            }
         }

#if UNITY_EDITOR
         if (isEditorLoadAddressable)
         {
            luaEnv.AddLoader(new LuaFileLoader(remoteAssetPath).LoadAddressableFile);
         }
         else
         {
            luaEnv.AddLoader(new LuaFileLoader(luaAssetPath).LoadFile);
         }
#else
       luaEnv.AddLoader(new LuaFileLoader(remoteAssetPath).LoadAddressableFile);
#endif

         luaEnv.Global.Set("LoadCallBakc", typeof(LoadCallBack<>));
         TextAsset mianScript =
            await AssetsManager.Instance.LoadAssetsUniTask<TextAsset>("Assets/AddressableAssets/xLua/Main.lua.txt");
         luaEnv.DoString(mianScript.text, "main");
         luaEnv.Global.Get("MainStart", out _luaMainStart);
         luaEnv.Global.Get("Update", out _luaUpdate);
         luaEnv.Global.Get("FixedUpdate", out _luaFixedUpdate);
         luaEnv.Global.Get("LateUpdate", out _luaLateUpdate);

      }

      private void Update()
      {
         if (luaEnv == null)
            return;

         if (_luaUpdate != null)
            _luaUpdate();

         if (Time.time - lastGCTime > GCInterval)
         {
            luaEnv.Tick();
            lastGCTime = Time.time;
         }
      }

      private void FixedUpdate()
      {
         if (luaEnv == null)
            return;

         if (_luaFixedUpdate != null)
            _luaFixedUpdate();
      }

      private void LateUpdate()
      {
         if (luaEnv == null)
            return;

         if (_luaLateUpdate != null)
            _luaLateUpdate();
      }

      protected override void OnDestroy()
      {
         base.OnDestroy();
         luaAssetPath = null;
         injections = null;
         _luaUpdate = null;
         _luaFixedUpdate = null;
         _luaLateUpdate = null;
         if (CSharpCallLua.IsInitialized)
         {
            CSharpCallLua.Instance.Dispose();
         }

         if (LuaUISystem.IsInitialized)
         {
            LuaUISystem.Instance.Dispose();
         }

         if (luaEnv != null)
         {
            luaEnv.Dispose();
         }
      }
   }
}

