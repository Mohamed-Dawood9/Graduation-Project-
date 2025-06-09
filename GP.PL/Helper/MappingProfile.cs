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
             CreateMap<Appointment,AppointmentViewModel>().ReverseMap()
                .ForMember(dest => dest.Notes, opt => opt.MapFrom(src => src.Notes.Select(n => n.Content).ToList()))
                 .ForMember(dest => dest.OrginalPhotoPath, opt => opt.MapFrom(src => src.OrginalPhotoPath));
            CreateMap<ApplicationUser, UserViewModel>().ReverseMap().ForMember(dest => dest.ProfilePhoto, opt => opt.MapFrom(src => src.ProfilePhoto));
            CreateMap<Note, NoteViewModel>(); // If NoteViewModel is used

            CreateMap<Analysis, AnalysisViewModel>()
            .ForMember(dest => dest.Notes, opt => opt.MapFrom(src => src.Notes));
            CreateMap<AnalysisViewModel, Analysis>();

        }

    }
}
