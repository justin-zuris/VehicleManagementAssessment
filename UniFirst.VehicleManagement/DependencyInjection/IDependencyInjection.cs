using System;

namespace UniFirst.VehicleManagement
{
    public interface IDependencyInjection
    {
        T Resolve<T>() where T : class;

        object Resolve(Type type);
    }
}