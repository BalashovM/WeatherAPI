using AutoMapper;
using MetricsManager.DAL.Models;
using MetricsManager.Responses;

namespace MetricsManager
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<AgentModel, AgentManagerDto>();
            CreateMap<CpuMetricModel, CpuMetricManagerDto>();
            CreateMap<DotNetMetricModel, DotNetMetricManagerDto>();
            CreateMap<NetworkMetricModel, NetworkMetricManagerDto>();
            CreateMap<HddMetricModel, HddMetricManagerDto>();
            CreateMap<RamMetricModel, RamMetricManagerDto>();
        }
    }
}