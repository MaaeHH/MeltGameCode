using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MeltAssignerSave
{

    public List<MeltRecord> meltRecords;
    public List<MeltRecord> meltRecordsCustom;

    public MeltAssignerSave(List<MeltRecord> records, List<MeltRecord> recordsCustom )
    {
        this.meltRecords = records;
        this.meltRecordsCustom = recordsCustom;
    }
}
