
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
public class ShopDataManager
{
    public static string SHOP_FILE_PATH = "Assets/Resources/buildings_in_shop.csv";

    private List<BuildingShopData> buildingsDataList = new List<BuildingShopData>();

    public ShopDataManager()
    {
        loadShopData();
    }

    private void loadShopData()
    {
        StreamReader sr = new StreamReader(SHOP_FILE_PATH);

        sr.ReadLine(); //Pula a primeira linha

        while (!sr.EndOfStream)
        {
            string[] lineDataList = sr.ReadLine().Split(',');
            BuildingTypeData currentTypeData = new BuildingTypeData();

            for(int i=0;i< lineDataList.Length;i++)
            {
                if(lineDataList[i].Trim() != "")
                {
                    BuildingShopData bsd = new BuildingShopData();
                    bsd.category = i;
                    bsd.buildingId = int.Parse(lineDataList[i]);
                    buildingsDataList.Add(bsd);
                }
            }
        }

        sr.Close();
    }

    public List<BuildingShopData> getShopBuildingsByCategory(int category)
    {
        List<BuildingShopData> buildingsDataListByCategory = new List<BuildingShopData>();

        foreach(BuildingShopData bd in buildingsDataList)
        {
            if (bd.category == category)
                buildingsDataListByCategory.Add(bd);
        }

        return buildingsDataListByCategory;
    }
}
