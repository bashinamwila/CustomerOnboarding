﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.DalMock.Entitites
{
    public class BankingDetailsEntity
    {
        public string TenantId { get; set; } = String.Empty;
        public string BankId { get; set; } = String.Empty;
        public int BranchId { get; set; }
        public string AccountNumber { get; set; } = String.Empty;
        public byte[] LastChanged { get; set; } = default!;
    }
}
