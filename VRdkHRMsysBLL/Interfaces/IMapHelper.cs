﻿namespace VRdkHRMsysBLL.Interfaces
{
    public interface IMapHelper
    {
        void MapRangeChanges<TypeToMapFrom, TypeToMapTo>(TypeToMapFrom[] source, TypeToMapTo[] destination);
        TypeToMapTo Map<TypeToMapFrom, TypeToMapTo>(TypeToMapFrom model);
        TypeToMapTo[] MapCollection<TypeToMapFrom, TypeToMapTo>(TypeToMapFrom[] model);
        OuterTypeToMapTo NestedMap<OuterTypeToMapFrom, OuterTypeToMapTo, InnerTypeToMapFrom, InnerTypeToMapTo>(OuterTypeToMapFrom source);
        OuterTypeToMapTo NestedMap<OuterTypeToMapFrom, OuterTypeToMapTo,
                                   FirstInnerTypeToMapFrom, FirstInnerTypeToMapTo,
                                   SecondInnerTypeToMapFrom, SecondInnerTypeToMapTo>(OuterTypeToMapFrom source);
        void MapChanges<TypeToMapFrom, TypeToMapTo>(TypeToMapFrom source, TypeToMapTo destination);
        void MapNestedChanges<OuterTypeToMapFrom, OuterTypeToMapTo,
                                          FirstInnerTypeToMapFrom, FirstInnerTypeToMapTo,
                                          SecondInnerTypeToMapFrom, SecondInnerTypeToMapTo>(OuterTypeToMapFrom source, OuterTypeToMapTo destination);
        OuterTypeToMapTo[] NestedMapCollection<OuterTypeToMapFrom, OuterTypeToMapTo,
                                         FirstInnerTypeToMapFrom, FirstInnerTypeToMapTo,
                                         SecondInnerTypeToMapFrom, SecondInnerTypeToMapTo>(OuterTypeToMapFrom[] source);
    }
}