using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductImporter.Logic.Transformations.Util
{
    public class SystemOutLogger : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            var type = invocation.TargetType.FullName;
            var method = invocation.Method.Name;
            var args = string.Join(',', invocation.Arguments.Select(x => x.ToString()));

            Console.Out.WriteLine($"Entering '{type}.{method}' with parameters '{args}'");

            invocation.Proceed();

            Console.Out.WriteLine($"Leaving '{type}.{method}' with parameters '{args}'");
        }
    }
}
