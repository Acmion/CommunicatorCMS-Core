using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace CommunicatorCms.Core
{
    public class Extension : DynamicObject
    {
        public int asd { get; set; } = 0;

        private IDictionary<string, object> _members = new Dictionary<string, object>();

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            _members[binder.Name] = value;
            return true;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object? result)
        {
            return _members.TryGetValue(binder.Name, out result);
        }
    }
}
