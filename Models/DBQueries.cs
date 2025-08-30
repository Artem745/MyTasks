using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Data.Sqlite;

namespace MyTasks
{
    public class DBQueries
    {
        public static void CreateDB()
        {
            using var connection = new SqliteConnection("Data Source=tasks.db");
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = @"CREATE TABLE IF NOT EXISTS Task (id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, name TEXT, description TEXT, difficulty TEXT, category TEXT, isDone INTEGER DEFAULT 0)";
            command.ExecuteNonQuery();
        }

        public static void InsertDB(string? name, string? description, string? difficulty, string? category)
        {
            using var connection = new SqliteConnection("Data Source=tasks.db");
            connection.Open();
            var command = connection.CreateCommand();

            command.CommandText =
            @"
                INSERT INTO Task (name, description, difficulty, category)
                VALUES ($name, $description, $difficulty, $category)
            ";
            command.Parameters.AddWithValue("$name", name);
            command.Parameters.AddWithValue("$description", description);
            command.Parameters.AddWithValue("$difficulty", difficulty);
            command.Parameters.AddWithValue("$category", category);
            command.ExecuteNonQuery();
        }
       
        public static void DeleteDB(int Id)
        {
            using var connection = new SqliteConnection("Data Source=tasks.db");
            connection.Open();
            var command = connection.CreateCommand();

            command.CommandText = "DELETE FROM Task WHERE id = $id";
            command.Parameters.AddWithValue("$id", Id);
            command.ExecuteNonQuery();
        }

        public static void SetDoneDB(int Id)
        {
            using var connection = new SqliteConnection("Data Source=tasks.db");
            connection.Open();
            var command = connection.CreateCommand();

            command.CommandText = "SELECT isDone FROM Task WHERE id = $id";
            command.Parameters.AddWithValue("$id", Id);
            int IsDone = 0;
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    IsDone = 1 - Convert.ToInt32(reader.GetString(0));
                }
            }
            command.CommandText = "UPDATE Task SET isDone = $isDone WHERE id = $id";
            command.Parameters.AddWithValue("$isDone", IsDone);
            command.ExecuteNonQuery();
        }

        public static ObservableCollection<MyTask> SelectDB(string category)
        {
            ObservableCollection<MyTask> notes = [];
            using var connection = new SqliteConnection("Data Source=tasks.db");
            connection.Open();
            var command = connection.CreateCommand();

            command.CommandText = "SELECT id, name, description, difficulty, isDone FROM Task WHERE category = $category";
            command.Parameters.AddWithValue("$category", category);

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                string DifficultyColor = "";
                string DoneColor = "";
                switch (reader.GetString(3))
                {
                    case "Easy":
                        DifficultyColor = "#1e871e";
                        break;
                    case "Medium":
                        DifficultyColor = "#d3c500";
                        break;
                    case "Hard":
                        DifficultyColor = "#ff0000";
                        break;
                }
                switch (reader.GetString(4))
                {
                    case "0":
                        DoneColor = "#3aa6ff";
                        break;
                    case "1":
                        DoneColor = "#535353";
                        break;
                }
                MyTask task = new()
                {
                    Id = reader.GetString(0),
                    Name = reader.GetString(1),
                    Description = reader.GetString(2),
                    Difficulty = reader.GetString(3),
                    DifficultyColor = DifficultyColor,
                    IsDone = reader.GetString(4) == "0" ? "Done" : "Undone",
                    DoneColor = DoneColor,
                };
                notes.Add(task);
            }
            return notes;
        }
    }
    public class MyTask
    {
        public string Id { get; set; } = "";
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public string Difficulty { get; set; } = "";
        public string DifficultyColor { get; set; } = "";
        public string IsDone { get; set; } = "";
        public string DoneColor { get; set; } = "";

    }
}