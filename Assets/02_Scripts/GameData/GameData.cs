

public class UnitBaseData
{
    public long UID;
    public int AttackPower;
    public int HP;
}

public class EnemyData : UnitBaseData
{
    public DataManager.UnitInfo RefInfo;
    public int ActionOrder { get; protected set; }

}

public class PlayerData : UnitBaseData
{
    public DataManager.CharacterInfo RefInfo;
    public bool Traciking { get; set; }
}