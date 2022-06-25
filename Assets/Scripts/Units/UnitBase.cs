using System.Collections;
using System.Collections.Generic;

public class UnitBase
{
    public int hp {get; set;}
    public int attack {get; set;}
    public int difficultyValue {get; set;}

    public readonly EnemyType type;

    public static UnitBase Goblin(){
        return new UnitBase(EnemyType.Goblin,50,1, 1);
    }

    public static UnitBase GoblinBrute(){
        return new UnitBase(EnemyType.GoblinBrute,100,3, 2);
    }

    public static UnitBase Knight(){
        return new UnitBase(EnemyType.Knight,100,3, 3);
    }

    public UnitBase(EnemyType type, int hp, int attack, int difficulty){
        this.hp = hp;
        this.attack = attack;
        this.type = type;
    }


}
