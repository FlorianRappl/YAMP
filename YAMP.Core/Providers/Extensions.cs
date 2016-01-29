using System;
using System.Linq;
using System.Reflection;

namespace YAMP
{
    static class TypeExtensions
    {
        public static bool IsCastableTo(this Type from, Type to)
        {
            if (to.IsAssignableFrom(from))
                return true;

            var methods = to.GetMethods(BindingFlags.Public | BindingFlags.Static).Union(from.GetMethods(BindingFlags.Public | BindingFlags.Static))
                .Where(m => m.ReturnType == to && (m.Name == "op_Implicit" || m.Name == "op_Explicit") && m.GetParameters()[0].ParameterType == from);
            return methods.Any();
        }

        public static bool SupportsCastFrom(this Type from, object instance)
        {
            var to = instance.GetType();
            return IsCastableTo(from, to);
        }

        public static object Cast(this Type to, object instance)
        {
            var from = instance.GetType();

            if (to.IsAssignableFrom(from))
                return instance;

            var method = to.GetMethods(BindingFlags.Public | BindingFlags.Static).Union(from.GetMethods(BindingFlags.Public | BindingFlags.Static))
                .Where(m => m.ReturnType == to && (m.Name == "op_Implicit" || m.Name == "op_Explicit") && m.GetParameters()[0].ParameterType == from).First();
            return method.Invoke(null, new object[] { instance });
        }
    }
}
