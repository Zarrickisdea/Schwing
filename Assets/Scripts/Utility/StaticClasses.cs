public static class InputStrings
{
    public static readonly string Move = "Move";
    public static readonly string Look = "Look";
    public static readonly string Jump = "Jump";
    public static readonly string Fire = "Fire";
    public static readonly string Aim = "Aim";
}

public static class LayerMasks
{
    public static readonly int Swing = 1 << 6;
    public static readonly int Default = 1 << 0;
    public static readonly int All = ~0;
}

public static class CustomTags
{
    public static readonly string Swing = "Swing";
    public static readonly string Spawn = "Spawn";
}

public static class SceneNames
{
    public static readonly string StartScene = "StartScene";
    public static readonly string MainScene = "MainScene";
    public static readonly string GameOverScene = "GameOverScene";
}
