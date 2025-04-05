﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.DalMock.Entitites
{
    public class BranchEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = String.Empty;
        public string BranchCode { get; set; } = String.Empty;
        public byte[]? LastChanged { get; set; } = null;
        public string BankId { get; set; } = String.Empty;
    }
}
