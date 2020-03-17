using System;
using System.Collections.Generic;
using System.Text;

namespace BasicProject.Services.Implementation
{
    public class TaxService : ITaxService
    {
        private decimal taxRate;
        private decimal tax;

        public decimal TaxAmount(decimal totalAmount)
        {
            if(totalAmount<= 1042)
            {
                //tax free rate
                taxRate = 0.0m;
                tax = totalAmount * taxRate;
            }
            else if(totalAmount > 1042 && totalAmount <= 3125)
            {
                //tax rates will be applied
                taxRate = 0.2m;
                //income tax
                tax = (1042 * 0.0m) + ((totalAmount - 1042) * taxRate);
            }
            else if(totalAmount> 3125 && totalAmount <= 12500)
            {
                taxRate = 0.4m;
                //income tax
                tax = (1042 * 0.0m) + ((3125 - 1042) * 0.20m)+ ((totalAmount -3125) * taxRate);
            }
            else if(totalAmount > 12500)
            {
                taxRate = 0.45m;
                //income tax
                tax = (1042 * 0.0m) + ((3125 - 1042) * 0.20m) + ((12500 - 3125) * 0.4m) + ((totalAmount - 12500) * taxRate);
            }

            return tax;
        }
    }
}
