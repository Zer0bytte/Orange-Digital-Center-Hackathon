using Domains;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Orange_Digital_Center
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        string AccessToken = "";
        private async void btnGetStudents_ClickAsync(object sender, EventArgs e)
        {
            string Response = await RestGetMethodAsync(@"StudentManager/GetAllStudents");
            var Students = JsonConvert.DeserializeObject<List<TbStudent>>(Response);
            foreach (var student in Students)
            {
                dgvStudents.Rows.Add(
                    student.StudentId,
                    student.StudentName,
                    student.StudentEmail,
                    student.StudentPhone,
                    student.StudentCollege,
                    student.CreatedAt);


            }
        }
        async Task<string> RestGetMethodAsync(string APILink)
        {
            var client = new RestClient($"https://localhost:44333/api/{APILink}");
            var request = new RestRequest();
            request.AddHeader("Authorization", AccessToken);
            var Response = await client.GetAsync(request);
            return Response.Content;
        }

        void RestPost(string APILink)
        {

        }


        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private async void btnLogin_ClickAsync(object sender, EventArgs e)
        {
            LoginModel login = new LoginModel();
            login.Username = txtUsername.Text;
            login.Password = txtPassword.Text;

            AccessToken = await RestPostMethodAsync(@"Authentication/Auth", login);

            AccessToken = $"Bearer {AccessToken.Replace("\"", "")}";
            MessageBox.Show(AccessToken);
        }


        async Task<string> RestPostMethodAsync(string APILink, object JsonModel)
        {
            var client = new RestClient();
            var request = new RestRequest($"https://localhost:44333/api/{APILink}");
            request.AddJsonBody(JsonModel);
            request.AddHeader("Authorization", AccessToken);
            var Response = await client.PostAsync(request);
            return Response.Content.Replace("\"", "");

        }
        private async void btnGetStudentDegrees_ClickAsync(object sender, EventArgs e)
        {
            try
            {
                var StudentId = Convert.ToInt32(dgvStudents.SelectedRows[0].Cells[0].FormattedValue);
                var Response = await RestGetMethodAsync($"StudentManager/GetStudentExams/{StudentId}");
                var RevisionData = JsonConvert.DeserializeObject<TbRevision>(Response);
                if (RevisionData is null)
                {
                    MessageBox.Show("No Revisions Found");

                    lblStudentDegree.Text = $"Student Degree: {0}";
                    lblRightDegrees.Text = $"Student Right Degrees: {0}";
                    lblWrongDegrees.Text = $"Student Wrong Degrees: {0}";
                    return;
                }
                lblStudentDegree.Text = $"Student Degree: {RevisionData.StudentDegree}";
                lblRightDegrees.Text = $"Student Right Degrees: {RevisionData.TotalRightDegrees}";
                lblWrongDegrees.Text = $"Student Wrong Degrees: {RevisionData.TotalWrongDegrees}";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        async Task LoadCategoriesAsync()
        {
            var Response = await RestGetMethodAsync("Categroies/GetAllCategories");
            var Categories = JsonConvert.DeserializeObject<List<TbCategroie>>(Response);
            dataGridView1.Rows.Clear();
            foreach (var category in Categories)
            {
                dataGridView1.Rows.Add(
                    category.CategoryId,
                    category.CategoryName);

            }
        }
        private async void btnGetCategories_ClickAsync(object sender, EventArgs e)
        {
            try
            {

                await LoadCategoriesAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                txtCategoryName.Text = dataGridView1.SelectedRows[0].Cells[1].FormattedValue.ToString();

            }
            catch (Exception)
            {

            }

        }

        private async void btnUpdateCategory_ClickAsync(object sender, EventArgs e)
        {
            CategoryModel categoryUpdateModel = new CategoryModel();
            categoryUpdateModel.CateogryId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].FormattedValue);
            categoryUpdateModel.CategoryName = txtCategoryName.Text;
            var UpdateResponse = await RestPostMethodAsync("Categroies/UpdateCategory", categoryUpdateModel);
            MessageBox.Show(UpdateResponse);
            await LoadCategoriesAsync();

        }

        private async void btnAddCategory_ClickAsync(object sender, EventArgs e)
        {
            CategoryModel categoryUpdateModel = new CategoryModel();
            categoryUpdateModel.CategoryName = txtAddCategoryName.Text;
            var Response = await RestPostMethodAsync("Categroies/AddCategory", categoryUpdateModel);
            MessageBox.Show(Response);

            await LoadCategoriesAsync();

        }

        private async void tnDeleteCAtegory_ClickAsync(object sender, EventArgs e)
        {
            CategoryModel categoryModel = new CategoryModel();
            categoryModel.CateogryId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].FormattedValue);
            var Response = await RestPostMethodAsync("Categroies/DeleteCategory", categoryModel);
            MessageBox.Show(Response);
            await LoadCategoriesAsync();


        }

        private async void button6_ClickAsync(object sender, EventArgs e)
        {
            try
            {
                var ResponseCat = await RestGetMethodAsync("Categroies/GetAllCategories");
                var Categories = JsonConvert.DeserializeObject<List<TbCategroie>>(ResponseCat);
                foreach (var category in Categories)
                {
                    ComboboxItem Cat = new ComboboxItem();
                    Cat.Text = category.CategoryName;
                    Cat.Value = category.CategoryId;
                    ComboCategories.Items.Add(Cat);
                }

                var Response = await RestGetMethodAsync("Courses/GetAllCourses");
                var Courses = JsonConvert.DeserializeObject<List<TbCourse>>(Response);

                dgvCourses.Rows.Clear();
                foreach (var Course in Courses)
                {
                    dgvCourses.Rows.Add(
                        Course.CourseId,
                        Course.CourseName,
                        Course.CourseLevel,
                        Course.Category.CategoryName);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }
        private void dgvCourses_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                txtCourseName.Text = dgvCourses.SelectedRows[0].Cells[1].FormattedValue.ToString();
                txtCourseLevel.Text = dgvCourses.SelectedRows[0].Cells[2].FormattedValue.ToString();
                ComboCategories.SelectedText = dgvCourses.SelectedRows[0].Cells[3].FormattedValue.ToString();
            }
            catch (Exception)
            {

            }

        }

        private void btnUpdateCourse_ClickAsync(object sender, EventArgs e)
        {
            //var Response = await RestPostMethodAsync("Categroies/DeleteCategory", categoryModel);
            //MessageBox.Show(Response);

        }

        private async void button2_ClickAsync(object sender, EventArgs e)
        {
            try
            {
                dgvExams.Rows.Clear();
                ComboCourses.Items.Clear();
                var CoursesResponse = await RestGetMethodAsync("Courses/GetAllCourses");
                var Courses = JsonConvert.DeserializeObject<List<TbCourse>>(CoursesResponse);
                foreach (var course in Courses)
                {
                    ComboboxItem Crs = new ComboboxItem();
                    Crs.Value = course.CourseId;
                    Crs.Text = course.CourseName;
                    ComboCourses.Items.Add(Crs);
                }
                var ExamsResponse = await RestGetMethodAsync("Exams/GetAllExams");
                var Exams = JsonConvert.DeserializeObject<List<TbExam>>(ExamsResponse);
                foreach (var Exam in Exams)
                {
                    dgvExams.Rows.Add(Exam.ExamId, Exam.ExamName, Exam.Course.CourseName);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }

        private void tabPage5_Click(object sender, EventArgs e)
        {

        }

        private async void GetQuestions_ClickAsync(object sender, EventArgs e)
        {
            try
            {
                dgvQuestions.Rows.Clear();
                int ExamId = Convert.ToInt32(dgvExams.SelectedRows[0].Cells[0].FormattedValue);
                var ExamQuestionsResponse = await RestGetMethodAsync($"Questions/GetExamQuestions?ExamId={ExamId}");
                var Questions = JsonConvert.DeserializeObject<List<TbQuestion>>(ExamQuestionsResponse);
                foreach (var Question in Questions)
                {
                    dgvQuestions.Rows.Add(Question.Id, Question.QuestionContent, Question.QuestionRightAnswer,
                        Question.FirstChoice,
                        Question.SecondChoice,
                        Question.ThirdChoice,
                        Question.FourthChoice);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }

        private void dgvQuestions_SelectionChanged(object sender, EventArgs e)
        {
            try
            {

                txtQuestionContent.Text = dgvQuestions.SelectedRows[0].Cells[1].FormattedValue.ToString();
                txtRightAnswer.Text = dgvQuestions.SelectedRows[0].Cells[2].FormattedValue.ToString();
                txtFirstChoice.Text = dgvQuestions.SelectedRows[0].Cells[3].FormattedValue.ToString();
                txtSecondChoice.Text = dgvQuestions.SelectedRows[0].Cells[4].FormattedValue.ToString();
                txtThirdChoice.Text = dgvQuestions.SelectedRows[0].Cells[5].FormattedValue.ToString();
                txtFourthChoice.Text = dgvQuestions.SelectedRows[0].Cells[6].FormattedValue.ToString();

            }
            catch (Exception)
            {


            }
        }

        private async void btnAddQuestion_ClickAsync(object sender, EventArgs e)
        {
            try
            {
                TbQuestion Question = new TbQuestion();
                Question.QuestionContent = txtQuestionContent.Text;
                Question.QuestionRightAnswer = txtRightAnswer.Text;
                Question.FirstChoice = txtFirstChoice.Text;
                Question.SecondChoice = txtSecondChoice.Text;
                Question.ThirdChoice = txtThirdChoice.Text;
                Question.FourthChoice = txtFourthChoice.Text;
                Question.ExamId = Convert.ToInt32(dgvExams.SelectedRows[0].Cells[0].FormattedValue);
                var Response = await RestPostMethodAsync("Questions/AddQuestion", Question);
                MessageBox.Show(Response);
                btnGetExamQuestions.PerformClick();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }


        }

        private async void btnUpdateQuestion_ClickAsync(object sender, EventArgs e)
        {
            try
            {
                TbQuestion Question = new TbQuestion();
                Question.Id = Convert.ToInt32(dgvQuestions.SelectedRows[0].Cells[0].FormattedValue);
                Question.QuestionContent = txtQuestionContent.Text;
                Question.QuestionRightAnswer = txtRightAnswer.Text;
                Question.FirstChoice = txtFirstChoice.Text;
                Question.SecondChoice = txtSecondChoice.Text;
                Question.ThirdChoice = txtThirdChoice.Text;
                Question.FourthChoice = txtFourthChoice.Text;
                Question.ExamId = Convert.ToInt32(dgvExams.SelectedRows[0].Cells[0].FormattedValue);
                var Response = await RestPostMethodAsync("Questions/UpdateQuestion", Question);
                MessageBox.Show(Response);
                btnGetExamQuestions.PerformClick();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }

        private async void btnDeleteQuestion_ClickAsync(object sender, EventArgs e)
        {
            try
            {
                TbQuestion Question = new TbQuestion();
                Question.Id = Convert.ToInt32(dgvQuestions.SelectedRows[0].Cells[0].FormattedValue);
                var Response = await RestPostMethodAsync("Questions/DeleteQuestion", Question);
                MessageBox.Show(Response);
                btnGetExamQuestions.PerformClick();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }

        private async void btnAddExam_ClickAsync(object sender, EventArgs e)
        {
            try
            {
                TbExam Exam = new TbExam();
                Exam.ExamName = txtExamName.Text;
                var SelectedCourse = ComboCourses.SelectedItem as ComboboxItem;
                Exam.CourseId = Convert.ToInt32(SelectedCourse.Value);
                var Response = await RestPostMethodAsync("Exams/CreateExam", Exam);
                MessageBox.Show(Response);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }

        }

        private async void btnGetEnrolls_ClickAsync(object sender, EventArgs e)
        {
            try
            {
                dgvEnrolls.Rows.Clear();
                var Response = await RestGetMethodAsync("Enrolls/GetEnrolls");
                var Enrolls = JsonConvert.DeserializeObject<List<TbEnroll>>(Response);
                foreach (var Enroll in Enrolls)
                {
                    dgvEnrolls.Rows.Add(Enroll.Student.StudentName, Enroll.StudentId,
                        Enroll.Student.StudentEmail,
                        Enroll.Course.CourseName,
                        Enroll.CourseId);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }

        }

        private async void btnSendVerificationCode_ClickAsync(object sender, EventArgs e)
        {
            try
            {
                SendCodeModel sendCodeModel = new SendCodeModel();
                sendCodeModel.StudentId = Convert.ToInt32(dgvEnrolls.SelectedRows[0].Cells[1].FormattedValue);
                sendCodeModel.Email = dgvEnrolls.SelectedRows[0].Cells[2].FormattedValue.ToString();
                sendCodeModel.CourseId = Convert.ToInt32(dgvEnrolls.SelectedRows[0].Cells[4].FormattedValue);


                var Response = await RestPostMethodAsync("Authentication/SendVerificationCode", sendCodeModel);
                MessageBox.Show(Response);
                btnGetEnrolls.PerformClick();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private async void btnCreateCourse_ClickAsync(object sender, EventArgs e)
        {
            TbCourse Course = new TbCourse();
            Course.CourseName = txtCourseName.Text;
            Course.CourseLevel = txtCourseLevel.Text;
            var SelectedCategory = ComboCategories.SelectedItem as ComboboxItem;

            Course.CategoryId = Convert.ToInt32(SelectedCategory.Value);
            var Response = await RestPostMethodAsync("Courses/CreateCourse", Course);
            MessageBox.Show(Response);

        }

        private async void button1_ClickAsync(object sender, EventArgs e)
        {
            LoginModel Registermodel = new LoginModel();
            Registermodel.Username = txtRegisterSubadmin.Text;
            Registermodel.Password = txtRegisterPassword.Text;
            var Response = await RestPostMethodAsync("Authentication/RegisterSubAdmin", Registermodel);
            MessageBox.Show(Response);

        }
    }


    public class ComboboxItem
    {
        public string Text { get; set; }
        public object Value { get; set; }

        public override string ToString()
        {
            return Text;
        }
    }
}
