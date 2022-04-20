using BusinessLogic.StudentLogic.Classes;
using Domains;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using ODC_Students.Controllers;
using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Student_ODC
{
    public partial class Form1 : Form
    {
        string AccessToken = "";
        string StudentId = "";
        string CourseId = "";

        public Form1()
        {
            InitializeComponent();
        }
        async Task<string> RestGetMethodAsync(string APILink)
        {
            var client = new RestClient($"https://localhost:44377/student/api/{APILink}");
            var request = new RestRequest();
            request.AddHeader("Authorization", AccessToken);
            var Response = await client.GetAsync(request);
            return Response.Content;
        }
        async Task<string> RestPostMethodAsync(string APILink, object JsonModel)
        {
            var client = new RestClient();
            var request = new RestRequest($"https://localhost:44377/student/api/{APILink}");
            request.AddJsonBody(JsonModel);
            request.AddHeader("Authorization", AccessToken);
            var Response = await client.PostAsync(request);
            return Response.Content;

        }
        private async void btnLogin_ClickAsync(object sender, EventArgs e)
        {
            LoginModel loginModel = new LoginModel();
            loginModel.Username = txtUsername.Text;
            loginModel.Password = txtPassword.Text;

            var Response = await RestPostMethodAsync("Authentication/LoginStudent", loginModel);
            if (Response.Length > 15)
            {

                MessageBox.Show(Response.Replace("\"", ""));
                StudentId = GetStudentId(Response.Replace("\"", ""));
                AccessToken = $"Bearer {Response.Replace("\"", "")}";
            }
        }

        private async void btnRegister_ClickAsync(object sender, EventArgs e)
        {
            RegisterModel RegisterModel = new RegisterModel();
            RegisterModel.Username = txtUsernameRegst.Text;
            RegisterModel.Password = txtPasswordRegst.Text;
            RegisterModel.College = txtCollege.Text;
            RegisterModel.Phone = txtPhone.Text;
            RegisterModel.Email = txtEmail.Text;

            var Response = await RestPostMethodAsync("Authentication/RegisterStudent", RegisterModel);
            MessageBox.Show(Response);
            AccessToken = Response;
        }

        private async void btnRefreshCourses_ClickAsync(object sender, EventArgs e)
        {
            var Response = await RestGetMethodAsync("StudentCourses/ViewCourses");
            var Courses = JsonConvert.DeserializeObject<List<TbCourse>>(Response);
            foreach (var course in Courses)
            {
                Button CrsButton = new Button();
                CrsButton.FlatStyle = FlatStyle.Flat;
                CrsButton.Text = course.CourseName + " ( Enroll Now )";
                CrsButton.Tag = course.CourseId;
                CrsButton.Size = new Size(279, 224);
                panelCourses.Controls.Add(CrsButton);

                CrsButton.Click += async delegate (object sender, EventArgs e)
                {
                    EnrollModel enrollModel = new EnrollModel();
                    enrollModel.CourseId = Convert.ToInt32(CrsButton.Tag);
                    enrollModel.StudentId = Convert.ToInt32(StudentId);
                    var Response = await RestPostMethodAsync("StudentCourses/EnrollToCourse", enrollModel);
                    MessageBox.Show(Response);
                };
            }
        }


        public string GetStudentId(string token)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(token))
                {

                    string secret = "NIUXq4yk8GTGelfusivAHfSrdPEHvXWMXA1Khnlhnpk=";
                    var key = Encoding.ASCII.GetBytes(secret);
                    var handler = new JwtSecurityTokenHandler();
                    var validations = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                    var claims = handler.ValidateToken(token, validations, out var tokenSecure);
                    return claims.Identities.FirstOrDefault().Claims.FirstOrDefault().ToString().Replace("StudentId: ", "");
                }
            }
            catch (Exception)
            {

            }

            return "";
        }

        private void tabPage3_Click(object sender, EventArgs e)
        {

        }

        private async void btnSumbitVCode_ClickAsync(object sender, EventArgs e)
        {
            CourseVerificationCodeModel courseVerificationCodeModel = new CourseVerificationCodeModel();
            courseVerificationCodeModel.StudentId = Convert.ToInt32(StudentId);
            courseVerificationCodeModel.VCode = Convert.ToInt32(txtVCode.Text);
            var Response = await RestPostMethodAsync("StudentCourses/SetVerificationCode", courseVerificationCodeModel);
            var Course = JsonConvert.DeserializeObject<SetCodeResult>(Response);
            if (Course.Message.Contains("successfully"))
            {

                var ExamResponse = await RestGetMethodAsync($"StudentCourses/GetCourseExam?CourseId={Course.CourseId}");
                var Questions = JsonConvert.DeserializeObject<List<TbQuestion>>(ExamResponse);
                foreach (var question in Questions)
                {
                    QuestionTemplate questionTemplate = new QuestionTemplate();
                    questionTemplate.Tag = question.Id;
                    questionTemplate.lblQuestion.Text = question.QuestionContent;
                    questionTemplate.RadioChoice1.Text = question.FirstChoice;
                    questionTemplate.RadioChoice2.Text = question.SecondChoice;
                    questionTemplate.RadioChoice3.Text = question.ThirdChoice;
                    questionTemplate.RadioChoice4.Text = question.FourthChoice;
                    questionTemplate.Size = new Size(724, 266);
                    panelQuestions.Controls.Add(questionTemplate);
                }
                btnSubmitAnswers.Enabled = true;
            }
            else
            {
                MessageBox.Show(Course.Message);
            }
        }

        private async void btnSubmitAnswers_ClickAsync(object sender, EventArgs e)
        {
            List<ModelAnswer> modelAnswer = new List<ModelAnswer>();
            foreach (QuestionTemplate control in panelQuestions.Controls)
            {
                var CheckedRadio = control.Controls.OfType<RadioButton>()
                           .FirstOrDefault(n => n.Checked);
                if (CheckedRadio is null)
                {
                    if (MessageBox.Show("There is un answered questions do you want to submit?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                    {
                        return;
                    }

                    modelAnswer.Add(new ModelAnswer()
                    {
                        StudentAnswer = "UnAnswered"
                    });
                }
                else
                {
                    modelAnswer.Add(new ModelAnswer()
                    {
                        QeustionId = Convert.ToInt32(control.Tag),
                        StudentAnswer = CheckedRadio.Text,
                        StudentId = Convert.ToInt32(StudentId)
                    });
                }
            }
            //send student answers to server
            var Response = await RestPostMethodAsync("StudentCourses/PublishStudentAnswers", modelAnswer);
            MessageBox.Show(Response);
            btnSubmitAnswers.Enabled = false;
            panelQuestions.Controls.Clear();
        }

    }
}
