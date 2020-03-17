using System;
using System.Collections.Generic;
using System.Text;

namespace BasicProject.Services
{
    public interface INationalInsuranceContributionService
    {
        decimal NIContribution(decimal totalAmount);
    }
}
