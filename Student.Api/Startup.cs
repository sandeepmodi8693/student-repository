﻿using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Student.Api.Startup))]

namespace Student.Api
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
