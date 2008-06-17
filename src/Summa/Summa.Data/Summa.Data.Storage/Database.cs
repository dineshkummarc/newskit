using System;
using System.Collections;
using System.Text;

using System.Data;
using Mono.Data.SqliteClient;

namespace Summa {
    namespace Data {
        namespace Storage {
            public class Database {
                private string uri = "/home/eosten/.config/newskit/database.db";
                private const string Uri = "URI=file:///home/eosten/.config/newskit/database.db";
                private IDbConnection db;
                
                public Database() {
                    bool exists = System.IO.File.Exists(uri);
                    
                    db = new SqliteConnection("Version=3,"+Uri);
                    db.Open();
                    
                    if (!exists) {
                        Initialize();
                    }
                }
                
                private void NonQueryCommand(string commandtext) {
                    IDbCommand dbcmd = db.CreateCommand();
                    dbcmd.CommandText = commandtext;
                    dbcmd.ExecuteNonQuery();
                    dbcmd.Dispose();
                    dbcmd = null;
                }
                
                private string EscapeParam(string parameter) {
                    try {
                        return parameter;
                    } catch ( Exception e ) {
                        Summa.Core.Util.Log("Null reference", e);
                        return "";
                    }
                }
                
                private void Initialize() {
                    NonQueryCommand("create table Summa (id INTEGER PRIMARY KEY, version VARCHAR(50))");
                    
                    NonQueryCommand("create table Feeds (id INTEGER PRIMARY KEY, uri VARCHAR(50), generated_name VARCHAR(50), name VARCHAR(50), author VARCHAR(50), subtitle VARCHAR(50), image VARCHAR(50), license VARCHAR(50), etag VARCHAR(50), hmodified VARCHAR(50), status VARCHAR(50), tags VARCHAR(50), favicon VARCHAR(50))");
                    
                    NonQueryCommand(String.Format("insert into Summa values (null, {0})", @"""0""")); 
                }
                
                private string GenerateRandomName() {
                    Random random = new Random();
                    StringBuilder builder = new StringBuilder();
                    
                    for (int i=0; i < 8; i++) {
                        builder.Append(Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65))));
                    }
                    return builder.ToString();
                }
                
                private string GetGeneratedName(string uri) {
                    IDbCommand dbcmd = db.CreateCommand();
                    dbcmd.CommandText = @"select * from Feeds";
                    IDataReader reader = dbcmd.ExecuteReader();
                    string name = null;
                    while(reader.Read()) {
                        if ( reader.GetString(1) == uri ) {
                            name = reader.GetString(2);
                            break;
                        }
                    }
                    reader.Close();
                    reader = null;
                    dbcmd.Dispose();
                    dbcmd = null;
                    return name;
                }
                
                public string CreateFeed(string uri, string name, string author, string subtitle, string image, string license, string etag, string hmodified, string status, string tags, string favicon) {
                    string generated_name = GenerateRandomName();
                    
                    IDbCommand dbcmd = db.CreateCommand();
                    dbcmd.CommandText = @"insert into Feeds values (null, :uri, :genname, :name, :author, :subtitle, :image, :license, :etag, :hmodified, :status, :tags, :favicon)";
                    
                    SqliteParameter uri_parameter = new SqliteParameter();
                    uri_parameter.Value = EscapeParam(uri);
                    uri_parameter.ParameterName = @":uri";
                    dbcmd.Parameters.Add(uri_parameter);
                    
                    SqliteParameter genname_parameter = new SqliteParameter();
                    genname_parameter.Value = EscapeParam(generated_name);
                    genname_parameter.ParameterName = @":genname";
                    dbcmd.Parameters.Add(genname_parameter);
                    
                    SqliteParameter name_parameter = new SqliteParameter();
                    name_parameter.Value = EscapeParam(name);
                    name_parameter.ParameterName = @":name";
                    dbcmd.Parameters.Add(name_parameter);
                    
                    SqliteParameter author_parameter = new SqliteParameter();
                    author_parameter.Value = EscapeParam(author);
                    author_parameter.ParameterName = @":author";
                    dbcmd.Parameters.Add(author_parameter);
                    
                    SqliteParameter sub_parameter = new SqliteParameter();
                    sub_parameter.Value = EscapeParam(subtitle);
                    sub_parameter.ParameterName = @":subtitle";
                    dbcmd.Parameters.Add(sub_parameter);
                    
                    SqliteParameter image_parameter = new SqliteParameter();
                    image_parameter.Value = EscapeParam(image);
                    image_parameter.ParameterName = @":image";
                    dbcmd.Parameters.Add(image_parameter);
                    
                    SqliteParameter license_parameter = new SqliteParameter();
                    license_parameter.Value = EscapeParam(license);
                    license_parameter.ParameterName = @":license";
                    dbcmd.Parameters.Add(license_parameter);
                    
                    SqliteParameter etag_parameter = new SqliteParameter();
                    etag_parameter.Value = EscapeParam(etag);
                    etag_parameter.ParameterName = @":etag";
                    dbcmd.Parameters.Add(etag_parameter);
                    
                    SqliteParameter hmodified_parameter = new SqliteParameter();
                    hmodified_parameter.Value = EscapeParam(hmodified);
                    hmodified_parameter.ParameterName = @":hmodified";
                    dbcmd.Parameters.Add(hmodified_parameter);
                    
                    SqliteParameter status_parameter = new SqliteParameter();
                    status_parameter.Value = EscapeParam(status);
                    status_parameter.ParameterName = @":status";
                    dbcmd.Parameters.Add(status_parameter);
                    
                    SqliteParameter tags_parameter = new SqliteParameter();
                    tags_parameter.Value = EscapeParam(tags);
                    tags_parameter.ParameterName = @":tags";
                    dbcmd.Parameters.Add(tags_parameter);
                    
                    SqliteParameter fav_parameter = new SqliteParameter();
                    fav_parameter.Value = EscapeParam(favicon);
                    fav_parameter.ParameterName = @":favicon";
                    dbcmd.Parameters.Add(fav_parameter);
                    
                    dbcmd.ExecuteNonQuery();
                    dbcmd.Dispose();
                    dbcmd = null;
                    
                    NonQueryCommand("create table "+generated_name+" (id INTEGER PRIMARY KEY, title VARCHAR(50), uri VARCHAR(50), date VARCHAR(50), last_updated VARCHAR(50), author VARCHAR(50), tags VARCHAR(50), content VARCHAR(50), encuri VARCHAR(50), read VARCHAR(50), flagged VARCHAR(50))");
                    
                    return generated_name;
                }
                
                public void DeleteFeed(string uri) {
                    NonQueryCommand("drop table "+GetGeneratedName(uri));
                    NonQueryCommand(@"delete from Feeds where uri="""+uri+@"""");
                }
                
                public string[] GetFeed(string uri) {
                    string[] feed = new string[13];
                    
                    IDbCommand dbcmd = db.CreateCommand();
                    dbcmd.CommandText = "select * from Feeds where uri=:uri";
                    SqliteParameter param = new SqliteParameter();
                    param.Value = uri;
                    param.ParameterName = @":uri";
                    dbcmd.Parameters.Add(param);
                    IDataReader reader = dbcmd.ExecuteReader();
                    
                    while(reader.Read()) {
                        feed[0] = reader.GetString(0).ToString(); // integer primary key
                        feed[1] = reader.GetString(1); // uri
                        feed[2] = reader.GetString(2); // generated_name
                        feed[3] = reader.GetString(3); // name
                        feed[4] = reader.GetString(4); // author
                        feed[5] = reader.GetString(5); // subtitle
                        feed[6] = reader.GetString(6); // image
                        feed[7] = reader.GetString(7); // license
                        feed[8] = reader.GetString(8); // etag
                        feed[9] = reader.GetString(9); // hmodified
                        feed[10] = reader.GetString(10); // status
                        feed[11] = reader.GetString(11); // tags
                        feed[12] = reader.GetString(12); //favicon
                    }
                    reader.Close();
                    reader = null;
                    dbcmd.Dispose();
                    dbcmd = null;
                    return feed;
                }
                
                public ArrayList GetFeeds() {
                    ArrayList list = new ArrayList();
                    
                    IDbCommand dbcmd = db.CreateCommand();
                    dbcmd.CommandText = "select * from Feeds";
                    IDataReader reader = dbcmd.ExecuteReader();
                    while(reader.Read()) {
                        list.Add(GetFeed(reader.GetString(1)));
                    }
                    reader.Close();
                    reader = null;
                    dbcmd.Dispose();
                    dbcmd = null;
                    return list;
                }
                
                public bool FeedExists(string url) {
                    bool exists = false;
                    foreach (string[] feed in GetFeeds()) {
                        if ( feed[1] == url ) {
                            exists = true;
                        }
                    }
                    return exists;
                }
                
                public ArrayList GetPosts(string feeduri) {
                    ArrayList list = new ArrayList();
                    
                    IDbCommand dbcmd = db.CreateCommand();
                    dbcmd.CommandText = "select * from "+GetGeneratedName(feeduri);
                    IDataReader reader = dbcmd.ExecuteReader();
                    while(reader.Read()) {
                        string[] item = new string[10];
                        item[0] = reader.GetString(1); //title
                        item[1] = reader.GetString(2); //uri
                        item[2] = reader.GetString(3); //date
                        item[3] = reader.GetString(4); //last_updated
                        item[4] = reader.GetString(5); //author
                        item[5] = reader.GetString(6); //tags
                        item[6] = reader.GetString(7); //content
                        item[7] = reader.GetString(8); //encuri
                        item[8] = reader.GetString(9); //read
                        item[9] = reader.GetString(10); //flagged
                        
                        list.Add(item);
                    }
                    reader.Close();
                    reader = null;
                    dbcmd.Dispose();
                    dbcmd = null;
                    return list;
                }
                
                public string[] GetItem(string feeduri, string uri) {
                    ArrayList list = GetPosts(feeduri);
                    
                    foreach (string[] item in list) {
                        if ( item[1] == uri ) {
                            return item;
                        }
                    }
                    return null;
                }
                
                public void AddItem(string feeduri, string title, string uri, string date, string last_updated, string author, string tags, string content, string encuri, string read, string flagged) {
                    string generated_name = GetGeneratedName(feeduri);
                    
                    IDbCommand dbcmd = db.CreateCommand();
                    dbcmd.CommandText = @"insert into "+generated_name+" values (null, :title, :uri, :date, :lastup, :author, :tags, :content, :encuri, :read, :flagged)";
                    
                    SqliteParameter title_parameter = new SqliteParameter();
                    title_parameter.Value = EscapeParam(title);
                    title_parameter.ParameterName = @":title";
                    dbcmd.Parameters.Add(title_parameter);
                    
                    SqliteParameter uri_parameter = new SqliteParameter();
                    uri_parameter.Value = EscapeParam(uri);
                    uri_parameter.ParameterName = @":uri";
                    dbcmd.Parameters.Add(uri_parameter);
                    
                    SqliteParameter date_parameter = new SqliteParameter();
                    date_parameter.Value = EscapeParam(date);
                    date_parameter.ParameterName = @":date";
                    dbcmd.Parameters.Add(date_parameter);
                    
                    SqliteParameter lu_parameter = new SqliteParameter();
                    lu_parameter.Value = EscapeParam(last_updated);
                    lu_parameter.ParameterName = @":lastup";
                    dbcmd.Parameters.Add(lu_parameter);
                    
                    SqliteParameter author_parameter = new SqliteParameter();
                    author_parameter.Value = EscapeParam(author);
                    author_parameter.ParameterName = @":author";
                    dbcmd.Parameters.Add(author_parameter);
                    
                    SqliteParameter tags_parameter = new SqliteParameter();
                    tags_parameter.Value = EscapeParam(tags);
                    tags_parameter.ParameterName = @":tags";
                    dbcmd.Parameters.Add(tags_parameter);
                    
                    SqliteParameter content_parameter = new SqliteParameter();
                    content_parameter.Value = EscapeParam(content);
                    content_parameter.ParameterName = @":content";
                    dbcmd.Parameters.Add(content_parameter);
                    
                    SqliteParameter encuri_parameter = new SqliteParameter();
                    encuri_parameter.Value = EscapeParam(encuri);
                    encuri_parameter.ParameterName = @":encuri";
                    dbcmd.Parameters.Add(encuri_parameter);
                    
                    SqliteParameter read_parameter = new SqliteParameter();
                    read_parameter.Value = EscapeParam("False");
                    read_parameter.ParameterName = @":read";
                    dbcmd.Parameters.Add(read_parameter);
                    
                    SqliteParameter flagged_parameter = new SqliteParameter();
                    flagged_parameter.Value = EscapeParam("False");
                    flagged_parameter.ParameterName = @":flagged";
                    dbcmd.Parameters.Add(flagged_parameter);
                    
                    dbcmd.ExecuteNonQuery();
                    dbcmd.Dispose();
                    dbcmd = null;
                }
                
                public void ChangeFeedInfo(string feeduri, string property, string intended_value) {
                    NonQueryCommand("update Feeds set "+property+@"="""+intended_value+@""" where uri="""+feeduri+@"""");
                }
                
                public void ChangeItemInfo(string feeduri, string itemuri, string property, string intended_value) {
                    NonQueryCommand("update "+GetGeneratedName(feeduri)+" set "+property+@"="""+intended_value+@""" where uri="""+itemuri+@"""");
                }
            }
        }
    }
}
