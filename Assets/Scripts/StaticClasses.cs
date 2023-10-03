public static class InputStrings
{
    public static string Move = "Move";
    public static string Look = "Look";
    public static string Jump = "Jump";
    public static string Fire = "Fire";
    public static string Aim = "Aim";
}

public static class LayerMasks
{
    public static readonly int Swing = 1 << 6;
    public static readonly int Default = 1 << 0;
    public static readonly int All = ~0;
}
