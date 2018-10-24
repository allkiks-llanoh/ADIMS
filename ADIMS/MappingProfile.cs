using AutoMapper;
using ADIMS.Models;
using ADIMS.ViewModels;
using ADIMS.ViewModels.DataCapture_CropProduction;

namespace ADIMS
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<PreliminaryCropCuttingViewModel, preliminary_cropcutting_exercise>();

            CreateMap<ActualCropCuttingViewModel, actual_cropcutting_exercise>();

            CreateMap<AddCountyWeatherDataViewModel, county_weather_data>();
            CreateMap<AddCropProductionStatisticsViewModel, crop_production_statistics>();
            CreateMap<AddDisasterWarningDataViewModel, disaster_warning_data>();
            CreateMap<AddEarlyWarningViewModel, early_warning>();
            CreateMap<AddFoodStocksDataViewModel, foodstocks_data>();
            CreateMap<AddGovtSubsidizedFertlizerViewModel, govt_subsidized_fertilizer>();
            CreateMap<AddInputCostAndAvailabilityViewModel, input_cost_and_availability>();
            CreateMap<AddPlantingFertilizerViewModel, planting_fertilizer>();
            CreateMap<AddPriceTrendViewModel, price_trend>();
            CreateMap<AddTopDressingFertilizerViewModel, top_dressing_fertilizer>();

            CreateMap<EditCropViewModel, crop>();
        }
    }
}