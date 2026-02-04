public static class StageCollect
{
    public static void CollectStage(int stageIndex)
    {
        LevelManager.Instance.LoadLevel(stageIndex);
    }
}
