using Abp.Dependency;
using GraphQL.Types;
using GraphQL.Utilities;
using SoftGrid.Queries.Container;
using System;

namespace SoftGrid.Schemas
{
    public class MainSchema : Schema, ITransientDependency
    {
        public MainSchema(IServiceProvider provider) :
            base(provider)
        {
            Query = provider.GetRequiredService<QueryContainer>();
        }
    }
}