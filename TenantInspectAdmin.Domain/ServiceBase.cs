using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TenantInspectAdmin.Domain
{
    public abstract class ServiceBase
    {
        protected readonly MapperConfiguration _mapperConfiguration;
        public ServiceBase()
        {
            _mapperConfiguration = new MapperConfiguration(mc =>
            {
               
            });
        }
    }
}
