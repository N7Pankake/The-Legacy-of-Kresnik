using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillBook : MonoBehaviour
{
    [SerializeField]
    private Image castingBar;

    [SerializeField]
    private Text skillName;

    [SerializeField]
    private Text castTime;

    [SerializeField]
    private Image icon;

    [SerializeField]
    private CanvasGroup canvasGroup;

    [SerializeField]
    private Skill[] skills;

    private Coroutine skillRoutine;

    private Coroutine fadeRoutine;

    void Start()
    {
        
    }
    
    void Update()
    {
        
    }

    public Skill CastSkill(int index)
    {
        castingBar.fillAmount = 0;

        castingBar.color = skills[index].MyBarColor;
        
        skillName.text = skills[index].MyName;

        icon.sprite = skills[index].MyIcon;

        skillRoutine = StartCoroutine(Progress(index));

        fadeRoutine = StartCoroutine(FadeBar());

        return skills[index];
    }

    private IEnumerator Progress (int index)
    {
        float timePassed = Time.deltaTime;

        float rate = 1.0f / skills[index].MyCastTime;

        float progress = 0.0f;

        while (progress <= 1.0)
        {
            castingBar.fillAmount = Mathf.Lerp(0, 1, progress);

            progress += rate * Time.deltaTime;

            timePassed += Time.deltaTime;

            castTime.text = (skills[index].MyCastTime - timePassed).ToString("F2");
            
            if (skills[index].MyCastTime - timePassed < 0)
            {
                castTime.text = "0.00";
            }

            yield return null;
        }

        StopCasting();
    }

    private IEnumerator FadeBar()
    {
        float rate = 1.0f / 0.50f ;

        float progress = 0.0f;

        while (progress <= 1.0)
        {
            canvasGroup.alpha = Mathf.Lerp(0, 1, progress);

            progress += rate * Time.deltaTime;

            yield return null;
        }
    }

    public virtual void StopCasting()
    {
        if (fadeRoutine != null)
        {
            StopCoroutine(fadeRoutine);
            canvasGroup.alpha = 0;
            fadeRoutine = null;
        }

        if (skillRoutine != null)
        {
            StopCoroutine(skillRoutine);
            skillRoutine = null;
        }
    }
}
