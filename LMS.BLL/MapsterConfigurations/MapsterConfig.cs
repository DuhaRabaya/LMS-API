using LMS.DAL.DTO.Response.CoursesResponses;
using LMS.DAL.DTO.Response.SubmissionResponses;
using LMS.DAL.DTO.Response.TaskResponse;
using LMS.DAL.Models;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.BLL.MapsterConfigurations
{
    public static class MapsterConfig
    {
        public static void MapsterConfRegister()
        {
            TypeAdapterConfig<Course, CourseResponse>.NewConfig()
              .Map(dest => dest.Instructor, source => source.Instructor.UserName);

            TypeAdapterConfig<Course, CourseResponseForStudent>.NewConfig()
            .Map(dest => dest.Name, src => src.Translations.FirstOrDefault(t =>
                 t.Language == MapContext.Current.Parameters["lang"].ToString()).Name)
           .Map(dest => dest.Description, src => src.Translations.FirstOrDefault(t =>
                 t.Language == MapContext.Current.Parameters["lang"].ToString()).Description)
           .Map(dest => dest.Instructor, source => source.Instructor.UserName)
           .Map(dest => dest.Thumbnail, source => $"http://localhost:5165/Images/{source.Thumbnail}")
            .Map(dest => dest.FinalPrice, src => src.FinalPrice);

            TypeAdapterConfig<Course, CourseResponse>.NewConfig()
                .Map(dest => dest.Thumbnail, source => $"http://localhost:5165/Images/{source.Thumbnail}")
                .Map(dest => dest.FinalPrice, src => src.FinalPrice);


            TypeAdapterConfig<TaskItem, TaskResponse>.NewConfig()
            .Map(dest => dest.Title, src => src.Translations.FirstOrDefault(t =>
                 t.Language == MapContext.Current.Parameters["lang"].ToString()).Title)
           .Map(dest => dest.Description, src => src.Translations.FirstOrDefault(t =>
                 t.Language == MapContext.Current.Parameters["lang"].ToString()).Description)
           .Map(dest => dest.AttachmentUrl, source => $"http://localhost:5165/Tasks/{source.AttachmentUrl}");

            TypeAdapterConfig<Submission, SubmissionResponse>.NewConfig()
              .Map(dest => dest.StudentName, source => source.Student.UserName);
        }
    }
}
