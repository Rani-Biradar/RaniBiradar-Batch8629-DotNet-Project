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
    public partial class issue_books : Form
    {
        SqlConnection con = new SqlConnection(@" Data Source=DESKTOP-UFP98S6\SQLEXPRESS;Initial Catalog=Library_Management_System;Integrated Security=True;Pooling=False");

        public issue_books()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int i = 0;
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select *from  student_info where Student_Enrollment_No ='" + txt_enrollment.Text + "'";
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            i = Convert.ToInt32(dt.Rows.Count.ToString());

            if (i == 0)
            {
                MessageBox.Show("this enrollment not found");

            }
            else
            {



                foreach (DataRow dr in dt.Rows)
                {
                    txt_studentname.Text  = dr["Student_Name"].ToString();
                    txt_studentdepartment.Text = dr["student_Department"].ToString();
                    txt_studentsem.Text = dr["Student_Sem"].ToString();
                    txt_studentcontact.Text = dr["Student_Contact"].ToString();
                    txt_studentemail.Text = dr["Student_Email"].ToString();
                   


                }

            }
        }
        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void issue_books_Load(object sender, EventArgs e)
        {
            if(con.State==ConnectionState.Open )
            {
                con.Close();
            }
            con.Open();
        }

        private void txt_booksname_KeyUp(object sender, KeyEventArgs e)
        {
            int count = 0;
            if(e.KeyCode!=Keys.Enter)
            {
                listBox1.Items.Clear();

                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "select *from  books_info where Books_Name like ('%" +txt_booksname.Text + "%')";
                cmd.ExecuteNonQuery();
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                count = Convert.ToInt32(dt.Rows.Count.ToString());

                if(count > 0)
                {
                    listBox1.Visible = true;
                    foreach(DataRow dr in dt.Rows)
                    {
                        listBox1.Items.Add(dr["Books_Name"].ToString());
                    }
                }
            }
        }

        private void txt_booksname_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.Down )
            {
                listBox1.Focus();
                //you might need to select one value to allow arrow keys

                listBox1.SelectedIndex = 0;
            }
                    
        }

        private void listBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                txt_booksname.Text = listBox1.SelectedItem.ToString();
                listBox1.Visible = false;
            }
        }

        private void listBox1_MouseClick(object sender, MouseEventArgs e)
        {
            txt_booksname.Text = listBox1.SelectedItem.ToString();
            listBox1.Visible = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int books_qty = 0;
            SqlCommand cmd2 = con.CreateCommand();
            cmd2.CommandType = CommandType.Text;
            cmd2.CommandText = "select * from books_info where books_name='"+txt_booksname .Text +"'";
            cmd2.ExecuteNonQuery();
            DataTable dt2 = new DataTable();
            SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
            da2.Fill(dt2);
            foreach (DataRow dr2 in dt2.Rows )
            {
                books_qty = Convert.ToInt32(dr2["Available_Quantity"].ToString());
            }

            if (books_qty > 0)
            {


                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "insert into issue_books values('" + txt_enrollment.Text + "','" + txt_studentname.Text + "','" + txt_studentdepartment.Text + "','" + txt_studentsem.Text + "','" + txt_studentcontact.Text + "','" + txt_studentemail.Text + "','" + txt_booksname.Text + "','" + dateTimePicker1.Value.ToString() + "','')";
                cmd.ExecuteNonQuery();

                SqlCommand cmd1 = con.CreateCommand();
                cmd1.CommandType = CommandType.Text;
                cmd1.CommandText = "update books_info set Available_Quantity = Available_Quantity-1 where Books_Name='" + txt_booksname.Text + "'";
                cmd1.ExecuteNonQuery();

                MessageBox.Show("Books issued succesfully");
            }
            else
            {
                MessageBox.Show("Books not available");
            }
        }
    }
}
