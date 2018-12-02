using AutoMapper;
using VRdkHRMsysBLL.Interfaces;

namespace VRdkHRMsysBLL.Helpers
{
    public class MapHelper : IMapHelper
    {
        public  TypeToMapTo Map<TypeToMapFrom, TypeToMapTo>(TypeToMapFrom model)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<TypeToMapFrom, TypeToMapTo>()).CreateMapper();

            return mapper.Map<TypeToMapFrom, TypeToMapTo>(model);
        }

        public  TypeToMapTo[] MapCollection<TypeToMapFrom, TypeToMapTo>(TypeToMapFrom[] model)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<TypeToMapFrom, TypeToMapTo>()).CreateMapper();

            return mapper.Map<TypeToMapFrom[], TypeToMapTo[]>(model);
        }

        public void MapChanges<TypeToMapFrom, TypeToMapTo>(TypeToMapFrom source, TypeToMapTo destination)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<TypeToMapFrom, TypeToMapTo>()).CreateMapper();

            mapper.Map(source, destination);
        }

        public void MapNestedChanges<OuterTypeToMapFrom, OuterTypeToMapTo,
                                          FirstInnerTypeToMapFrom, FirstInnerTypeToMapTo,
                                          SecondInnerTypeToMapFrom, SecondInnerTypeToMapTo>(OuterTypeToMapFrom source, OuterTypeToMapTo destination)
        {
            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<OuterTypeToMapFrom, OuterTypeToMapTo>();
                cfg.CreateMap<FirstInnerTypeToMapFrom, FirstInnerTypeToMapTo>();
                cfg.CreateMap<SecondInnerTypeToMapFrom, SecondInnerTypeToMapTo>();
            }
           ).CreateMapper();

            mapper.Map(source, destination);
        }

        public OuterTypeToMapTo NestedMap<OuterTypeToMapFrom, OuterTypeToMapTo, InnerTypeToMapFrom, InnerTypeToMapTo>(OuterTypeToMapFrom source)
        {
            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<OuterTypeToMapFrom, OuterTypeToMapTo>();
                cfg.CreateMap<InnerTypeToMapFrom, InnerTypeToMapTo>();
            }
            ).CreateMapper();

            return mapper.Map<OuterTypeToMapFrom, OuterTypeToMapTo>(source);
        }

        public OuterTypeToMapTo NestedMap<OuterTypeToMapFrom, OuterTypeToMapTo,
                                          FirstInnerTypeToMapFrom, FirstInnerTypeToMapTo, 
                                          SecondInnerTypeToMapFrom,SecondInnerTypeToMapTo>(OuterTypeToMapFrom source)
        {
            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<OuterTypeToMapFrom, OuterTypeToMapTo>();
                cfg.CreateMap<FirstInnerTypeToMapFrom, FirstInnerTypeToMapTo>();
                cfg.CreateMap<SecondInnerTypeToMapFrom, SecondInnerTypeToMapTo>();
            }
            ).CreateMapper();

            return mapper.Map<OuterTypeToMapFrom, OuterTypeToMapTo>(source);
        }
    }
}
