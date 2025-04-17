using Csla;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.BusinessLibrary.BaseTypes
{
    public interface ILimit : IBusinessBase
    {
        int Id { get; }
        decimal? Minimum { get; set; }
        decimal? Maximum { get; set; }
    }
}
