using Autofac;
using QueryPresure.Postgres.App;

namespace QueryPressure.Postgres.App;

public class PostgresAppModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<PostgresConnectionProviderCreator>()
          .AsImplementedInterfaces();


    }
}