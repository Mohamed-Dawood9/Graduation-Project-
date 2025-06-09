using Demo.BLL.Repositories;
using GP.BLL.Interfaces;
using GP.DAL.Data;
using GP.DAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GP.BLL.Repositries
{
    public class NotesRepositry: GenericRepository<Note>,INotesInterface
    {
        public NotesRepositry(AppDbContext dbContext):base(dbContext)
        {
            
        }

        public IEnumerable<Note> GetAllByID(int appointmentId)
        {
            return _DbContext.Notes.Where(n => n.AppointmentId == appointmentId);
        }
        public void Remove(Note note)
        {
            _DbContext.Notes.Remove(note); 
        }
    }
}
