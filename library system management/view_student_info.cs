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
    public partial class view_student_info : Form
    {
        SqlConnection con = new SqlConnection(@" Data Source=DESKTOP-UFP98S6\SQLEXPRESS;Initial Catalog=Library_Management_System;Integrated Security=True;Pooling=False");

        public view_student_info()
        {
            InitializeComponent();
        }

        private void view_student_info_Load(object sender, EventArgs e)
        {

            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            con.Open();

            fill_grid();

           
        }
        public void fill_grid()
        {
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select * from student_info";
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }
        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                dataGridView1.Columns.Clear();
                dataGridView1.Refresh();
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "select * from student_info where Student_Name like ('%"+textBox1.Text +"%')";
                cmd.ExecuteNonQuery();
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int i;
            i = Convert.ToInt32(dataGridView1.SelectedCells[0].Value.ToString());
            SqlCommand cmd = con. CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select * from student_info where id = "+i+"";
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);

            foreach(DataRow dr in dt.Rows)
            {
                studentname.Text = dr["Student_Name"].ToString();
                enrollmentno.Text = dr["Student_Enrollment_No"].ToString();
                department.Text = dr["Student_Department"].ToString();
                sem.Text = dr["Student_Sem"].ToString();
                contact.Text = dr["Student_Contact"].ToString();
                email.Text = dr["Student_Email"].ToString();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int i;
            i = Convert.ToInt32(dataGridView1.SelectedCells[0].Value.ToString());

            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "update student_info set Student_Name='"+studentname .Text +"',Student_Enrollment_No='"+enrollmentno.Text +"',Student_Department='"+department .Text +"',Student_Sem='"+sem .Text +"',Student_Contact='"+contact .Text +"',Student_Email='"+email .Text +"' where id="+i+" ";
            cmd.ExecuteNonQuery();
            fill_grid();
            MessageBox.Show("record updated succesfully");
        }
    }
}
