using System;

namespace InstantMultiplayer.Initialization
{
    public abstract class ATypeInitializationHandler<T>: ITypeInitializationHandler
    {
        private readonly Type _type;
        public ATypeInitializationHandler()
        {
            _type = typeof(T);
        }

        public void HandleType(Type type)
        {
            if (_type.IsAssignableFrom(type))
                HandleGenericType(type);
        }

        public abstract void HandleGenericType(Type type);
    }
}
