using System;

namespace YAMP
{
    abstract class PropertyFunction<T> : SystemFunction where T : Value
    {
        string _propertyName;

        public PropertyFunction(string propertyName)
        {
            _propertyName = propertyName;
        }

        protected abstract object GetValue(T parameter);

        public Value Function(object obj, T parameter)
        {
            obj.GetType().GetProperty(_propertyName).SetValue(obj, GetValue(parameter), null);
            return parameter;
        }

        public virtual Value Function(T parameter)
        {
            return Function(Context.LastPlot, parameter);
        }
    }
}
