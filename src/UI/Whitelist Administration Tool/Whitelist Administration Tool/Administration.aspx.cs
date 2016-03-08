using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Providers.Entities;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using Whitelist_Administration_Tool.Properties;

namespace Whitelist_Administration_Tool
{
    public partial class Administration : Page
    {
        //SQLiteConnection dbConnection;
        private string loginUserName = string.Empty;
        private string loginPassword = string.Empty;

        protected Administration()
        {
            // Create the sqlite database if it does not exist
            if (!File.Exists(DatabaseHelper.Database))
            {
                SQLiteConnection.CreateFile(DatabaseHelper.Database);
            }

            // Create tables if they do not exist
            if (!DatabaseHelper.IsTableExists(DatabaseHelper.AdminsTable))
            {
                DatabaseHelper.CreateAdminsTable();

                // Give the initial default user
                DatabaseHelper.InsertAdmin("admin", "admin", 10);
            }

            if (!DatabaseHelper.IsTableExists(DatabaseHelper.UsersTable))
            {
                DatabaseHelper.CreateUsersTable();

                this.LoadJson();
            }

            if (!DatabaseHelper.IsTableExists(DatabaseHelper.ApplicationsTable))
            {
                DatabaseHelper.CreateApplicationTable();
            }
        }

        private void LoadJson()
        {
            dynamic jsonobj = JsonConvert.DeserializeObject(File.ReadAllText(Settings.Default.WhitelistPath + Settings.Default.JsonWhitelistName));

            foreach (var obj in jsonobj)
            {
                int banned = int.Parse(DatabaseHelper.FetchBannedFishbans(obj.name.ToString()));
                DatabaseHelper.InsertUser(obj.name.ToString(), obj.uuid.ToString(), banned, string.Empty, "-");
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void OnLoginButtonClicked(object sender, EventArgs e)
        {
            this.loginUserName = this.txtUser.Text;
            this.loginPassword = this.txtPassword.Text;

            if (DatabaseHelper.IsLoginValid(this.loginUserName, this.loginPassword))
            {
                // Store the session
                Session["loggedIn"] = true;
                Session["userName"] = this.loginUserName;
                Session["password"] = this.loginPassword;

                // Navigate to the panel page.
                Response.Redirect("~/AdminControlPanel.aspx");
            }
            else
            {
                labelMessage.InnerText = string.Format("Error: Invalid login!");
            }
        }
    }
}