using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Status
{
    public UnitCode unitCode { get; } // 바꿀 수 없게 get만
    public string name { get; set; }
    public int maxHp { get; set; }
    public int nowHp { get; set; }
    public int atkDmg { get; set; }
    public float atkSpeed { get; set; }
    public float moveSpeed { get; set; }
    public float atkRange { get; set; }
    public float fieldOfVision { get; set; }

    public Status(UnitCode unitCode, string name, int maxHp, int atkDmg, float atkSpeed, float moveSpeed, float atkRange, float fieldOfVision)
    {
        this.unitCode = unitCode;
        this.name = name;
        this.maxHp = maxHp;
        nowHp = maxHp;
        this.atkDmg = atkDmg;
        this.atkSpeed = atkSpeed;
        this.moveSpeed = moveSpeed;
        this.atkRange = atkRange;
        this.fieldOfVision = fieldOfVision;
    }

    public Status()
    {
    }

    public Status SetUnitStatus(UnitCode unitCode)
    {
        Status status = null;

        switch (unitCode)
        {
            case UnitCode.doggy:
                status = new Status(unitCode, "doggy", 50, 10, 1f, 8f, 0, 0);
                break;
            case UnitCode.enemy1:
                status = new Status(unitCode, "Enemy1", 100, 3, 1.5f, 2f, 1.5f, 7f);
                break;
            case UnitCode.enemy2:
                status = new Status(unitCode,"enemy2",100, 10, 1.5f, 2f, 1.5f, 7f);
                break;
            case UnitCode.boss:
                status = new Status(unitCode,"boss",100, 10, 1.5f, 2f, 1.5f, 14f);
                break;
        }
        return status;
    }
}
