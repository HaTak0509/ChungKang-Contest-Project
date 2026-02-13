using UnityEngine;

public class AnimationStrings
{
    public static string IsGroundParameterName = "IsGround";
    public static string IsMovingParameterName = "IsMoving";
    public static string IsPushingParameterName = "IsPushing";
    public static string IsJumpParameterName = "IsJump";
    public static string DashParameterName = "Dash";
    public static string IsDashParameterName = "IsDash";
    public static string IsKnockbackParameterName = "IsKnockback";
    public static string IsDeathParameterName = "IsDeath";
    public static string IsDeathBoolParameterName = "IsDeathBool";
    public static string IsSwimParameterName = "IsSwim";
    public static string IsHorizontalSwimParameterName = "IsHorizontalSwim";
    public static string IsVerticalSwimParameterName = "IsVerticalSwim";
    public static string LandParameterName = "Land";
    public static string IsSkyParameyterName = "IsSky";

    public static string OnButtonParameterName = "OnButton";
    public static string OnPlatformParameterName = "OnPlatform";

    public static int IsGround = Animator.StringToHash(IsGroundParameterName);
    public static int IsMoving = Animator.StringToHash(IsMovingParameterName);
    public static int IsPushing = Animator.StringToHash(IsPushingParameterName);
    public static int IsJump = Animator.StringToHash(IsJumpParameterName);
    public static int Dash = Animator.StringToHash(DashParameterName);
    public static int IsDash = Animator.StringToHash(IsDashParameterName);
    public static int IsDeathBool = Animator.StringToHash(IsDeathBoolParameterName);
    public static int IsKnockback = Animator.StringToHash(IsKnockbackParameterName);
    public static int IsDeath = Animator.StringToHash(IsDeathParameterName);
    public static int IsSwim = Animator.StringToHash(IsSwimParameterName);
    public static int IsHorizontalSwim = Animator.StringToHash(IsHorizontalSwimParameterName);
    public static int IsVerticalSwim = Animator.StringToHash(IsVerticalSwimParameterName);
    public static int Land = Animator.StringToHash(LandParameterName);
    public static int IsSky = Animator.StringToHash(IsSkyParameyterName);

    public static int OnButton = Animator.StringToHash(OnButtonParameterName);
    public static int OnPlatform = Animator.StringToHash(OnPlatformParameterName);
}
