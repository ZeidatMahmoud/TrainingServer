using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace androidapp.Models
{
    public class TFilter : IOperationFilter
    {
        public readonly TFilter _filter ;
        public TFilter(TFilter filter)
        {
            _filter = filter;

        }
        public void Apply(Operation operation, OperationFilterContext context)
        {
            throw new NotImplementedException();
        }
    }
}
