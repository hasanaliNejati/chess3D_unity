using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Chess
{
    [CreateAssetMenu(fileName = "Table generate data", menuName = "SciptableObject/table generate data", order = 1)]
    public class TableGenetareData : SerializedScriptableObject
    {
        [Header("P,B,R,K,Q,King")]
        [TableMatrix()]
        public string[,] table = new string[8, 8];

        [Button("clear")]
        public void ClearTble()
        {
            table = new string[8, 8];
        }
    }
}