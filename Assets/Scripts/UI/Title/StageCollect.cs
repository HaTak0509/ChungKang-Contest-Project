public static class StageCollect
{
    public static void CollectStage(int stageIndex)
    {
        if (stageIndex <= LevelManager.Instance.saveMaxLevel)
            LevelManager.Instance.LoadLevel(stageIndex);
    }
}
