using System;
using System.Collections.Generic;
using AutoMapper;
using Nexus.Service.Profiles;

namespace Nexus.Tests
{
    public static class MapperHelper
    {
        public static IRuntimeMapper CreateNewMapperInstance(IEnumerable<Profile> profiles)
        {
            MapperConfiguration mapperConfiguration = new MapperConfiguration(config =>
            {
                foreach (var profile in profiles)
                {
                    config.AddProfile(profile);
                }
            });

            Mapper mapper = new Mapper(mapperConfiguration);
            return mapper.DefaultContext.Mapper;
        }

        /// <summary>
        /// Bazı tanımlanmış profiller ile donatılmış yeni bir Mapper örneği dönderir.
        /// </summary>
        /// <returns></returns>
        public static IRuntimeMapper CreateNewMapperInstance()
        {
            object profile = new BaseMappingsProfile();
            MapperConfiguration mapperConfiguration = new MapperConfiguration(config =>
            {
                config.AddProfile<BaseMappingsProfile>();
                config.AddProfile<NoteProfile>();
                config.AddProfile<ProjectProfile>();
            });
            
            Mapper mapper = new Mapper(mapperConfiguration);
            return mapper.DefaultContext.Mapper;
        }
    }
}
