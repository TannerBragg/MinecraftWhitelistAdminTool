using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using Whitelist_Administration_Tool.Properties;

namespace Whitelist_Administration_Tool
{
    public partial class AdminControl : UserControl
    {
        /// <summary>
        /// The Admin who is currently logged in.
        /// </summary>
        internal string LoginUserName { get; set; }

        /// <summary>
        /// The Admin's password who is currently logged in.
        /// </summary>
        internal string LoginPassword { get; set; }

        /// <summary>
        /// The ctor.
        /// </summary>
        public AdminControl()
        {
            this.playersTable = new Table();
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.PopulateTables();
        }

        /// <summary>
        /// Populate both the pending and whitelist tables.
        /// </summary>
        private void PopulateTables()
        {
            this.PopulatePendingTable();
            this.PopulateWhitelistTable();
        }

        /// <summary>
        /// Populate the pending table.
        /// </summary>
        private void PopulatePendingTable()
        {
            if (this.pendingTable != null)
            {
                this.pendingTable.Rows.Clear();
            }

            using (SQLiteConnection c = DatabaseHelper.OpenDatabase())
            {
                using (SQLiteDataReader reader = DatabaseHelper.GetTableReader(c, DatabaseHelper.ApplicationsTable))
                {
                    TableRow hr = new TableRow();

                    TableCell nameHCell = this.CreateCell("NAME", true);
                    TableCell uidhCell = this.CreateCell("UID", true);
                    TableCell emailHCell = this.CreateCell("EMAIL", true);
                    TableCell bannedHCell = this.CreateCell("BANNED", true);
                    TableCell approveHCell = this.CreateCell("APPROVE", true);
                    TableCell removeHCell = this.CreateCell("DENY", true);

                    hr.Cells.AddRange(new[]
                    {
                        nameHCell,
                        uidhCell,
                        emailHCell,
                        bannedHCell,
                        approveHCell,
                        removeHCell,
                    });

                    if (this.pendingTable != null)
                    {
                        this.pendingTable.Rows.Add(hr);

                        while (reader.Read())
                        {
                            // Get the contents from the reader
                            var name = reader["name"].ToString();
                            var uid = reader["uid"].ToString();
                            var email = reader["email"].ToString();
                            var banned = reader["banned"].ToString();

                            TableRow tr = new TableRow();

                            TableCell nameCell = this.CreateCell(name);
                            TableCell uidCell = this.CreateCell(uid);
                            TableCell emailCell = this.CreateEmail(email);
                            TableCell bannedCell = this.CreateBannedCell(int.Parse(banned), name);
                            TableCell approveCell = ApproveCell(uid, name, int.Parse(banned), email);
                            TableCell removeCell = RemoveCell(uid, DatabaseHelper.ApplicationsTable);

                            tr.Cells.AddRange(new[]
                            {
                                nameCell,
                                uidCell,
                                emailCell,
                                bannedCell,
                                approveCell,
                                removeCell
                            });

                            this.pendingTable.Rows.Add(tr);
                        }
                    }
                }
            }

        }

        private TableCell CreateEmail(string email)
        {
            if (!string.IsNullOrEmpty(email))
            {
                var button = new ImageButton();
                button.ImageUrl = "Images/email.png";
                button.Click += delegate
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "mailto",
                       "<script type = 'text/javascript'>parent.location='mailto:" + email +
                       "'</script>");
                };
                Page.ClientScript.RegisterForEventValidation(button.UniqueID);
                TableCell approveCell = new TableCell();
                approveCell.Controls.Add(button);
                return approveCell;
            }

            return this.CreateCell(string.Empty);
        }

        private TableCell CreateBannedCell(int banned, string name)
        {
            string image;
            if (banned == 0)
            {
                image = "Images/user_green.png";
            }
            else if (banned == 1)
            {
                image = "Images/user_red.png";
            }
            else
            {
                image = "Images/user_grey.png";
            }

            var button = new ImageButton();
            button.ImageUrl = image;
            button.Click += delegate
            {
                Response.Redirect(string.Format("http://fishbans.com/u/{0}", name));
            };
            Page.ClientScript.RegisterForEventValidation(button.UniqueID);
            TableCell approveCell = new TableCell();
            approveCell.Controls.Add(button);
            return approveCell;
        }

        /// <summary>
        /// Populate the whitelist table.
        /// </summary>
        private void PopulateWhitelistTable()
        {
            if (this.playersTable != null)
            {
                this.playersTable.Rows.Clear();
            }

            
            using (SQLiteConnection c = DatabaseHelper.OpenDatabase())
            {
                using (SQLiteDataReader reader = DatabaseHelper.GetTableReader(c, DatabaseHelper.UsersTable))
                {
                    TableRow hr = new TableRow();

                    TableCell nameHCell = this.CreateCell("NAME", true);
                    TableCell uidhCell = this.CreateCell("UID", true);
                    TableCell emailHCell = this.CreateCell("EMAIL", true);
                    TableCell bannedHCell = this.CreateCell("BANNED", true);
                    TableCell approvedByHCell = this.CreateCell("APPROVED BY", true);
                    TableCell removeHCell = this.CreateCell("REMOVE", true);

                    hr.Cells.AddRange(new[]
                    {
                        nameHCell,
                        uidhCell,
                        emailHCell,
                        bannedHCell,
                        approvedByHCell,
                        removeHCell
                    });

                    if (this.playersTable != null)
                    {
                        this.playersTable.Rows.Add(hr);

                        while (reader.Read())
                        {
                            // Get the contents from the reader
                            var name = reader["name"].ToString();
                            var uid = reader["uid"].ToString();
                            var banned = reader["banned"].ToString();
                            var email = reader["email"].ToString();
                            var approvedBy = reader["approvedBy"].ToString();

                            TableRow tr = new TableRow();

                            TableCell nameCell = this.CreateCell(name);
                            TableCell uidCell = this.CreateCell(uid);
                            TableCell emailCell = this.CreateEmail(email);
                            TableCell bannedCell = this.CreateBannedCell(int.Parse(banned), name);
                            TableCell approvedByCell = this.CreateCell(approvedBy);
                            TableCell removeCell = this.RemoveCell(uid, DatabaseHelper.UsersTable);

                            tr.Cells.AddRange(new[]
                            {
                                nameCell,
                                uidCell,
                                emailCell,
                                bannedCell,
                                approvedByCell,
                                removeCell
                            });

                            this.playersTable.Rows.Add(tr);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Create an approval cell to aprove a user.
        /// </summary>
        /// <param name="uid">The UUID of the user.</param>
        /// <param name="name">The Screen Name of the user.</param>
        /// <param name="banned">The banned state.</param>
        /// <param name="email">The email of the user (optional)</param>
        /// <returns>A cell with a button to automatically approve a user into the whitelist.</returns>
        private TableCell ApproveCell(string uid, string name, int banned, string email = null)
        {
            // Treat nulls as empty strings.
            if (email == null)
            {
                email = string.Empty;
            }

            var button = new ImageButton();
            button.ImageUrl = "Images/check_green.png";
            button.Click += delegate
            {
                if (DatabaseHelper.IsUidExists(uid, DatabaseHelper.ApplicationsTable))
                {
                    string sql = string.Format("DELETE FROM {0} WHERE uid='{1}'",
                        DatabaseHelper.ApplicationsTable,
                        uid);
                    DatabaseHelper.ExectuteNonQuery(sql);

                    DatabaseHelper.InsertUser(name, uid, banned, email, this.LoginUserName);

                    this.PopulateTables();
                    this.WriteWhitelists();
                    Response.Redirect(Request.RawUrl);
                }
            };
            Page.ClientScript.RegisterForEventValidation(button.UniqueID);
            TableCell approveCell = new TableCell();
            approveCell.Controls.Add(button);
            return approveCell;
        }

        /// <summary>
        /// Create a remove cell to delete a user from a particular table.
        /// </summary>
        /// <param name="uid">The UUID of the user to delete.</param>
        /// <param name="table">The table to delete the user from.</param>
        /// <returns>A cell that contains a button to remove a user of UUID.</returns>
        private TableCell RemoveCell(string uid, string table)
        {
            var removeButton = new ImageButton();
            removeButton.ImageUrl = "Images/xmark_red.png";
            removeButton.Click += delegate
            {
                string sql = string.Format("DELETE FROM {0} WHERE uid='{1}'", table,
                    uid);
                DatabaseHelper.ExectuteNonQuery(sql);
                this.PopulateTables();
                this.WriteWhitelists();
                Response.Redirect(Request.RawUrl);
            };
            Page.ClientScript.RegisterForEventValidation(removeButton.UniqueID);
            TableCell removeCell = new TableCell();
            removeCell.Controls.Add(removeButton);
            return removeCell;
        }

        /// <summary>
        /// Write the whitelist out to file.
        /// </summary>
        private void WriteWhitelists()
        {

            using (SQLiteConnection c = DatabaseHelper.OpenDatabase())
            {
                using (SQLiteDataReader reader = DatabaseHelper.GetTableReader(c, DatabaseHelper.UsersTable))
                {
                    List<dynamic> objsList = new List<dynamic>();
                    while (reader.Read())
                    {
                        // Get the contents from the reader
                        var name = reader["name"].ToString();
                        var uid = reader["uid"].ToString();

                        dynamic obj = new ExpandoObject();
                        obj.name = name;
                        obj.uuid = uid;

                        objsList.Add(obj);
                    }

                    dynamic[] dynObjects = objsList.ToArray();

                    var serializeObject = JsonConvert.SerializeObject(dynObjects);

                    File.WriteAllText(Settings.Default.WhitelistPath + Settings.Default.JsonWhitelistName,
                        serializeObject);
                }
            }
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
            string user = this.txtAddUser.Text;
            string uid = this.txtAddUid.Text;

            if (string.IsNullOrEmpty(uid) && !string.IsNullOrEmpty(user))
            {
                uid = DatabaseHelper.FetchUuidFishbans(user);
            }

            if (!string.IsNullOrEmpty(user) && !string.IsNullOrEmpty(uid))
            {
                if (!DatabaseHelper.IsUidExists(uid, DatabaseHelper.UsersTable))
                {
                    // Dont allow sql injections
                    if (uid.Contains(";") || user.Contains(";"))
                    {
                        this.messageSubmitReport.Attributes["class"] = "alert-danger";
                        this.messageSubmitReport.InnerText = "Invalid token detected!";
                        return;
                    }

                    try
                    {
                        int banned = int.Parse(DatabaseHelper.FetchBannedFishbans(user));
                        // Insert the user into the database table
                        DatabaseHelper.InsertUser(user, uid, banned, string.Empty, this.LoginUserName);

                        this.messageSubmitReport.Attributes["class"] = "alert-success";
                        this.messageSubmitReport.InnerText = "Add Success!";

                        this.PopulateWhitelistTable();
                        this.WriteWhitelists();
                        Response.Redirect(Request.RawUrl);
                    }
                    catch (Exception)
                    {
                        // Basically, we just tried to insert something that already has a uuid in the database... Its okay this time.
                        this.messageSubmitReport.Attributes["class"] = "alert-warn";
                        this.messageSubmitReport.InnerText = "Something may have gone wrong...";
                    }
                }
            }
            else
            {
                this.messageSubmitReport.Attributes["class"] = "alert-danger";
                this.messageSubmitReport.InnerText = "Invalid!";
            }
            
        }

        protected void OnButtonRemoveClicked(object sender, EventArgs e)
        {
            string uid = this.txtAddUid.Text;
            string username = this.txtAddUser.Text;

            if (string.IsNullOrEmpty(uid) && !string.IsNullOrEmpty(username))
            {
                uid = DatabaseHelper.FetchUuidFishbans(username);
            }

            if (!string.IsNullOrEmpty(uid))
            {
                if (DatabaseHelper.IsUidExists(uid, DatabaseHelper.UsersTable))
                {
                    // Dont allow sql injections
                    if (uid.Contains(";"))
                    {
                        return;
                    }

                    try
                    {
                        string sql = string.Format("DELETE FROM {0} WHERE uid='{1}'", DatabaseHelper.UsersTable, uid);
                        DatabaseHelper.ExectuteNonQuery(sql);

                        this.PopulateWhitelistTable();
                        this.WriteWhitelists();
                        Response.Redirect(Request.RawUrl);
                    }
                    catch (Exception)
                    {
                        // Something went wrong.  Oh well.
                    }
                }
            }
            else
            {
                this.messageSubmitReport.Attributes["class"] = "alert-danger";
                this.messageSubmitReport.InnerText = "Invalid!";
            }
        }

        protected void OnManageAdminsButtonClicked(object sender, EventArgs e)
        {
            Response.Redirect("ManageAdmins.aspx");
        }
    }
}