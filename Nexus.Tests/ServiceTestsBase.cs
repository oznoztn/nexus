using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Nexus.Service.Profiles;

namespace Nexus.Tests
{
    public class ServiceTestsBase
    {
        public void ReInitMapper()
        {
            Mapper.Reset();
            Mapper.Initialize(config =>
            {
                config.AddProfile(new BaseMappingsProfile());
            });
        }
    }
}
