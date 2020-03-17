using System;
using System.Collections.Generic;
using System.Text;

namespace BasicProject.Services.Implementation
{
    public class NationalInsuranceContributionService : INationalInsuranceContributionService
    {
        public decimal NIRate { get; private set; }
        public decimal NIC { get; private set; }

        public decimal NIContribution(decimal totalAmount)
        {
            if(totalAmount < 719)
            {
                //lower earning limit rate & below Primary Threshold
                NIRate = 0.0m;
                NIC = 0m;
            }
            else if(totalAmount >= 19 && totalAmount <= 4167)
            {
                NIRate = .12m;
                NIC = ((totalAmount - 719) * NIRate);
            }
            else if(totalAmount > 4167)
            {
                NIRate = 0.02m;
                NIC = ((4167 - 719) * .12m) + ((totalAmount - 4167) * NIRate);
            }
            return NIC;
        }
    }
}
