using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A2MohammadSajidNadaf
{
    class CalculationHistory
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Expression { get; set; }
        public int Result { get; set; }
        public DateTime Timestamp { get; set; }
    }

    public CalculationHistory()
    {
        Timestamp = DateTime.Now;
    }
}
