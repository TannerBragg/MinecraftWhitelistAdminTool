using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SQLite;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebGrease;

namespace Whitelist_Administration_Tool
{
    public partial class ManageAdmins : System.Web.UI.Page
    {
        private string loginUserName;
        private string loginPassword;
        private int powerlevel;
        private bool validPasswords = false;

        protected void Page_Preinit(object sender, EventArgs e)
        {
            try
            {
                if ((bool)Session["loggedIn"])
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
            this.PopulateTable();
        }

        private void PopulateTable()
        {
            if (this.adminsTable != null)
            {
                this.adminsTable.Rows.Clear();
            }

            using (SQLiteConnection c = DatabaseHelper.OpenDatabase())
            {
                using (SQLiteDataReader reader = DatabaseHelper.GetTableReader(c, DatabaseHelper.AdminsTable, "username"))
                {
                    TableRow hr = new TableRow();

                    TableCell nameHCell = this.CreateCell("USER", true);
                    TableCell powerHCell = this.CreateCell("POWER", true);
                    TableCell removeHCell = this.CreateCell("REMOVE", true);

                    hr.Cells.AddRange(new[]
                    {
                        nameHCell,
                        powerHCell,
                        removeHCell,
                    });

                    if (this.adminsTable != null)
                    {
                        this.adminsTable.Rows.Add(hr);

                        while (reader.Read())
                        {
                            // Get the contents from the reader
                            var username = reader["username"].ToString();
                            var powerlevel = reader["powerlevel"].ToString();

                            // Display the user's data if he's the one logged in
                            if (username == this.loginUserName)
                            {
                                this.labelUsername.InnerText = username;
                                this.labelPowerLevel.InnerText = powerlevel;
                                this.powerlevel = int.Parse(powerlevel);
                            }

                            TableRow tr = new TableRow();

                            TableCell nameCell = this.CreateCell(username);
                            TableCell powerCell = this.CreateCell(powerlevel);
                            TableCell removeCell = RemoveCell(username, int.Parse(powerlevel), DatabaseHelper.AdminsTable);

                            tr.Cells.AddRange(new[]
                            {
                                nameCell,
                                powerCell,
                                removeCell
                            });

                            this.adminsTable.Rows.Add(tr);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Create a remove cell to delete a user from a particular table.
        /// </summary>
        /// <param name="username">The UUID of the user to delete.</param>
        /// <param name="table">The table to delete the user from.</param>
        /// <returns>A cell that contains a button to remove a user of UUID.</returns>
        private TableCell RemoveCell(string username, int powerlevel, string table)
        {
            var removeButton = new ImageButton();
            removeButton.ImageUrl = "Images/xmark_red.png";
            removeButton.Click += delegate
            {
                if (this.loginUserName != username)
                {
                    if (powerlevel < this.powerlevel)
                    {
                        string sql = string.Format("DELETE FROM {0} WHERE username='{1}'", table,
                            username);
                        DatabaseHelper.ExectuteNonQuery(sql);
                        this.PopulateTable();
                        Response.Redirect(Request.RawUrl);
                    }
                    else
                    {
                        this.message.Attributes["class"] = "alert-danger";
                        this.message.InnerText = "You can not remove admins with higher power than you!";
                    }
                }
                else
                {
                    this.message.Attributes["class"] = "alert-danger";
                    this.message.InnerText = "You can not remove yourself!";
                }
            };
            Page.ClientScript.RegisterForEventValidation(removeButton.UniqueID);
            TableCell removeCell = new TableCell();
            removeCell.Controls.Add(removeButton);
            return removeCell;
        }

        /// <summary>
        /// Create a cell for tables.
        /// </summary>
        /// <param name="text">The text of the cell.</param>
        /// <param name="bold">Want the text bold?</param>
        /// <returns>Returns a simple cell with text bold or not.</returns>
        private TableCell CreateCell(string text, bool bold = false)
        {
            TableCell tableCell = new TableCell();
            tableCell.Text = text;
            tableCell.Font.Bold = bold;
            return tableCell;
        }

        protected void OnButtonAddClicked(object sender, EventArgs e)
        {
            string newUser = this.txtAddUser.Text;
            string newPassword = this.txtPassword.Text;
            int newPowerlevel;

            // Dont let anyone hack the database...
            if (newUser.Contains(";") || newUser.Contains(";"))
            {
                this.message.Attributes["class"] = "alert-danger";
                this.message.InnerText = "Invalid Token(s) entered!";
                return;
            }
            
            if(int.TryParse(this.txtPower.Text, out newPowerlevel))
            {
                // make sure the passwords match
                if (this.txtPassword.Text != this.txtPasswordRepeat.Text)
                {
                    this.validPasswords = false;
                    this.formGroup.Attributes["class"] = "form-group has-error";
                    this.message.Attributes["class"] = "alert-danger";
                    this.message.InnerText = "Passwords do not match!";
                    return;
                }
                this.validPasswords = true;

                if (newPowerlevel < this.powerlevel)
                {
                    if (validPasswords)
                    {
                        if (!DatabaseHelper.IsUserNameExists(newUser, DatabaseHelper.AdminsTable))
                        {
                            DatabaseHelper.InsertAdmin(newUser, newPassword, newPowerlevel);
                            this.message.Attributes["class"] = "alert-success";
                            this.message.InnerText = "User Added!";
                            this.adminsTable.Rows.Clear();
                            this.PopulateTable();
                        }
                    }
                    else
                    {
                        this.message.Attributes["class"] = "alert-danger";
                        this.message.InnerText = "Password is not valid!";
                    }
                }
                else
                {
                    this.message.Attributes["class"] = "alert-danger";
                    this.message.InnerText = "You can not create a user with the same or greater power as you!";
                }
            }
            else
            {
                this.message.Attributes["class"] = "alert-danger";
                this.message.InnerText = "Invalid Powerlevel Input!";
            }
        }

        protected void OnTextPasswordsChanged(object sender, EventArgs e)
        {
            
            
        }
    }
}