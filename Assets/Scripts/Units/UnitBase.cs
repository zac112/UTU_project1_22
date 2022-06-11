using System.Collections;
using System.Collections.Generic;

public class UnitBase
{
    public int hp {get; private set;}
    public int attack {get; private set;}
    public readonly EnemyType type;

    public static UnitBase Goblin(){
        return new UnitBase(EnemyType.Goblin,50,1);
    }

    public static UnitBase GoblinBrute(){
        return new UnitBase(EnemyType.GoblinBrute,100,3);
    }

    public static UnitBase Knight(){
        return new UnitBase(EnemyType.Knight,100,3);
    }

    public UnitBase(EnemyType type, int hp, int attack){
        this.hp = hp;
        this.attack = attack;
        this.type = type;
    }


}
