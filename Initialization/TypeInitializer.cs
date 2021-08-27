using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InstantMultiplayer.Initialization
{
    public class TypeInitializer
    {
        public static TypeInitializer Instance => _instance ?? (_instance = new TypeInitializer());
        private static TypeInitializer _instance;

        private TypeInitializer() { }

        public bool Initialized { get; private set; }

        public bool Initialize()
        {
            if (Initialized)
                return false;
            try
            {
                Internal_Initialize();
            }
            catch(Exception e)
            {
                throw new Exception($"Failed to initiale {nameof(TypeInitializer)}:", e);
            }
            Initialized = true;
            return true;
        }

        private void Internal_Initialize()
        {
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => t.IsClass && !t.IsAbstract)
                .ToList();
            var typeInitHandlerType = typeof(ITypeInitializationHandler);
            var typeInitHandlers = new List<ITypeInitializationHandler>();
            foreach(var type in types.Where(t => typeInitHandlerType.IsAssignableFrom(t)))
            {
                var instance = (ITypeInitializationHandler)Activator.CreateInstance(type);
                typeInitHandlers.Add(instance);
            }
            foreach (var type in types)
                foreach (var handler in typeInitHandlers)
                    handler.HandleType(type);
        }
    }
}
