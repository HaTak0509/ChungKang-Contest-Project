using UnityEngine;

public class AnimationStrings
{
    public static string IsGroundParameterName = "IsGround";
    public static string IsMovingParameterName = "IsMoving";
    public static string IsPushingParameterName = "IsPushing";
    public static string IsJumpParameterName = "IsJump";
    public static string IsDashParameterName = "IsDash";
    public static string IsKnockbackParameterName = "IsKnockback";

    public static int IsGround = Animator.StringToHash(IsGroundParameterName);
    public static int IsMoving = Animator.StringToHash(IsMovingParameterName);
    public static int IsPushing = Animator.StringToHash(IsPushingParameterName);
    public static int IsJump = Animator.StringToHash(IsJumpParameterName);
    public static int IsDash = Animator.StringToHash(IsDashParameterName);
    public static int IsKnockback = Animator.StringToHash(IsKnockbackParameterName);
}
