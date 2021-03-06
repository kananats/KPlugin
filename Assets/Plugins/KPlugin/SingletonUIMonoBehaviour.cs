﻿using UnityEngine;

namespace KPlugin
{
    [RequireComponent(typeof(RectTransform))]
    public class SingletonUIMonoBehaviour<T> : SingletonMonoBehaviour<T> where T : SingletonUIMonoBehaviour<T>
    {
        private RectTransform _transform = null;

        public new RectTransform transform
        {
            get
            {
                if (_transform == null)
                    _transform = (RectTransform)base.transform;

                return _transform;
            }
        }
    }
}
