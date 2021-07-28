using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets
{
    public abstract class ASyncMemberInterpolator<T> : ASyncMemberInterpolatorBase
    {
        public T LastValue { get; set; }
        public T CurrentValue { get; set; }
        internal override Type GenericType => typeof(T);
    }
}
