using System.IO;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace XFramework
{
    public class LuaFileLoader
    {

        private string _searchPath;

        public LuaFileLoader(string searchPath)
        {
            _searchPath = searchPath;
        }

        public byte[] LoadFile(ref string filepath)
        {
#if UNITY_EDITOR
            var path = StringUtil.Concat(_searchPath, filepath.Replace('.', '/'), ".lua");
            if (File.Exists(path))
                return File.ReadAllBytes(path);
            return null;
#else
            string path = $"{_searchPath}/{filepath}.lua.txt";
            TextAsset luaAsset = AssetsManager.Instance.LoadAssets<TextAsset>(path);
            if (luaAsset != null)
            {
                return luaAsset.bytes;
            }
            return null;
#endif
        }

        public byte[] LoadAddressableFile(ref string filepath)
        {
            string path = $"{_searchPath}/{filepath}.lua.txt";
            TextAsset luaAsset = AssetsManager.Instance.
                LoadAssets<TextAsset>(path);
            if (luaAsset != null)
            {
                return luaAsset.bytes;
            }

            return null;
        }
    }
}