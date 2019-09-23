using System;
using AutoMapper;
using Nexus.Areas.Admin.Models;
using Nexus.Service.DTOs;
using Nexus.Shared;

namespace Nexus.Profiles
{
    public class ProjectModelProfile : Profile
    {
        public ProjectModelProfile()
        {
            CreateMap<ProjectDto, ProjectModel>()
                .ForMember(dest => dest.IsOngoingProject, o => o.MapFrom(source => !source.DateFinished.HasValue))
                .ForPath(dest => dest.MonthFrom, o => o.MapFrom(source => source.DateStarted.Month))
                .ForPath(dest => dest.YearFrom, o => o.MapFrom(source => source.DateStarted.Year))
                .ForMember(dest => dest.MonthTo, os =>
                {
                    os.PreCondition(c => c.DateFinished.HasValue);
                    os.MapFrom(source => source.DateFinished.Value.Month);
                })
                .ForMember(dest => dest.YearTo, o =>
                {
                    o.PreCondition(c => c.DateFinished.HasValue);
                    o.MapFrom(source => source.DateFinished.Value.Year);
                });

            CreateMap<ProjectModel, ProjectDto>()
                .ForMember(dest => dest.Slug, o =>
                {
                    o.MapFrom((sourceModel, destDto) =>
                    {
                        // Adding
                        switch (sourceModel.Id)
                        {
                            case default(int) when string.IsNullOrWhiteSpace(sourceModel.Slug):
                                return Slug.Create(sourceModel.Title);
                            case default(int):
                                return sourceModel.Slug;
                        }

                        // Editing
                        if (!string.IsNullOrWhiteSpace(sourceModel.Slug))
                            return sourceModel.Slug;
                        else
                            return Slug.Create(sourceModel.Title);
                    });
                })
                .ForMember(dest => dest.DateStarted, o => o.MapFrom(source => new DateTime(source.YearFrom, source.MonthFrom, 1)))
                .ForMember(dest => dest.DateFinished, os =>
                {
                    os.PreCondition(c => c.MonthTo.HasValue);
                    os.PreCondition(c => c.YearTo.HasValue);
                    os.MapFrom(source => new DateTime(source.YearTo.Value, source.MonthTo.Value, 1));
                });
        }
    }
}