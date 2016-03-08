using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Providers.Entities;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using Whitelist_Administration_Tool.Properties;

namespace Whitelist_Administration_Tool
{
    public partial class SubmitApplication : Page
    {
        private string userName = string.Empty;
        private string uid = string.Empty;
        private string email = string.Empty;

        protected SubmitApplication()
        {
            // Create the sqlite database if it does not exist
            if (!File.Exists(DatabaseHelper.Database))
            {
                SQLiteConnection.CreateFile(DatabaseHelper.Database);
            }

            if (!DatabaseHelper.IsTableExists(DatabaseHelper.ApplicationsTable))
            {
                DatabaseHelper.CreateApplicationTable();
            }

            this.messageSubmitReport = new HtmlGenericControl();
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void OnSubmitButtonClicked(object sender, EventArgs e)
        {
            // Get the username and uuid from the fields.
            this.userName = this.txtUserName.Text;
            this.uid = this.txtUid.Text;
            this.email = this.txtEmail.Text;

            if (string.IsNullOrEmpty(userName))
            {
                message.InnerText = "Please enter a valid Screen Name.";
                return;
            }

            // If they opted out of filling in the uuid...
            if (string.IsNullOrEmpty(this.uid))
            {
                // Look up the UUID from the fishbans api
                this.uid = DatabaseHelper.FetchUuidFishbans(this.userName);
            }

            // Prevent sql queries from being injected and make sure the uuid is not null or empty
            if (!string.IsNullOrEmpty(uid) && !this.userName.Contains(";") && !this.uid.Contains(";") && !this.email.Contains(";"))
            {
                // Lets not add anything twice...
                if (DatabaseHelper.IsUidExists(this.uid, DatabaseHelper.UsersTable))
                {
                    this.messageSubmitReport.Attributes["class"] = "alert-success";
                    this.messageSubmitReport.InnerText = string.Format("Already whitelisted! {0}", this.uid);
                }
                else if (DatabaseHelper.IsUidExists(this.uid, DatabaseHelper.ApplicationsTable))
                {
                    this.messageSubmitReport.Attributes["class"] = "alert-warning";
                    this.messageSubmitReport.InnerText = string.Format("UUID Already Pending! {0}", this.uid);
                }
                else if (!DatabaseHelper.IsUidExists(this.uid, DatabaseHelper.ApplicationsTable))
                {
                    DatabaseHelper.InsertApplication(this.userName, this.uid, this.email, 0);

                    this.messageSubmitReport.Attributes["class"] = "alert-success";
                    this.messageSubmitReport.InnerText = string.Format("Application Submited! {0}", this.uid);
                }
            }
            else
            {
                this.messageSubmitReport.Attributes["class"] = "alert-danger";
                this.messageSubmitReport.InnerText = "Error! Invalid Username/UUID/Email Entered.";
            }
        }
    }
}