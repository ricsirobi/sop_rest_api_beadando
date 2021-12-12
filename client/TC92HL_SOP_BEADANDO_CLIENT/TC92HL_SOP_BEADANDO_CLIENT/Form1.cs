using RestSharp;
using RestSharp.Serialization.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TC92HL_SOP_BEADANDO_CLIENT
{
    public partial class Form1 : Form
    {
        String URL = "http://localhost/sop_rest_api_beadando/server/";
        String ROUTE = "index.php";
        public static data.User UserLoggedIn = null;
        private static int loggedUserId = -1;


        List<data.Lego> legoList = new List<data.Lego>();
        List<data.Category> categoryList = new List<data.Category>();
        String loggedToken = null;


        public Form1()
        {
            InitializeComponent();
            button2.Enabled = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void allLegoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            label10.Text = "get lego";
            var client = new RestClient(URL);
            var request = new RestRequest(ROUTE, Method.GET);
            IRestResponse<List<data.Lego>> response = client.Execute<List<data.Lego>>(request);
            listBox1.Items.Clear(); legoList.Clear();
            foreach (data.Lego item in response.Data)
            {
                listBox1.Items.Add(item.Code + "\t" + item.Name /*+ "\t" + item.Category + "\t" + item.HUFprice + " Ft"*/);
                legoList.Add(item);
            }
        }
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";

            switch (label10.Text)
            {
                case "get lego":
                    textBox3.Text = legoList[listBox1.SelectedIndex].Code;
                    textBox4.Text = legoList[listBox1.SelectedIndex].Name;
                    textBox5.Text = legoList[listBox1.SelectedIndex].Category.ToString();
                    textBox6.Text = legoList[listBox1.SelectedIndex].HUFprice.ToString();
                    break;
                case "get all category":
                    textBox5.Text = categoryList[listBox1.SelectedIndex].Id.ToString();
                    break;
                default:
                    break;
            }
        }
        private void newLegoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var client = new RestClient(URL);
            var request = new RestRequest(ROUTE + "?tok=" + loggedToken, Method.POST);  //index is kellett mögé
            request.RequestFormat = DataFormat.Json;
            try
            {
                data.Lego put = new data.Lego
                {
                    Code = textBox3.Text,
                    Name = textBox4.Text,
                    Category = int.Parse(textBox5.Text),
                    HUFprice = int.Parse(textBox6.Text)
                };
                request.AddBody(put);

                legoList.Add(put);
                IRestResponse response = client.Execute(request);
                label1.Text = response.StatusCode.ToString();

                allLegoToolStripMenuItem_Click(null, null);
                checkResponse(response.Content);
            }
            catch (Exception eeee)
            {
                MessageBox.Show("Hiányzó adatok");
            }

        }

        private void kijelöltLegoTörléseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (adminToolStripMenuItem.Text.Contains("Admin"))
            {
                var client = new RestClient(URL);
                String ROUTE = "index.php/{code}{tok}";
                var request = new RestRequest(ROUTE, Method.DELETE);
                request.AddParameter("code", textBox3.Text);
                request.AddParameter("tok", loggedToken);
                IRestResponse response = client.Execute(request);
                checkResponse(response.Content);
                allLegoToolStripMenuItem_Click(null, null);
            }
            else
            {
                MessageBox.Show("A törléshez admin jogosultásg szükséges");
            }
        }
        private void button1_Click_1(object sender, EventArgs e)
        {
            label10.Text = "Login";
            String uname = textBox1.Text;
            String password = textBox2.Text;
            String URL = "http://localhost/sop_rest_api_beadando/server/login.php?login=1" + "&u=" + uname + "&p=" + password;
            var request = new RestRequest(Method.POST);
            request.RequestFormat = DataFormat.Json;
            var client = new RestClient(URL);
            IRestResponse<List<data.User>> response = client.Execute<List<data.User>>(request);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                MessageBox.Show(response.StatusDescription);
                return;
            }
            else
            {
                if (response.Data[0].Token != null)
                {
                    loggedUserId = response.Data[0].Id;
                    MessageBox.Show("Sikeres bejelentkezés");
                    loggedToken = response.Data[0].Token;
                    loggedInToolStripMenuItem.Text = "Bejelentkezve";

                    if (loggedToken.Split('|')[1] == "1")
                    {
                        adminToolStripMenuItem.Text = "Admin";
                    }
                    else
                    {
                        adminToolStripMenuItem.Text = "User";
                    }
                    button1.Enabled = false;
                    button2.Enabled = true;
                }
                else
                {
                    MessageBox.Show("Sikertelen bejelentkezés");
                }
            }
        }
        private void oneLegoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            label10.Text = "get lego";
            string code = "?code=" + textBox3.Text;
            {
                var client = new RestClient(URL);
                var request = new RestRequest(ROUTE + code, Method.GET);

                IRestResponse<List<data.Lego>> response = client.Execute<List<data.Lego>>(request);
                listBox1.Items.Clear();
                legoList.Clear();
                foreach (data.Lego item in response.Data)
                {
                    listBox1.Items.Add(item.Code + "\t" + item.Name);
                    legoList.Add(item);
                }
            }
        }

        private void allCategoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            label10.Text = "get all category";
            string code = "?category=-1";
            {
                var client = new RestClient(URL);
                var request = new RestRequest(ROUTE + code, Method.GET);

                IRestResponse<List<data.Category>> response = client.Execute<List<data.Category>>(request);
                listBox1.Items.Clear();
                categoryList.Clear();
                foreach (data.Category item in response.Data)
                {
                    listBox1.Items.Add(item.Id + "\t" + item.Name);
                    data.Category put = new data.Category
                    {
                        Id = item.Id,
                        Name = item.Name
                    };
                    categoryList.Add(put);
                }
            }
        }

        private void oneCategoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            label10.Text = "get lego";
            string code = "?category=" + textBox5.Text;

            var client = new RestClient(URL);
            var request = new RestRequest(ROUTE + code, Method.GET);

            IRestResponse<List<data.Lego>> response = client.Execute<List<data.Lego>>(request);
            listBox1.Items.Clear();
            legoList.Clear();
            foreach (data.Lego item in response.Data)
            {
                listBox1.Items.Add(item.Code + "\t" + item.Name + "\t" + item.HUFprice);
                data.Lego put = new data.Lego
                {
                    Code = item.Code,
                    Name = item.Name,
                    Category = item.Category,
                    HUFprice = item.HUFprice
                };
                legoList.Add(put);
            }
        }

        private void editLegoadminToolStripMenuItem_Click(object sender, EventArgs e)
        {
            String URL = "http://localhost/sop_rest_api_beadando/server/index.php?tok=" + loggedToken;
            var request = new RestRequest(Method.PUT);
            var client = new RestClient(URL);
            request.RequestFormat = DataFormat.Json;
            try
            {
                data.Lego put = new data.Lego
                {
                    Id = legoList[listBox1.SelectedIndex].Id,
                    Code = textBox3.Text,
                    Name = textBox4.Text,
                    Category = int.Parse(textBox5.Text),
                    HUFprice = int.Parse(textBox6.Text)
                };
                request.AddBody(put);
                var response = client.Execute(request);

                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    MessageBox.Show(response.StatusDescription);
                    return;
                }
                else
                {
                    checkResponse(response.Content);

                }
                allLegoToolStripMenuItem_Click(null, null);
            }
            catch (Exception eee)
            {
                MessageBox.Show("Hiányzó adatok.");
            }
        }
        private void checkResponse(String content)
        {
            if (content.Contains("Successfully."))
            {
                MessageBox.Show("Sikeres művelet");
            }
            else
            {
                MessageBox.Show("Sikertelen művelet");
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            {
                loggedUserId = -1;
                label10.Text = "Logout";
                String URL = "http://localhost/sop_rest_api_beadando/server/login.php?logout=" + loggedToken;
                var request = new RestRequest(Method.POST);
                request.RequestFormat = DataFormat.Json;
                var client = new RestClient(URL);
                var response = client.Execute(request);
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    MessageBox.Show(response.StatusDescription);
                    return;
                }
                else
                {
                    loggedToken = "";
                    loggedInToolStripMenuItem.Text = "Nincs bejelentkezve";
                    adminToolStripMenuItem.Text = "No1";
                    button1.Enabled = true;
                    button2.Enabled = false;
                    loggedUserId = -1;
                    MessageBox.Show("Kijelentkezve");
                }
            }
        }

        private void adminToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void myLegoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            label10.Text = "get lego";
            string userid = "?userid=" + (loggedUserId);
            if (loggedUserId != -1)
            {
                var client = new RestClient(URL);
                var request = new RestRequest(ROUTE + userid, Method.GET);
                IRestResponse<List<data.Lego>> response = client.Execute<List<data.Lego>>(request);
                try
                {
                    listBox1.Items.Clear();
                    legoList.Clear();
                    foreach (data.Lego item in response.Data)
                    {

                        listBox1.Items.Add(item.Code + "\t" + item.Name);
                        legoList.Add(item);
                    }
                }
                catch (Exception err)
                {
                    MessageBox.Show("Jelenleg nincs egy darab se.");
                }
            }
            else
            {
                MessageBox.Show("Bejelentkezés szükséges ennek a használatához");
            }
        }

        private void kijeloltLegoMegvanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                String route = "index.php?userid=" + loggedUserId + "&legoid=" + legoList[listBox1.SelectedIndex].Id + "&tok=" + loggedToken;
                var request = new RestRequest(Method.POST);
                request.RequestFormat = DataFormat.Json;
                var client = new RestClient(URL + route);


                var response = client.Execute(request);

                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    MessageBox.Show(response.StatusDescription);
                    return;
                }
                else
                {
                    if (response.Content.Contains("Successfully") && loggedToken != null && loggedToken != "")
                    {
                        MessageBox.Show("Sikeres hozzáadás");
                        myLegoToolStripMenuItem_Click(null, null);
                    }
                    else
                    {
                        MessageBox.Show("Sikertelen hozzáadás");
                    }
                }
            }
            catch (Exception errr)
            {
                MessageBox.Show("Hiányzó adat (Nincs kijelölve semmi)");
            }
        }

        private void kijelöltLegoNincsMegToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                String route = "index.php?userid=" + loggedUserId + "&legoid=" + legoList[listBox1.SelectedIndex].Id + "&tok=" + loggedToken;
                //textBox1.Text = route;
                var request = new RestRequest(Method.DELETE);
                request.RequestFormat = DataFormat.Json;
                var client = new RestClient(URL + route);
                var response = client.Execute(request);

                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    MessageBox.Show(response.StatusDescription);
                    return;
                }
                else
                {
                    if (response.Content.Contains("Successfully") && loggedToken != null && loggedToken != "")
                    {
                        MessageBox.Show("Sikeres törlés a meglévőek közül");
                        myLegoToolStripMenuItem_Click(null, null);
                    }
                    else
                    {
                        MessageBox.Show("Sikertelen törlés a meglévőek közül");
                    }
                }
            }
            catch (Exception errr)
            {
                MessageBox.Show("Hiányzó adat (Nincs kijelölve semmi)");
            }
        }
    }
}
