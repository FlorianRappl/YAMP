namespace YAMP
{
    using System;
    using System.Linq;
    using System.Reflection;

    static class TypeExtensions
    {
        public static bool IsCastableTo(this Type from, Type to)
        {
            if (!to.IsAssignableFrom(from))
            {
                var flags = BindingFlags.Public | BindingFlags.Static;
                var methods = to.GetMethods(flags).Union(from.GetMethods(flags))
                    .Where(m => m.ReturnType == to && (m.Name.Equals("op_Implicit") || m.Name.Equals("op_Explicit")) && m.GetParameters()[0].ParameterType == from);
                return methods.Any();
            }

            return true;
        }

        public static Boolean SupportsCastFrom(this Type from, Object instance)
        {
            var to = instance.GetType();
            return IsCastableTo(from, to);
        }

        public static Object Cast(this Type to, Object instance)
        {
            var from = instance.GetType();

            if (!to.IsAssignableFrom(from))
            {
                var flags = BindingFlags.Public | BindingFlags.Static;
                var method = to.GetMethods(flags).Union(from.GetMethods(flags))
                    .Where(m => m.ReturnType == to && (m.Name == "op_Implicit" || m.Name == "op_Explicit") && m.GetParameters()[0].ParameterType == from).First();
                return method.Invoke(null, new[] { instance });
            }
                
            return instance;
        }
    }
}
