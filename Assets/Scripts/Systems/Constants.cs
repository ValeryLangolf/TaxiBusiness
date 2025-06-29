public static class Constants
{
    public const float MinSpeed = 0.1f;
    public const float MaxSpeed = 30f;

    public const float MinWearResistance = 0.1f;
    public const float MaxWearResistance = 30f;

    public const float MinFuelEfficiency = 0.1f;
    public const float MaxFuelEfficiency = 30f;

    public const float MinMoneyRate = 1f;

    public const float RatingMultiplier = 10f;

    public const float InitialBalance = 0f;
    public const float InitialVolumeSound = 0.8f;
    public const float InitialVolumeMusic = 0.2f;

    public const float FullFuel = 1f;
    public const float OriginalPriceFuel = 20f;
    public const float FuelFillingSpeed = 0.2f;
    public const float FuelConsumptionMultiplier = 0.01f;

    public const float FullRepair = 1f;
    public const float OriginalPriceRepair = 30f;
    public const float RepairFillingSpeed = 0.2f;
    public const float WearResistanceConsumptionMultiplier = 0.004f;    
}

public static class FuelParams
{
    private static float s_CurrentPrice = Constants.OriginalPriceFuel;

    public static float CurrentPrice => s_CurrentPrice;

    public static void SetCurrentPrice(float value) =>
        s_CurrentPrice = value;

    public static float SetDefaultPrice() =>
        s_CurrentPrice = Constants.OriginalPriceFuel;
}

public static class RepairParams
{
    private static float s_CurrentPrice = Constants.OriginalPriceRepair;

    public static float CurrentPrice => s_CurrentPrice;

    public static void SetCurrentPrice(float value) =>
        s_CurrentPrice = value;

    public static float SetDefaultPrice() =>
        s_CurrentPrice = Constants.OriginalPriceRepair;
}