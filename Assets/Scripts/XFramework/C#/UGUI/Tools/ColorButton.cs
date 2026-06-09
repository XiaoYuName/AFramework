using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ColorButton : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,IPointerDownHandler,IPointerUpHandler,IPointerClickHandler
{
    [BoxGroup("Pointer Colors"),HorizontalGroup("Pointer Colors/Color"),LabelText("鼠标进入Color")]
    public Color PointerEnterColor;
    
    [BoxGroup("Pointer Colors"),HorizontalGroup("Pointer Colors/Color"),LabelText("鼠标离开Color")]
    public Color PointerExitColor;
    
    private Tweener _scaleTweener;

    [FoldoutGroup("Tweener"),LabelText("是否有缩放动画")]
    public bool isTweener;
    [FoldoutGroup("Tweener"),LabelText("缩放比例"),ShowIf("isTweener")]
    public float TweenerScale = 1.1f;
    [FoldoutGroup("Tweener"),LabelText("曲线动画"),ShowIf("isTweener")]
    public Ease TweenerEase = Ease.OutBounce;
    [FoldoutGroup("Tweener"),LabelText("曲线动画"),ShowIf("isTweener")]
    public float TweenerDuration = 0.1f;

    
    #region UnityEvent
    
    [LabelText("鼠标移入事件"),FoldoutGroup("事件")]
    public Action OnEnter;

    [LabelText("鼠标移出事件"),FoldoutGroup("事件")]
    public Action OnExit;
    
    [LabelText("鼠标点击事件"),FoldoutGroup("事件")]
    public Action OnClick;

    [LabelText("鼠标抬起事件"),FoldoutGroup("事件")]
    public Action OnPointUp;

    [LabelText("鼠标按下事件"),FoldoutGroup("事件")]
    public Action OnPointDown;

    #endregion

    private Image _image;

    private void Awake()
    {
        _image = GetComponent<Image>();
        if (_image == null)
        {
            Debug.LogError("对象没有Image组件");
            return;
        }
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_image != null)
        {
            _image.color = PointerEnterColor;
        }

        if (isTweener)
        {
            _scaleTweener?.Kill();
            _scaleTweener = transform.DOScale(TweenerScale, TweenerDuration).SetEase(TweenerEase);
        }

        
        OnEnter?.Invoke();
        
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_image != null)
        {
            _image.color = PointerExitColor;
        }

        if (isTweener)
        {
            _scaleTweener?.Kill();
            _scaleTweener = transform.DOScale(Vector3.one, TweenerDuration).SetEase(TweenerEase);
        }
        OnExit?.Invoke();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnPointDown?.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        OnPointUp?.Invoke();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnClick?.Invoke();
    }
}
