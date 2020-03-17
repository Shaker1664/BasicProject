using System;
using System.Collections.Generic;
using System.Text;

namespace BasicProject.Services
{
    public interface ITaxService
    {
        decimal TaxAmount(decimal totalAmount);
    }
}
