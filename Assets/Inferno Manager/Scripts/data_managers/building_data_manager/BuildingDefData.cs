public class BuildingDefData
{
    public int buildId;
    public int hitpoint;
    public int buildGoldPrice;
    public int buildelixirprice;
    public int buildGemPrice;
    public int buildSulfurPrice;
    public int buildTime;
    public int levelRequired;
    public int rankRequired;
    public int soulProductionInc;
    public int hellfireCapacity;
    public int elixirCapacity;
    public int sulfurCapacity;
    public int evilnessCapacity;
    public int hellfireStorage;
    public int elixirStorage;
    public int sulfurStorage;
    public int evilnessStorage;
    public int hellfireProduction;
    public int elixirProduction;
    public int sulfurProduction;
    public int evilnessProduction;

    public string getObjectStringData()
    {
        return buildId +
        ", " + hitpoint +
        ", " + buildGoldPrice +
        ", " + buildelixirprice +
        ", " + buildGemPrice +
        ", " + buildSulfurPrice +
        ", " + buildTime +
        ", " + levelRequired +
        ", " + rankRequired +
        ", " + soulProductionInc +
        ", " + hellfireCapacity +
        ", " + elixirCapacity +
        ", " + sulfurCapacity +
        ", " + evilnessCapacity +
        ", " + hellfireStorage +
        ", " + elixirStorage +
        ", " + sulfurStorage +
        ", " + evilnessStorage +
        ", " + hellfireProduction +
        ", " + elixirProduction +
        ", " + sulfurProduction +
        ", " + evilnessProduction;
    }
}
