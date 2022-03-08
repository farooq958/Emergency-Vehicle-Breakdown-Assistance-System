using System;
using System.Collections.Generic;
using System.Text;

namespace EVBAS.Model
{
  public  class Mlocation
    {

        public double mlattitude { get; set; }
        public double mlongitude { get; set; }

        public int Id
        {
            get;
            set;
        }
        public string Name { get; set; }
        public string Details { get; set; }
        public string Email { get; set; }
    }
}
