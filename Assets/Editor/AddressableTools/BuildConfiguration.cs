using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;

namespace UniTaskFramework
{
    /// <summary>
    /// 一键Addressable 包配置
    /// </summary>
    [CreateAssetMenu(fileName = "BuildConfig",menuName = "Configs/Build/BuildConfig")]
    public class BuildConfiguration : SerializedScriptableObject
    {
        [Title("Addressable配置")]
        [FoldoutGroup("Addressable配置")]
        [FoldoutGroup("Addressable配置/分包资源"),FolderPath,LabelText("远程资源路径")]
        public string RemoteAssetPath;
        [FoldoutGroup("Addressable配置/分包资源"),AssetsOnly,LabelText("打包预设信息")]
        public  AddressableAssetGroup RemoteAssetSettings;
        
        
        /// <summary>
        /// xlua原始脚本路径
        /// </summary>
        [FoldoutGroup("Addressable配置/XLua设置"),FolderPath,LabelText("Lua脚本路径")]
        public string xLuaPath;

        /// <summary>
        /// xlua导出脚本路径
        /// </summary>
        [FoldoutGroup("Addressable配置/XLua设置"),FolderPath,LabelText("转换后路径")]
        public string xLuaOutPutPath;
        
        [FoldoutGroup("Addressable配置/XLua设置")]
        public AddressableAssetGroup XLuaSettings;
    }
}

