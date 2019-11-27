using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shop_op
{
    class Goods
    {
        private string Serial_no;
        private string g_name;
        private double g_price;
        private string g_unitnum;
        private string g_unit;

        public string Serial_no1 { get => Serial_no; set => Serial_no = value; }
        public string G_name { get => g_name; set => g_name = value; }
        public double G_price { get => g_price; set => g_price = value; }
        public string G_unitnum { get => g_unitnum; set => g_unitnum = value; }
        public string G_unit { get => g_unit; set => g_unit = value; }
    }
}
