using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class MadnessState : IEmotionState
{
    //*************************************************************
    // [ 코드 설명 ] :
    // 감정에 따른 여러 행동방식을 유사한 구조로 작성해서, 유지보수와
    // 확장성을 챙기기 위해 인터페이스 형식을 사용함
    //*************************************************************

    public EmotionType Type => EmotionType.Madness;
    private LayerMask _targetLayer = LayerMask.GetMask("Enemy");
    private HashSet<Monster> targetsInInfluence = new HashSet<Monster>();

    public void OnEnter(Monster monster)
    { 

    }

    public void UpdateState(Monster monster)
    {
        // 1. 현재 범위 내에 있는 모든 콜라이더 검출
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(monster.transform.position, monster.InteractRange, _targetLayer);

        HashSet<Monster> currentDetected = new HashSet<Monster>();

        // 2. 검출된 대상들에게 효과 적용
        foreach (var col in hitColliders)
        {
            Monster status = col.GetComponent<Monster>();
         
            if(status == monster)
                continue;

            
            if (status != null)
            {
                currentDetected.Add(status);

                // 새로 들어온 대상이라면 상태 부여
                if (!targetsInInfluence.Contains(status))
                {
                    status.SetStatus("Aura_Freeze", true);
                    targetsInInfluence.Add(status);
                }
            }
        }

        // 3. 이전에는 있었지만, 지금은 검출되지 않은(범위를 벗어난) 대상들 정리
        List<Monster> toRemove = new List<Monster>();
        foreach (var tracked in targetsInInfluence)
        {
            if (!currentDetected.Contains(tracked))
            {
                if (tracked != null) tracked.SetStatus("Aura_Freeze", false);
                toRemove.Add(tracked);
            }
        }

        foreach (var target in toRemove)
        {
            targetsInInfluence.Remove(target);
        }
    }

    public void OnExit(Monster monster) { 
    
        foreach(var target in targetsInInfluence)
        {
            if (target != null) target.SetStatus("Aura_Freeze", false);
        }
    
    }



}