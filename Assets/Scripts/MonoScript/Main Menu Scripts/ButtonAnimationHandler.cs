using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonAnimationHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    [SerializeField] private float _verticalMoveAmount = 30f;
    [SerializeField] private float _moveTime = 0.1f;
    [Range(0f, 2f), SerializeField] private float _scaleAmount = 1.5f;

    //private Vector3 _startPos;
    private Vector3 _startScale;

    private void Awake()
    {
        //_startPos = transform.position;
        _startScale = transform.localScale;
    }
    private void OnDisable()
    {
        transform.localScale = _startScale;
    }

    private IEnumerator MoveButton(bool startingAnimation)
    {
        //Vector3 endPosition;
        Vector3 endScale;

        float elapsedTime = 0f;
        while (elapsedTime < _moveTime)
        {
            elapsedTime += Time.unscaledDeltaTime;
            if (startingAnimation)
            {
                //endPosition = _startPos + new Vector3(0f, _verticalMoveAmount, 0f);
                endScale = _startScale * _scaleAmount;
            }
            else
            {
                //endPosition = _startPos;
                endScale = _startScale;
            }

            //Vector3 lerpedPos = Vector3.Lerp(transform.position, endPosition, (elapsedTime / _moveTime));
            Vector3 lerpedScale = Vector3.Lerp(transform.localScale, endScale, (elapsedTime / _moveTime));

            //transform.position = lerpedPos;
            transform.localScale = lerpedScale;

            yield return null;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        eventData.selectedObject = gameObject;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        eventData.selectedObject = null;
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (eventData.selectedObject.GetComponent<Button>().interactable)
        {
            StartCoroutine(MoveButton(true));
        }
    }

    public void OnDeselect(BaseEventData eventData)
    {
        StartCoroutine(MoveButton(false));
    }
}
