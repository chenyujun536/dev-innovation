using System.Collections.Generic;
using System.Threading.Tasks;
using ShootRate.Models;
using SQLite;

namespace ShootRate.Data
{
    public class NoteDatabase
    {
        readonly SQLiteAsyncConnection database;

        public NoteDatabase(string dbPath)
        {
            database = new SQLiteAsyncConnection(dbPath);
            database.CreateTableAsync<Game>().Wait();
            database.CreateTableAsync<TenShoot>().Wait();
        }

        public Task<List<Game>> GetNotesAsync()
        {
            //Get all notes.
            return database.Table<Game>().ToListAsync();
        }

        public Task<Game> GetNoteAsync(int id)
        {
            // Get a specific note.
            return database.Table<Game>()
                            .Where(i => i.ID == id)
                            .FirstOrDefaultAsync();
        }

        public Task<int> SaveNoteAsync(Game note)
        {
            if (note.ID != 0)
            {
                // Update an existing note.
                return database.UpdateAsync(note);
            }
            else
            {
                // Save a new note.
                return database.InsertAsync(note);
            }
        }

        public Task<int> DeleteNoteAsync(Game note)
        {
            // Delete a note.
            return database.DeleteAsync(note);
        }
    }
}