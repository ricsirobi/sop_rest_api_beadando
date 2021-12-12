using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TC92HL_SOP_BEADANDO_CLIENT.data
{
    class Lego
    {
        private int id;
        private string code;
        private string name;
        private int category;
        private int price;


        public int Id { get => id; set => id = value; }
        public string Code { get => code; set => code= value; }
        public string Name { get => name; set => name = value; }
        public int Category { get => category; set => category= value; }
        public int HUFprice { get => price; set => price = value; }

        
    }
}
