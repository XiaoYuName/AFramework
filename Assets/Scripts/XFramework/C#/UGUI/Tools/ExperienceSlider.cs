using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace XFramework
{
    public class ExperienceSlider : UIBase
    {
        private Image m_Image;
        private TextMeshProUGUI m_Text;
        private TextMeshProUGUI m_levelTex;
        private int maxValue;

        public int m_Value { get; private set; }

        /// <summary>
        /// 初始化方法,一般不需要手动调用
        /// </summary>
        public override void Init()
        {
            m_Image = Get<Image>("Slider");
            m_Text = Get<TextMeshProUGUI>("Slider/Process");
            m_levelTex = Get<TextMeshProUGUI>("CharacterLevel");
        }

        public void Initialized(int maxValue, int value)
        {
            m_Text.text = value + "/" + maxValue;
            this.maxValue = maxValue;
            //m_Value = value;
        }

        public void SetLevel(int value)
        {
            m_levelTex.text = value.ToString();
        }

        public void SetValue(int value)
        {
            if (m_Value != value)
            {
                m_Value = value;
                m_Image.DOFillAmount((float)value / maxValue,0.25f);
                //m_Image.fillAmount = value / 1f;
                m_Text.text = value + "/" + maxValue;
            }
        }
    }
}

