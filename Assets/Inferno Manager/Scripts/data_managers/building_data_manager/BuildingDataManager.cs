using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class BuildingDataManager
{
    public static string BUILDINGS_TYPE_FILE_PATH = "Assets/Resources/buildings_type.csv";
    public static string BUILDINGS_DEF_FILE_PATH = "Assets/Resources/buildings_def.csv";

    private List<BuildingTypeData> typeDataList = new List<BuildingTypeData>();

    public BuildingDataManager()
    {
        loadTypesData();
        loadDefsData();
    }

    private void loadTypesData()
    {
        StreamReader sr = new StreamReader(BUILDINGS_TYPE_FILE_PATH);

        sr.ReadLine(); //Pula a primeira linha

        while (!sr.EndOfStream)
        {
            string[] lineDataList = sr.ReadLine().Split(',');

            BuildingTypeData currentTypeData = new BuildingTypeData();

            currentTypeData.id = int.Parse( lineDataList[0] );
            currentTypeData.name = lineDataList[1];
            currentTypeData.info = lineDataList[2];
            currentTypeData.tileX = int.Parse(lineDataList[3]);
            currentTypeData.tileZ = int.Parse(lineDataList[4]);
            currentTypeData.levelMax = int.Parse(lineDataList[5]);
            currentTypeData.category = int.Parse(lineDataList[6]);
            currentTypeData.maxCount = lineDataList[7].Replace(";", ",");

            typeDataList.Add(currentTypeData);
        }

        sr.Close();
    }

    private void loadDefsData()
    {
        StreamReader sr = new StreamReader(BUILDINGS_DEF_FILE_PATH);

        sr.ReadLine(); //Pula a primeira linha

        while (!sr.EndOfStream)
        {
            string[] lineDataList = sr.ReadLine().Split(',');

            BuildingDefData currentDefData = new BuildingDefData();
            
            currentDefData.buildId = int.Parse(lineDataList[0]);
            currentDefData.hitpoint = int.Parse(lineDataList[1]);
            currentDefData.buildGoldPrice = int.Parse(lineDataList[2]);
            currentDefData.buildelixirprice = int.Parse(lineDataList[3]);
            currentDefData.buildGemPrice = int.Parse(lineDataList[4]);
            currentDefData.buildSulfurPrice = int.Parse(lineDataList[5]);
            currentDefData.buildTime = int.Parse(lineDataList[6]);
            currentDefData.levelRequired = int.Parse(lineDataList[7]);
            currentDefData.rankRequired = int.Parse(lineDataList[8]);
            currentDefData.soulProductionInc = int.Parse(lineDataList[9]);
            currentDefData.hellfireCapacity = int.Parse(lineDataList[10]);
            currentDefData.elixirCapacity = int.Parse(lineDataList[11]);
            currentDefData.sulfurCapacity = int.Parse(lineDataList[12]);
            currentDefData.evilnessCapacity = int.Parse(lineDataList[13]);
            currentDefData.hellfireStorage = int.Parse(lineDataList[14]);
            currentDefData.elixirStorage = int.Parse(lineDataList[15]);
            currentDefData.sulfurStorage = int.Parse(lineDataList[16]);
            currentDefData.evilnessStorage = int.Parse(lineDataList[17]);
            currentDefData.hellfireProduction = int.Parse(lineDataList[18]);
            currentDefData.elixirProduction = int.Parse(lineDataList[19]);
            currentDefData.sulfurProduction = int.Parse(lineDataList[20]);
            currentDefData.evilnessProduction = int.Parse(lineDataList[21]);

            getTypeDataList(currentDefData.buildId).defDataList.Add(currentDefData);
        }

        sr.Close();
    }

    private BuildingTypeData getTypeDataList(int id)
    {
        foreach (BuildingTypeData td in typeDataList)
            if (td.id == id)
                return td;

        Debug.Log("Não existe o id " + id + " no arquivo buildings_type.csv");
        return null;
    }

    public List<BuildingTypeData> getBuildingsDataList()
    {
        return typeDataList;
    }


}
