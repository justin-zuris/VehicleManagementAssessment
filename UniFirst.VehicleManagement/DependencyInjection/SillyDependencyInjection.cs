using System;
using System.Collections.Concurrent;
using System.Reflection;

namespace UniFirst.VehicleManagement
{
    public enum ObjectLifeTime
    {
        Transient = 0,
        Singleton
    }

    public class SimpleDependencyInjection : IDependencyInjection
    {
        private static ConcurrentDictionary<Type, Tuple<Type, ObjectLifeTime>> _typeMappings = new ConcurrentDictionary<Type, Tuple<Type, ObjectLifeTime>>();
        private static ConcurrentDictionary<Type, object> _staticObjects = new ConcurrentDictionary<Type, object>();

        public SimpleDependencyInjection()
        {
        }

        public void Register<TI>(string classType, ObjectLifeTime lifetime = ObjectLifeTime.Transient)
        {
            var t2 = Assembly.GetExecutingAssembly().GetType(classType);
            _typeMappings[typeof(TI)] = new Tuple<Type, ObjectLifeTime>(t2, lifetime);
        }

        public void Register<TI, T>(ObjectLifeTime lifetime = ObjectLifeTime.Transient)
            where T : class, TI
        {
            _typeMappings[typeof(TI)] = new Tuple<Type, ObjectLifeTime>(typeof(T), lifetime);
        }

        public T Resolve<T>() where T : class
        {
            return Resolve(typeof(T)) as T;
        }

        public object Resolve(Type type)
        {
            object o = null;
            var defaultConstructor = type.IsClass ? type.GetConstructor(new Type[] { }) : null;
            if (_typeMappings.ContainsKey(type))
            {
                var mapping = _typeMappings[type];
                switch (mapping.Item2)
                {
                    case ObjectLifeTime.Singleton:
                        if (_staticObjects.ContainsKey(type))
                        {
                            o = _staticObjects[type];
                        }
                        else
                        {
                            o = InstantiateType(mapping.Item1);
                            _staticObjects[type] = o;
                        }
                        break;

                    case ObjectLifeTime.Transient:
                    default:
                        o = InstantiateType(mapping.Item1);
                        break;
                }
            }
            else if (defaultConstructor != null)
            {
                o = defaultConstructor.Invoke(new object[] { });
            }
            else
            {
                throw new Exception(string.Format("Interface with type \"{0}\" not registered in IoC Container or class has no default constructor.", type.ToString()));
            }

            return o;
        }

        private object InstantiateType(Type type)
        {
            var bestConstructor = GetBestConstructor(type);
            var parms = bestConstructor.GetParameters();
            if (bestConstructor == null)
            {
                throw new Exception(string.Format("Type \"{0}\" cannot be instantiated.", type.ToString()));
            }
            else
            {
                var args = new object[parms.Length];
                for (int i = 0; i < args.Length; i++)
                {
                    args[i] = Resolve(parms[i].ParameterType);
                }
                return bestConstructor.Invoke(args);
            }
        }

        private static bool HasDefaultConstructor(Type t)
        {
            var defaultConstructor = t.GetConstructor(new Type[] { });
            return defaultConstructor != null;
        }

        private static ConstructorInfo GetBestConstructor(Type type)
        {
            int bestParmCount = -1;
            ConstructorInfo bestConstructor = null;
            foreach (var ctor in type.GetConstructors())
            {
                bool valid = true;
                var parms = ctor.GetParameters();
                var paramCount = parms.Length;
                foreach (var param in parms)
                {
                    if (!_typeMappings.ContainsKey(param.ParameterType) && !HasDefaultConstructor(param.ParameterType))
                    {
                        valid = false;
                        break;
                    }
                }
                if (valid && paramCount > bestParmCount)
                {
                    bestParmCount = paramCount;
                    bestConstructor = ctor;
                }
            }

            return bestConstructor;
        }
    }
}