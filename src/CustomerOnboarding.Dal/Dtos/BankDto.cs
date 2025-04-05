﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.Dal.Dtos
{
    public class BankDto
    {
        public string Id { get; set; } = String.Empty;
        public string Name { get; set; } = String.Empty;
        public string SwiftCode { get; set; } = String.Empty;   
        public byte[]? LastChanged { get; set; } = null;
    }
}
