﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
namespace shop_op
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private SqlConnection connection;
        private DataSet dataSt;
        

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'database1DataSet.Goods' table. You can move, or remove it, as needed.
            this.goodsTableAdapter.Fill(this.database1DataSet.Goods);
            string constr = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Database1.mdf;Integrated Security=True";
            connection = new SqlConnection(constr);
            try // try open connection
            { connection.Open(); }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.ToString());
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            
            string g_id = textBox1.Text;
            string g_name = textBox2.Text;
            double g_price = Convert.ToDouble(textBox3.Text);
           if (textBox3.Text == null) {
                g_price = 0;
                }
            int g_unit_multip = Convert.ToInt32(numericUpDown1.Value);
            string g_unit = textBox4.Text;
            int g_quantity = Convert.ToInt32(textBox5.Text);
            // ค้นหาสินค้าด้วย Serial No (g_id)
            SelectData(g_id);
            if (textBox1.Text == null || textBox5.Text == null) //ถ้าช่อง Serial No หรือ จำนวนสินค้า เป็น null แสดง กรุณากรอก Serial No และ จำนวนสินค้า
            { 
                MessageBox.Show("กรุณากรอก Serial No และ จำนวนสินค้า");
            }
            else // SerialNo กับ จำนวนสินค้า ไม่เป็น null
            {
                if (dataSt.Tables["check"].Rows.Count == 0) // ถ้าไม่มีสินค้าในตารางจะทำการสร้างสินค้าใหม่ลงตาราง
                {

                    string stmt2 = "INSERT INTO Goods(Serial_no,g_name,g_price,g_unitnum,g_unit,g_quantity) VALUES (@g_id,@g_name, @g_price, @g_unit_multip , @g_unit,@g_quantity);";
                    SqlCommand cm2 = new SqlCommand(stmt2, connection);

                    cm2.Parameters.Clear();
                    cm2.Parameters.AddWithValue("g_id", g_id);
                    cm2.Parameters.AddWithValue("g_name", g_name);
                    cm2.Parameters.AddWithValue("g_price", g_price);
                    cm2.Parameters.AddWithValue("g_unit_multip", g_unit_multip);
                    cm2.Parameters.AddWithValue("g_unit", g_unit);
                    cm2.Parameters.AddWithValue("g_quantity", g_quantity);
                    cm2.ExecuteNonQuery();
                    // โหลดขอมูลลงตารางใหม่
                    loadData();
                    dataGridView1.DataSource = dataSt.Tables["goods"];
                    MessageBox.Show("เพิ่มสินค้าใหม่แล้ว");
                }
                else // ถ้ามีสืนค้าในตารางแล้วจะทำการเพิ่มสินค้าที่มีอยู่เดิมโดยใช้ Serial no ค้นหาและ g_quantity ของใหม่
                {
                    // นำค้า g_quantity เพื่อมาคำนวนหา g_quantity ใหม่
                    int db_quantity = Convert.ToInt32(dataSt.Tables["check"].Rows[0]["g_quantity"]);
                    int new_quantity = db_quantity + g_quantity;
                    string stmt2 = "UPDATE Goods SET g_quantity = @new_quantity WHERE Serial_no = @g_id;";
                    SqlCommand cm2 = new SqlCommand(stmt2, connection);
                    cm2.Parameters.Clear();
                    cm2.Parameters.AddWithValue("new_quantity", new_quantity);
                    cm2.Parameters.AddWithValue("g_id", g_id);
                    cm2.ExecuteNonQuery();
                    // โหลดขอมูลลงตารางใหม่
                    loadData();
                    dataGridView1.DataSource = dataSt.Tables["goods"];
                    MessageBox.Show("แก้ไขจำนวนแล้ว");
                }
            }
        }
        private void SelectData(String g_id){
           // ค้นหาข้อมูลด้วย Serial No 
            string stmt = "SELECT * from Goods WHERE (Serial_no = @g_id);";
            SqlCommand cm = new SqlCommand(stmt, connection);
            SqlDataAdapter adapter = new SqlDataAdapter(cm);
            cm.Parameters.Clear();
            cm.Parameters.AddWithValue("g_id", g_id);
            cm.ExecuteNonQuery();
            dataSt = new DataSet();
            adapter.Fill(dataSt, "check");
            
        }

       private void loadData(){
            // รับค่าจาก DB ใหม่เพื่อเตรียมแก้ไข dataGridView
            string stmt = "SELECT * from Goods;";
            SqlCommand cm = new SqlCommand(stmt, connection);
            SqlDataAdapter adapter = new SqlDataAdapter(cm);
   
            cm.ExecuteNonQuery();
            dataSt = new DataSet();
            adapter.Fill(dataSt, "goods");
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            //สำหรับ Refresh ข้อมูลในตาราง
            loadData();
            dataGridView1.DataSource = dataSt.Tables["goods"];
        }
    }
}
