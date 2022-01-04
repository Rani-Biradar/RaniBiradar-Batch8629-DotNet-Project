using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Libray_Management_Project
{
    public partial class return_books : Form
    {
        SqlConnection con = new SqlConnection(@" Data Source=DESKTOP-UFP98S6\SQLEXPRESS;Initial Catalog=Library_Management_System;Integrated Security=True;Pooling=False");

        public return_books()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            panel2.Visible = true;
            fill_grid(textBox1.Text);
        }

        private void return_books_Load(object sender, EventArgs e)
        {
            if(con.State ==ConnectionState.Open )
            {
                con.Close();
            }
            con.Open();
        }
        public void fill_grid(string enrollment)
        {
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select *from issue_books where Student_Enrollment_No='"+enrollment .ToString ()+"'and Book_Return_Date=''";
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            panel3.Visible = true;
            int i;
            i = Convert.ToInt32(dataGridView1.SelectedCells[0].Value.ToString());
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select *from issue_books where id="+i+"";
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                lbl_booksname.Text = dr["Books_Name"].ToString();
                lbl_issuedate.Text = Convert.ToString(dr["Books_Issue_Date"].ToString());
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int i;
            i = Convert.ToInt32(dataGridView1.SelectedCells[0].Value.ToString());

            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "update issue_books set Book_Return_Date='" + dateTimePicker1.Value.ToString() +"' where id=" + i + "";
            cmd.ExecuteNonQuery();

            SqlCommand cmd1 = con.CreateCommand();
            cmd1.CommandType = CommandType.Text;
            cmd1.CommandText = "update books_info set Available_Quantity=Available_Quantity+1 where Books_Name='"+lbl_booksname.Text +"'";
            cmd1.ExecuteNonQuery();

            MessageBox.Show("Books Return Successfully");
            
            panel3.Visible = true;
            fill_grid(textBox1.Text);
        }
    }
}
