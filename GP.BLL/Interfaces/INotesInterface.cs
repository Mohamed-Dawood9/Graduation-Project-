using GP.DAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GP.BLL.Interfaces
{
    public interface INotesInterface:IGenericInterface<Note>
    {
        IEnumerable<Note>GetAllByID(int id);
        void Remove (Note note);
    }
}
