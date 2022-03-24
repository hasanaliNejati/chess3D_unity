using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class PanelScript : MonoBehaviour
{

    public float exitTime = 1;
    public bool unscaledTime;
    float _time;
    public string exitAnimationName = "end panel";
    public Animator animator;
    public bool startFad;
    public bool endFad;
    public bool replayAnim;
    public string replayAnimName = "replay";
    CanvasGroup _canvasGroup;
    CanvasGroup canvasGroup { get { return _canvasGroup?_canvasGroup:_canvasGroup = GetComponent<CanvasGroup>(); } }
    private void Awake()
    {
        activeBool = gameObject.activeSelf;
        if (activeBool && startFad)
            _time = 0;
        else
            _time = exitTime;
    }

    public void SetActive(bool active)
    {
        if (active)
        {
            if(!activeBool && gameObject.activeSelf)
            {
                gameObject.SetActive(false);
            }
            if(replayAnim && gameObject.activeSelf && animator)
            {
                animator.Play(replayAnimName, -1, 0);
            }
            gameObject.SetActive(true);
            if (startFad)
                canvasGroup.alpha = 0;
            canvasGroup.blocksRaycasts = true;
            activeBool = true;
        }
        else
        {

            if (animator)
                animator.Play(exitAnimationName);
            //else
            //    GetComponent<Animator>().Play(exitAnimationName);
            if (startFad)
                canvasGroup.alpha = 1;
            canvasGroup.blocksRaycasts = false;
            activeBool = false;

        }
    }

    bool activeBool;
    private void Update()
    {
        if (activeBool)
        {
            if (_time < exitTime)
                _time +=  unscaledTime?Time.unscaledDeltaTime : Time.deltaTime;
            if (startFad)
            {
                canvasGroup.alpha = _time/exitTime;
            }
            else
            {
                canvasGroup.alpha = 1;
            }
        }
        else
        {

            _time -= unscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
            if (endFad)
            {
                canvasGroup.alpha = _time/exitTime;
            }

            if (_time <= 0)
                gameObject.SetActive(false);
        }
    }

}
