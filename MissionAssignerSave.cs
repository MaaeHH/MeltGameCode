using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MissionAssignerSave
{

    public List<MissionInstance> missionRecords;
    public List<InProgressMission> inProgressMissionRecords;
    public List<UnselectableMission> unselectableMissions;

    public MissionAssignerSave(List<MissionInstance> records, List<InProgressMission> ipmRecords, List<UnselectableMission> unselectable)
    {
        this.missionRecords = records;
        this.inProgressMissionRecords = ipmRecords;
        this.unselectableMissions = unselectable;
    }
}
