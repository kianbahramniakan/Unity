using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu]
public class GameLevelData : ScriptableObject
{
    [System.Serializable]
    public struct CatagoryRecord
    {
        public string catagoryName;
        public List<BoardData> boardData;
    }

    public List<CatagoryRecord> data;
}
