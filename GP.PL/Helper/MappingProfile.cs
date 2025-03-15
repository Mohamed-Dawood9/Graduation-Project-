using AutoMapper;
using GP.DAL.Data.Models;
using GP.PL.VIewModel;
using Microsoft.AspNetCore.Identity;

namespace GP.PL.Helper
{
    public class MappingProfile:Profile
    {
       public MappingProfile() {

             CreateMap<Patient,PatientViewModel>().ReverseMap();
        }
    }
}
