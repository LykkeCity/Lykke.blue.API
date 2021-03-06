﻿using System.ComponentModel.DataAnnotations;

namespace Lykke.blue.Api.Requests
{
    public class CreatePledgeRequest
    {
        [Required]
        public int CO2Footprint { get; set; }
        [Required]
        public int ClimatePositiveValue { get; set; }
    }
}
