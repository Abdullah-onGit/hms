﻿using System;
using System.Collections.Generic;

namespace hms.Models
{
    public partial class CountryMaster
    {
        public int CountryMasterAutoId { get; set; }
        public string Isocode { get; set; }
        public string CountryName { get; set; }
        public int PhoneCode { get; set; }
    }
}
