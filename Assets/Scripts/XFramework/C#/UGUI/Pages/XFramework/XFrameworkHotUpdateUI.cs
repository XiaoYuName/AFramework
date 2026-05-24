using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace XFramework
{
    public class XFrameworkHotUpdateUI : UIBase
    {
        #region HotUpdatePage

        private RectTransform HotUpdateRect;

        private Slider processSlider;
        private TextMeshProUGUI processText;

        #endregion
        
        #region HotUpdateTipsPage

        private RectTransform HotUpdateTipRect;

        private Button CancelButton;

        private Button UpdateButton;
        

        #endregion

        #region WaitGame

        private RectTransform WaitGameRect;

        private Button OnClickGameButton;

        #endregion
        
        public override void Init()
        {
            HotUpdateRect = Get<RectTransform>("UIMask/HotUpdatePage");
            HotUpdateTipRect = Get<RectTransform>("UIMask/HotUpdateTipsPage");
            WaitGameRect = Get<RectTransform>("UIMask/WaitGamePage");
            processSlider = Get<Slider>("UIMask/HotUpdatePage/Slider_LoadingBar");
            processText = Get<TextMeshProUGUI>("UIMask/HotUpdatePage/Text_LoadingPercent");
            CancelButton = Get<Button>("UIMask/HotUpdateTipsPage/CancelButton");
            UpdateButton = Get<Button>("UIMask/HotUpdateTipsPage/UpdateButton");
            OnClickGameButton = Get<Button>("UIMask/WaitGamePage/OnClickGame");
            Bind(OnClickGameButton,OnClickGame,"");
        }

        public override void Open()
        {
            base.Open();
        }

        public override void Close()
        {
            base.Close();
        }

        public void OptionPage(HotUpdateState state)
        {
            HotUpdateRect.gameObject.SetActive(state == HotUpdateState.Download);
            HotUpdateTipRect.gameObject.SetActive(state == HotUpdateState.UseButton);
            WaitGameRect.gameObject.SetActive(state == HotUpdateState.WaitGame);
        }


        #region 进度条
        
        private Tweener processTweener;
        
        public void InitProcess(float value,float minValue = 0,float maxValue = 1)
        { 
            processSlider.minValue = minValue;
            processSlider.maxValue = maxValue;
            processSlider.value = value;
            processText.text =  $"{(int)(processSlider.value * 100)} %";
        }
        
        public void UpdateProcess(float process)
        {
            if (processTweener != null)
                processTweener.Kill();
            processTweener = processSlider.DOValue(process, 3f)
                .OnUpdate(() =>
                {
                    processText.text = $"{(int)(processSlider.value * 100)} %";
                });
        }

        public async UniTask UpdateProcess(float process, float time)
        {
            if (processTweener != null)
                processTweener.Kill();
            processTweener = processSlider.DOValue(process, time)
                .OnUpdate(() =>
                {
                    processText.text = $"{(int)(processSlider.value * 100)} %";
                });
            await processTweener.AsyncWaitForCompletion();
        }

        public void SetProcess(float process)
        {
            processSlider.value = process;
            processText.text =  $"{(int)(processSlider.value * 100)} %";
        }

        public async UniTask CompleteProcess(float time)
        {
            processTweener = processSlider.DOValue(processSlider.maxValue,time)
                .OnUpdate(() =>
                {
                    processText.text =  $"{(int)(processSlider.value * 100)} %";
                });
            await processTweener.AsyncWaitForCompletion();
        }


        #endregion

        #region UpdateButton

        public void InitUpdateButton(Action cancelCallBack, Action updateCallBack)
        {
            Bind(CancelButton,cancelCallBack,"");
            Bind(UpdateButton,updateCallBack,"");
        }

        public void UpdateButtonClick()
        {
            
        }
        
        public void CancelButtonClick()
        {
            
        }
        

        #endregion
        
        
        #region WaitGame
   
        private void OnClickGame()
        {
            GameManager.Instance.StarGame();
        }
        
        #endregion

    }
}

