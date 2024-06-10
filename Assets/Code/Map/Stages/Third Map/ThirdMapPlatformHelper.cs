using UnityEngine;

public class ThirdMapPlatformHelper : BaseMapPlatformHelper
{
    [Header("STEPS")]
    [SerializeField] 
    private MapChanges _step0;
    [SerializeField] 
    private MapChanges _step1;
    [SerializeField] 
    private MapChanges _step2;
    [SerializeField] 
    private MapChanges _step3;
    [SerializeField] 
    private MapChanges _step4;
    [SerializeField] 
    private MapChanges _step5;

    [Header("SECTIONS")]
    [SerializeField]
    private WaterSection _waterSection;
    [SerializeField]
    private SkySection _skySection;

    protected override void InitializeAllSteps()
    {
        _step0.Initialize(this);
        _step1.Initialize(this);
        _step2.Initialize(this);
        _step3.Initialize(this);
        _step4.Initialize(this);
        _step5.Initialize(this);
    }


    protected override void ResetAllSteps()
    {
        _step1.Reset();
        _step2.Reset();
        _step3.Reset();
        _step4.Reset();
        _step5.Reset();

        _step0.Apply();
        _lastMapChangeApplied = _step0;
    }

    protected override void OnChangeMap(int mapId, int changeId)
    {
        if (mapId != ConstantValues.THIRD_MAP_ID) return;

        switch (changeId)
        {
            case ThirdMapChanges.UNLOCK_STEP_1:
                ApplyStepChange(_step1);
                _waterSection.ActivateBorder(1);
                _skySection.ActivateBorder(1);
                return;
            case ThirdMapChanges.UNLOCK_STEP_2:
                ApplyStepChange(_step2);
                _waterSection.ActivateBorder(2);
                _skySection.ActivateBorder(2);
                return;
            case ThirdMapChanges.UNLOCK_STEP_3:
                ApplyStepChange(_step3);
                _waterSection.ActivateBorder(3);
                _skySection.ActivateBorder(3);
                return;
            case ThirdMapChanges.UNLOCK_STEP_4:
                ApplyStepChange(_step4);
                return;
            case ThirdMapChanges.UNLOCK_STEP_5:
                ApplyStepChange(_step5);
                return;
        }
    }

}