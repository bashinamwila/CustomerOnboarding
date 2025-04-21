using Csla;
using CustomerOnboarding.Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CustomerOnboarding.BusinessLibrary.Types
{
    [Serializable]
    public class VariableTypeTypeList : ReadOnlyListBase<VariableTypeTypeList, VariableTypeTypeInfo>
    {
        public VariableTypeTypeInfo? GetPattern(string pattern)
        {
            foreach (var info in this)
            {
                if (Regex.IsMatch(pattern, info.Pattern))
                {
                    return info;
                }
            }
            return null;
        }

        [Fetch]
        private void Fetch([Inject] IVariableTypeDal dal,
            [Inject] IChildDataPortal<VariableTypeTypeInfo> portal)
        {
            IsReadOnly = false;
            var list = dal.Fetch();
            foreach (var item in list)
                this.Add(portal.FetchChild(item));
            IsReadOnly = true;
        }
    }
}
