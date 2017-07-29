using BudgetAttempt.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BudgetAttempt.Data.Models;
using System.Data.SQLite;

namespace BudgetAttempt.Data.Storage.SQLite
{
    public class SQLiteBudgetStorage : IBudgetStorage
    {
        private string _connstr = "";
        public IEnumerable<BudgetLine> FetchLines(BudgetFilter Filter)
        {
            var data = new List<BudgetLine>();
            var sql = "SELECT * FROM Lines";
            var whereStmt = " WHERE";
            using (var conn = OpenConnection())
            {
                using (var cmd = new SQLiteCommand(conn))
                {
                    if (Filter.BudgetCategory.HasValue)
                    {
                        whereStmt += " AND categoryid=@catId";
                        cmd.Parameters.AddWithValue("@catId", Filter.BudgetCategory.Value);
                    }
                    if (Filter.BudgetType.HasValue)
                    {
                        whereStmt += " AND budgettype=@typeId";
                        cmd.Parameters.AddWithValue("@typeId", Filter.BudgetType.Value);
                    }
                    if (Filter.EndDate.HasValue)
                    {
                        whereStmt += " AND budgetdate <= @end";
                        cmd.Parameters.AddWithValue("@end", Filter.EndDate);
                    }
                    if (!string.IsNullOrEmpty(Filter.Keywords))
                    {
                        whereStmt += " AND comment LIKE @words+'%'";
                        cmd.Parameters.AddWithValue("@words", Filter.Keywords);
                    }
                    if (Filter.OwnerId.HasValue)
                    {
                        whereStmt += " AND personid=@personId";
                        cmd.Parameters.AddWithValue("@personId", Filter.OwnerId.Value);
                    }
                    if (Filter.StartDate.HasValue)
                    {
                        whereStmt += " AND budgetdate >= @start";
                        cmd.Parameters.AddWithValue("@start", Filter.StartDate);
                    }
                    whereStmt = whereStmt.Replace("WHERE AND ", "WHERE ");
                    if (whereStmt.Trim() == "WHERE")
                    {
                        whereStmt = "";
                    }
                    sql += whereStmt + " ORDER BY budgetdate DESC";
                    if (Filter.Page.HasValue && Filter.PerPage.HasValue)
                    {
                        sql += string.Format(" LIMIT {0} OFFSET {1}", Filter.PerPage.Value, Filter.PerPage.Value * Filter.Page.Value);
                    }
                    sql += ";";
                    cmd.CommandText = sql;
                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            data.Add(new BudgetLine()
                            {
                                Amount = Convert.ToDecimal(dr["amount"]),
                                BudgetType = Convert.ToInt32(dr["budgettype"]),
                                category = Convert.ToInt32(dr["categoryid"]),
                                Comment = dr["comment"].ToString(),
                                Date = Convert.ToDateTime(dr["budgetdate"]),
                                Id = Convert.ToInt32(dr["id"]),
                                person = Convert.ToInt32(dr["personid"])
                            });
                        }
                    }
                }
            }
            return data;
        }

        public BudgetLine FetchLine(int Id)
        {
            BudgetLine retval = null;
            using (var conn = OpenConnection())
            {
                using (var cmd = new SQLiteCommand(conn))
                {
                    cmd.CommandText = "SELECT * FROM Lines WHERE id=@id;";
                    cmd.Parameters.AddWithValue("@id", Id);
                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            retval = new BudgetLine()
                            {
                                Id = Convert.ToInt32(dr["id"]),
                                Amount = Convert.ToDecimal(dr["amount"]),
                                BudgetType = Convert.ToInt32(dr["budgettype"]),
                                category = Convert.ToInt32(dr["categoryid"]),
                                Comment = dr["comment"].ToString(),
                                Date = Convert.ToDateTime(dr["budgetdate"]),
                                person = Convert.ToInt32(dr["personid"])
                            };
                        }
                    }
                }
            }
            return retval;
        }

        public Person FetchPerson(int Id)
        {
            Person retval = null;
            using (var conn = OpenConnection())
            {
                using (var cmd = new SQLiteCommand(conn))
                {
                    cmd.CommandText = "SELECT * FROM Persons WHERE id=@id;";
                    cmd.Parameters.AddWithValue("@id", Id);
                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            retval = new Person()
                            {
                                Id = Convert.ToInt32(dr["id"]),
                                FirstName = dr["firstname"].ToString(),
                                LastName = dr["lastname"].ToString()
                            };
                        }
                    }
                }
            }
            return retval;
        }

        public Category FetchCategory(int Id)
        {
            Category retval = null;
            using (var conn = OpenConnection())
            {
                using (var cmd = new SQLiteCommand(conn))
                {
                    cmd.CommandText = "SELECT * FROM Categories WHERE id=@id;";
                    cmd.Parameters.AddWithValue("@id", Id);
                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            retval = new Category()
                            {
                                Id = Convert.ToInt32(dr["id"]),
                                BudgetType = Convert.ToInt32(dr["budgettype"]),
                                Description = dr["description"].ToString()
                            };
                        }
                    }
                }
            }
            return retval;
        }

        public IEnumerable<Category> FetchCategories(int? BudgetType)
        {
            var data = new List<Category>();
            var sql = "SELECT * FROM Categories";
            var whereStmt = "";
            using (var conn = OpenConnection())
            {
                using (var cmd = new SQLiteCommand(conn))
                {
                    if (BudgetType.HasValue)
                    {
                        whereStmt += " WHERE budgettype=@typeId";
                        cmd.Parameters.AddWithValue("@typeId", BudgetType);
                    }

                    sql += whereStmt + " ORDER BY budgettype, description;";
                    cmd.CommandText = sql;
                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            data.Add(new Category()
                            {
                                Id = Convert.ToInt32(dr["id"]),
                                BudgetType = Convert.ToInt32(dr["budgettype"]),
                                Description = dr["description"].ToString()
                            });
                        }
                    }
                }
            }
            return data;
        }

        public Person CreatePerson(Person person)
        {
            long id = 0;
            using (var conn = OpenConnection())
            {
                using (var cmd = new SQLiteCommand(conn))
                {
                    var transaction = conn.BeginTransaction();
                    cmd.CommandText = "INSERT INTO Persons (firstname, lastname) VALUES (@firstname, @lastname)";
                    cmd.Parameters.AddWithValue("@firstname", person.FirstName);
                    cmd.Parameters.AddWithValue("@lastname", person.LastName);
                    cmd.ExecuteNonQuery();
                    id = conn.LastInsertRowId;
                    transaction.Commit();
                }
            }
            if (id > 0)
            {
                return FetchPerson((int)id);
            }
            return null;
        }

        public Person UpdatePerson(Person person)
        {
            using (var conn = OpenConnection())
            {
                using (var cmd = new SQLiteCommand(conn))
                {
                    var transaction = conn.BeginTransaction();
                    cmd.CommandText = "UPDATE Persons SET firstname=@firstname, lastname=@lastname WHERE id=@id";
                    cmd.Parameters.AddWithValue("@id", person.Id);
                    cmd.Parameters.AddWithValue("@lastname", person.LastName);
                    cmd.Parameters.AddWithValue("@lastname", person.LastName);
                    cmd.ExecuteNonQuery();
                }
            }
            return FetchPerson(person.Id);
        }

        public SQLiteBudgetStorage(string ConnectionString)
        {
            _connstr = ConnectionString;
            using (var con = OpenConnection())
            {
                CreateIfNeeded(TableNames.Persons, con);
                CreateIfNeeded(TableNames.BudgetTypes, con);
                CreateIfNeeded(TableNames.Categories, con);
                CreateIfNeeded(TableNames.Lines, con);
                AddTypesIfNeeded(con);
            }
        }

        private void CreateIfNeeded(TableNames TableName, SQLiteConnection conn)
        {
            using (var cmd = new SQLiteCommand(conn))
            {
                cmd.CommandText = Definitions[TableName];
                cmd.ExecuteNonQuery();
            }
        }

        private void AddTypesIfNeeded(SQLiteConnection conn)
        {
            using (var cmd = new SQLiteCommand(conn))
            {
                cmd.CommandText = "SELECT COUNT(*) FROM BudgetTypes WHERE Id=1;";
                var res = Convert.ToInt32(cmd.ExecuteScalar());
                if (res == 0)
                {
                    cmd.CommandText = "INSERT INTO BudgetTypes (id, description) VALUES (1, 'income');";
                    cmd.ExecuteNonQuery();
                }
                cmd.CommandText = "SELECT COUNT(*) FROM BudgetTypes WHERE Id=2;";
                res = Convert.ToInt32(cmd.ExecuteScalar());
                if (res == 0)
                {
                    cmd.CommandText = "INSERT INTO BudgetTypes (id, description) VALUES (2, 'expense');";
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private SQLiteConnection OpenConnection()
        {
            if (string.IsNullOrEmpty(_connstr))
            {
                throw new NullReferenceException("The connection string was not defined");
            }
            var con = new SQLiteConnection(_connstr);
            con.Open();
            using (var cmd = new SQLiteCommand(con))
            {
                cmd.CommandText = "PRAGMA foreign_keys = ON;";
                cmd.ExecuteNonQuery();
            }
            return con;
        }

        public Category FetchCategoryByNameAndType(Category category)
        {
            Category retval = null;
            using (var conn = OpenConnection())
            {
                using (var cmd = new SQLiteCommand(conn))
                {
                    cmd.CommandText = "SELECT * FROM Categories WHERE budgettype=@budgettype AND description=@description;";
                    cmd.Parameters.AddWithValue("@budgettype", category.BudgetType);
                    cmd.Parameters.AddWithValue("@description", category.Description);
                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            retval = new Category()
                            {
                                Id = Convert.ToInt32(dr["id"]),
                                BudgetType = Convert.ToInt32(dr["budgettype"]),
                                Description = dr["description"].ToString()
                            };
                        }
                    }
                }
            }
            return retval;
        }

        public Category CreateCategory(Category category)
        {
            long id = 0;
            using (var conn = OpenConnection())
            {
                using (var cmd = new SQLiteCommand(conn))
                {
                    var transaction = conn.BeginTransaction();
                    cmd.CommandText = "INSERT INTO Categories (budgettype, description) VALUES (@budgettype, @description)";
                    cmd.Parameters.AddWithValue("@budgettype", category.BudgetType);
                    cmd.Parameters.AddWithValue("@description", category.Description);
                    cmd.ExecuteNonQuery();
                    id = conn.LastInsertRowId;
                    transaction.Commit();
                }
            }
            if (id > 0)
            {
                return FetchCategory((int)id);
            }
            return null;
        }

        public BudgetLine CreateLine(BudgetLine data)
        {
            long id = 0;
            using (var conn = OpenConnection())
            {
                using (var cmd = new SQLiteCommand(conn))
                {
                    var transaction = conn.BeginTransaction();
                    cmd.CommandText = "INSERT INTO Lines (budgettype, budgetdate, personid, amount, comment, categoryid) VALUES (@budgettype, DATE(@budgetdate), @personid, @amount, @comment, @categoryid)";
                    cmd.Parameters.AddWithValue("@budgettype", data.BudgetType);
                    cmd.Parameters.AddWithValue("@budgetdate", data.Date.ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture));
                    cmd.Parameters.AddWithValue("@personid", data.person);
                    cmd.Parameters.AddWithValue("@amount", data.Amount);
                    cmd.Parameters.AddWithValue("@comment", data.Comment);
                    cmd.Parameters.AddWithValue("@categoryid", data.category);
                    cmd.ExecuteNonQuery();
                    id = conn.LastInsertRowId;
                    transaction.Commit();
                }
            }
            if (id > 0)
            {
                return FetchLine((int)id);
            }
            return null;
        }

        public IEnumerable<Person> FetchPersons()
        {
            var data = new List<Person>();
            using (var conn = OpenConnection())
            {
                using (var cmd = new SQLiteCommand(conn))
                {
                    cmd.CommandText = "SELECT * FROM Persons;";
                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            data.Add(new Person()
                            {
                                Id = Convert.ToInt32(dr["id"]),
                                FirstName = dr["firstname"].ToString(),
                                LastName = dr["lastname"].ToString()
                            });
                        }
                    }
                }
            }
            return data;
        }

        private enum TableNames
        {
            Lines,
            Persons,
            Categories,
            BudgetTypes
        }

        private Dictionary<TableNames, string> Definitions = new Dictionary<TableNames, string>() {
            {
                TableNames.Lines, "CREATE TABLE IF NOT EXISTS Lines ("+
                    "  id INTEGER PRIMARY KEY ASC"+
                    ", budgettype INT NOT NULL"+
                    ", budgetdate DATE NOT NULL"+
                    ", personid INT NOT NULL"+
                    ", amount NUMERIC NOT NULL"+
                    ", comment TEXT NOT NULL"+
                    ", categoryid INT NOT NULL"+
                    ", FOREIGN KEY(budgettype) REFERENCES BudgetTypes(id) ON DELETE CASCADE ON UPDATE CASCADE"+
                    ", FOREIGN KEY(personid) REFERENCES Persons(id) ON DELETE CASCADE ON UPDATE CASCADE"+
                    ", FOREIGN KEY(categoryid) REFERENCES Categories(id) ON DELETE CASCADE ON UPDATE CASCADE"+
                    ");"
            },
            {
                TableNames.Persons, "CREATE TABLE IF NOT EXISTS Persons ("+
                    "  id INTEGER PRIMARY KEY ASC"+
                    ", firstname TEXT NOT NULL"+
                    ", lastname TEXT NOT NULL"+
                    ");"
            },
            {
                TableNames.Categories, "CREATE TABLE IF NOT EXISTS Categories ("+
                    "  id INTEGER PRIMARY KEY ASC"+
                    ", budgettype INT NOT NULL"+
                    ", description TEXT NOT NULL"+
                    ", FOREIGN KEY(budgettype) REFERENCES BudgetTypes(id) ON DELETE CASCADE ON UPDATE CASCADE"+
                    ");"
            },
            {
                TableNames.BudgetTypes, "CREATE TABLE IF NOT EXISTS BudgetTypes ("+
                    "  id INTEGER PRIMARY KEY"+
                    ", description TEXT NOT NULL"+
                    ");"
            }

        };
    }
}

