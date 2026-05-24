using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace XFramework
{
    public class UpdateManager : MonoSingleton<UpdateManager>
    {
        private class Updater
        {
            public Object checkObject;
            public bool isCheckObject;

            /// <summary>
            /// 执行方法
            /// </summary>
            public Action action;

            /// <summary>
            /// 删除标记
            /// </summary>
            public bool markDeleted;

            public bool usedOnce = false;

            public void Clear()
            {
                checkObject = null;
                isCheckObject = false;
                action = null;
                markDeleted = false;
            }
        }

        //Updater列表
        private readonly List<Updater> _updaters = new List<Updater>();

        //临时存储用来实际执行的Updater
        private readonly List<Updater> _runUpdaters = new List<Updater>();

        //Updater队列
        private readonly Queue<Updater> _updaterPool = new Queue<Updater>();

        //出列，获取一个updater
        private Updater GetUpdater()
        {
            if (_updaterPool.Count > 0)
            {
                return _updaterPool.Dequeue();
            }

            return new Updater();
        }

        //加入执行语句
        public void AddHandler(Action action, Object go = null)
        {
            if (action == null)
                return;

            bool exist = false;
            //遍历查看是否已有该updater
            foreach (var u in _updaters)
            {
                if (u.action == action && !u.markDeleted)
                {
                    exist = true;
                    break;
                }
            }

            if (!exist)
            {
                var updater = GetUpdater();
                updater.checkObject = go;
                updater.isCheckObject = go != null;
                updater.action = action;
                updater.markDeleted = false;
                _updaters.Add(updater);
            }
        }

        /// <summary>
        /// 添加一次性执行语句
        /// </summary>
        /// <param name="action"></param>
        /// <param name="go"></param>
        public void AddHandlerOnce(Action action, Object go = null)
        {
            if (action == null)
                return;

            bool exist = false;
            //遍历查看是否已有该updater
            foreach (var u in _updaters)
            {
                if (u.action == action && !u.markDeleted)
                {
                    exist = true;
                    break;
                }
            }

            if (!exist)
            {
                var updater = GetUpdater();
                updater.checkObject = go;
                updater.isCheckObject = go != null;
                updater.action = action;
                updater.markDeleted = false;
                updater.usedOnce = true;
                _updaters.Add(updater);
            }
        }

        public void RemoveHandler(Action action)
        {
            Updater find = null;
            foreach (var u in _updaters)
            {
                if (u.action == action && !u.markDeleted)
                {
                    find = u;
                    break;
                }
            }

            if (find != null)
            {
                find.markDeleted = true;
            }
        }

        private bool IsValid(Updater updater)
        {
            if (updater.markDeleted)
                return false;

            if (updater.isCheckObject && updater.checkObject == null)
                return false;

            return true;
        }
        
        public void Update()
        {
            //Profiler.BeginSample("UpdateManager.Update.Head");
            _runUpdaters.Clear();
            foreach (var u in _updaters)
            {
                if (IsValid(u))
                    _runUpdaters.Add(u);
            }
            //Profiler.EndSample();

            foreach (var u in _runUpdaters)
            {
                if (IsValid(u))
                {
                    try
                    {
                        u.action();
                    }
                    catch (Exception e)
                    {
                        Debug.LogException(e);
                    }
                }
            }

            _runUpdaters.Clear();

            for (int i = _updaters.Count - 1; i >= 0; i--)
            {
                Updater u = _updaters[i];
                if (!IsValid(u) || u.usedOnce)
                {
                    _updaters.RemoveAt(i);
                    _updaterPool.Enqueue(u);
                }
            }
        }
    }
}
