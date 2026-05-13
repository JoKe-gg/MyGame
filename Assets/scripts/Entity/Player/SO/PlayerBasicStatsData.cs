using System;
using UnityEngine;

[Serializable]
public class PlayerBasicStatsData
{
    [SerializeField] private int _hP;
    [SerializeField] private int _xP;
    public int HP => _hP;
    public int XP => _xP;
}
