using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillBook : MonoBehaviour
{
    private static SkillBook instance;

    public static SkillBook MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SkillBook>();
            }
            return instance;
        }
    }

    [SerializeField]
    private Image castingBar;

    [SerializeField]
    private Text currentSkill;

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

    public Skill CastSkill(string skillName)
    {
        Skill skill = Array.Find(skills, x => x.MyName == skillName);

        castingBar.fillAmount = 0;

        castingBar.color = skill.MyBarColor;
        
        currentSkill.text = skill.MyName;

        icon.sprite = skill.MyIcon;

        skillRoutine = StartCoroutine(Progress(skill));

        fadeRoutine = StartCoroutine(FadeBar());

        return skill;
    }

    private IEnumerator Progress (Skill skill)
    {
        float timePassed = Time.deltaTime;

        float rate = 1.0f / skill.MyCastTime;

        float progress = 0.0f;

        while (progress <= 1.0)
        {
            castingBar.fillAmount = Mathf.Lerp(0, 1, progress);

            progress += rate * Time.deltaTime;

            timePassed += Time.deltaTime;

            castTime.text = (skill.MyCastTime - timePassed).ToString("F2");
            
            if (skill.MyCastTime - timePassed < 0)
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

    public Skill GetSkill(string skillName)
    {
       Skill skill = Array.Find(skills, x => x.MyName == skillName);

        return skill;
    }
}
