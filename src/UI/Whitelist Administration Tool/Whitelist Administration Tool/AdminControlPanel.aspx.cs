using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Ajax.Utilities;

namespace Whitelist_Administration_Tool
{
    public partial class AdminControlPanel : System.Web.UI.Page
    {
        private string loginUserName;
        private string loginPassword;

        public AdminControlPanel()
        {
            
        }

        protected void Page_Preinit(object sender, EventArgs e)
        {
            try
            {
                if ((bool) Session["loggedIn"])
                {
                
                    if (DatabaseHelper.IsLoginValid(Session["userName"].ToString(), Session["password"].ToString()))
                    {
                        // then continue if we are still good
                        this.loginUserName = Session["userName"].ToString();
                        this.loginPassword = Session["password"].ToString();
                    }
                    else
                    {
                        Response.Redirect("~/Administration.aspx");
                    }
                }
                else
                {
                    Response.Redirect("~/Default.aspx");
                }
            }
            catch (Exception)
            {

                Response.Redirect("~/Default.aspx");
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            AdminControl adminControl = (AdminControl) Page.LoadControl("~/AdminControl.ascx");
            adminControl.LoginUserName = this.loginUserName;
            adminControl.LoginPassword = this.loginPassword;

            this.adminPanel.Controls.Add(adminControl);
        }
    }
}