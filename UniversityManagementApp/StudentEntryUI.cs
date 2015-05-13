using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Sql;
using System.Configuration;

namespace UniversityManagementApp
{
    public partial class StudentEntryUI : Form
    {
        public StudentEntryUI()
        {
            InitializeComponent();
            deleteButton.Visible=false;
        }
        string connectionString =
                 ConfigurationManager.ConnectionStrings["UnivertyManagementDB"].ConnectionString;

         bool isUpdateModeOn = false;
         private int studentId;

        private void saveButton_Click(object sender, EventArgs e)
        {
            string name =nameTextButton.Text;
            string regNo = regNoTextBox.Text;
            string adress = addressTexrBox.Text;
            if (isUpdateModeOn)
            {
                deleteButton.Visible = true;
                SqlConnection connection = new SqlConnection(connectionString);


                //2. write query 

                string query = "UPDATE Students SET Name ='" + name + "', Address ='" + adress + "' WHERE ID = '" + studentId + "'";


                // 3. execute query 

                SqlCommand command = new SqlCommand(query, connection);

                connection.Open();
                int rowAffected = command.ExecuteNonQuery();
                connection.Close();
                if (rowAffected > 0)
                {
                    MessageBox.Show("Updated Successfully!");

                    saveButton.Text = "Save";
                    studentId = 0;
                    isUpdateModeOn = false;
                    regNoTextBox.Enabled = true;
                    ShowAllStudents();

                }
                else
                {
                    MessageBox.Show("Update Failed!");
                }

            }
            else
            {

                if (IsRegNoExists(regNo))
                {
                    MessageBox.Show("Reg no Exists!");
                    return;
                }

                //1.Connect to Database 

                string connectionString =
                    ConfigurationManager.ConnectionStrings["UnivertyManagementDB"].ConnectionString;
                SqlConnection connection = new SqlConnection(connectionString);

                //2.Write Query
                string query = "INSERT INTO Students Values('" + name + "','" + regNo + "','" + adress + "')";

                //3.Execyute Query
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                int rowAffected = command.ExecuteNonQuery();
                connection.Close();

                if (rowAffected > 0)
                {
                    MessageBox.Show("Insertion Succesfully");
                    ShowAllStudents();
                }
                else
                {
                    MessageBox.Show("Insertion Failed");
                }
            }

        }

        public bool IsRegNoExists(string regNo)
        {

          
            SqlConnection connection = new SqlConnection(connectionString);

            //2.Write Query
            string query = "SELECT * FROM Students Where RegNo='"+regNo+"' ";
            bool isRegNoExists = false;
            //3.Execyute Query
            SqlCommand command = new SqlCommand(query, connection);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                isRegNoExists = true;
                break;
            }
            reader.Close();
            connection.Close();
            return isRegNoExists;

        }

        private void showButton_Click(object sender, EventArgs e)
        {
           ShowAllStudents();

        }

        public void LoadStudentListView(List<Student> students)
        {
            showListView.Items.Clear();
            foreach (var student in students)
            {

               
                ListViewItem item = new ListViewItem(student.ID.ToString());
                item.SubItems.Add(student.Name);
                item.SubItems.Add(student.RegNo);
                item.SubItems.Add(student.Address);

                showListView.Items.Add(item);
            }


        }

        public void ShowAllStudents()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["UnivertyManagementDB"].ConnectionString;
            SqlConnection connection = new SqlConnection(connectionString);

            //2.Write Query
            string query = "SELECT * FROM Students";

            //3.Execyute Query
            SqlCommand command = new SqlCommand(query, connection);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();

            List<Student> studentList = new List<Student>();
            while (reader.Read())
            {
                Student student = new Student();
                student.ID = int.Parse(reader["ID"].ToString());
                student.Name = (reader["Name"].ToString());
                student.RegNo = (reader["RegNo"].ToString());
                student.Address = (reader["Address"].ToString());

                studentList.Add(student);

            }
            LoadStudentListView(studentList);
            reader.Close();
            connection.Close();

            
        }

        private void StudentEntryUI_Load(object sender, EventArgs e)
        {
            ShowAllStudents();
        }

        private void showListView_DoubleClick(object sender, EventArgs e)
        {
            ListViewItem item = showListView.SelectedItems[0];
            int id = int.Parse(item.Text.ToString());

            Student student = GetStudentByID(id);

            if (student != null)
            {
                isUpdateModeOn = true;
                saveButton.Text = "Update";
                studentId = student.ID;

                nameTextButton.Text = student.Name;
                regNoTextBox.Text = student.RegNo;
                addressTexrBox.Text = student.Address;

            }

        }

        public Student GetStudentByID(int id)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["UnivertyManagementDB"].ConnectionString;
            SqlConnection connection = new SqlConnection(connectionString);

            //2.Write Query
            string query = "SELECT * FROM Students WHERE ID='"+id+"'";

            //3.Execyute Query
            SqlCommand command = new SqlCommand(query, connection);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();

            List<Student> studentList = new List<Student>();
            while (reader.Read())
            {
                Student student = new Student();
                student.ID = int.Parse(reader["ID"].ToString());
                student.Name = (reader["Name"].ToString());
                student.RegNo = (reader["RegNo"].ToString());
                student.Address = (reader["Address"].ToString());

                studentList.Add(student);

            }
            
            reader.Close();
            connection.Close();
            return studentList.FirstOrDefault();

        }

        
    }
    
}
