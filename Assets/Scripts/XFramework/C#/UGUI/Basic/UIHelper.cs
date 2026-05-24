using UnityEngine;
using XLua;
using Object = UnityEngine.Object;
using UnityEngine.UI;

namespace XFramework
{
    [LuaCallCSharp]
    public static class UIHelper 
    {
        [LuaCallCSharp]
        public static Component GetComponentName(Transform tf,string type)
        {
            return tf.GetComponent(type);
        }
        
        /// <summary>
        /// 清空所有子物体
        /// </summary>
        /// <param name="rect">Rect 对象</param>
        public static void Clear(RectTransform rect)
        {
            if(rect.childCount <=0)return;
            foreach (Transform transform in rect)
            {
                Object.Destroy(transform.gameObject);
            }
        }
        /// <summary>
        /// 清空所有子物体
        /// </summary>
        /// <param name="rect">Rect 对象</param>
        public static void Clear(Transform rect)
        {
            if(rect.childCount <=0)return;
            foreach (Transform transform in rect)
            {
                Object.Destroy(transform.gameObject);
            }
        }
        /// <summary>
        /// 清空所有子物体
        /// </summary>
        /// <param name="obj">Obj 对象</param>
        public static void Clear(GameObject obj)
        {
            Clear(obj.transform as RectTransform);
            // JsonConvert.DeserializeObject<Json>()
        }

        /// <summary>
        /// 清空所有子物体
        /// </summary>
        /// <param name="offets">偏移offets</param>
        /// <param name="rect"></param>
        public static void OffetsClear(int offets, RectTransform rect)
        {
            if(rect.childCount <= offets)return;
            for (int i = offets; i < rect.childCount; i++)
            {
                Object.Destroy(rect.GetChild(i).gameObject);
            }
        }

        [LuaCallCSharp]
        public static RectTransform GetRect(GameObject obj)
        {
            return obj.gameObject.transform as RectTransform;
        }
        
    }
}
