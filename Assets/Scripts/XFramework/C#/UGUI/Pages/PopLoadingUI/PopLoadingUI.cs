using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace XFramework
{
    public class PopLoadingUI : UIBase
    {
        private TextMeshProUGUI m_TipsTex;
        private TextMeshProUGUI m_ProgressTex;
        private Slider m_ProgressSlider;
        
        private Tweener m_ProgressTween;
        
        
        /// <summary>
        /// 初始化方法,一般不需要手动调用
        /// </summary>
        public override void Init()
        {
            m_TipsTex = Get<TextMeshProUGUI>("UIMask/TipsTex");
            m_ProgressTex = Get<TextMeshProUGUI>("UIMask/Slider/Fill Area/Fill/ProgressTex");
            m_ProgressSlider = Get<Slider>("UIMask/Slider");
        }

        public void Initialized(float min, float max)
        {
            m_ProgressSlider.minValue = min;
            m_ProgressSlider.maxValue = max;
            m_ProgressSlider.value = min;
        }

        public void SetProgress(float value,float duration = 0.5f,string tips ="")
        {
            m_ProgressTween?.Kill();
            if (!string.IsNullOrEmpty(tips))
            {
                m_TipsTex.text = tips;
            }
            m_ProgressTween = m_ProgressSlider.DOValue(value, duration)
                .OnUpdate(() =>
                {
                    m_ProgressTex.text = (int)(m_ProgressSlider.value * 100) + "%";
                });
        }

        public void SetProgressComplete(float duration = 0.5f, Action complete = null)
        {
            m_ProgressTween?.Kill();
            m_ProgressTween = m_ProgressSlider.DOValue(m_ProgressSlider.maxValue, duration)
                .OnUpdate(() =>
                {
                    m_ProgressTex.text = (int)(m_ProgressSlider.value * 100) + "%";
                });
            m_ProgressTween.OnComplete(() =>
            {
                complete?.Invoke();
                Close();
            });
        }
    }
}

