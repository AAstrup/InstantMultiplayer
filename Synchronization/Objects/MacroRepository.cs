using System;
using System.Collections.Generic;
using System.Text;

namespace InstantMultiplayer.Synchronization.Objects
{
    public class MacroRepository: ARepositoryBase
    {
        public static MacroRepository Instance => _instance ?? (_instance = new MacroRepository());
        private static MacroRepository _instance;

        private readonly List<ARepositoryBase> _repositores;

        public MacroRepository()
        {
            _repositores = new List<ARepositoryBase>
            {
                ResourceRepository.Instance,
                ReferenceRepository.Instance
            };
        }

        public override bool TryGetObject(int id, Type type, out UnityEngine.Object obj)
        {
            foreach (var repo in _repositores)
                if (repo.TryGetObject(id, type, out obj))
                    return true;
            obj = null;
            return false;
        }
    }
}
