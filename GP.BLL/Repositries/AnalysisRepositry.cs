using Demo.BLL.Repositories;
using GP.BLL.Interfaces;
using GP.DAL.Data;
using GP.DAL.Data.Models;
using Microsoft.EntityFrameworkCore;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GP.BLL.Repositries
{
    public class AnalysisRepositry: GenericRepository<Analysis>, IAnalysisInterFace
    {
        public AnalysisRepositry(AppDbContext dbContext) : base(dbContext)
        {

        }
        public  Analysis GetById(int id)
        {
            return _DbContext.Analyses
                .Include(a => a.Notes) // Eagerly load Notes
                .FirstOrDefault(a => a.Id == id);
        }
    }
}
