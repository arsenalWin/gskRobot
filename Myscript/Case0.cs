using UnityEngine;
using System.Collections;

public class Case0 : CaseClass
{

    public override IEnumerator Teach()
    {
        for (; stage_current < StageTotal; stage_current++)
        {
            PhaseStateSetting();
            yield return new WaitForSeconds(1.0f);
        }
    }
    public override void TransPosition()//旋转参考点位置的确定
    {
        ;
    }
    public override void ToolPosition()
    {
        ;
    } //末端加持装置的位置

}
