using UnityEngine;

public class SecondMapPlatformHelper : BaseMapPlatformHelper
{
    [Header("STEPS")]
    [SerializeField]
    private MapChanges _stepZero;

    [SerializeField]
    private MapChanges _stepOne;
    [SerializeField]
    private MapChanges _stepOneOne;
    [SerializeField]
    private MapChanges _stepOneTwo;
    [SerializeField]
    private MapChanges _stepOneThree;
    [SerializeField]
    private MapChanges _stepOneFour;

    [SerializeField]
    private MapChanges _stepTwo;
    [SerializeField]
    private MapChanges _stepTwoOne;
    [SerializeField]
    private MapChanges _stepTwoTwo;
    [SerializeField]
    private MapChanges _stepTwoThree;

    [SerializeField]
    private MapChanges _stepThree;

    protected override void ResetAllSteps()
    {
        _stepOne.Reset();
        _stepOneOne.Reset();
        _stepOneTwo.Reset();
        _stepOneThree.Reset();
        _stepOneFour.Reset();

        _stepTwo.Reset();
        _stepTwoOne.Reset();
        _stepTwoTwo.Reset();
        _stepTwoThree.Reset();

        _stepThree.Reset();

        _stepZero.Apply();
        _lastMapChangeApplied = _stepZero;
    }

     protected override  void InitializeAllSteps()
    {
        _stepZero.Initialize(this);
        _stepOne.Initialize(this);
        _stepOneOne.Initialize(this);
        _stepOneTwo.Initialize(this);
        _stepOneThree.Initialize(this);
        _stepOneFour.Initialize(this);

        _stepTwo.Initialize(this);
        _stepTwoOne.Initialize(this);
        _stepTwoTwo.Initialize(this);
        _stepTwoThree.Initialize(this);

        _stepThree.Initialize(this);
    }

     protected override  void OnChangeMap(int mapId, int changeId)
    {
        if (mapId != ConstantValues.SECOND_MAP_ID) return;

        if (changeId == SecondMapChanges.UNLOCK_FIRST_STEP)
        {
            Debug.Log("Step (1)");
            ApplyStepChange(_stepOne);
            return;
        }

        if (changeId == SecondMapChanges.UNLOCK_FIRST_STEP_ONE)
        {
            Debug.Log("Step (1) - 1");
            ApplyStepChange(_stepOneOne);
            return;
        }

        if (changeId == SecondMapChanges.UNLOCK_FIRST_STEP_TWO)
        {
            Debug.Log("Step (1) - 2");
            ApplyStepChange(_stepOneTwo);
            return;
        }

        if (changeId == SecondMapChanges.UNLOCK_FIRST_STEP_THREE)
        {
            Debug.Log("Step (1) - 3");
            ApplyStepChange(_stepOneThree);
            return;
        }

        if (changeId == SecondMapChanges.UNLOCK_FIRST_STEP_FOUR)
        {
            Debug.Log("Step (1) - 4");
            ApplyStepChange(_stepOneFour);
            return;
        }

        if (changeId == SecondMapChanges.UNLOCK_SECOND_STEP)
        {
            Debug.Log("Step (2)");
            ApplyStepChange(_stepTwo);
            return;
        }

        if (changeId == SecondMapChanges.UNLOCK_SECOND_STEP_ONE)
        {
            Debug.Log("Step (2) - 1");
            ApplyStepChange(_stepTwoOne);
            return;
        }

        if (changeId == SecondMapChanges.UNLOCK_SECOND_STEP_TWO)
        {
            Debug.Log("Step (2) - 2");
            ApplyStepChange(_stepTwoTwo);
            return;
        }

        if (changeId == SecondMapChanges.UNLOCK_SECOND_STEP_THREE)
        {
            Debug.Log("Step (2) - 3");
            ApplyStepChange(_stepTwoThree);
            return;
        }

        if (changeId == SecondMapChanges.UNLOCK_THIRD_STEP)
        {
            Debug.Log("Step (3)");
            ApplyStepChange(_stepThree);
            return;
        }

        if (changeId == SecondMapChanges.HACK_TEST_BOSS)
        {
            TestUnlockBoss();
        }
    }

    private void TestUnlockBoss()
    {
        ApplyStepChange(_stepThree);
    }

}