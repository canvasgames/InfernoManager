using System.Collections.Generic;

public class BuildingTypeData 
{
    public int id;
    public string name;
    public string info;
    public int tileX;
    public int tileZ;
    public int levelMax;
    public int category;
    public string maxCount;

    public List<BuildingDefData> defDataList = new List<BuildingDefData>();

    public string getObjectStringData()
    {
        return (string) (id + ", " + name + ", " + info + ", " + tileX + ", " + tileZ + ", " + levelMax + ", " + category + ", " + maxCount);
    }
}
