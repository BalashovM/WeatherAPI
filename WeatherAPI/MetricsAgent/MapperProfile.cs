using AutoMapper;
using MetricsAgent.DAL.Models;
using MetricsAgent.Responses;
namespace MetricsAgent
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<CpuMetric, CpuMetricDto>();
            CreateMap<DotNetMetric, DotNetMetricDto>();
            CreateMap<NetworkMetric, NetworkMetricDto>();
            CreateMap<HddMetric, HddMetricDto>();
            CreateMap<RamMetric, RamMetricDto>();
        }
    }
}